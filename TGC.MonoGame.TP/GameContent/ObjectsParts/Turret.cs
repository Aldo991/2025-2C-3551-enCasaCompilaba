#region Using Statements
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
    private ModelBone _turretBone;
    private Matrix _turretBoneOriginalTransform;
    // private float _turretAngle;
    private ModelMesh _cannonMesh;
    private ModelBone _cannonBone;
    private Matrix _cannonBoneOriginalTransform;
    // private float _cannonAngle;
    private Quaternion _turretRotation;
    private Matrix[] _boneTransform;
    public Turret(Model model)
    {
        _model = model;
        _effect = model.Meshes[0].MeshParts[0].Effect;
        // _turretAngle = 0f;
        // _cannonAngle = 0f;
        _turretRotation = Quaternion.Identity;
        _boneTransform = new Matrix[_model.Bones.Count];
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
        foreach (var bone in _model.Bones)
        {
            if (bone.Name == TurretName)
            {
                _turretBone = bone;
                _turretBoneOriginalTransform = bone.Transform;
            }
        }
    }
    private void GetCAnnonMeshesAndBonesFromModel()
    {
        foreach (var mesh in _model.Meshes)
        {
            if (mesh.Name == CannonName)
                _cannonMesh = mesh;
        }
        foreach (var bone in _model.Bones)
        {
            if (bone.Name == CannonName)
            {
                _cannonBone = bone;
                _cannonBoneOriginalTransform = bone.Transform;
            }
        }
    }
    public ModelBone GetCannonBone() => _cannonBone;
    public void Update(float cameraHorizontalAngle, float tankRotation)
    {
        // float relativeAngle = cameraHorizontalAngle - tankRotation * 0.001f;

        // Torreta
        // _turretAngle -= offsetX * 0.001f;
        // var turretRotation = Matrix.CreateRotationZ(-relativeAngle);
        // _turretBone.Transform = turretRotation * _turretBoneOriginalTransform;

        // Cannon
        // _cannonAngle -= offsetY * 0.001f;
        /*
        var cannonRotation = Matrix.CreateRotationX(relativeAngle);
        _model.CopyAbsoluteBoneTransformsTo(_boneTransform);
        _cannonBone.Transform = cannonRotation * _turretBone.Transform;
        */
    }
    public void Draw(Matrix world, Matrix view, Matrix projection)
    {
        _model.CopyAbsoluteBoneTransformsTo(_boneTransform);

        _effect.Parameters["View"].SetValue(view);
        _effect.Parameters["Projection"].SetValue(projection);
        _effect.Parameters["DiffuseColor"]?.SetValue(Color.Brown.ToVector3());
        _effect.Parameters["TreadmillsOffset"].SetValue(0.0f);
        // Torreta
        Matrix boneTransform = _boneTransform[_turretBone.Index];
        var boneWorld = boneTransform * world;
        _effect.Parameters["World"].SetValue(boneWorld);
        _turretMesh.Draw();

        // Cannon
        boneTransform = _boneTransform[_cannonBone.Index];
        boneWorld = boneTransform * world;
        _effect.Parameters["World"].SetValue(boneWorld);
        _cannonMesh.Draw();
    }
    public bool ContainMesh(string meshName)
    {
        return meshName == TurretName || meshName == CannonName;
    }
}