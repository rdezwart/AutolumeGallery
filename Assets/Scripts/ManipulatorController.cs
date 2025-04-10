using extOSC;
using UnityEngine;

/// <summary>
/// Parent class for Autolume manipulator podiums.
/// </summary>
public abstract class ManipulatorController : MonoBehaviour
{
    public Transform anchor;
    public Transform resetPoint;
    public Transform manipulator;

    [Header("OSC Settings")] public OSCTransmitter transmitter;
    public string address;
    [Range(0f, 0.5f)] public float repeatRate = 0.2f;

    protected virtual void Start()
    {
        MatchTransforms(resetPoint, manipulator);
        anchor.position = resetPoint.position;

        if (transmitter == null)
            transmitter = GameObject.Find("OSC Manager").GetComponent<OSCTransmitter>();

        InvokeRepeating(nameof(SendToAutolume), 0f, repeatRate);
    }

    /// <summary>
    /// Compiles and sends data to Autolume over OSC, in a format specified by subclasses.
    /// </summary>
    /// <seealso cref="SendValues(float[])"/>
    protected abstract void SendToAutolume();

    /// <summary>
    /// Sends exactly three floats over OSC to <see cref="address"/>, mapped to sub-addresses <c>/x</c>, <c>/y</c>, and
    /// <c>/z</c>
    /// </summary>
    /// <param name="values">The values to send.</param>
    protected void SendValues(float[] values)
    {
        SendValues(values, new[] { "x", "y", "z" });
    }

    /// <summary>
    /// Sends data over OSC to <see cref="address"/>, mapped to specified <paramref name="letters"/>.
    /// </summary>
    /// <remarks><paramref name="values"/> and <paramref name="letters"/> should be of equal length.</remarks>
    /// <param name="values">The floats to send.</param>
    /// <param name="letters">The sub-address each float is mapped to.</param>
    protected void SendValues(float[] values, string[] letters)
    {
        if (values.Length != letters.Length)
        {
            Debug.LogError("Length of value and letter arrays do not match", this);
            CancelInvoke(nameof(SendToAutolume));
            return;
        }

        for (var i = 0; i < values.Length; i++)
        {
            var message = new OSCMessage($"{address}/{letters[i]}");
            message.AddValue(OSCValue.Float(values[i]));

            if (transmitter != null)
                transmitter.Send(message);
        }
    }

    /// <summary>
    /// Resets child <c>Manipulator</c> to original position, rotation, and scale.
    /// </summary>
    public virtual void ResetManipulator()
    {
        MatchTransforms(manipulator.transform, resetPoint.transform);
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