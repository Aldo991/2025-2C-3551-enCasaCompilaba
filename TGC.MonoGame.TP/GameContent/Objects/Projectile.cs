#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace TGC.MonoGame.TP;

public class Projectile : GameObject
{
    private Effect _effect;
    private Vector3 _direction;
    private float _speed;
    private float _lifeTime;
    private float _elapsedTime;
    private bool _isActive;
    public Projectile(Model model, Vector3 startPosition, Vector3 direction,
        float speed = 50f, float lifetime = 5f, float scale = 0.005f, float rotation = 0f)
    {
        _model = model;
        _effect = model.Meshes[0].MeshParts[0].Effect;
        _position = startPosition;
        _direction = Vector3.Normalize(direction);
        _speed = speed;
        _lifeTime = lifetime;
        _elapsedTime = 0;
        _isActive = true;
        _scale = scale;
        _rotation = rotation;
    }
    public bool IsActive
    {
        get => _isActive;
    }
    public override void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _position += _direction * _speed * deltaTime;
        _elapsedTime += deltaTime;
        if (_elapsedTime > _lifeTime)
            _isActive = false;
        _world = Matrix.CreateScale(_scale) * Matrix.CreateRotationY(_rotation) * Matrix.CreateTranslation(_position);
    }
    public override void Draw(GameTime gameTime, Matrix view, Matrix projection)
    {
        if (!_isActive)
            return;

        // Set the View and Projection matrices, needed to draw every 3D model.
        _effect.Parameters["View"].SetValue(view);
        _effect.Parameters["Projection"].SetValue(projection);
        _effect.Parameters["DiffuseColor"].SetValue(Color.White.ToVector3());
        foreach (var mesh in _model.Meshes)
        {
            _effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * _world);
            mesh.Draw();
        }
    }
}