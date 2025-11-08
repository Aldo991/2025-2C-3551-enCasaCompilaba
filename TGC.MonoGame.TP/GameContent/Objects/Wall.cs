#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace TGC.MonoGame.TP;

public class Wall : GameObject
{
    private Effect _effect;
    private BoundingBox CreateBoundingBox(Model model, Matrix world)
    {
        Vector3 min = Vector3.One * float.MaxValue;
        Vector3 max = Vector3.One * float.MinValue;

        foreach (var mesh in model.Meshes)
        {
            foreach (var meshPart in mesh.MeshParts)
            {
                var vertexBuffer = meshPart.VertexBuffer;
                var declaration = vertexBuffer.VertexDeclaration;
                var vertexSize = declaration.VertexStride;
                var vertexData = new byte[vertexBuffer.VertexCount * vertexSize];
                vertexBuffer.GetData(vertexData);

                for (int i = 0; i < vertexBuffer.VertexCount; i++)
                {
                    var position = new Vector3(
                        BitConverter.ToSingle(vertexData, i * vertexSize),
                        BitConverter.ToSingle(vertexData, i * vertexSize + 4),
                        BitConverter.ToSingle(vertexData, i * vertexSize + 8)
                    );
                    position = Vector3.Transform(position, world);

                    min = Vector3.Min(min, position);
                    max = Vector3.Max(max, position);
                }
            }
        }

        return new BoundingBox(min, max);
    }
    public Wall(
        Model model,
        Vector3 position,
        float scale = 0.05f,
        float rotation = 0f
        )
    {
        _model = model;
        _effect = model.Meshes[0].MeshParts[0].Effect;
        _position = position;
        _scale = scale;
        _rotation = MathHelper.ToRadians(rotation);
        _world = Matrix.CreateScale(_scale) * Matrix.CreateRotationY(_rotation) * Matrix.CreateTranslation(_position);
        _boundingBox = CreateBoundingBox(model, _world);
        _collisionRadius = 20f; // Set collision radius for walls
    }
    public override void Update(GameTime gameTime)
    {
        _world = Matrix.CreateScale(_scale) * Matrix.CreateRotationY(_rotation) * Matrix.CreateTranslation(_position);
    }
    public override void Draw(GameTime gameTime, Matrix view, Matrix projection)
    {
        _effect.Parameters["View"].SetValue(view);
        _effect.Parameters["Projection"].SetValue(projection);
        _effect.Parameters["DiffuseColor"]?.SetValue(Color.Black.ToVector3());
        _effect.Parameters["Texture"]?.SetValue(_texture);
        foreach (var mesh in _model.Meshes)
        {
            _effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * _world);
            mesh.Draw();
        }
    }
}