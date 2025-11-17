#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.Samples.Collisions;
#endregion

namespace TGC.MonoGame.TP;

public class Tree : GameObject
{
    private Effect _effect;
    public Tree(
        Model model,
        Vector3 position,
        float scale = 1f,
        float rotation = 0f)
    {
        _model = model;
        _effect = model.Meshes[0].MeshParts[0].Effect;
        _position = position;
        _scale = scale;
        _rotation = MathHelper.ToRadians(rotation);
        _world = Matrix.CreateScale(_scale) * Matrix.CreateRotationY(_rotation) * Matrix.CreateTranslation(_position);
        CreateBoundingBoxToDraw();
    }
    public override void Update(GameTime gameTime)
    {
        _world = Matrix.CreateScale(_scale) * Matrix.CreateRotationY(_rotation) * Matrix.CreateTranslation(_position);
    }
    public override void Draw(GameTime gameTime, Matrix view, Matrix projection)
    {
        _effect.Parameters["View"].SetValue(view);
        _effect.Parameters["Projection"].SetValue(projection);
        _effect.Parameters["DiffuseColor"]?.SetValue(Color.DarkGreen.ToVector3());
        if (_texture != null)
            _effect.Parameters["Texture"]?.SetValue(_texture);
        foreach (var mesh in _model.Meshes)
        {
            _effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * _world);
            mesh.Draw();
        }
    }
}