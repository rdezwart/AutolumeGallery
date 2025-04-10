using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScreenshotManager : MonoBehaviour
{
    public InputActionReference screenshotInput;
    public AudioSource cameraClick;

    public int captureWidth = 1920;
    public int captureHeight = 1080;

    private Camera _camera;

    private void Start()
    {
        screenshotInput.action.performed += TakeScreenshot;

        _camera = GetComponent<Camera>();
    }

    /// <summary>
    /// Custom <c>InputAction</c> received from an <c>XR Controller</c>. Will take a screenshot of the <c>Camera</c> on
    /// this object.
    /// </summary>
    /// <param name="context">Contains information about what triggered the action.</param>
    private void TakeScreenshot(InputAction.CallbackContext context)
    {
        cameraClick.Play();
        StartCoroutine(SaveCameraView(_camera));
    }

    /// <summary>
    /// Coroutine to save the camera's view to a file.
    /// </summary>
    /// <remarks>
    /// Referenced: <see href="https://gamedevbeginner.com/how-to-capture-the-screen-in-unity-3-methods">"Game Dev Beginner" article</see>
    /// and <see href="https://discussions.unity.com/t/how-to-save-a-picture-take-screenshot-from-a-camera-in-game/5792/8">Unity forum response</see>.
    /// </remarks>
    /// <param name="cam">Camera component to render from.</param>
    private IEnumerator SaveCameraView(Camera cam)
    {
        // Wait for frame to finish rendering
        yield return new WaitForEndOfFrame();

        // Create textures to read into
        var screenTexture = new RenderTexture(captureWidth, captureHeight, 24);
        var screenshot = new Texture2D(captureWidth, captureHeight, TextureFormat.RGB24, false);

        // Render camera to a RenderTexture
        cam.targetTexture = screenTexture;
        cam.Render();

        // Read the RenderTexture to a Texture2D
        RenderTexture.active = screenTexture;
        screenshot.ReadPixels(new Rect(0, 0, captureWidth, captureHeight), 0, 0);

        // Reset 
        cam.targetTexture = null;
        RenderTexture.active = null;

        // Prepare data for writing
        var filename = GetUniqueFileName();
        var fileData = screenshot.EncodeToPNG();

        // Write image to new file
        new System.Threading.Thread(() => { File.WriteAllBytes(filename, fileData); }).Start();
    }

    /// <summary>
    /// Generates the path to a unique PNG file name in a 'Screenshots' folder, using current date and time.
    /// </summary>
    /// <remarks>
    /// Referenced: <see href="https://discussions.unity.com/t/how-to-save-a-picture-take-screenshot-from-a-camera-in-game/5792/8">Unity forum response</see>.
    /// </remarks>
    /// <returns>Path to a unique PNG file name.</returns>
    private static string GetUniqueFileName()
    {
        // Save to default data path
        var folder = Application.dataPath;
        if (Application.isEditor)
        {
            // Put screenshots in a folder above the asset path so Unity doesn't index the files
            var stringPath = folder + "/..";
            folder = Path.GetFullPath(stringPath);
        }

        folder += "/Screenshots";

        // Make sure directory exists
        Directory.CreateDirectory(folder);
        var filename = $"{folder}/screenshot_{DateTime.Now:yyyy-MM-dd_HH.mm.ss}.png";

        return filename;
    }
}