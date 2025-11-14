#region Using Statements
using System;
using System.Collections.Generic;
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
    private Vector3 _initialPosition;

    public Projectile(Model model, Vector3 startPosition, Vector3 direction, Texture2D texture = null,
        float speed = 100f, float lifetime = 3f, float scale = 0.001f, float rotation = 0f)
    {
        _model = model;
        _effect = model.Meshes[0].MeshParts[0].Effect;
        _position = startPosition;
        _direction = Vector3.Normalize(direction);
        _speed = speed;
        _lifeTime = lifetime;
        _elapsedTime = 0;
        _isActive = true;
        _rotation = rotation;
        _texture = texture;
        List<string> meshModels = new List<string>();
        List<string> boneModel = new List<string>();
        foreach (var mesh in _model.Meshes)
        {
            meshModels.Add(mesh.Name);
            boneModel.Add(mesh.ParentBone.Name);
        }
        _initialPosition = startPosition;
        // _initialYPosition = _position.Y * direction.Y;
        _scale = scale;
    }
    public bool IsActive() => _isActive;
    private Vector3 CalculatePosition()
    {
        Vector2 xz = new Vector2(_direction.X, _direction.Z);
        xz.Normalize();
        var deltaX = _initialPosition.X + _speed * _elapsedTime * xz.X;
        var deltaY = _initialPosition.Y + _direction.Y * _elapsedTime - Gravity * MathF.Pow(_elapsedTime, 2) / 20;
        var deltaZ = _initialPosition.Z + _speed * _elapsedTime * xz.Y;
        return new Vector3(deltaX, deltaY, deltaZ);
    }
    public override void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _position = CalculatePosition();
        _rotation += deltaTime;
        _elapsedTime += deltaTime;
        if (_elapsedTime > _lifeTime)
            _isActive = false;
        _world = Matrix.CreateScale(_scale) * Matrix.CreateRotationX(_rotation) * Matrix.CreateTranslation(_position);
    }
    public override void Draw(GameTime gameTime, Matrix view, Matrix projection)
    {
        if (!_isActive)
            return;

        // Set the View and Projection matrices, needed to draw every 3D model.
        _effect.Parameters["View"].SetValue(view);
        _effect.Parameters["Projection"].SetValue(projection);
        _effect.Parameters["DiffuseColor"]?.SetValue(Color.Green.ToVector3());
        _effect.Parameters["Texture"]?.SetValue(_texture);
        foreach (var mesh in _model.Meshes)
        {
            _effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * _world);
            mesh.Draw();
        }
    }
}