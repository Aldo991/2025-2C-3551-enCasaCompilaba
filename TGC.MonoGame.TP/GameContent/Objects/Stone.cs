#region Using Statements
using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.Samples.Collisions;
#endregion

namespace TGC.MonoGame.TP;

public class Stone : GameObject
{
    public const float DefaultScaleLittleStone = 0.0015f;
    public const float DefaultScaleBigStone = 0.03f;
    private Effect _effect;
    public Stone(
        Model model,
        Vector3 position,
        float scale,
        float rotation = 0f,
        Texture2D texture = null
        )
    {
        _model = model;
        _effect = model.Meshes[0].MeshParts[0].Effect;
        _position = position;
        _scale = scale;
        _rotation = MathHelper.ToRadians(rotation);
        _world = Matrix.CreateScale(_scale) * Matrix.CreateRotationY(_rotation) * Matrix.CreateTranslation(_position);
        CreateBoundingBoxToDraw();
        if (scale == DefaultScaleBigStone)
        {
            _boxWidth = _boundingBoxToDraw.Max.X - _boundingBoxToDraw.Min.X;
            _boxHeight = _boundingBoxToDraw.Max.Y - _boundingBoxToDraw.Min.Y;
            _boxLength = _boundingBoxToDraw.Max.Z - _boundingBoxToDraw.Min.Z;
            CreateCollisionBox();
        }
    }
        private void CreateCollisionBox()
    {
        Box boxShape = new Box(_boxWidth, _boxHeight, _boxLength);
        TypedIndex boxIndex = GameManager.AddShapeToSimulation(boxShape);
        CollidableDescription collidableDescription = new CollidableDescription(boxIndex, 0.1f);
        BodyActivityDescription bodyActivityDescription = new BodyActivityDescription(0.01f);
        var position = _position.ToNumerics();
        // ajustes por desfase del modelo

        var bodyDescription = BodyDescription.CreateKinematic(
            position,
            collidableDescription,
            bodyActivityDescription
        );
        _bodyHandle = GameManager.AddBodyToSimulation(bodyDescription, this);
    }
    public override void Update(GameTime gameTime)
    {
    }
    
    public override void Draw(GameTime gameTime, Matrix view, Matrix projection)
    {
        _effect.Parameters["View"].SetValue(view);
        _effect.Parameters["Projection"].SetValue(projection);
        if(_texture != null)
            _effect.Parameters["Texture"].SetValue(_texture);
        foreach (var mesh in _model.Meshes)
        {
            _effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * _world);
            mesh.Draw();
        }
    }
}