#region Using Statements
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace TGC.MonoGame.TP;

public class Wheels
{
    private List<string> WheelsNames = new List<string> {
        "Wheel1", "Wheel2", "Wheel8", "Wheel7", "Wheel6", "Wheel5", "Wheel4", "Wheel3",
        "Wheel9", "Wheel16", "Wheel15", "Wheel14", "Wheel13", "Wheel12", "Wheel11", "Wheel10",
    };
    private List<string> Treadmills = new List<string>
    {
        "Treadmill1", "Treadmill2"
    };
    private Model _model;
    private Effect _effect;
    private Texture2D _wheelTexture;
    private Texture2D _treadmillTexture;
    private List<ModelMesh> _wheelsMeshes;
    private List<ModelBone> _wheelsBones;
    private List<Matrix> _wheelsBonesTransforms;
    private List<Matrix> _wheelBonesOriginalTransform;
    private float _wheelRotation;
    private Matrix _matrixWheelRotation;
    private List<ModelMesh> _treadmillsMesh;
    private List<ModelBone> _treadmillsBone;
    private float _treadmillsOffset;
    private Matrix[] _boneTransform;
    public Wheels(Model model)
    {
        _model = model;
        _effect = model.Meshes[0].MeshParts[0].Effect;
        _wheelsMeshes = new List<ModelMesh>();
        _wheelsBones = new List<ModelBone>();
        _wheelBonesOriginalTransform = new List<Matrix>();
        _wheelsBonesTransforms = new List<Matrix>();
        _wheelRotation = 0f;
        _treadmillsMesh = new List<ModelMesh>();
        _treadmillsBone = new List<ModelBone>();
        _treadmillsOffset = 0f;
        GetWheelsMeshesAndBonesFromModel();
        GetTrackMeshAndBoneFromModel();
        _boneTransform = new Matrix[_model.Bones.Count];
    }
    private void GetWheelsMeshesAndBonesFromModel()
    {
        foreach (var mesh in _model.Meshes)
        {
            if (WheelsNames.Contains(mesh.Name))
                _wheelsMeshes.Add(mesh);
        }
        foreach (var bone in _model.Bones)
        {
            if (WheelsNames.Contains(bone.Name))
            {
                _wheelsBones.Add(bone);
                _wheelBonesOriginalTransform.Add(bone.Transform);
                _wheelsBonesTransforms.Add(bone.Transform);
            }
        }
    }
    private void GetTrackMeshAndBoneFromModel()
    {
        foreach (var mesh in _model.Meshes)
        {
            if (Treadmills.Contains(mesh.Name))
                _treadmillsMesh.Add(mesh);
        }
        foreach (var bone in _model.Bones)
        {
            if (Treadmills.Contains(bone.Name))
                _treadmillsBone.Add(bone);
        }
    }
    public void Update(GameTime gameTime, float velocity)
    {
        _treadmillsOffset += velocity * 0.2f;
        _wheelRotation += velocity;
        _matrixWheelRotation = Matrix.CreateRotationX(_wheelRotation);
    }
    public void Draw(Matrix world, Matrix view, Matrix projection)
    {
        _model.CopyAbsoluteBoneTransformsTo(_boneTransform);

        _effect.Parameters["View"].SetValue(view);
        _effect.Parameters["Projection"].SetValue(projection);
        _effect.Parameters["DiffuseColor"]?.SetValue(Color.White.ToVector3());
        _effect.Parameters["Texture"]?.SetValue(_wheelTexture);
        for (int i = 0; i < _wheelsMeshes.Count; i++)
        {
            ModelMesh mesh = _wheelsMeshes[i];
            Matrix boneTransform = _boneTransform[_wheelsMeshes[i].ParentBone.Index];
            var boneWorld = _matrixWheelRotation * boneTransform * world;
            _effect.Parameters["World"].SetValue(boneWorld);
            mesh.Draw();
        }
        _effect.Parameters["TreadmillsOffset"].SetValue(_treadmillsOffset);
        _effect.Parameters["Texture"]?.SetValue(_treadmillTexture);
        for (int i = 0; i < _treadmillsMesh.Count; i++)
        {
            ModelMesh mesh = _treadmillsMesh[i];
            Matrix boneTransform = _boneTransform[_treadmillsBone[i].Index];
            var boneWorld = boneTransform * world;
            _effect.Parameters["World"].SetValue(boneWorld);
            mesh.Draw();
        }
    }
    public bool ContainMesh(string meshName)
    {
        return WheelsNames.Contains(meshName) || Treadmills.Contains(meshName);
    }
    public void SetWheelTexture(Texture2D texture) => _wheelTexture = texture;
    public void SetTreadmillTexture(Texture2D texture) =>  _treadmillTexture = texture;
}