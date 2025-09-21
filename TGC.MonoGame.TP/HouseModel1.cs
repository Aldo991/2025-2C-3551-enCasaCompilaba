using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP.Zero;

internal class HouseModel1
{

    private  Effect _effect;
    private  Model _model;
    private Vector3 _position;

    private  Matrix _world;

    public HouseModel1(ContentManager content, string contentFolder3D, string contentFolderEffects)
    {
        _model = content.Load<Model>(contentFolder3D + "arqui/House2/cottage_fbx");

        _effect = content.Load<Effect>(contentFolderEffects + "BasicShader");


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
        _world = Matrix.CreateScale(0.3f) * Matrix.CreateTranslation(_position);
    }
    
    public void Draw(GameTime gameTime, Matrix view, Matrix projection)
    {
        // Set the View and Projection matrices, needed to draw every 3D model.

        _effect.Parameters["View"].SetValue(view);
        _effect.Parameters["Projection"].SetValue(projection);
        _effect.Parameters["DiffuseColor"].SetValue(Color.Gray.ToVector3());

        foreach (var mesh in _model.Meshes)
        {
            _effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * _world);
            mesh.Draw();
        }
    }
}