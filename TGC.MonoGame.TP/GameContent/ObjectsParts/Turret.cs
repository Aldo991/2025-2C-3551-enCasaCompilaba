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
        _model.CopyAbsoluteBoneTransformsTo(_boneTransform);
        return _matrixTurretRotation * _boneTransform[_cannonMesh.ParentBone.Index];
    }
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
            _cannonAngle = (float)Math.Clamp((double)_cannonAngle, -0.12, 0.06);
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
        Matrix boneTransform = _boneTransform[_turretMesh.ParentBone.Index];
        var boneWorld = _matrixTurretRotation * boneTransform * world;
        _effect.Parameters["World"].SetValue(boneWorld);
        _turretMesh.Draw();

        _model.CopyAbsoluteBoneTransformsTo(_boneTransform);

        // Cannon
        boneTransform = _boneTransform[_cannonMesh.ParentBone.Index];
        boneWorld = _matrixCannonRotation * boneTransform * world;
        _effect.Parameters["World"].SetValue(boneWorld);
        _cannonMesh.Draw();
    }
    public bool ContainMesh(string meshName)
    {
        return meshName == TurretName || meshName == CannonName;
    }
}