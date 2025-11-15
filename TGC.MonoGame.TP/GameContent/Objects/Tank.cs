#region Using Statements
using System.Collections.Generic;
using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
// using TGC.MonoGame.Samples.Geometries.Textures;
using BoundingBox = BepuUtilities.BoundingBox;
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
    private bool _isShooting;
    private float _life;
    private int _score;
    private bool _isPlayer;
    private EnemyAction _enemyAction;
    private Song _shootSound;
    private BoxPrimitive boxPrimitive;
    private Matrix boxWorld;
    private bool mostrarCaja;
    private float altoCaja;
    private float anchoCaja;
    private float profundidadCaja;
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
        _wheels = new Wheels(_model);
        _wheels.SetWheelTexture(_texture);
        _wheels.SetTreadmillTexture(_texture);
        _turret = new Turret(_model);
        _meshes = new List<ModelMesh>();
        MeshesTanque();
        anchoCaja = 2f;
        altoCaja = 1.5f;
        profundidadCaja = 3.5f;
        mostrarCaja = true;
        Vector3 boxSize = new Vector3(anchoCaja, altoCaja, profundidadCaja);
        Texture2D boxTexture = ContentLoader.GetTexture("house", 3);
        boxPrimitive = new BoxPrimitive(_graphicsDevice, boxSize, boxTexture);
        CreateCollisionBox();
    }
    public new void SetTexture(Texture2D texture)
    {
        base.SetTexture(texture);
        _wheels.SetWheelTexture(texture);
        _wheels.SetTreadmillTexture(texture);
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
        Vector3 turretPos = cannonWorld.Translation;
        Vector3 turretDirection = cannonWorld.Down;
        Vector3 cannonDirection = _turret.GetCannonDirection();
        if (_shootSound != null)
            MediaPlayer.Play(_shootSound);
        return new Projectile(_projectileModel, turretPos, turretDirection, _projectileTexture);
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
    public void SetProjectileTexture(Texture2D texture) => _projectileTexture = texture;
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
        // seteo esa posición nueva a la variable _position, que me sirve para graficar el tanque
        _position = pose.Position;

        // roto el tanque según el ángulo de _rotation, usando un quaternion
        Quaternion quaternion = Quaternion.CreateFromAxisAngle(Vector3.UnitY, _rotation);
        // transformo ese quaternion en matriz
        Matrix rotationMatrix = Matrix.CreateFromQuaternion(quaternion);
        // asigno esa horientación a la colisión
        pose.Orientation = quaternion.ToNumerics();
        // construyo la matriz de mundo según la escala, la rotación del cuaternión y la traslación
        _world = Matrix.CreateScale(_scale) * rotationMatrix * Matrix.CreateTranslation(_position);

        // BORRAR, ESTO ES PARA VER LA CAJA QUE SIMULA LA COLISIÓN
        boxWorld = rotationMatrix * Matrix.CreateTranslation(_position);

        Vector3 boxSize = new Vector3(anchoCaja, altoCaja, profundidadCaja);
        Texture2D boxTexture = ContentLoader.GetTexture("house", 3);
        boxPrimitive = new BoxPrimitive(_graphicsDevice, boxSize, boxTexture);

        // Recalcular transforms absolutos luego de modificar los locales
        _model.CopyAbsoluteBoneTransformsTo(_boneTransforms);

        _wheels.Update(gameTime, _velocity);
        _turret.Update(_isPlayer);
        if (!_isPlayer)
            _enemyAction.Update(gameTime,GameManager.Instance);
    }
    public override void Draw(GameTime gameTime, Matrix view, Matrix projection)
    {
        if (mostrarCaja)
            boxPrimitive.Draw(boxWorld, view, projection);
        
        Vector3 ambientColor = Color.Yellow.ToVector3();
        Vector3 specularColor = Color.White.ToVector3();
        float kAmbient = 0.2f;
        float KDiffuse = 0.6f;
        float KSpecular = 0.2f;
        float shininess = 15f;
        Vector3 lightPosition = new Vector3(1000, 100, 1000);
        Vector3 eyePosition = GameManager.GetCameraPosition();
        Matrix inverseTransposeWorld = Matrix.Invert(Matrix.Transpose(_world));

        _effect.Parameters["EyePosition"].SetValue(eyePosition);
        _effect.Parameters["InverseTransposeWorld"].SetValue(inverseTransposeWorld);
        _effect.Parameters["LightPosition"].SetValue(lightPosition);
        _effect.Parameters["AmbientColor"].SetValue(ambientColor);
        _effect.Parameters["SpecularColor"].SetValue(specularColor);
        _effect.Parameters["KAmbient"].SetValue(kAmbient);
        _effect.Parameters["KDiffuse"].SetValue(KDiffuse);
        _effect.Parameters["KSpecular"].SetValue(KSpecular);
        _effect.Parameters["Shininess"].SetValue(shininess);

        _effect.Parameters["View"].SetValue(view);
        _effect.Parameters["Projection"].SetValue(projection);
        _effect.Parameters["Texture"]?.SetValue(_texture);
        _effect.Parameters["TreadmillsOffset"].SetValue(0.0f);
        _effect.Parameters["DiffuseColor"]?.SetValue(Color.White.ToVector3());
        
        if (_textureNormal != null)
            _effect.Parameters["Normals"]?.SetValue(_textureNormal);
        foreach (var mesh in _meshes)
        {
            var worldMesh = _boneTransforms[mesh.ParentBone.Index] * _world;
            _effect.Parameters["World"].SetValue(worldMesh);
            mesh.Draw();
        }

        _turret.Draw(_world, view, projection);
        _wheels.Draw(_world, view, projection);
    }
    public void SetShootSound(Song soundEffect) => _shootSound = soundEffect;
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
        // Box boxShape = new Box(boxWidht, boxHeight, boxLength);
        Box boxShape = new Box(anchoCaja, altoCaja, profundidadCaja);
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
        _bodyHandle = GameManager.AddBodyToSimulation(bodyDescription);
    }
    #endregion

    /// BORRAR
    public void CambiarVida(float cantidad) => _life += cantidad;
    public void CambiarCaja() => mostrarCaja = !mostrarCaja;
    public void CambiarTamanioCaja(float cant) => altoCaja += cant;
    public Vector3 GetTankFrontDirection() => _tankFrontDirection;
    public float GetAltoCaja() => altoCaja;
}