#region Using Statements
using System;
using System.Collections.Generic;
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
    private Effect _effect;
    private Vector3 _tankFrontDirection;
    private Wheels _wheels;
    private List<ModelMesh> _meshes;
    private Matrix[] _boneTransforms;
    private bool _isMovingforward;
    private float _velocity;
    private Model _projectileModel;
    private bool _isShooting;
    private float _life;
    private int _score;
    // private ElementosLand _elementosLand;
    private Vector3 _previousPosition;
    private float _groundOffset = 0.0f;
    private Song _shootSound;
    // private readonly int _turretBoneIndex = 31;   // torreta (lo usabas para disparar)
    // private readonly int _gunBoneIndex = 32;   // cañón (el hueso hijo que eleva el cañón)
    // Rotaciones relativas de torreta y cañón
    private float _turretYaw = 0f;                // rotación Y local respecto al casco
    private float _gunPitch = 0f;                // rotación X local del cañón
    // Límites de elevación del cañón
    private static readonly float GunPitchMin = MathHelper.ToRadians(-10f);
    private static readonly float GunPitchMax = MathHelper.ToRadians(+20f);
    // Transforms originales de cada bone (para no acumular errores)
    private Matrix[] _bindPose;                   // transform local original de cada bone
    private Matrix _gunWorldAbs;                  // último transform absoluto del cañón
    private List<int> _turretGroup;               // huesos que giran con la torreta
    private bool _modelZUp = false;               // si el modelo está en Z-up (en vez de Y-up)

    // Cámara “pegada” al cañón
    public Matrix ViewFromGun;
    public Vector3 CameraPositionFromGun;
    private Vector3 _turretDirection;
    public Matrix GunWorldAbs => _gunWorldAbs;
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

    // Índices resueltos de torreta/cañón y helper para encontrarlos por nombre
    private int _turretBone;  // fallback a _turretBoneIndex si no se resuelve por nombre
    private int _gunBone;     // puede ser -1 si el modelo no tiene cañón separado
    public Vector3 GetTurretDirection() => _turretDirection;
    private int FindBoneIndex(params string[] names)
    {
        if (_model?.Bones == null) return -1;
        for (int i = 0; i < _model.Bones.Count; i++)
        {
            var name = _model.Bones[i].Name ?? string.Empty;
            var ln = name.ToLowerInvariant();
            foreach (var q in names)
            {
                if (string.IsNullOrWhiteSpace(q)) continue;
                if (ln.Contains(q.ToLowerInvariant())) return i;
            }
        }
        return -1;
    }
    public Tank(
        Model model,
        Vector3 position,
        float scale = 1f,
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
        _boneTransforms = new Matrix[model.Bones.Count];
        _bindPose = new Matrix[model.Bones.Count];
        for (int i = 0; i < model.Bones.Count; i++)
            _bindPose[i] = model.Bones[i].Transform;
        _previousPosition = position;
        _collisionRadius = 60f; // Set collision radius for tank
        _wheels = new Wheels(_model);
        _meshes = new List<ModelMesh>();
        MeshesTanque();
        /*
        // Resolver bones: primero por nombre, si no, usar índices por defecto y validar rangos
        _turretBone = FindBoneIndex("turret", "torreta");
        if (_turretBone < 0 || _turretBone >= _model.Bones.Count)
            _turretBone = 31; // valor por defecto anterior

        _gunBone = FindBoneIndex("gun", "cannon", "canon", "cañon", "barrel");
        if (_gunBone < 0 || _gunBone >= _model.Bones.Count)
            _gunBone = -1; // puede no existir un bone de cañón separado

        // Construir grupo de huesos que deben girar con la torreta
        _turretGroup = new List<int>();
        void AddIfValid(int idx)
        {
            if (idx >= 0 && idx < _model.Bones.Count && !_turretGroup.Contains(idx))
                _turretGroup.Add(idx);
        }
        AddIfValid(_turretBone);
        AddIfValid(FindBoneIndex("turret_front"));
        AddIfValid(FindBoneIndex("cage"));
        AddIfValid(FindBoneIndex("smoke_dischcharger"));
        AddIfValid(FindBoneIndex("exhaust   "));
        AddIfValid(FindBoneIndex("barrel"));
        AddIfValid(FindBoneIndex("coaxial_gun"));
        AddIfValid(FindBoneIndex("periscope"));
        AddIfValid(FindBoneIndex("hatch"));
        */
    }
    public bool GetIsShooting() => _isShooting;
    public void SetIsShooting(bool shoot) => _isShooting = shoot;
    public float GetLife() => _life;
    public void SetLife(float life) => _life = life;
    public float LifePercent() => _life / InitialLife;
    public int GetScore() => _score;
    public void SetScore(int score) => _score = score;
    // Permite establecer el yaw relativo de la torreta respecto al casco
    public void SetTurretYaw(float yawRelative)
    {
        _turretYaw = yawRelative;
    }
    // Setea el pitch del cañón en radianes, clamped a los límites internos
    public void SetGunPitch(float pitchRadians)
    {
        _gunPitch = MathHelper.Clamp(pitchRadians, GunPitchMin, GunPitchMax);
    }
    public void AimWithMouse(float deltaX, float deltaY, float sensitivity = 0.01f)
    {
        _turretYaw += deltaX * sensitivity;                 // girar torreta (Y)
        _gunPitch = MathHelper.Clamp(_gunPitch - deltaY * sensitivity, // invertido para “levantarse”
        GunPitchMin, GunPitchMax);         // limitar elevación
    }
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
        int turretIndex = (_turretBone >= 0 && _turretBone < _model.Bones.Count) ? _turretBone : 31;
        ModelBone turretBone = _model.Bones[turretIndex];
        Matrix turretWorld = _boneTransforms[turretBone.Index];
        Matrix turretWorldPos = turretWorld * _world;
        Vector3 turretPos = turretWorldPos.Translation;
        Vector3 turretDirection = turretWorldPos.Up;
        _turretDirection = turretDirection;
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
        /*
        // Asegurar que _turretBone apunte a la parte superior correcta
        if ((_turretBone < 0 || _turretBone >= _model.Bones.Count) && (_gunBone >= 0 && _gunBone < _model.Bones.Count))
        {
            var parent = _model.Bones[_gunBone].Parent;
            if (parent != null) _turretBone = parent.Index;
        }
        if (_turretBone < 0 || _turretBone >= _model.Bones.Count)
            _turretBone = Math.Min(31, _model.Bones.Count - 1);

        // Aplicar yaw de torreta a todos los huesos asociados, rotando alrededor del pivote de la torreta
        var pivot = _bindPose[_turretBone].Translation;
        // Yaw: eje Y para rigs Y-up y eje Z para rigs Z-up, rotando alrededor del pivote de la torreta
        var Ryaw = _modelZUp ? Matrix.CreateRotationY(_turretYaw) : Matrix.CreateRotationZ(_turretYaw);
        var Tpivot = Matrix.CreateTranslation(-pivot);
        var Tinv = Matrix.CreateTranslation(pivot);

        foreach (var idx in _turretGroup)
        {
            var baseLocal = _bindPose[idx];
            if (idx == _gunBone)
            {
                // Rotación de pitch local del cañón, luego yaw global de la torreta
                // Pitch del cañón: elevar sobre eje X
                baseLocal = Matrix.CreateRotationX(_gunPitch) * baseLocal;
            }
            // Aplicar yaw alrededor del pivote de la torreta
            _model.Bones[idx].Transform = Tpivot * Ryaw * Tinv * baseLocal;
        }
        */

        // Recalcular transforms absolutos luego de modificar los locales
        // _model.CopyAbsoluteBoneTransformsTo(_boneTransforms);
        // Recalcular transforms absolutos luego de aplicar rotaciones locales

        // 5) Construir la cámara enganchada al cañón: posición “tras y arriba” del mantelete
        /*
        var turretAbs = _boneTransforms[_turretBone] * _world;
        Matrix gunAbs;
        if (_gunBone >= 0 && _gunBone < _boneTransforms.Length)
            gunAbs = _boneTransforms[_gunBone] * _world;
        else
            gunAbs = turretAbs;
        _gunWorldAbs = gunAbs;


        // Ojo: en tu modelo el “forward” de la torreta/cañón podría ser Up/Forward/Right.
        // Usabas 'Up' para disparar; mantenemos esa convención:
        Vector3 gunForward = Vector3.Normalize(gunAbs.Up);       // dirección donde apunta el cañón
        Vector3 gunOrigin = gunAbs.Translation;                 // punto del cañón (mantelete)

        float camBack = 120f;   // distancia hacia atrás
        float camUp = 60f;    // altura sobre el cañón

        CameraPositionFromGun = gunOrigin - gunForward * camBack + Vector3.Up * camUp;
        var lookAt = gunOrigin + gunForward * 100f;

        ViewFromGun = Matrix.CreateLookAt(CameraPositionFromGun, lookAt, Vector3.Up);
        */
        _wheels.Update(gameTime,_velocity);
    }
    public override void Draw(GameTime gameTime, Matrix view, Matrix projection)
    {
        var modelTransforms = new Matrix[_model.Bones.Count];
        _model.CopyAbsoluteBoneTransformsTo(modelTransforms);

        _effect.Parameters["View"].SetValue(view);
        _effect.Parameters["Projection"].SetValue(projection);
        _effect.Parameters["Texture"]?.SetValue(_texture);
        // _effect.Parameters["DiffuseColor"].SetValue(Color.White.ToVector3());
        foreach (var mesh in _meshes)
        {
            var worldMesh = modelTransforms[mesh.ParentBone.Index] * _world;
            _effect.Parameters["World"].SetValue(worldMesh);
            mesh.Draw();
        }
        /*
        foreach (var mesh in _model.Meshes)
        {
            var worldMesh = modelTransforms[mesh.ParentBone.Index] * _world;

            _effect.Parameters["World"].SetValue(worldMesh);
            /*
            foreach (var fx in mesh.Effects)
            {
                fx.Parameters["World"]?.SetValue(worldMesh);
                if (_texture != null)
                    fx.Parameters["Texture"].SetValue(_texture);
                fx.Parameters["ScrollOffset"]?.SetValue(scroll);
            }
            mesh.Draw();
        }
            */
        _wheels.Draw(_world, view, projection);
    }
    public void SetShootSound(Song soundEffect) => _shootSound = soundEffect;

    /// BORRAR
    public void CambiarVida(float cantidad) => _life += cantidad;
    public void CambiarY(float y) => _position.Y += y;
    public int WheelsTotalListMesh() => _wheels.TotalListMesh();
    public void MeshesTanque()
    {
        foreach (var mesh in _model.Meshes)
        {
            if (!_wheels.MeshNames().Contains(mesh.Name))
                _meshes.Add(mesh);
        }
    }
}