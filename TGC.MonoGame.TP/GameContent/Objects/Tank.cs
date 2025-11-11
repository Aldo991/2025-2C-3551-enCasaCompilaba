#region Using Statements
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using TGC.MonoGame.Samples.Collisions;
#endregion

namespace TGC.MonoGame.TP;

public class Tank : GameObject
{
    private const float TankMaxSpeed = .2f; // Unidades por segundo
    private const float RotationSpeed = .5f; // Radianes por segundo
    private const float Acceleration = .1f; // Aceleracion del tanque
    private const float InitialLife = 100f;
    public static float DefaultScale = 0.005f;
    private Effect _effect;
    private GraphicsDevice _graphicsDevice;
    private Vector3 _tankFrontDirection;
    private Wheels _wheels;
    private Turret _turret;
    private List<ModelMesh> _meshes;
    private Matrix[] _boneTransforms;
    private bool _isMovingforward;
    private float _velocity;
    private Model _projectileModel;
    private bool _isShooting;
    private float _life;
    private int _score;
    private bool _isPlayer;
    private EnemyAction _enemyAction;
    private Song _shootSound;
    /*
    // (revertido) sin offset adicional para el heading de la torreta
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
    */
    public Tank(
        Model model,
        Vector3 position,
        float scale = 0.01f,
        float rotation = 0f,
        Texture2D texture = null
        )
    {
        _model = model;
        _effect = model.Meshes[0].MeshParts[0].Effect;
        _position = position;
        _scale = scale;
        _rotation = MathHelper.ToRadians(rotation);
        _texture = texture;
        _life = InitialLife;
        _world = Matrix.CreateScale(_scale) * Matrix.CreateRotationY(_rotation) * Matrix.CreateTranslation(_position);
        _boundingBox = BoundingVolumesExtensions.CreateAABBFrom(model);
        _tankFrontDirection = Vector3.Transform(Vector3.Forward, Matrix.CreateRotationY(_rotation));
        _boneTransforms = new Matrix[model.Bones.Count];
        _velocity = 0f;
        _isMovingforward = true;
        _collisionRadius = 60f; // Set collision radius for tank
        _wheels = new Wheels(_model);
        _wheels.SetWheelTexture(_texture);
        _wheels.SetTreadmillTexture(_texture);
        _turret = new Turret(_model);
        _meshes = new List<ModelMesh>();
        MeshesTanque();
    }
    public new void SetTexture(Texture2D texture)
    {
        base.SetTexture(texture);
        _wheels.SetWheelTexture(texture);
        _wheels.SetTreadmillTexture(_texture);
    }
    public void SetIsPlayer(bool isPlayer)
    {
        _isPlayer = isPlayer;
        if (!isPlayer)
            _enemyAction = new EnemyAction(this);
    }
    public void SetGraphicsDevice(GraphicsDevice graphicsDevice) => _graphicsDevice = graphicsDevice;
    public bool GetIsShooting() => _isShooting;
    public void SetIsShooting(bool shoot) => _isShooting = shoot;
    public float GetLife() => _life;
    public void SetLife(float life) => _life = life;
    public float LifePercent() => _life / InitialLife;
    public int GetScore() => _score;
    public void SetScore(int score) => _score = score;
    public void MoveForwardTank(GameTime gameTime)
    {
        if (_isMovingforward || (!HasVelocity() && !_isMovingforward))
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _velocity += Acceleration * deltaTime;
            if (_velocity > TankMaxSpeed)
                _velocity = TankMaxSpeed;
            _position += _tankFrontDirection * _velocity;
            _isMovingforward = true;
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
    public bool HasVelocity() => _velocity > 0;
    public void DecelerateTank(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _velocity -= Acceleration * deltaTime * 30f;
        if (_velocity < 0)
            _velocity = 0;
        if (_isMovingforward)
            _position += _tankFrontDirection * _velocity;
        else
            _position -= _tankFrontDirection * _velocity;
    }
    public void RotateTankRight(GameTime gameTime)
    {
        if (_isMovingforward)
            RotateTank(gameTime, true);
        else
            RotateTank(gameTime, false);
    }
    public void RotateTankLeft(GameTime gameTime)
    {
        if (_isMovingforward)
            RotateTank(gameTime, false);
        else
            RotateTank(gameTime, true);
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
        Matrix cannonBoneTraslation = _turret.GetCannonTraslation();
        Matrix cannonWorld = cannonBoneTraslation * _world;
        Vector3 turretPos = cannonWorld.Translation;
        Vector3 turretDirection = cannonWorld.Down;
        if (_shootSound != null)
            MediaPlayer.Play(_shootSound);
        return new Projectile(_projectileModel, turretPos, turretDirection);
    }
    public void SetGround(ElementosLand elementos)
    {
        float gy = elementos.SampleGroundHeight(_position.X, _position.Z);
    }
    public Vector3 GetCannonDirection()
    {
        Matrix cannonBoneTraslation = _turret.GetCannonTraslation();
        Matrix cannonWorld = cannonBoneTraslation * _world;
        var cannonDirection = cannonWorld.Up;
        cannonDirection.Normalize();
        return cannonDirection;
    }
    public Vector3 GetCannonPosition()
    {
        Matrix cannonBoneTraslation = _turret.GetCannonTraslation();
        Matrix cannonWorld = cannonBoneTraslation * _world;
        return cannonWorld.Translation;
    }
    public void SetProjectileModel(Model model) => _projectileModel = model;
    public override void Update(GameTime gameTime)
    {
        _world = Matrix.CreateScale(_scale) * Matrix.CreateRotationY(_rotation) * Matrix.CreateTranslation(_position);
        // _boundingBox = CreateBoundingBox(_model, _world);

        // Recalcular transforms absolutos luego de modificar los locales
        _model.CopyAbsoluteBoneTransformsTo(_boneTransforms);

        _wheels.Update(gameTime, _velocity);
        _turret.Update(_isPlayer);
        if (!_isPlayer)
        {
            _enemyAction.Update(gameTime,GameManager.Instance);
        }
    }
    public override void Draw(GameTime gameTime, Matrix view, Matrix projection)
    {
        var modelTransforms = new Matrix[_model.Bones.Count];
        _model.CopyAbsoluteBoneTransformsTo(modelTransforms);

        _effect.Parameters["View"].SetValue(view);
        _effect.Parameters["Projection"].SetValue(projection);
        _effect.Parameters["Texture"]?.SetValue(_texture);
        _effect.Parameters["TreadmillsOffset"].SetValue(0.0f);
        _effect.Parameters["DiffuseColor"]?.SetValue(Color.White.ToVector3());
        foreach (var mesh in _meshes)
        {
            var worldMesh = modelTransforms[mesh.ParentBone.Index] * _world;
            _effect.Parameters["World"].SetValue(worldMesh);
            mesh.Draw();
        }
        _turret.Draw(_world, view, projection);
        _wheels.Draw(_world, view, projection);
    }
    public void SetShootSound(Song soundEffect) => _shootSound = soundEffect;
    public void MeshesTanque()
    {
        foreach (var mesh in _model.Meshes)
        {
            if (!_wheels.ContainMesh(mesh.Name) && !_turret.ContainMesh(mesh.Name))
                _meshes.Add(mesh);
        }
    }
    /// BORRAR
    public void CambiarVida(float cantidad) => _life += cantidad;
    public Vector3 GetTankFrontDirection() => _tankFrontDirection;
}