#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace TGC.MonoGame.TP;

public class Stone : GameObject
{
    private Model _model;
    private Effect _effect;

    public Stone(
        Model model,
        Vector3 position,
        float scale = 1f,
        float rotation = 0f,
        Texture2D texture = null
        )
    {
        _model = model;
        _effect = model.Meshes[0].MeshParts[0].Effect;
        _position = position;
        _scale = scale;
        _texture = texture;
        _rotation = MathHelper.ToRadians(rotation);
        _world = Matrix.CreateScale(0.3f) * Matrix.CreateTranslation(_position);
        // Define local AABB for stone (approximate dimensions)
        _localAABB = new BoundingBox(new Vector3(-30, -30, -30), new Vector3(30, 30, 30));
    }
    
    public override void Update(GameTime gameTime)
    {
        _world = Matrix.CreateTranslation(_position);
    }
    
    public override void Draw(GameTime gameTime, Matrix view, Matrix projection)
    {
        // Set the View and Projection matrices, needed to draw every 3D model.
        _effect.Parameters["View"].SetValue(view);
        _effect.Parameters["Projection"].SetValue(projection);
        //_effect.Parameters["DiffuseColor"].SetValue(Color.Black.ToVector3());
        foreach (var mesh in _model.Meshes)
        {
            _effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * _world);
            if (_texture != null)
                _effect.Parameters["Texture"].SetValue(_texture);
            mesh.Draw();
        }
    }
}