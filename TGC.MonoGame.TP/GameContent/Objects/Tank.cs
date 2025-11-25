#region Using Statements
using System;
using System.Collections.Generic;
using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace TGC.MonoGame.TP;

public class Tank : GameObject
{
    private const float TankMaxSpeed = 10f; // Unidades por segundo
    private const float RotationSpeed = .5f; // Radianes por segundo
    private const float Acceleration = 5f; // Aceleracion del tanque
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
    private Texture2D _projectileTexture;
    private Texture2D _projectileNormal;
    private List<Projectile> _projectiles;
    private bool _isShooting;
    private float _life;
    private int _score;
    private int _kills;
    private bool _isPlayer;
    private EnemyAction _enemyAction;
    private SoundEffect _shootSound;
    private SoundEffect _deadSound;
    private SoundEffect _hitSound;
    public Tank(
        Model model,
        Vector3 position,
        float scale = 0.01f,
        float rotation = 0f,
        Texture2D texture = null
        )
    {
        _graphicsDevice = GameManager.GetGraphicsDevice();
        _model = model;
        _effect = model.Meshes[0].MeshParts[0].Effect;
        _position = position;
        _scale = scale;
        _rotation = MathHelper.ToRadians(rotation);
        _texture = texture;
        _life = InitialLife;
        _tankFrontDirection = Vector3.Transform(Vector3.Forward, Matrix.CreateRotationY(_rotation));
        _boneTransforms = new Matrix[model.Bones.Count];
        _velocity = 0f;
        _isMovingforward = true;
        _projectiles = new List<Projectile>();
        _wheels = new Wheels(_model);
        _wheels.SetWheelTexture(_texture);
        _wheels.SetTreadmillTexture(_texture);
        _turret = new Turret(_model);
        _turret.SetTurretTexture(_texture);
        _turret.SetCannonTexture(_texture);
        _meshes = new List<ModelMesh>();
        MeshesTanque();
        CreateBoundingBoxToDraw();
        CreateCollisionBox();
        _deadSound = ContentLoader.GetSoundEffect("dead");
        _hitSound = ContentLoader.GetSoundEffect("metal-hit2");
    }
    public new void SetTexture(Texture2D texture)
    {
        base.SetTexture(texture);
        _wheels.SetWheelTexture(texture);
        _wheels.SetTreadmillTexture(texture);
    }
    public new void SetNormal(Texture2D normal)
    {
        base.SetNormal(normal);
        _turret.SetTurretNormal(normal);
        _turret.SetCannonNormal(normal);
        _wheels.SetTreadmillNormal(normal);
        _wheels.SetWheelNormal(normal);
    }
    public void SetIsPlayer(bool isPlayer)
    {
        _isPlayer = isPlayer;
        if (!isPlayer)
            _enemyAction = new EnemyAction(this);
    }
    public bool GetIsPlayer() => _isPlayer;
    public bool GetIsShooting() => _isShooting;
    public void SetIsShooting(bool shoot) => _isShooting = shoot;
    public float GetLife() => _life;
    public void SetLife(float life) => _life = life;
    public float LifePercent() => _life / InitialLife;
    public int GetScore() => _score;
    public int GetKills() => _kills;
    public void SetTreadmillTexture(Texture2D texture) => _wheels.SetTreadmillTexture(texture);
    public void SetTreadmillNormal(Texture2D texture) => _wheels.SetTreadmillNormal(texture);
    public void SetScore(int score) => _score = score;
    public void SetKills(int kills) => _kills = kills;
    public void MoveForwardTank(GameTime gameTime)
    {
        GameManager.SetAwakeTrue(_bodyHandle);
        if (_isMovingforward || (!HasVelocity() && !_isMovingforward))
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _velocity += Acceleration * deltaTime;
            if (_velocity > TankMaxSpeed)
                _velocity = TankMaxSpeed;
            _isMovingforward = true;
        }
        else
            DecelerateTank(gameTime);
    }
    public void MoveBackwardTank(GameTime gameTime)
    {
        GameManager.SetAwakeTrue(_bodyHandle);
        if (!_isMovingforward || (!HasVelocity() && _isMovingforward))
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _velocity += Acceleration * deltaTime;
            if (_velocity > TankMaxSpeed)
                _velocity = TankMaxSpeed;
            _isMovingforward = false;
        }
        else
            DecelerateTank(gameTime);
    }
    public bool HasVelocity() => _velocity > 0;
    public void DecelerateTank(GameTime gameTime)
    {
        GameManager.SetAwakeTrue(_bodyHandle);
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _velocity -= Acceleration * deltaTime * 3f;
        if (_velocity < 0)
            _velocity = 0;
    }
    public void RotateTankRight(GameTime gameTime)
    {
        GameManager.SetAwakeTrue(_bodyHandle);
        if (_isMovingforward)
            RotateTank(gameTime, true);
        else
            RotateTank(gameTime, false);
    }
    public void RotateTankLeft(GameTime gameTime)
    {
        GameManager.SetAwakeTrue(_bodyHandle);
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
        Vector3 cannonPos = cannonWorld.Translation;
        Vector3 turretDirection = cannonWorld.Down;
        Vector3 cannonDirection = _turret.GetCannonDirection();
        if (_shootSound != null)
            _shootSound.Play();
        Projectile p = new Projectile(_projectileModel, cannonPos, cannonDirection, this, _projectileTexture, _projectileNormal);
        _projectiles.Add(p);
        return p;
    }
    public bool OwnProjectile(Projectile p) => _projectiles.Contains(p);
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
    public void SetProjectileTexture(Texture2D texture) => _projectileTexture = texture;
    public void SetProjectileNormal(Texture2D texture) => _projectileNormal = texture;
    public void ChangeSensitivity(float sensitivity) => _turret.ChangeSensitivity(sensitivity);
    public override void Update(GameTime gameTime)
    {
        // body de la colisión
        BodyReference body = GameManager.GetBodyReference(_bodyHandle);
        // pose del body, donde está la velocidad y la orientación
        var pose = body.Pose;
        // obtengo la dirección hacia donde quiere ir el tanque
        var direction = _tankFrontDirection * _velocity;
        if (!_isMovingforward)
            direction = -direction;
        // aplico la velocidad al cuerpo de la colisión
        body.Velocity.Linear = direction.ToNumerics();
        var position = pose.Position;
        // Normal del terreno bajo el tanque
        Vector3 terrainNormal = Land.GetTerrainNormal(position.X ,position.Z);

        // roto el tanque según el ángulo de _rotation, usando un quaternion
        Quaternion quaternion = Quaternion.CreateFromAxisAngle(Vector3.UnitY, _rotation);
        // transformo ese quaternion en matriz
        Matrix rotationMatrix = Matrix.CreateFromQuaternion(quaternion);
        // asigno esa horientación a la colisión
        pose.Orientation = quaternion.ToNumerics();
        // construyo la matriz de mundo según la escala, la rotación del cuaternión y la traslación
        _world = Matrix.CreateScale(_scale) * rotationMatrix * Matrix.CreateTranslation(_position);
        

        Vector3 forward = _tankFrontDirection;
        forward.Normalize();

        Vector3 right = Vector3.Cross(terrainNormal, forward);
        right.Normalize();

        forward = Vector3.Cross(right, terrainNormal);

        Matrix orientation = Matrix.CreateWorld(position, forward, terrainNormal);
  
        _world = Matrix.CreateScale(_scale) * orientation;
        position.Y = Land.Height(position.X, position.Z);
        body.Pose.Position.Y = position.Y + 0.9f;
        // seteo esa posición nueva a la variable _position, que me sirve para graficar el tanque
        _position = body.Pose.Position;

        UpdateBoundingBoxToDraw();

        // Recalcular transforms absolutos luego de modificar los locales
        _model.CopyAbsoluteBoneTransformsTo(_boneTransforms);

        _wheels.Update(_velocity, _isMovingforward);
        _turret.Update(_isPlayer);
        if (!_isPlayer)
            _enemyAction.Update(gameTime,GameManager.Instance);
    }
    public override void Draw(GameTime gameTime, Matrix view, Matrix projection)
    {        
        Vector3 specularColor = Color.White.ToVector3();
        Matrix inverseTransposeWorld = Matrix.Invert(Matrix.Transpose(_world));

        GameManager.SetIluminationParameters(
            _effect,
            inverseTransposeWorld, // este si es necesario, es propio de cada instancia de cada objeto
            specularColor // este si debería ser propio de cada objeto no?
        );

        _effect.Parameters["View"].SetValue(view);
        _effect.Parameters["Projection"].SetValue(projection);
        _effect.Parameters["Texture"]?.SetValue(_texture);
        _effect.Parameters["NormalTexture"]?.SetValue(_textureNormal);
        _effect.Parameters["TreadmillsOffset"].SetValue(0.0f);
        _effect.Parameters["DiffuseColor"]?.SetValue(Color.White.ToVector3());
        foreach (var mesh in _meshes)
        {
            var worldMesh = _boneTransforms[mesh.ParentBone.Index] * _world;
            _effect.Parameters["World"].SetValue(worldMesh);
            mesh.Draw();
        }

        _turret.Draw(_world, view, projection);
        _wheels.Draw(_world, view, projection);
    }
    public void SetShootSound(SoundEffect soundEffect) => _shootSound = soundEffect;
    public void AddKill() => _kills += 1;
    public void AddScore(int points) => _score += points;
    #region PRIVATE METHODS
    private void MeshesTanque()
    {
        foreach (var mesh in _model.Meshes)
        {
            if (!_wheels.ContainMesh(mesh.Name) && !_turret.ContainMesh(mesh.Name))
                _meshes.Add(mesh);
        }
    }
    private void CreateCollisionBox()
    {
        Box boxShape = new Box(_boxWidth, _boxHeight, _boxLength);
        var boxInertia = boxShape.ComputeInertia(0.1f);
        TypedIndex boxIndex = GameManager.AddShapeToSimulation(boxShape);
        CollidableDescription collidableDescription = new CollidableDescription(boxIndex, 0.1f);
        BodyActivityDescription bodyActivityDescription = new BodyActivityDescription(0.01f);
        var position = _position.ToNumerics();
        var bodyDescription = BodyDescription.CreateDynamic(
            position,
            boxInertia,
            collidableDescription,
            bodyActivityDescription
        );
        _bodyHandle = GameManager.AddBodyToSimulation(bodyDescription, this);
    }
    #endregion
    public void ChangeLife(float amount)
    {
        _life += amount;
        if (_life == 0)
        {
            _deadSound.Play();
            if (GetIsPlayer())
            {
                GameManager.SetState(GameState.Defeat);
                GameManager.SetWasDefeated(true);                
            }
        }
    }
    public void PlayHitSound()
    {
        var volume = Math.Clamp(GameManager.GetVolume(), 0f, 1f);
        if (_hitSound.Name == "metal-hit")
            volume = .01f;
        _hitSound.Play(volume, 0, 0);
    }
    public Vector3 GetTankFrontDirection() => _tankFrontDirection;
}