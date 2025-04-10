/// <summary>
/// Autolume manipulator using an orb that can move (X, Y, Z).
/// </summary>
public class PodiumController : ManipulatorController
{
    protected override void SendToAutolume()
    {
        var diff = manipulator.position - resetPoint.position;
        float[] values =
        {
            diff.x * 15f,
            diff.y * 15f,
            diff.z * 15f
        };

        SendValues(values);
    }
}