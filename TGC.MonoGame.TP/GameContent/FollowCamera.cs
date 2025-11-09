using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace TGC.MonoGame.TP;

public class FollowCamera
{
    public const float DefaultFieldOfViewDegrees = MathHelper.PiOver4;
    public const float DefaultNearPlaneDistance = 1f;
    public const float DefaultFarPlaneDistance = 2000f;
    public const float OrbitAngularSpeed = 0.35f;
    public const float OrbitVerticalAngle = 0.25f;

    private Matrix View;
    private Matrix Projection;

    // Estado
    private Vector3 Position;
    private Vector3 TargetPosition;
    private Vector3 UpDirection = Vector3.Up;

    private int CenterXPosition;
    private int CenterYPosition;
    private float Radius;
    private float VerticalAngle;
    private float HorizontalAngle;
    private float Sensitivity;

    private bool _lockToGun = false;
    private float _camBack = 120f;   // qué tanto va “detrás” del cañón
    private float _camUp   = 60f;    // altura respecto del cañón
    private float _camAhead = 100f;  // qué tan lejos mira por delante

    public FollowCamera(
        float aspectRatio,
        int centerX, int centerY, float radius, float sensitivity = 0.001f,
        float nearPlaneDistance = DefaultNearPlaneDistance,
        float farPlaneDistance  = DefaultFarPlaneDistance,
        float fieldOfViewDegrees = DefaultFieldOfViewDegrees)
    {
        CenterXPosition = centerX;
        CenterYPosition = centerY;
        Radius = radius;
        HorizontalAngle = MathHelper.PiOver2;
        VerticalAngle = 0.3f;
        Sensitivity = sensitivity;
        BuildProjection(aspectRatio, nearPlaneDistance, farPlaneDistance, fieldOfViewDegrees);
    }

    public void BuildProjection(float aspectRatio, float nearPlaneDistance, float farPlaneDistance,
        float fieldOfViewDegrees)
    {
        Projection = Matrix.CreatePerspectiveFieldOfView(
            MathHelper.ToRadians(fieldOfViewDegrees), aspectRatio, nearPlaneDistance, farPlaneDistance);
    }

    private void BuildView() => View = Matrix.CreateLookAt(Position, TargetPosition, UpDirection);

    public void UpdateOrbit(Vector3 targetPosition, int mouseX, int mouseY)
    {
        if (_lockToGun) return; // ignorar si estamos en modo cañón

        int offsetX = mouseX - CenterXPosition;
        int offsetY = mouseY - CenterYPosition;

        HorizontalAngle += offsetX * Sensitivity;
        VerticalAngle   += offsetY * Sensitivity;
        VerticalAngle = MathHelper.Clamp(VerticalAngle, -MathHelper.PiOver2 + 0.1f, MathHelper.PiOver2 - 0.1f);

        var offset = CalculateOffsetPosition();
        Position = targetPosition + offset;
        TargetPosition = targetPosition;
        BuildView();
    }

    private Vector3 CalculateOffsetPosition()
    {
        float x = Radius * (float)(Math.Cos(HorizontalAngle) * Math.Cos(VerticalAngle));
        float y = Radius * (float)Math.Sin(VerticalAngle);
        float z = Radius * (float)(Math.Sin(HorizontalAngle) * Math.Cos(VerticalAngle));
        return new Vector3(x, y, z);
    }
    // Auto-orbit mode: rotates around target at fixed angular speed, ignoring mouse
    public void UpdateOrbitAuto(
        Vector3 targetPosition,
        GameTime gameTime,
        float orbitAngularSpeed = OrbitAngularSpeed,
        float orbitVerticalAngle = OrbitVerticalAngle
    )
    {
        if (_lockToGun) return;
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        HorizontalAngle += orbitAngularSpeed * dt;
        VerticalAngle = orbitVerticalAngle;
        var offset = CalculateOffsetPosition();
        Position = targetPosition + offset;
        TargetPosition = targetPosition;
        BuildView();
    }

    public void SetLockToGun(bool enabled) => _lockToGun = enabled;

    /// <summary>
    /// Reconstruye la cámara en base al transform absoluto del cañón (gunWorld).
    /// gunWorld: matriz del hueso del cañón * world del tanque.
    /// </summary>
    /// <param name="gunWorld">Transform absoluto del cañón</param>
    /// <param name="useUpAsForward">En tu rig usabas Up como “forward” del cañón</param>
    public void UpdateLockedToGun(Matrix gunWorld, bool useUpAsForward = true)
    {
        if (!_lockToGun) return;

        Vector3 gunForward = useUpAsForward ? gunWorld.Up : gunWorld.Forward;
        gunForward.Normalize();

        Vector3 gunOrigin = gunWorld.Translation;

        Position       = gunOrigin - gunForward * _camBack + Vector3.Up * _camUp;
        TargetPosition = gunOrigin + gunForward * _camAhead;

        BuildView();
    }

    // ---------------- CHASE MODE (detrás del objetivo) ----------------
    public void UpdateChase(Vector3 origin, Vector3 forward, float back, float up, float ahead)
    {
        if (forward.LengthSquared() < 1e-6f) forward = Vector3.Forward;
        forward.Normalize();
        Position       = origin - forward * back + Vector3.Up * up;
        TargetPosition = origin + forward * ahead;
        BuildView();
    }
    public void UpdateOrbitBehind(Vector3 targetPosition, Vector3 forward, int mouseX, int mouseY)
    {
        if (_lockToGun) return;
        // var mouseX = GameManager.GetMousePositionX();
        // var mouseX = GameManager.GetMousePositionX();

        int offsetX = mouseX - CenterXPosition;
        int offsetY = mouseY - CenterYPosition;

        HorizontalAngle += offsetX * Sensitivity;
        VerticalAngle    = MathHelper.Clamp(VerticalAngle + offsetY * Sensitivity, -MathHelper.PiOver2 + 0.1f, MathHelper.PiOver2 - 0.1f);

        var offset = CalculateOffsetPosition();
        Position = targetPosition + offset;
        TargetPosition = targetPosition;
        BuildView();
    }

    public void SetRadius(float radius)
    {
        Radius = Math.Max(1f, radius);
    }

    public Matrix ViewMatrix => View;
    public Matrix ProjectionMatrix => Projection;

    // Exponer posición, objetivo y forward para apuntar cañón
    public Vector3 PositionWorld => Position;
    public Vector3 TargetWorld => TargetPosition;
    public Vector3 Forward => Vector3.Normalize(TargetPosition - Position);
    public float GetHorizontalAngle() => HorizontalAngle;
    // Permite tunear offsets sin recompilar
    public void SetGunCameraOffsets(float back, float up, float ahead)
    {
        _camBack = back; _camUp = up; _camAhead = ahead;
    }
}