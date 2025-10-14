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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace TGC.MonoGame.TP;

public abstract class GameObject
{
    protected Matrix _world { get; set; }
    protected Vector3 _position { get; set; }
    protected float _scale { get; set; }
    protected float _rotation { get; set; }
    protected Texture2D _texture { get; set; }
    protected BoundingBox _localAABB;
    public abstract void Update(GameTime gameTime);
    public abstract void Draw(GameTime gameTime, Matrix view, Matrix projection);

    public BoundingBox GetWorldAABB()
    {
        var corners = _localAABB.GetCorners();
        Vector3 min = new Vector3(float.MaxValue);
        Vector3 max = new Vector3(float.MinValue);
        var rotationMatrix = Matrix.CreateRotationY(_rotation);
        var scaleMatrix = Matrix.CreateScale(_scale);
        foreach (var corner in corners)
        {
            var scaledCorner = Vector3.Transform(corner, scaleMatrix);
            var rotatedCorner = Vector3.Transform(scaledCorner, rotationMatrix);
            var worldCorner = rotatedCorner + _position;
            min = Vector3.Min(min, worldCorner);
            max = Vector3.Max(max, worldCorner);
        }
        return new BoundingBox(min, max);
    }

    public Vector3 Position
    {
        get => _position;
        set => _position = value;
    }
    public float Scale
    {
        get => _scale;
        set => _scale = value;
    }
    public float Rotation
    {
        get => _rotation;
        set => _rotation = value;
    }
    public Texture2D Texture
    {
        get => _texture;
        set => _texture = value;
    }
}