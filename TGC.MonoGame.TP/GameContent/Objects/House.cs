#region Using Statements
using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace TGC.MonoGame.TP;

public class House : GameObject
{
    public const float DefaultScaleHouse = 0.003f;
    private Effect _effect;
    // private BoxPrimitive boxPrimitive;
    // private Matrix boxWorld;
    private float boxWidht;
    private float boxHeight;
    private float boxLength;
    public House(
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
        boxWidht = 10f;
        boxHeight = 4.5f;
        boxLength = 5f;
        /*
        Vector3 boxSize = new Vector3(boxWidht, boxHeight, boxLength);
        Texture2D boxTexture = ContentLoader.GetTexture("house", 3);
        boxPrimitive = new BoxPrimitive(graphicsDevice, boxSize, boxTexture);
        */
        CreateBoundingBoxToDraw();
        CreateCollisionBox();
    }
    private void CreateCollisionBox()
    {
        Box boxShape = new Box(boxWidht, boxHeight, boxLength);
        TypedIndex boxIndex = GameManager.AddShapeToSimulation(boxShape);
        CollidableDescription collidableDescription = new CollidableDescription(boxIndex, 0.1f);
        BodyActivityDescription bodyActivityDescription = new BodyActivityDescription(0.01f);
        var position = _position.ToNumerics();
        // ajustes por desfase del modelo
        position.Y += 2.25f;
        position.X -= 1f;
        var rotationY = MathHelper.ToRadians(3f);
        Quaternion rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, rotationY);

        var bodyDescription = BodyDescription.CreateKinematic(
            position,
            collidableDescription,
            bodyActivityDescription
        );
        var currentOrientation = bodyDescription.Pose.Orientation;
        bodyDescription.Pose.Orientation = Quaternion.Normalize(rotation * currentOrientation).ToNumerics();
        _bodyHandle = GameManager.AddBodyToSimulation(bodyDescription, this);
    }
    public override void Update(GameTime gameTime)
    {
        // Creo que esto se puede sacar, hacer en el constructor y listo. Total, no se van a mover
        Quaternion quaternion = Quaternion.CreateFromAxisAngle(Vector3.UnitY,_rotation);
        Matrix rotationMatrix = Matrix.CreateFromQuaternion(quaternion);
        _world = Matrix.CreateScale(_scale) * rotationMatrix * Matrix.CreateTranslation(_position);
        /*
        var boxPosition = _position + new Vector3(-1f, 2.25f, 0);
        var boxRotation = _rotation + MathHelper.ToRadians(3f);
        Quaternion boxQuaternion = Quaternion.CreateFromAxisAngle(Vector3.UnitY, boxRotation);
        Matrix boxRotationMatrix = Matrix.CreateFromQuaternion(boxQuaternion);
        boxWorld = boxRotationMatrix * Matrix.CreateTranslation(boxPosition);

        Vector3 boxSize = new Vector3(anchoCaja, altoCaja, profundidadCaja);
        Texture2D boxTexture = ContentLoader.GetTexture("house", 3);
        boxPrimitive = new BoxPrimitive(graphicsDevice, boxSize, boxTexture);
        */
    }
    public override void Draw(GameTime gameTime, Matrix view, Matrix projection)
    {
        Vector3 specularColor = Color.LightYellow.ToVector3();
        Matrix inverseTransposeWorld = Matrix.Invert(Matrix.Transpose(_world));

        GameManager.SetIluminationParameters(
            _effect,
            inverseTransposeWorld,
            specularColor
        );

        // boxPrimitive.Draw(boxWorld, view, projection);
        _effect.Parameters["View"].SetValue(view);
        _effect.Parameters["Projection"].SetValue(projection);
        _effect.Parameters["Texture"]?.SetValue(_texture);
        _effect.Parameters["NormalTexture"]?.SetValue(_textureNormal);
        _effect.Parameters["DiffuseColor"]?.SetValue(Color.Gray.ToVector3());
        foreach (var mesh in _model.Meshes)
        {
            _effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * _world);
            mesh.Draw();
        }
    }
}