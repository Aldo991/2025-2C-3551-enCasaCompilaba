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
    private ModelMesh _cannonMesh;
    private float _cannonAngle;
    private Matrix _matrixCannonRotation;
    private Matrix[] _boneTransform;
    private Vector3 _cannonDirection;
    public Turret(Model model)
    {
        _model = model;
        _effect = model.Meshes[0].MeshParts[0].Effect;
        _turretAngle = 0f;
        _cannonAngle = 0f;
        _boneTransform = new Matrix[_model.Bones.Count];
        _matrixTurretRotation = Matrix.CreateRotationZ(-_turretAngle);
        _matrixCannonRotation = Matrix.CreateRotationX(_cannonAngle);
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
    public void Update(bool isPlayer)
    {
        if (isPlayer)
        {
            int offsetX = GameManager.GetMousePositionX() - GameManager.GetScreenCenterWidth();
            int offsetY = GameManager.GetMousePositionY() - GameManager.GetScreenCenterHeight();
            // Torreta
            _turretAngle += offsetX * 0.001f;
            _matrixTurretRotation = Matrix.CreateRotationZ(-_turretAngle);
            // Cannon
            _cannonAngle += offsetY * 0.001f;
            _cannonAngle = (float)Math.Clamp((double)_cannonAngle, -0.2, 0.06);
            _matrixCannonRotation = Matrix.CreateRotationX(_cannonAngle);
        }
    }
    public void Draw(Matrix world, Matrix view, Matrix projection)
    {
        _model.CopyAbsoluteBoneTransformsTo(_boneTransform);

        _effect.Parameters["View"].SetValue(view);
        _effect.Parameters["Projection"].SetValue(projection);
        _effect.Parameters["DiffuseColor"]?.SetValue(Color.Brown.ToVector3());
        _effect.Parameters["TreadmillsOffset"].SetValue(0.0f);
        // Torreta
        Matrix boneTransformTurret = _boneTransform[_turretMesh.ParentBone.Index];
        var boneWorldTurret = _matrixTurretRotation * boneTransformTurret * world;
        _effect.Parameters["World"].SetValue(boneWorldTurret);
        _turretMesh.Draw();

        // Cannon
        Matrix boneTransformCannon = _boneTransform[_cannonMesh.ParentBone.Index];
        var _1 = boneTransformTurret;
        var _2 = boneTransformCannon;
        var _3 = _matrixTurretRotation;
        var _4 = _matrixCannonRotation;
        var boneWorldCannon1 = _matrixCannonRotation * boneTransformCannon * world;
        var boneWorldCannon2 = _matrixCannonRotation * boneTransformCannon * boneWorldTurret;
        var boneWorldCannon3 = _matrixCannonRotation * _matrixTurretRotation * boneTransformCannon * world;
        _cannonDirection = boneWorldCannon3.Down;


        _effect.Parameters["World"].SetValue(boneWorldCannon3);
        _cannonMesh.Draw();
    }
    public bool ContainMesh(string meshName)
    {
        return meshName == TurretName || meshName == CannonName;
    }
}