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
#endregion

namespace TGC.MonoGame.TP;

public abstract class GameObject
{
    protected Matrix _world { get; set; }
    protected Vector3 _position { get; set; }
    protected float _scale { get; set; }
    protected float _rotation { get; set; }
    public abstract void Update(GameTime gameTime);
    public abstract void Draw(GameTime gameTime, Matrix view, Matrix projection);

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
}