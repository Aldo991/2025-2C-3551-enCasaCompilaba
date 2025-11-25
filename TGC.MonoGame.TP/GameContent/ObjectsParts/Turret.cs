#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace TGC.MonoGame.TP;

public class Turret
{
    private string TurretName = "Turret";
    private string CannonName = "Cannon";
    private Model _model;
    private Effect _effect;
    private ModelMesh _turretMesh;
    private float _turretAngle;
    private Matrix _matrixTurretRotation;
    private Texture2D _turretTexture;
    private Texture2D _turretNormal;
    private ModelMesh _cannonMesh;
    private float _cannonAngle;
    private Matrix _matrixCannonRotation;
    private Texture2D _cannonTexture;
    private Texture2D _cannonNormal;
    private Matrix[] _boneTransform;
    private Vector3 _cannonDirection;
    private float _sensitivity;
    public Turret(Model model)
    {
        _model = model;
        _effect = model.Meshes[0].MeshParts[0].Effect;
        _turretAngle = 0f;
        _cannonAngle = 0f;
        _boneTransform = new Matrix[_model.Bones.Count];
        _matrixTurretRotation = Matrix.CreateRotationZ(-_turretAngle);
        _matrixCannonRotation = Matrix.CreateRotationX(_cannonAngle);
        _sensitivity = 0.001f;
        GetTurretMeshesAndBonesFromModel();
        GetCAnnonMeshesAndBonesFromModel();
    }
    private void GetTurretMeshesAndBonesFromModel()
    {
        foreach (var mesh in _model.Meshes)
        {
            if (mesh.Name == TurretName)
                _turretMesh = mesh;
        }
    }
    private void GetCAnnonMeshesAndBonesFromModel()
    {
        foreach (var mesh in _model.Meshes)
        {
            if (mesh.Name == CannonName)
                _cannonMesh = mesh;
        }
    }
    public Matrix GetCannonTraslation()
    {
        return _matrixCannonRotation * _matrixTurretRotation * _boneTransform[_cannonMesh.ParentBone.Index];
    }
    public Vector3 GetCannonDirection() => _cannonDirection;
    public void ChangeSensitivity(float sensitivity) => _sensitivity += sensitivity;
    public void Update(bool isPlayer)
    {
        if (isPlayer)
        {
            int offsetX = GameManager.GetMousePositionX() - GameManager.GetScreenCenterWidth();
            int offsetY = GameManager.GetMousePositionY() - GameManager.GetScreenCenterHeight();
            // Torreta
            _turretAngle += offsetX * _sensitivity;
            _matrixTurretRotation = Matrix.CreateRotationZ(-_turretAngle);
            // Cannon
            _cannonAngle += offsetY * _sensitivity;
            // _cannonAngle = (float)Math.Clamp((double)_cannonAngle, -0.2, 0.06);
            _matrixCannonRotation = Matrix.CreateRotationX(_cannonAngle);
        }
    }
    public void Draw(Matrix world, Matrix view, Matrix projection)
    {
        _model.CopyAbsoluteBoneTransformsTo(_boneTransform);

        _effect.Parameters["View"].SetValue(view);
        _effect.Parameters["Projection"].SetValue(projection);
        _effect.Parameters["Texture"]?.SetValue(_turretTexture);
        _effect.Parameters["NormalTexture"]?.SetValue(_turretNormal);
        _effect.Parameters["TreadmillsOffset"].SetValue(0.0f);
        _effect.Parameters["DiffuseColor"]?.SetValue(Color.Brown.ToVector3());
        // Torreta
        Matrix boneTransformTurret = _boneTransform[_turretMesh.ParentBone.Index];
        var boneWorldTurret = _matrixTurretRotation * boneTransformTurret * world;
        _effect.Parameters["World"].SetValue(boneWorldTurret);
        _turretMesh.Draw();

        // Cannon
        _effect.Parameters["Texture"]?.SetValue(_cannonTexture);
        _effect.Parameters["NormalTexture"]?.SetValue(_cannonNormal);
        _effect.Parameters["TreadmillsOffset"].SetValue(0.0f);
        _effect.Parameters["DiffuseColor"]?.SetValue(Color.Brown.ToVector3());
        var boneWorldCannon = _matrixCannonRotation * boneWorldTurret;
        _cannonDirection = boneWorldCannon.Down;

        _effect.Parameters["World"].SetValue(boneWorldCannon);
        _cannonMesh.Draw();
    }
    public bool ContainMesh(string meshName)
    {
        return meshName == TurretName || meshName == CannonName;
    }
    public void SetTurretTexture(Texture2D texture) => _turretTexture = texture;
    public void SetTurretNormal(Texture2D texture) => _turretNormal = texture;
    public void SetCannonTexture(Texture2D texture) => _cannonTexture = texture;
    public void SetCannonNormal(Texture2D texture) => _cannonNormal = texture;
}