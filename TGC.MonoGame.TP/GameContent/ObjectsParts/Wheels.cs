#region Using Statements
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace TGC.MonoGame.TP;
/*
Meshes del tanque T90

"Hull"
"Treadmill1"

"Turret"
"Cannon"
"Treadmill2"

*/
public class Wheels
{
    private Model _model;
    private List<string> WheelsNames = new List<string> {
        "Wheel1", "Wheel2", "Wheel8", "Wheel7", "Wheel6", "Wheel5", "Wheel4", "Wheel3",
        "Wheel9", "Wheel16", "Wheel15", "Wheel14", "Wheel13", "Wheel12", "Wheel11", "Wheel10"
    };
    private string trackName = "track";
    private Effect _effect;
    private List<ModelMesh> _wheelsMeshes;
    private List<ModelBone> _wheelsBones;
    private List<Matrix> _wheelBonesOriginalTransform;
    private float _wheelRotation;
    private ModelMesh _trackMesh;
    private ModelBone _trackBone;
    private float _trackRotation;
    private Matrix[] _boneTransform;

    public Wheels(Model model)
    {
        _model = model;
        _effect = model.Meshes[0].MeshParts[0].Effect;
        _wheelsMeshes = new List<ModelMesh>();
        _wheelsBones = new List<ModelBone>();
        _wheelBonesOriginalTransform = new List<Matrix>();
        _wheelRotation = 0f;
        _trackRotation = 0f;
        GetWheelsMeshesAndBonesFromModel();
        GetTrackMeshAndBoneFromModel();
        _boneTransform = new Matrix[_model.Bones.Count];
    }
    private void GetWheelsMeshesAndBonesFromModel()
    {
        foreach (var mesh in _model.Meshes)
        {
            var name = mesh.Name;
            if (WheelsNames.Contains(name))
                _wheelsMeshes.Add(mesh);
        }
        foreach (var bone in _model.Bones)
        {
            var name = bone.Name;
            if (WheelsNames.Contains(bone.Name))
            {
                _wheelsBones.Add(bone);
                _wheelBonesOriginalTransform.Add(bone.Transform);
            }
        }
    }
    private void GetTrackMeshAndBoneFromModel()
    {
        foreach (var mesh in _model.Meshes)
        {
            if (mesh.Name == trackName)
                _trackMesh = mesh;
        }
        foreach (var bone in _model.Bones)
        {
            if (bone.Name == trackName)
                _trackBone = bone;
        }
    }
    public void Update(GameTime gameTime, float velocity)
    {
        _wheelRotation += velocity;
        _trackRotation += velocity;
        var wheelRotation = Matrix.CreateRotationX(_wheelRotation);
        for (int i = 0; i < _wheelsBones.Count; i++)
        {
            ModelBone bone = _wheelsBones[i];
            bone.Transform = wheelRotation * _wheelBonesOriginalTransform[i];
        }
    }
    public void Draw(Matrix world, Matrix view, Matrix projection)
    {
        _model.CopyAbsoluteBoneTransformsTo(_boneTransform);

        _effect.Parameters["View"].SetValue(view);
        _effect.Parameters["Projection"].SetValue(projection);
        // _effect.Parameters["DiffuseColor"].SetValue(Color.Black.ToVector3());
        for (int i = 0; i < _wheelsMeshes.Count; i++)
        {
            ModelMesh mesh = _wheelsMeshes[i];
            // Matrix boneTransform = _wheelsBones[i].Transform;
            Matrix boneTransform = _boneTransform[_wheelsBones[i].Index];
            var boneWorld = boneTransform * world;
            _effect.Parameters["World"].SetValue(boneWorld);
            mesh.Draw();
        }
    }
    public int TotalListMesh() => _wheelsMeshes.Count;
    public List<string> MeshNames() => WheelsNames;
}