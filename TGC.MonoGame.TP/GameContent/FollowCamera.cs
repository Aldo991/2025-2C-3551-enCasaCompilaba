using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
    private int CenterXPosition;
    private int CenterYPosition;
    private float Radius;
    private float VerticalAngle;
    private float HorizontalAngle;

    public FollowCamera(float aspectRatio, Vector3 position, Vector3 targetPosition,
        int centerX, int centerY, float radius,
        float nearPlaneDistance = DefaultNearPlaneDistance,
        float farPlaneDistance = DefaultFarPlaneDistance, float fieldOfViewDegrees = DefaultFieldOfViewDegrees)
    {
        UpDirection = Vector3.Up;
        CenterXPosition = centerX;
        CenterYPosition = centerY;
        Radius = radius;
        HorizontalAngle = MathHelper.PiOver2;
        VerticalAngle = 0.3f;
        Vector3 offset = CalculateOffsetPosition();
        Position = position + offset;
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
    private void BuildView()
    {
        View = Matrix.CreateLookAt(Position, TargetPosition, UpDirection);
    }

    public void UpdateCamera(Vector3 targetPosition, int mousePositionX, int mousePositionY)
    {
        float sensitivity = 0.001f;
        int offsetX = mousePositionX - CenterXPosition;
        int offsetY = mousePositionY - CenterYPosition;
        // Ajusto el eje horizontal y vertical
        HorizontalAngle += offsetX * sensitivity;
        VerticalAngle += offsetY * sensitivity;

        VerticalAngle = MathHelper.Clamp(VerticalAngle, -MathHelper.PiOver2 + 0.1f, MathHelper.PiOver2 - 0.1f);

        // Creo el vector donde se va a ubicar la cámara, que es el borde de una esfera


        Vector3 Offset = CalculateOffsetPosition();
        Position = targetPosition + Offset;

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

    public Matrix ViewMatrix => View;
    public Matrix ProjectionMatrix => Projection;
    public float HorizontalAngle => HorizontalAngle;
}