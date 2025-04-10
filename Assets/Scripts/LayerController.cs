/// <summary>
/// Autolume manipulator using a wheel that can rotate (Z) and move (X, Y).
/// </summary>
public class LayerController : ManipulatorController
{
    protected override void SendToAutolume()
    {
        var diff = manipulator.position - resetPoint.position;
        float[] values =
        {
            diff.x * 10f,
            diff.y * -10f,
            manipulator.rotation.eulerAngles.z,
            manipulator.localScale.x * 5f
        };

        SendValues(values, new[] { "x", "y", "r", "s" });
    }
}