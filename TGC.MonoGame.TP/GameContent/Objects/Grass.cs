#region Using Statements

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.Samples.Collisions;
#endregion

namespace TGC.MonoGame.TP;

public class Grass : GameObject
{
    private Effect _effect;
    public Grass(
        Model model,
        Vector3 position,
        float scale = 1f,
        float rotation = 0f)
    {
        _model = model;
        _effect = model.Meshes[0].MeshParts[0].Effect;
        var graphicsDevice = GameManager.GetGraphicsDevice();
        _effect = new BasicEffect(graphicsDevice)
        {
            TextureEnabled = false,
            LightingEnabled = true,
            PreferPerPixelLighting = true,
        };
        _position = position;
        _scale = scale;
        _rotation = MathHelper.ToRadians(rotation);
        CreateBoundingBoxToDraw();
        _world = Matrix.CreateScale(_scale) * Matrix.CreateRotationY(_rotation) * Matrix.CreateTranslation(_position);
    }
    public override void Update(GameTime gameTime)
    {
        
    }
    public override void Draw(GameTime gameTime, Matrix view, Matrix projection)
    {
        if (_effect == null || _model == null)
            return;

        _effect.Parameters["View"].SetValue(view);
        _effect.Parameters["Projection"].SetValue(projection);
        _effect.Parameters["World"].SetValue(_world);

        foreach (var mesh in _model.Meshes)
        {
            foreach (var part in mesh.MeshParts)
            {
                part.Effect = _effect;
            }
            mesh.Draw();
        }
    }
}