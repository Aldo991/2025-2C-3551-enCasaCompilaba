#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace TGC.MonoGame.TP;

public class Land
{
    private Model _model;
    private Effect _effect;
    private Vector3 _position;
    private float _rotation;
    private float _scale;
    private Matrix _world;
    private QuadPrimitive _quad;

    public Land(
        Model model,
        Vector3 position,
        float scale = 1f,
        float rotation = 0f
        )
    {
        _model = model;
        _effect = model.Meshes[0].MeshParts[0].Effect;
        _position = position;
        _scale = scale;
        _rotation = MathHelper.ToRadians(rotation);
        // _world = Matrix.CreateScale(_scale) * Matrix.CreateRotationY(_rotation) * Matrix.CreateTranslation(_position);
        _world = Matrix.CreateScale(20000f, 0f, 20000f);
    }
    public Land(GraphicsDevice graphicsDevice, Model model, Matrix world)
    {
        _quad = new QuadPrimitive(graphicsDevice);
        _effect = model.Meshes[0].MeshParts[0].Effect;
        // _world = Matrix.CreateScale(20000f, 0f, 20000f);
        _world = world;
    }
    
    public void Draw(GameTime gameTime, Matrix view, Matrix projection, Color color)
    {
        _effect.Parameters["View"].SetValue(view);
        _effect.Parameters["Projection"].SetValue(projection);
        _effect.Parameters["World"].SetValue(_world);
        _effect.Parameters["DiffuseColor"].SetValue(color.ToVector3());
        _quad.Draw(_effect);
        /*
        foreach (var mesh in _model.Meshes)
        {
            _effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * _world);
            mesh.Draw();
        }
        */
    }

    public Model GetModel() => _model;
    public Matrix GetWorld() => _world;
}

