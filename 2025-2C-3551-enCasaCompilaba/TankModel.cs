using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP;

internal class TankModel
{

    private  Effect _effect;
    private  Model _model;
    private Vector3 _position;

    private  Matrix _world;
    private const string ContentFolderTextures = "Textures/";
    private Texture _texture;

    public TankModel(ContentManager content, string contentFolder3D, string contentFolderEffects)
    {
        _model = content.Load<Model>(contentFolder3D + "tanque/CENTURION");

        _effect = content.Load<Effect>(contentFolderEffects + "TankShader");

        _texture = content.Load<Texture>(ContentFolderTextures + "camuflaje3");

        foreach (var mesh in _model.Meshes)
        {
            // A mesh contains a collection of parts.
            foreach (var meshPart in mesh.MeshParts)
            // Assign the loaded effect to each part.
            {
                meshPart.Effect = _effect;
            }
        }
    }

    public void Initialize(Vector3 vector)
    {
        _position = vector;
        _world = Matrix.CreateTranslation(_position);
    }
    
    public void Draw(GameTime gameTime, Matrix view, Matrix projection)
    {
        // Set the View and Projection matrices, needed to draw every 3D model.

        _effect.Parameters["View"].SetValue(view);
        _effect.Parameters["Projection"].SetValue(projection);
        _effect.Parameters["ModelTexture"].SetValue(_texture);

        foreach (var mesh in _model.Meshes)
        {
            _effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * _world);
            mesh.Draw();
        }
    }
}