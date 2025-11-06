#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
#endregion

namespace TGC.MonoGame.TP;

public class TGCGame : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private Tank _tank;
    private ElementosLand _elementosLand;
    private GameManager _gameManager;
    private Land _land;
    public TGCGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        // Título del juego en la ventana del programa
        Window.Title = "TankWars";
        // Ancho y altura de la ventana
        _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100;
        _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;
        Content.RootDirectory = "Content"; // Carpeta donde está el contenido del juego (modelos, sonidos, etc.)
        // Visibilidad del mouse
        IsMouseVisible = false;
    }

    protected override void Initialize()
    {
        // Instancio e inicializo GameManager, donde voy a controlar todos los objetos del juego
        _gameManager = GameManager.Instance;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        // Cargo todos los elementos del juego, como efectos, modelos, sprites, sonidos y texturas
        ContentLoader.Load(Content);
        // Este no lo entendí
        var world = Matrix.CreateScale(20000f, 0f, 20000f);
        _land = new Land(GraphicsDevice, ContentLoader.GetModel("land", 0), world);

        // Cargo los elementos del mundo, esto debería ir en GameManager?
        /// todo: revisar si esto debería ir en GameManager y pasarlo
        _gameManager.Initialize(Content, GraphicsDevice);
        _elementosLand = new ElementosLand(Content);

        // Instancio el tanque con todo lo necesario para funcionar. Este tanque es el del
        // Personaje que vamos a controlar. Revisar si debería estar en GameManager
        // Todo: revisar si debería estar en GameManager
        Model tankModel = ContentLoader.GetModel("tank", 1);
        Vector3 tankPosition = new Vector3(25f, 30f, 30f);
        Texture2D tankTexture = ContentLoader.GetTexture("tank", 0);
        float tankScale = 0.01f;
        _tank = new Tank(tankModel, tankPosition, tankScale, 0f, tankTexture);
        _tank.SetGround(_elementosLand);
        Model projectileModel = ContentLoader.GetModel("projectile", 0);
        _tank.SetProjectileModel(projectileModel);
        Song shootTank = ContentLoader.GetSoundEffect();
        _tank.SetShootSound(shootTank);

        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        // Delta t de tiempo entre intervalos de actualización
        float dt = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
        // Estado de las teclas del teclado, es decir, si están presionadas o no, etc.
        KeyboardState kb = Keyboard.GetState();
        // Si presiono el botón de salir, el estado del juego cambia a salir y se cierra la ventana.
        if (_gameManager.IsExit()) Exit();
        // _showScoreboard = kb.IsKeyDown(Keys.Tab);

        // Si estoy jugando, se me permiten movimientos como mover el tanque, acelerar, frenar,
        // disparar
        if (_gameManager.IsPlaying())
        {
            if (kb.IsKeyDown(Keys.Tab)) _gameManager.SetScoreboard(true);
            if (kb.IsKeyUp(Keys.Tab)) _gameManager.SetScoreboard(false);
            // Acelero el tanque
            if (kb.IsKeyDown(Keys.W)) _tank.MoveForwardTank(gameTime);
            // Desacelero el tanque
            if (kb.IsKeyUp(Keys.W) && kb.IsKeyUp(Keys.S)) _tank.DecelerateTank(gameTime);
            // Giro el tanque hacia la izquierda si tiene velocidad
            if (kb.IsKeyDown(Keys.A) && _tank.HasVelocity()) _tank.RotateTankLeft(gameTime);
            // Muevo el tanque en reversa
            if (kb.IsKeyDown(Keys.S)) _tank.MoveBackwardTank(gameTime);
            // Giro el tanque hacia la derecha si tiene velocidad
            if (kb.IsKeyDown(Keys.D) && _tank.HasVelocity()) _tank.RotateTankRight(gameTime);
            // Disparo projectiles
            if (kb.IsKeyDown(Keys.F) && !_tank.GetIsShooting())
            {
                Projectile p = _tank.Shoot();
                _gameManager.AddToProjectileManager(p);
                _tank.SetIsShooting(true);
            }
            if (kb.IsKeyUp(Keys.F) && _tank.GetIsShooting()) _tank.SetIsShooting(false);
            if (kb.IsKeyDown(Keys.Left)) _gameManager.CambiarBrujula(-1, 0);
            if (kb.IsKeyDown(Keys.Right)) _gameManager.CambiarBrujula(1, 0);
            if (kb.IsKeyDown(Keys.Up)) _gameManager.CambiarBrujula(0, -1);
            if (kb.IsKeyDown(Keys.Down)) _gameManager.CambiarBrujula(0, 1);
            if (kb.IsKeyDown(Keys.OemMinus)) _tank.CambiarVida(-1f);
            if (kb.IsKeyDown(Keys.OemPlus)) _tank.CambiarVida(1f);
            if (kb.IsKeyDown(Keys.Space)) _tank.CambiarY(.5f);
            if (kb.IsKeyDown(Keys.LeftShift)) _tank.CambiarY(-.5f);

            // De acá para abajo no entendí xd
            // Todo: entender esto
            // Tank Update
            var ms = Mouse.GetState();
            int mouseX = ms.X;
            int mouseY = ms.Y;
            int width = GraphicsDevice.Viewport.Width;
            int height = GraphicsDevice.Viewport.Height;


            _tank.Update(gameTime);
            var bodyForward = Vector3.Transform(Vector3.Forward, Matrix.CreateRotationY(_tank.GetRotation()));
            // UpdateOrbitBehind actualiza la cámara para que esta orbite  el tanque
            _gameManager.UpdateOrbitBehind(_tank.GetPosition(), bodyForward, mouseX, mouseY);

            /*// Esto hace que la cámara esté fija a la torreta, entonces la torreta siempre apunta hacia adelante.
            var camFwd = _gameManager.GetCameraForward();
            camFwd.Normalize();
            float yawAbs = !_tank.ModelZUp ? (float)Math.Atan2(camFwd.X, camFwd.Z)
                                            : (float)Math.Atan2(camFwd.X, camFwd.Y);
            float yawRel = MathHelper.WrapAngle(yawAbs - _tank.GetRotation() + MathHelper.Pi);
            _tank.SetTurretYaw(yawRel);
            if (!_tank.ModelZUp)
            {
                float pitch = (float)Math.Atan2(camFwd.Y, Math.Sqrt(camFwd.X * camFwd.X + camFwd.Z * camFwd.Z));
                _tank.SetGunPitch(pitch);
            }
            else
            {
                float pitch = (float)Math.Atan2(camFwd.Z, Math.Sqrt(camFwd.X * camFwd.X + camFwd.Y * camFwd.Y));
                _tank.SetGunPitch(pitch);
            }
            //*/
            IsMouseVisible = false;
            Mouse.SetPosition(width / 2, height / 2);
        }
        
        // Si presiono la tecla P o la tecla Escape, pauso el juego
        if ( (kb.IsKeyDown(Keys.P) || kb.IsKeyDown(Keys.Escape)) && !_gameManager.GetPressingPause())
        {
            if (_gameManager.IsPause()) // El juego está en pausa y va a dejar de estarlo
            { _gameManager.SetState(GameState.Playing); IsMouseVisible = false; }
            else // Se está jugando y se desea poner en pausa
            { _gameManager.SetState(GameState.Pause); IsMouseVisible = true; }
            // _gameManager.IsPressingPause = true;
            _gameManager.SetPressingPause(true);
        }
        if (kb.IsKeyUp(Keys.P) && kb.IsKeyUp(Keys.Escape) && _gameManager.GetPressingPause()) _gameManager.SetPressingPause(false);

        // Si el juego no está en estado de jugando (está en menú, opciones, etc.)
        // Hago que la cámara orbite sobre el tanque
        if (!_gameManager.IsPlaying())
        { _gameManager.UpdateOrbitAuto(_tank.GetPosition(), dt, 0.35f, 0.25f); IsMouseVisible = true; }

        // Actualizo el GameManager para que actualice todos los objetos del juego
        _gameManager.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        GraphicsDevice.Clear(ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
        GraphicsDevice.DepthStencilState = DepthStencilState.Default;

        // En el GameManager están todos los contenidos
        _gameManager.Draw(_elementosLand, _tank, gameTime, _land);
    }
    protected override void UnloadContent()
    {
        Content.Unload();
        base.UnloadContent();
    }
}
