/// <summary>
/// Autolume manipulator using a wheel that can rotate (Z) and move (X).
/// </summary>
public class SeedController : ManipulatorController
{
    /// <inheritdoc cref="ManipulatorController.SendToAutolume"/>
    protected override void SendToAutolume()
    {
        var diff = manipulator.position - resetPoint.position;
        float[] values =
        {
            diff.x * 10f,
            // diff.y * 15f
            manipulator.rotation.eulerAngles.z / 10f
        };

        SendValues(values, new[] { "latent", "noise" });
    }
}