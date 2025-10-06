#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace TGC.MonoGame.TP;

public class Wall : GameObject
{
    private Effect _effect;
    private Model _model;
    
    public Wall(
        Model model,
        Vector3 position,
        float scale = 35f,
        float rotation = 0f
        )
    {
        _model = model;
        _effect = model.Meshes[0].MeshParts[0].Effect;
        _position = position;
        _scale = scale;
        _rotation = MathHelper.ToRadians(rotation);
        _world = Matrix.CreateScale(_scale) * Matrix.CreateRotationY(_rotation) * Matrix.CreateTranslation(_position);
    }

    public void Rotate(float rotationDegrees)
    {
        _rotation = MathHelper.ToRadians(rotationDegrees);
    }

    public void ScaleWall(float scale)
    {
        _scale = scale;
    }
    
    public override void Update(GameTime gameTime)
    {
        _world = Matrix.CreateScale(_scale) * Matrix.CreateRotationY(_rotation) * Matrix.CreateTranslation(_position);
    }
    
    public override void Draw(GameTime gameTime, Matrix view, Matrix projection)
    {
        // Set the View and Projection matrices, needed to draw every 3D model.
        _effect.Parameters["View"].SetValue(view);
        _effect.Parameters["Projection"].SetValue(projection);
        _effect.Parameters["DiffuseColor"].SetValue(Color.Black.ToVector3());
        foreach (var mesh in _model.Meshes)
        {
            _effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * _world);
            mesh.Draw();
        }
    }
}