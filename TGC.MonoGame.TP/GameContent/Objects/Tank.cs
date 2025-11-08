#region Using Statements
using System;
using System.Collections.Generic;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
#endregion

namespace TGC.MonoGame.TP;

public class Tank : GameObject
{
    private const float TankMaxSpeed = 4f; // Unidades por segundo
    private const float RotationSpeed = 1.5f; // Radianes por segundo
    private const float Acceleration = .2f; // Aceleracion del tanque
    private const float InitialLife = 100f;
    public static float DefaultScale = 0.01f;
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
    // Estos atributos de abajo los seteo antes de hacer Tank.Update(), y es la posición en
    // X e Y del mouse en la pantalla. Lo hago antes de hacer Tank.Update() para no romper
    // la interfaz de Update(GameTime)
    private int _mouseX;
    private int _mouseY;
    private float _cameraHorizontalAngle;
    private Vector3 _previousPosition;
    private float _groundOffset = 0.0f;
    private Song _shootSound;
    // Rotaciones relativas de torreta y cañón
    // Límites de elevación del cañón
    private static readonly float GunPitchMin = MathHelper.ToRadians(-10f);
    private static readonly float GunPitchMax = MathHelper.ToRadians(+20f);
    private bool _modelZUp = false;               // si el modelo está en Z-up (en vez de Y-up)

    // Cámara “pegada” al cañón
    public Matrix ViewFromGun;
    public Vector3 CameraPositionFromGun;
    private Vector3 _turretDirection;
    public bool ModelZUp
    {
        get => _modelZUp;
        set => _modelZUp = value;
    }

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
    public Vector3 GetTurretDirection() => _turretDirection;
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
        _boundingBox = CreateBoundingBox(model, _world);
        _tankFrontDirection = Vector3.Transform(Vector3.Forward, Matrix.CreateRotationY(_rotation));
        _turretDirection = _tankFrontDirection;
        _turretDirection.Normalize();
        _boneTransforms = new Matrix[model.Bones.Count];
        _velocity = 0f;
        _isMovingforward = true;
        _previousPosition = position;
        _collisionRadius = 60f; // Set collision radius for tank
        _wheels = new Wheels(_model);
        _turret = new Turret(_model);
        _meshes = new List<ModelMesh>();
        MeshesTanque();
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
    public void SetOffsetXY(int mouseX, int mouseY)
    {
        _mouseX = mouseX;
        _mouseY = mouseY;
    }
    public void SetCameraHorizontalAngle(float horizontal) => _cameraHorizontalAngle = horizontal;
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
        ModelBone cannonBone = _turret.GetCannonBone();
        Matrix cannonBoneTransform = _boneTransforms[cannonBone.Index];
        Matrix cannonWorld = cannonBoneTransform * _world;
        Vector3 turretPos = cannonWorld.Translation;
        Vector3 turretDirection = -cannonWorld.Up;
        _turretDirection = turretDirection;
        if (_shootSound != null)
            MediaPlayer.Play(_shootSound);
        return new Projectile(_projectileModel, turretPos, turretDirection);
    }
    public void SetGround(ElementosLand elementos)
    {
        float gy = elementos.SampleGroundHeight(_position.X, _position.Z);
        _groundOffset = _position.Y - gy;
    }
    public void SetProjectileModel(Model model) => _projectileModel = model;
    // Dump de jerarquía: lista bones y meshes para identificar nombres
    /*
    public void DumpRig(string filePath = "rig_dump.txt")
    {
        try
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("=== BONES ===");
            for (int i = 0; i < _model.Bones.Count; i++)
            {
                var b = _model.Bones[i];
                var parent = b.Parent;
                sb.AppendLine($"[{i}] Bone='{b.Name}' Parent={(parent != null ? parent.Index : -1)} ParentName='{parent?.Name}'");
            }
            sb.AppendLine();
            sb.AppendLine("=== MESHES ===");
            foreach (var mesh in _model.Meshes)
            {
                var pb = mesh.ParentBone;
                sb.AppendLine($"Mesh='{mesh.Name}' ParentBoneIndex={pb.Index} ParentBoneName='{pb.Name}'");
            }
            File.WriteAllText(filePath, sb.ToString());
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"DumpRig failed: {ex.Message}");
        }
    }
    */

    /*
    // Permite configurar torreta/cañón por nombre exacto/parcial del mesh
    public bool ConfigureRigByMeshNames(string turretMeshName, string gunMeshName = null)
    {
        int FindMeshParentBone(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return -1;
            foreach (var mesh in _model.Meshes)
            {
                if ((mesh.Name ?? string.Empty).IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0)
                    return mesh.ParentBone.Index;
            }
            return -1;
        }

        int t = FindMeshParentBone(turretMeshName);
        if (t < 0 || t >= _model.Bones.Count) return false;
        _turretBone = t;

        int g = FindMeshParentBone(gunMeshName);
        if (g < 0 || g >= _model.Bones.Count)
        {
            // intentar como hijo de la torreta por nombre
            g = FindBoneIndex("gun", "cannon", "canon", "cañon", "barrel");
        }
        _gunBone = (g >= 0 && g < _model.Bones.Count) ? g : -1;
        return true;
    }
    */
    public override void Update(GameTime gameTime)
    {
        _world = Matrix.CreateScale(_scale) * Matrix.CreateRotationY(_rotation) * Matrix.CreateTranslation(_position);
        _boundingBox = CreateBoundingBox(_model, _world);

        // Recalcular transforms absolutos luego de modificar los locales
        _model.CopyAbsoluteBoneTransformsTo(_boneTransforms);

        // 5) Construir la cámara enganchada al cañón: posición “tras y arriba” del mantelete
        /*
        var turretAbs = _boneTransforms[_turretBone] * _world;
        Matrix gunAbs;
        if (_gunBone >= 0 && _gunBone < _boneTransforms.Length)
            gunAbs = _boneTransforms[_gunBone] * _world;
        else
            gunAbs = turretAbs;
        _gunWorldAbs = gunAbs;

        */
        _wheels.Update(gameTime, _velocity);
        _turret.Update(_cameraHorizontalAngle, _rotation);
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
        /*
        foreach (var mesh in _meshes)
        {
            var worldMesh = modelTransforms[mesh.ParentBone.Index] * _world;
            _effect.Parameters["World"].SetValue(worldMesh);
            mesh.Draw();
        }
        */
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
    /// BORRAR
    public void CambiarVida(float cantidad) => _life += cantidad;
    public void CambiarY(float y) => _position.Y += y;
    public void MeshesTanque()
    {
        foreach (var mesh in _model.Meshes)
        {
            if (!_wheels.ContainMesh(mesh.Name) && !_turret.ContainMesh(mesh.Name))
                _meshes.Add(mesh);
        }
    }
}