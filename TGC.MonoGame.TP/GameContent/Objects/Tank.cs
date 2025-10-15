#region Using Statements

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace TGC.MonoGame.TP;

public class Tank : GameObject
{
    private const float TankMaxSpeed = 40f; // Unidades por segundo
    private const float RotationSpeed = 1.5f; // Radianes por segundo
    private const float Acceleration = 4f; // Aceleracion del tanque
    private Effect _effect;
    private Vector3 _tankFrontDirection;
    private Matrix[] _boneTransforms;
    private bool _isMovingforward;
    private float _velocity;
    private Model _projectileModel;
    private bool _isShooting;
    private float _life;
    private int _score;
    private ElementosLand _elementosLand;

    private BoundingBox CreateBoundingBox(Model model, Matrix world)
    {
        Vector3 min = Vector3.One * float.MaxValue;
        Vector3 max = Vector3.One * float.MinValue;

        foreach (var mesh in model.Meshes)
        {
            foreach (var meshPart in mesh.MeshParts)
            {
                var vertexBuffer = meshPart.VertexBuffer;
                var declaration = vertexBuffer.VertexDeclaration;
                var vertexSize = declaration.VertexStride;
                var vertexData = new byte[vertexBuffer.VertexCount * vertexSize];
                vertexBuffer.GetData(vertexData);

                for (int i = 0; i < vertexBuffer.VertexCount; i++)
                {
                    var position = new Vector3(
                        BitConverter.ToSingle(vertexData, i * vertexSize),
                        BitConverter.ToSingle(vertexData, i * vertexSize + 4),
                        BitConverter.ToSingle(vertexData, i * vertexSize + 8)
                    );
                    position = Vector3.Transform(position, world);

                    min = Vector3.Min(min, position);
                    max = Vector3.Max(max, position);
                }
            }
        }

        return new BoundingBox(min, max);
    }
    public Tank(
        Model model,
        Vector3 position,
        float scale = 1f,
        float rotation = 0f,
        Texture2D texture = null,
        ElementosLand elementosLand = null
        
        )
    {
        _model = model;
        _effect = model.Meshes[0].MeshParts[0].Effect;
        _position = position;
        _scale = scale;
        _rotation = MathHelper.ToRadians(rotation);
        _texture = texture;
        _elementosLand = elementosLand;
        _world = Matrix.CreateScale(_scale) * Matrix.CreateRotationY(_rotation) * Matrix.CreateTranslation(_position);
        _boundingBox = CreateBoundingBox(model, _world);
        _tankFrontDirection = Vector3.Transform(Vector3.Forward, Matrix.CreateRotationY(_rotation));
        _boneTransforms = new Matrix[model.Bones.Count];
        _velocity = 0f;
        _isMovingforward = true;
        _boneTransforms = new Matrix[model.Bones.Count];
        _collisionRadius = 60f; // Set collision radius for tank
        
    } 
    public Model Model => _model; 
    public Vector3 Position => _position;
    public float Scale => _scale;
    public float Rotation => _rotation;
    
    
    public bool IsShooting
    {
        get => _isShooting;
        set => _isShooting = value;
    }
    public float Life
    {
        get => _life;
        set => _life = value;
    }
    public int Score
    {
        get => _score;
        set => _score = value;
    }
    public void MoveForwardTank(GameTime gameTime)
    {
        if (_isMovingforward || (!HasVelocity() && !_isMovingforward))
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _velocity += Acceleration * deltaTime;
            if (_velocity > TankMaxSpeed)
                _velocity = TankMaxSpeed;
            var newPosition = _tankFrontDirection * _velocity;
                _position += _tankFrontDirection * _velocity;
                _isMovingforward = true;
            if (_elementosLand!= null && _elementosLand.CheckCollisionMesh(this, newPosition))
            {
                
            }
            else
                _velocity = 10;
        }
        else
            DecelerateTank(gameTime);
    }
    public void MoveBackwardTank(GameTime gameTime)
    {
        if (!_isMovingforward || (!HasVelocity() && _isMovingforward))
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _velocity += Acceleration * deltaTime;
            if (_velocity > TankMaxSpeed)
                _velocity = TankMaxSpeed;
            _position -= _tankFrontDirection * _velocity;
            _isMovingforward = false;
        }
        else
            DecelerateTank(gameTime);
    }
    public bool HasVelocity()
    {
        return _velocity > 0;
    }
    public void DecelerateTank(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _velocity -= Acceleration * deltaTime;
        if (_velocity < 0)
            _velocity = 0;
        if (_isMovingforward)
            _position += _tankFrontDirection * _velocity;
        else
            _position -= _tankFrontDirection * _velocity;
    }
    public void RotateTankRight(GameTime gameTime)
    {
        RotateTank(gameTime, true);
    }
    public void RotateTankLeft(GameTime gameTime)
    {
        RotateTank(gameTime, false);
    }
    private void RotateTank(GameTime gameTime, bool right)
    {
        if (_velocity > 0)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (right)
                _rotation -= RotationSpeed * deltaTime;
            else
                _rotation += RotationSpeed * deltaTime;
            _tankFrontDirection = Vector3.Transform(Vector3.Forward, Matrix.CreateRotationY(_rotation));
        }
    }
    public Projectile Shoot()
    {
        ModelBone turretBone = _model.Bones[31];
        Matrix turretWorld = _boneTransforms[turretBone.Index];
        Matrix turretWorldPos = turretWorld * _world;
        Vector3 turretPos = turretWorldPos.Translation;
        Vector3 turretDirection = turretWorldPos.Up;
        // Console.WriteLine($"{turretDirection} dirección de la torreta");
        return new Projectile(_projectileModel, turretPos, turretDirection);
    }
    public Model ProjectileModel
    {
        get => _projectileModel;
        set => _projectileModel = value;
    }
    public void SetProjectileModel(Model model)
    {
        _projectileModel = model;
    }
    public override void Update(GameTime gameTime)
    {
        _model.CopyAbsoluteBoneTransformsTo(_boneTransforms);
        _world = Matrix.CreateScale(_scale) * Matrix.CreateRotationY(_rotation) * Matrix.CreateTranslation(_position);
        _boundingBox = CreateBoundingBox(_model, _world);
    }
    public override void Draw(GameTime gameTime, Matrix view, Matrix projection)
    {
        // Set the View and Projection matrices, needed to draw every 3D model.
        _effect.Parameters["View"].SetValue(view);
        _effect.Parameters["Projection"].SetValue(projection);
        _effect.Parameters["DiffuseColor"].SetValue(Color.GreenYellow.ToVector3());
        var modelMeshesBaseTransform = new Matrix[_model.Bones.Count];
        _model.CopyAbsoluteBoneTransformsTo(modelMeshesBaseTransform);
        foreach (var mesh in _model.Meshes)
        {
            var relativeTransform = modelMeshesBaseTransform[mesh.ParentBone.Index];
            _effect.Parameters["World"].SetValue(relativeTransform * _world);
            if (_texture != null)
                _effect.Parameters["Texture"].SetValue(_texture);
            mesh.Draw();
        }
    }
}