#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace TGC.MonoGame.TP;

public class Tank : GameObject
{
    private const float TankMaxSpeed = 40f; // Unidades por segundo
    private const float RotationSpeed = 1.5f; // Radianes por segundo
    private const float Acceleration = 4f; // Aceleracion del tanque
    private Model _model;
    private Effect _effect;
    private Vector3 _tankFrontDirection;
    private bool _isMovingforward;
    private float _velocity; // Velocidad actual del tanque

    public Tank(
        Model model,
        Vector3 position,
        float scale = 1f,
        float rotation = 0f,
        Texture2D texture = null)
    {
        _model = model;
        _effect = model.Meshes[0].MeshParts[0].Effect;
        _position = position;
        _scale = scale;
        _rotation = MathHelper.ToRadians(rotation);
        _texture = texture;
        _world = Matrix.CreateScale(_scale) * Matrix.CreateRotationY(_rotation) * Matrix.CreateTranslation(_position);
        _tankFrontDirection = Vector3.Transform(Vector3.Forward, Matrix.CreateRotationY(_rotation));
        _velocity = 0f;
        _isMovingforward = true;
    }
    public void MoveForwardTank(GameTime gameTime)
    {
        if (_isMovingforward || (!HasVelocity() && !_isMovingforward))
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _velocity += Acceleration * deltaTime;
            if (_velocity > TankMaxSpeed)
                _velocity = TankMaxSpeed;
            _position += _tankFrontDirection * _velocity;
            _isMovingforward = true;
        }
        else
            DecelerateTank(gameTime);
    }
    public void MoveBackwardTank(GameTime gameTime)
    {
        if (!_isMovingforward || (!HasVelocity() && _isMovingforward))
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _velocity += Acceleration * deltaTime;
            if (_velocity > TankMaxSpeed)
                _velocity = TankMaxSpeed;
            _position -= _tankFrontDirection * _velocity;
            _isMovingforward = false;
        }
        else
            DecelerateTank(gameTime);
    }
    public bool HasVelocity()
    {
        return _velocity > 0;
    }
    public void DecelerateTank(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _velocity -= Acceleration * deltaTime;
        if (_velocity < 0)
            _velocity = 0;
        if (_isMovingforward)
            _position += _tankFrontDirection * _velocity;
        else
            _position -= _tankFrontDirection * _velocity;
    }
    public void RotateTankRight(GameTime gameTime)
    {
        RotateTank(gameTime, true);
    }
    public void RotateTankLeft(GameTime gameTime)
    {
        RotateTank(gameTime, false);
    }
    private void RotateTank(GameTime gameTime, bool right)
    {
        if (_velocity > 0)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (right)
                _rotation -= RotationSpeed * deltaTime;
            else
                _rotation += RotationSpeed * deltaTime;
            _tankFrontDirection = Vector3.Transform(Vector3.Forward, Matrix.CreateRotationY(_rotation));
        }
    }
    public override void Update(GameTime gameTime)
    {
        _world = Matrix.CreateScale(_scale) * Matrix.CreateRotationY(_rotation) * Matrix.CreateTranslation(_position);
    }
    public override void Draw(GameTime gameTime, Matrix view, Matrix projection)
    {
        // Set the View and Projection matrices, needed to draw every 3D model.
        _effect.Parameters["View"].SetValue(view);
        _effect.Parameters["Projection"].SetValue(projection);
        _effect.Parameters["DiffuseColor"].SetValue(Color.GreenYellow.ToVector3());
        var modelMeshesBaseTransform = new Matrix[_model.Bones.Count];
        _model.CopyAbsoluteBoneTransformsTo(modelMeshesBaseTransform);
        foreach (var mesh in _model.Meshes)
        {
            var relativeTransform = modelMeshesBaseTransform[mesh.ParentBone.Index];
            _effect.Parameters["World"].SetValue(relativeTransform * _world);
            if (_texture != null)
                _effect.Parameters["Texture"].SetValue(_texture);
            mesh.Draw();
        }
    }
}