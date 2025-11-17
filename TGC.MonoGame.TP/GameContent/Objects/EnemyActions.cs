#region File Description
/// La idea de esta clase es delegar la lógica de cómo deben actuar los enemigos.
/// En esta clase pasa lo siguiente:
/// Si _elapsedTime > _totalTime, se generan un nuevo _totalTime y un nuevo _probability.
/// _totalTime va a saber por cuanto tiempo se va a estar haciendo la misma acción, mientras
/// que _elapsedTime no lo supere.
/// _probability lo uso para saber qué acción realizar. Por ejemplo, si _probability < 0.1, 
/// entonces tengo un 10% de probabilidad de disparar
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
#endregion

namespace TGC.MonoGame.TP;

public class EnemyAction
{
    private float _elapsedTime;
    private int _totalTime;
    private float _probabilityMove;
    private float _probabilityRotate;
    private float _probabilityShoot;
    private bool _canShoot;
    private Tank _tank;
    public EnemyAction(Tank tank)
    {
        _tank = tank;
        _totalTime = 3; // O sea, son 3 segundo hasta que se empiezan a mover
        _elapsedTime = 0f;
        _probabilityMove = 1f;
        _probabilityRotate = 1f;
        _probabilityShoot = 1;
        _canShoot = true;
    }
    public void Update(GameTime gameTime, GameManager gameManager)
    {
        // Reviso si el tiempo acumulado es mayor al tiempo total.
        if (_elapsedTime > _totalTime)
        {
            _canShoot = true;
            _elapsedTime = 0;
            Random random = new Random();
            _probabilityMove = random.NextSingle();
            _probabilityRotate = random.NextSingle();
            _probabilityShoot = random.NextSingle();
            _totalTime = (int)random.NextInt64() % 5;
            if (_totalTime == 0)
                _totalTime = 1;
        }
        if (_probabilityMove < .3)
            _tank.MoveForwardTank(gameTime);
        else if (_probabilityMove < .6)
            _tank.MoveBackwardTank(gameTime);
        else
            _tank.DecelerateTank(gameTime);
        if (_probabilityRotate < .3)
            _tank.RotateTankRight(gameTime);
        else if (_probabilityRotate < .6)
            _tank.RotateTankLeft(gameTime);
        if (_probabilityShoot < .5 && _canShoot)
        {
            Projectile p = _tank.Shoot();
            gameManager.AddToProjectileManager(p);
            _canShoot = false;
        }
        _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
    }
}