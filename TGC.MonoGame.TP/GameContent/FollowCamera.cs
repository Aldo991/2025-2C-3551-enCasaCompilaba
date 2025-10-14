using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace TGC.MonoGame.TP;

public class FollowCamera
{
    public const float DefaultFieldOfViewDegrees = MathHelper.PiOver4;
    public const float DefaultNearPlaneDistance = 1f;
    public const float DefaultFarPlaneDistance = 200000f;
    private const float RotationSpeed = 1.5f; // Radianes por segundo
    private Matrix View;
    private Matrix Projection;
    private Vector3 Position;
    private Vector3 TargetPosition;
    private Vector3 UpDirection;
    private int CenterXPosition;
    private int CenterYPosition;
    private float Radius;
    private float VerticalAngle;
    private float HorizontalAngle;
    private float Sensitivity;

    public FollowCamera(float aspectRatio,
        int centerX, int centerY, float radius, float sensitivity = 0.001f,
        float nearPlaneDistance = DefaultNearPlaneDistance,
        float farPlaneDistance = DefaultFarPlaneDistance, float fieldOfViewDegrees = DefaultFieldOfViewDegrees)
    {
        UpDirection = Vector3.Up;
        CenterXPosition = centerX;
        CenterYPosition = centerY;
        Radius = radius;
        HorizontalAngle = MathHelper.PiOver2;
        VerticalAngle = 0.3f;
        Sensitivity = sensitivity;
        BuildProjection(aspectRatio, nearPlaneDistance, farPlaneDistance, fieldOfViewDegrees);
        // BuildView();
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
        // Offset
        int offsetX = mousePositionX - CenterXPosition;
        int offsetY = mousePositionY - CenterYPosition;
        // Ajusto el eje horizontal y vertical teniendo en cuenta la sensibilidad
        HorizontalAngle += offsetX * Sensitivity;
        VerticalAngle += offsetY * Sensitivity;
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
}