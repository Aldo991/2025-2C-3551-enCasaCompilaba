// using System;
using System;
using Microsoft.Xna.Framework;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace TGC.MonoGame.TP;

public class FollowCamera
{
    public const float DefaultFieldOfViewDegrees = MathHelper.PiOver4;
    public const float DefaultNearPlaneDistance = 0.1f;
    public const float DefaultFarPlaneDistance = 200000f;
    private const float RotationSpeed = 1.5f; // Radianes por segundo
    private Matrix View { get; set; }
    private Matrix Projection { get; set; }
    private Vector3 Position { get; set; }
    private Vector3 TargetPosition { get; set; }
    private Vector3 UpDirection { get; set; }
    private Vector3 Offset { get; set; }

    public FollowCamera(float aspectRatio, Vector3 position, Vector3 targetPosition,
        float nearPlaneDistance = DefaultNearPlaneDistance,
        float farPlaneDistance = DefaultFarPlaneDistance, float fieldOfViewDegrees = DefaultFieldOfViewDegrees)
    {
        UpDirection = Vector3.Up;
        Offset = new Vector3(0, 3000f, 9000f);
        Position = position + Offset;
        TargetPosition = targetPosition;
        BuildProjection(aspectRatio, nearPlaneDistance, farPlaneDistance, fieldOfViewDegrees);
        BuildView();
    }

    public void BuildProjection(float aspectRatio, float nearPlaneDistance, float farPlaneDistance,
        float fieldOfViewDegrees)
    {
        Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(fieldOfViewDegrees), aspectRatio, nearPlaneDistance,
            farPlaneDistance);
    }
    public void RotateCameraRight(GameTime gameTime)
    {
        RotateCamera(gameTime, true);
    }
    public void RotateCameraLeft(GameTime gameTime)
    {
        RotateCamera(gameTime, false);
    }
    public void RotateCamera(GameTime gameTime, bool right)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        float rotationAngle = RotationSpeed * deltaTime;
        if (right)
            rotationAngle = -rotationAngle;
        Offset = Vector3.Transform(Offset, Matrix.CreateRotationY(rotationAngle));
    }

    private void BuildView()
    {
        View = Matrix.CreateLookAt(Position, TargetPosition, UpDirection);
    }

    public void UpdateCamera(Vector3 targetPosition)
    {
        Position = targetPosition + Offset;
        TargetPosition = targetPosition;
        BuildView();
    }
    public Matrix ViewMatrix => View;
    public Matrix ProjectionMatrix => Projection;
}