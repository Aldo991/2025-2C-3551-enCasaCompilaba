using System;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Zero;

namespace TGC.MonoGame.TP.Zero;

internal class FollowCamera
{
    private const float AxisDistanceToTarget = 18600f;
    private const float AngleFollowSpeed = 0.005f;
    private const float AngleThreshold = 0.85f;

    private Vector3 _currentRightVector = Vector3.Right;
    private Vector3 _pastRightVector = Vector3.Right;
    private float _rightVectorInterpolator;

    public FollowCamera(float aspectRatio)
    {
        Projection = Matrix.CreatePerspectiveFieldOfView(MathF.PI / 3f, aspectRatio, 0.1f, 100000f);
    }

    public Matrix Projection { get; private set; }

    public Matrix View { get; private set; }
    
    public void Update(GameTime gameTime, Matrix followedWorld)
    {
        // Obtengo el tiempo.
        var elapsedTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

        // Obtengo la posicion de la matriz de mundo que estoy siguiendo.
        var followedPosition = followedWorld.Translation;

        // Obtengo el vector Derecha de la matriz de mundo que estoy siguiendo.
        var followedRight = followedWorld.Right;

        // Si el producto escalar entre el vector Derecha anterior
        // y el actual es mas grande que un limite,
        // muevo el Interpolator (desde 0 a 1) mas cerca de 1.
        if (Vector3.Dot(followedRight, _pastRightVector) > AngleThreshold)
        {
            // Incremento el Interpolator.
            _rightVectorInterpolator += elapsedTime * AngleFollowSpeed;

            // No permito que Interpolator pase de 1.
            _rightVectorInterpolator = MathF.Min(_rightVectorInterpolator, 1f);

            _currentRightVector = Vector3.Lerp(_currentRightVector, followedRight,
                _rightVectorInterpolator * _rightVectorInterpolator);
        }
        else
        // Si el angulo no pasa de cierto limite, lo pongo de nuevo en cero.
        {
            _rightVectorInterpolator = 0f;
        }

        // Guardo el vector Derecha para usar en la siguiente iteracion.
        _pastRightVector = followedRight;

        // Calculo la posicion del a camara
        // tomo la posicion que estoy siguiendo, agrego un offset en los ejes Y y Derecha.
        var offsetedPosition = followedPosition
                               + _currentRightVector * AxisDistanceToTarget
                               + Vector3.Up * AxisDistanceToTarget;

        // Calculo el vector Arriba actualizado.
        // Nota: No se puede usar el vector Arriba por defecto (0, 1, 0).
        // Como no es correcto, se calcula con este truco de producto vectorial.

        // Calcular el vector Adelante haciendo la resta entre el destino y el origen
        // y luego normalizandolo (Esta operacion es cara!).
        // (La siguiente operacion necesita vectores normalizados)
        var forward = followedPosition - offsetedPosition;
        forward.Normalize();

        // Obtengo el vector Derecha asumiendo que la camara tiene el vector Arriba apuntando hacia arriba
        // y no esta rotada en el eje X (Roll).
        var right = Vector3.Cross(forward, Vector3.Up);

        // Una vez que tengo la correcta direccion Derecha, obtengo la correcta direccion Arriba usando
        // otro producto vectorial.
        var cameraCorrectUp = Vector3.Cross(right, forward);

        // Calculo la matriz de Vista de la camara usando la Posicion, La Posicion a donde esta mirando,
        // y su vector Arriba.
        View = Matrix.CreateLookAt(offsetedPosition, followedPosition, cameraCorrectUp);
    }
}