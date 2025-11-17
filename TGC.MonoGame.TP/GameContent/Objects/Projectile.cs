#region Using Statements
using System;
using System.Collections.Generic;
using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.Samples.Geometries;
#endregion

namespace TGC.MonoGame.TP;

public class Projectile : GameObject
{
    private Effect _effect;
    private Vector3 _direction;
    private float _speed;
    private float _lifeTime;
    private float _elapsedTime;
    private bool _isActive;
    private Vector3 _initialPosition;
    private SpherePrimitive spherePrimitive;
    private float sphereRadius;
    private Matrix sphereMatrix;

    public Projectile(Model model, Vector3 startPosition, Vector3 direction, Texture2D texture = null,
        float speed = 100f, float lifetime = 3f, float scale = 0.00025f, float rotation = 0f)
    {
        _model = model;
        _effect = model.Meshes[0].MeshParts[0].Effect;
        _direction = Vector3.Normalize(direction);
        _position = startPosition + (new Vector3(1f, 1f, 1f) * _direction);
        _speed = speed;
        _lifeTime = lifetime;
        _elapsedTime = 0;
        _isActive = true;
        _rotation = rotation;
        _texture = texture;
        _initialPosition = startPosition;
        _scale = scale;
        sphereRadius = .2f;
        spherePrimitive = new SpherePrimitive(GameManager.GetGraphicsDevice());
        CreateCollisionSphere();
    }
    private void CreateCollisionSphere()
    {
        Sphere sphereShape = new Sphere(sphereRadius);
        var sphereInertia = sphereShape.ComputeInertia(0.1f);
        TypedIndex sphereIndex = GameManager.AddShapeSphereToSimulation(sphereShape);
        CollidableDescription collidableDescription = new CollidableDescription(sphereIndex, 0.1f);
        BodyActivityDescription bodyActivityDescription = new BodyActivityDescription(0.01f);
        var position = _position.ToNumerics();

        var bodyDescription = BodyDescription.CreateDynamic(
            position,
            sphereInertia,
            collidableDescription,
            bodyActivityDescription
        );
        _bodyHandle = GameManager.AddBodyToSimulation(bodyDescription);
    }
    public bool IsActive() => _isActive;
    private Vector3 CalculateDirection()
    {
        var directionX = _direction.X * _speed;
        var directionY = _direction.Y * _speed;
        var directionZ = _direction.Z * _speed;
        return new Vector3(directionX, directionY, directionZ);
    }
    public override void Update(GameTime gameTime)
    {
        BodyReference body = GameManager.GetBodyReference(_bodyHandle);
        body.Awake = true;
        var pose = body.Pose;

        var direction = CalculateDirection();

        body.Velocity.Linear = direction.ToNumerics();

        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _position = pose.Position;
        _rotation += deltaTime;
        _elapsedTime += deltaTime;
        if (_elapsedTime > _lifeTime)
            _isActive = false;
        
        // roto el tanque según el ángulo de _rotation, usando un quaternion
        Quaternion quaternion = Quaternion.CreateFromAxisAngle(Vector3.UnitX, _rotation);
        // transformo ese quaternion en matriz
        Matrix rotationMatrix = Matrix.CreateFromQuaternion(quaternion);
        // asigno esa horientación a la colisión
        pose.Orientation = quaternion.ToNumerics();
        // construyo la matriz de mundo según la escala, la rotación del cuaternión y la traslación
        _world = Matrix.CreateScale(_scale) * rotationMatrix * Matrix.CreateTranslation(_position);
        // _world = Matrix.CreateScale(_scale) * Matrix.CreateRotationX(_rotation) * Matrix.CreateTranslation(_position);

        sphereMatrix = Matrix.CreateScale(sphereRadius) * rotationMatrix * Matrix.CreateTranslation(_position);
    }
    public override void Draw(GameTime gameTime, Matrix view, Matrix projection)
    {
        // spherePrimitive.Draw(sphereMatrix, view, projection);
        if (!_isActive)
            return;

        // Set the View and Projection matrices, needed to draw every 3D model.
        _effect.Parameters["View"].SetValue(view);
        _effect.Parameters["Projection"].SetValue(projection);
        _effect.Parameters["DiffuseColor"]?.SetValue(Color.Green.ToVector3());
        _effect.Parameters["Texture"]?.SetValue(_texture);
        foreach (var mesh in _model.Meshes)
        {
            _effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * _world);
            mesh.Draw();
        }
    }
}