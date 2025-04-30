using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Simple class to reset the scene on keyboard or controller input.
/// </summary>
public class ResetManager : MonoBehaviour
{
    public InputActionReference resetInput;

    private void Start()
    {
        resetInput.action.performed += ResetScene;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(ReloadCurrentScene());
        }
    }

    private void ResetScene(InputAction.CallbackContext context)
    {
        StartCoroutine(ReloadCurrentScene());
    }

    private static IEnumerator ReloadCurrentScene()
    {
        var asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        while (asyncLoad != null && !asyncLoad.isDone)
        {
            yield return null;
        }
    }
}