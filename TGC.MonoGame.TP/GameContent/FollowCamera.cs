#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Vector3 = Microsoft.Xna.Framework.Vector3;
#endregion

namespace TGC.MonoGame.TP;

public class FollowCamera
{
    // Constantes de la cámara
    public const float DefaultFieldOfViewDegrees = MathHelper.PiOver4;
    public const float DefaultNearPlaneDistance = 1f;
    public const float DefaultFarPlaneDistance = 2000f;
    public const float OrbitAngularSpeed = 0.35f;
    public const float OrbitVerticalAngle = 0.25f;
    // Matriz de vista y proyección de la cámara
    private Matrix View;
    private Matrix Projection;
    // Estado
    private Vector3 Position;
    private Vector3 TargetPosition;
    private Vector3 UpDirection = Vector3.Up;
    // Para optimización
    private BoundingFrustum _boundingFrustum;

    private float Radius;
    private float VerticalAngle;
    private float HorizontalAngle;
    private float Sensitivity;

    public FollowCamera(
        float aspectRatio,
        float radius, float sensitivity = 0.001f,
        float nearPlaneDistance = DefaultNearPlaneDistance,
        float farPlaneDistance = DefaultFarPlaneDistance,
        float fieldOfViewDegrees = DefaultFieldOfViewDegrees)
    {
        Radius = radius;
        HorizontalAngle = MathHelper.PiOver2;
        VerticalAngle = 0.3f;
        Sensitivity = sensitivity;
        BuildProjection(aspectRatio, nearPlaneDistance, farPlaneDistance, fieldOfViewDegrees);
        _boundingFrustum = new BoundingFrustum(View * Projection);
    }
    // Construye la proyección de la cámara
    public void BuildProjection(float aspectRatio, float nearPlaneDistance, float farPlaneDistance,
        float fieldOfViewDegrees)
    {
        Projection = Matrix.CreatePerspectiveFieldOfView(
            MathHelper.ToRadians(fieldOfViewDegrees), aspectRatio, nearPlaneDistance, farPlaneDistance);
    }
    // Construtye la vista de la cámara
    private void BuildView() => View = Matrix.CreateLookAt(Position, TargetPosition, UpDirection);
    // Calcula el offset de la posición, estaba pensado para usarlo como una cámara que orbita el
    // tanque usando el mouse
    private Vector3 CalculateOffsetPosition()
    {
        float x = Radius * (float)(Math.Cos(HorizontalAngle) * Math.Cos(VerticalAngle));
        float y = Radius * (float)Math.Sin(VerticalAngle);
        float z = Radius * (float)(Math.Sin(HorizontalAngle) * Math.Cos(VerticalAngle));
        return new Vector3(x, y, z);
    }
    private Vector3 CalculateOffsetPosition(Vector3 direction)
    {
        direction *= Radius;
        direction.Y += 200f;
        return direction;
    }
    // Este método setea la cámara detrás el tanque cuando se está jugando. Sería como el "Update"
    // de la cámara cuando el modo de juego es "Playing"
    public void SetCameraDirection(Vector3 target, Vector3 direction)
    {
        Vector3 offset = CalculateOffsetPosition(direction);
        Position =  target + offset;
        TargetPosition = target;
        BuildView();
        _boundingFrustum.Matrix = View * Projection;
    }
    // Este método hace que la cámara orbite el tanque cuando el juego está en pausa. Sería como
    // el "Update" de la cámara cuando el modo de juego no es "Playing"
    public void UpdateOrbitAuto(
        Vector3 targetPosition,
        GameTime gameTime,
        float orbitAngularSpeed = OrbitAngularSpeed,
        float orbitVerticalAngle = OrbitVerticalAngle
    )
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        HorizontalAngle += orbitAngularSpeed * dt;
        VerticalAngle = orbitVerticalAngle;
        var offset = CalculateOffsetPosition();
        Position = targetPosition + offset;
        TargetPosition = targetPosition;
        BuildView();
        _boundingFrustum.Matrix = View * Projection;
    }
    public bool IsOnCamera(BoundingBox box) => _boundingFrustum.Intersects(box);
    public bool IsOnCamera(BoundingSphere sphere) => _boundingFrustum.Intersects(sphere);
    public Vector3 GetCameraPosition() => Position;
    public Matrix ViewMatrix => View;
    public Matrix ProjectionMatrix => Projection;
}