#region FileDescription
/*
    * GameObject.cs
    * 
    * Clase abstracta que representa un objeto genérico en el juego.
    * Define propiedades comunes como posición, escala y rotación,
    * y métodos abstractos para actualizar y dibujar el objeto.
    *
*/
#endregion

#region Using Statements
using BepuPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace TGC.MonoGame.TP;

public abstract class GameObject
{
    protected const float Gravity = 9.81f;
    protected Model _model;
    protected Matrix _world;
    protected Vector3 _position;
    protected float _scale;
    protected float _rotation;
    protected Texture2D _texture;
    protected BoundingBox _boundingBox;
    protected float _collisionRadius;
    protected Texture2D _textureNormal;
    protected BodyHandle _bodyHandle;

    public abstract void Update(GameTime gameTime);
    public abstract void Draw(GameTime gameTime, Matrix view, Matrix projection);
    public BoundingBox GetBounding()
    {
        BodyReference body = GameManager.GetBodyReference(_bodyHandle);
        BoundingBox box = new BoundingBox(
            body.BoundingBox.Min,
            body.BoundingBox.Max
        );
        return box;
    }
    public Vector3 GetPosition() => _position;
    public void SetNormal(Texture2D normal) => _textureNormal = normal;
    public void SetPosition(Vector3 position) => _position = position;
    public float GetScale() => _scale;
    public void SetScale(float scale) => _scale = scale;
    public float GetRotation() => _rotation;
    public void SetRotation(float rotation) => _rotation = rotation;
    public Texture2D GetTexture() => _texture;
    public void SetTexture(Texture2D texture) => _texture = texture;
    public float CollisionRadius() => _collisionRadius;
}
