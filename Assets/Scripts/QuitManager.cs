using UnityEngine;

/// <summary>
/// Simple class to close the application when hitting "Escape".
/// </summary>
public class QuitManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}