using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PanelController : MonoBehaviour
{
    public Transform visuals;
    public Transform visualsResetPoint;
    private Quaternion _originalRotation;
    public XRSocketInteractor homeInteractor;

    private bool _selected;
    private bool _isRotating;
    private bool _isScaling;

    [Header("Events")] public InputActionReference rotateClockwise;
    public InputActionReference rotateCounterClockwise;
    public InputActionReference flipHorizontally;
    public InputActionReference flipVertically;

    public UnityEvent<PanelController> panelInserted = new();
    public UnityEvent<PanelController> panelRemoved = new();
    public UnityEvent<PanelController> panelGrabbed = new();

    private void Start()
    {
        rotateClockwise.action.started += RotateCW;
        rotateCounterClockwise.action.started += RotateCCW;
        flipHorizontally.action.started += FlipHorizontal;
        flipVertically.action.started += FlipVertical;

        MatchTransforms(visualsResetPoint, visuals);
        _originalRotation = transform.rotation;
    }

    /// <summary>
    /// Custom <c>InputAction</c> received from an <c>XR Controller</c>. Will rotate the <see cref="visuals"/> of this
    /// <c>PanelController</c> 90deg clockwise.
    /// </summary>
    /// <param name="context">Contains information about what triggered the action.</param>
    private void RotateCW(InputAction.CallbackContext context)
    {
        if (_selected)
            RotateBy(-90f);
    }

    /// <summary>
    /// Custom <c>InputAction</c> received from an <c>XR Controller</c>. Will rotate the <see cref="visuals"/> of this
    /// <c>PanelController</c> 90deg counter-clockwise.
    /// </summary>
    /// <param name="context">Contains information about what triggered the action.</param>
    private void RotateCCW(InputAction.CallbackContext context)
    {
        if (_selected)
            RotateBy(90f);
    }

    /// <summary>
    /// Custom <c>InputAction</c> received from an <c>XR Controller</c>. Will horizontally flip the
    /// <see cref="visuals"/> of this <c>PanelController</c>.
    /// </summary>
    /// <param name="context">Contains information about what triggered the action.</param>
    private void FlipHorizontal(InputAction.CallbackContext context)
    {
        if (_selected)
        {
            // Account for rotation, should always be locked to clean right angles
            ScaleBy(Mathf.Round(visuals.rotation.eulerAngles.z) % 180 == 0
                ? new Vector3(-1f, 1f, 1f)
                : new Vector3(1f, -1f, 1f));
        }
    }

    /// <summary>
    /// Custom <c>InputAction</c> received from an <c>XR Controller</c>. Will vertically flip the <see cref="visuals"/>
    /// of this <c>PanelController</c>.
    /// </summary>
    /// <param name="context">Contains information about what triggered the action.</param>
    private void FlipVertical(InputAction.CallbackContext context)
    {
        if (_selected)
        {
            // Account for rotation, should always be locked to clean right angles
            // Had roundoff errors with rotating in negative directions back to 0, have to round
            ScaleBy(Mathf.Round(visuals.rotation.eulerAngles.z) % 180 == 0
                ? new Vector3(1f, -1f, 1f)
                : new Vector3(-1f, 1f, 1f));
        }
    }

    public void OnSelectEnter(SelectEnterEventArgs args)
    {
        if (args.interactorObject.transform.gameObject.CompareTag("Socket"))
        {
            panelInserted.Invoke(this);
        }
        else
        {
            panelGrabbed.Invoke(this);
            _selected = true;
        }
    }

    public void OnSelectExit(SelectExitEventArgs args)
    {
        if (args.interactorObject.transform.gameObject.CompareTag("Socket"))
        {
            panelRemoved.Invoke(this);
        }
        else
        {
            _selected = false;
        }
    }

    /// <summary>
    /// Rotates the <see cref="visuals"/> of this <c>PanelController</c> over time.
    /// </summary>
    /// <param name="degrees">The number of degrees to rotate from the initial position.</param>
    private void RotateBy(float degrees)
    {
        Vector3 targetRotation = visuals.rotation.eulerAngles;
        targetRotation.z += degrees;

        if (!_isRotating && !_isScaling)
            StartCoroutine(LerpRotation(Quaternion.Euler(targetRotation), 0.5f));
    }

    /// <summary>
    /// Coroutine to linearly interpolate the rotation of the <see cref="visuals"/> to a target value. 
    /// </summary>
    /// <param name="endValue">Target rotation.</param>
    /// <param name="duration">How long the rotation will take.</param>
    private IEnumerator LerpRotation(Quaternion endValue, float duration)
    {
        // Referenced: https://gamedevbeginner.com/the-right-way-to-lerp-in-unity-with-examples/ 

        float time = 0f;
        Quaternion startValue = visuals.rotation;
        _isRotating = true;

        while (time < duration)
        {
            visuals.rotation = Quaternion.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        visuals.rotation = endValue;
        _isRotating = false;
    }

    /// <summary>
    /// Scales the <see cref="visuals"/> of this <c>PanelController</c> over time.
    /// </summary>
    /// <param name="scale">The target scale value.</param>
    private void ScaleBy(Vector3 scale)
    {
        Vector3 targetScale = visuals.localScale;
        for (var i = 0; i < 3; i++)
        {
            targetScale[i] *= scale[i];
        }

        if (!_isRotating && !_isScaling)
            StartCoroutine(LerpScale(targetScale, 0.5f));
    }

    /// <summary>
    /// Coroutine to linearly interpolate the scale of the <see cref="visuals"/> to a target value. 
    /// </summary>
    /// <param name="endValue">Target scale.</param>
    /// <param name="duration">How long the scaling will take.</param>
    private IEnumerator LerpScale(Vector3 endValue, float duration)
    {
        // Referenced: https://gamedevbeginner.com/the-right-way-to-lerp-in-unity-with-examples/ 

        float time = 0;
        Vector3 startScale = visuals.localScale;
        _isScaling = true;

        while (time < duration)
        {
            visuals.localScale = Vector3.Lerp(startScale, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        visuals.localScale = endValue;
        _isScaling = false;
    }

    /// <summary>
    /// Resets child <c>Manipulator</c> to original position, rotation, and scale.
    /// </summary>
    public void ResetPanel()
    {
        MatchTransforms(visuals.transform, visualsResetPoint.transform);
        transform.rotation = _originalRotation;
        homeInteractor.StartManualInteraction((IXRSelectInteractable)gameObject.GetComponent<XRGrabInteractable>());
    }

    /// <summary>
    /// Sets all values of one <c>Transform</c> to those of another.
    /// </summary>
    /// <param name="from">The <c>Transform</c> to modify.</param>
    /// <param name="to">The target <c>Transform</c>.</param>
    private static void MatchTransforms(Transform from, Transform to)
    {
        from.position = to.position;
        from.rotation = to.rotation;
        from.localScale = to.localScale;
    }
}