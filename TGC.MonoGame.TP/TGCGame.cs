#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
#endregion

namespace TGC.MonoGame.TP;

public class TGCGame : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private Tank _tank;
    private GameManager _gameManager;
    public TGCGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        // Título del juego en la ventana del programa
        Window.Title = "TankWars";
        // Ancho y altura de la ventana
        _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100;
        _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;
        _graphics.ApplyChanges();
        Content.RootDirectory = "Content"; // Carpeta donde está el contenido del juego (modelos, sonidos, etc.)
        // Visibilidad del mouse
        IsMouseVisible = false;
        MediaState state = MediaPlayer.State;
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

        _gameManager.InitializeIndependentContent(GraphicsDevice, _graphics);
        InitializeTank();
        // Inicializo el Game Manager
        _gameManager.InitializeDependentContent(_tank);
        // Cargo los elementos del mundo, esto debería ir en GameManager?
        /// todo: revisar si esto debería ir en GameManager y pasarlo

        // Instancio el tanque con todo lo necesario para funcionar. Este tanque es el del
        // Personaje que vamos a controlar. Revisar si debería estar en GameManager
        // Todo: revisar si debería estar en GameManager


        GameManager.CreateEnemies(GameManager.GetEnemiesPerRound());

        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        // Seteo información como la posición del mouse
        _gameManager.SetGameInfo();
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
            if (kb.IsKeyUp(Keys.W) && kb.IsKeyUp(Keys.S) && _tank.HasVelocity()) _tank.DecelerateTank(gameTime);
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

            _tank.Update(gameTime);

            var cannonDirection = _tank.GetCannonDirection();
            var cannonPosition = _tank.GetCannonPosition();
            _gameManager.SetCameraBehindTank(cannonPosition, cannonDirection);

            IsMouseVisible = false;
            Mouse.SetPosition(GameManager.GetScreenCenterWidth(), GameManager.GetScreenCenterHeight());
        }
        
        // Si presiono la tecla P o la tecla Escape, pauso el juego
        if ( (kb.IsKeyDown(Keys.P) || kb.IsKeyDown(Keys.Escape)) && !_gameManager.GetPressingPause())
        {
            if (_gameManager.IsPause() && !GameManager.WasDefeated()) // El juego está en pausa y va a dejar de estarlo
            {
                GameManager.SetState(GameState.Playing);
                IsMouseVisible = false;
                _gameManager.SetCameraBehindTank(_tank.GetCannonPosition(), _tank.GetCannonDirection());
                Mouse.SetPosition(GameManager.GetScreenCenterWidth(), GameManager.GetScreenCenterHeight());
                MediaPlayer.Stop();
            }
            else // Se está jugando y se desea poner en pausa
            { GameManager.SetState(GameState.Menu); IsMouseVisible = true; }
            _gameManager.SetPressingPause(true);
        }
        if (kb.IsKeyUp(Keys.P) && kb.IsKeyUp(Keys.Escape) && _gameManager.GetPressingPause())
            _gameManager.SetPressingPause(false);

        // Si el juego no está en estado de jugando (está en menú, opciones, etc.)
        // Hago que la cámara orbite sobre el tanque
        if (!_gameManager.IsPlaying())
        { _gameManager.UpdateOrbitAuto(_tank.GetPosition(), gameTime); IsMouseVisible = true; }

        // Actualizo el GameManager para que actualice todos los objetos del juego
        _gameManager.Update(gameTime);
        base.Update(gameTime);
    }
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        GraphicsDevice.BlendState = BlendState.Opaque;
        GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;

        // En el GameManager están todos los contenidos
        _gameManager.Draw(_tank, gameTime);
    }
    protected override void UnloadContent()
    {
        Content.Unload();
        base.UnloadContent();
    }
    private void InitializeTank()
    {
        Model tankModel = ContentLoader.GetModel("tank", 0);
        var height = Land.Height(25f, 30f);
        Vector3 tankPosition = new Vector3(25f, height, 30f);
        Texture2D tankTexture = ContentLoader.GetTexture("tank", 0);
        _tank = new Tank(tankModel, tankPosition, Tank.DefaultScale, 0f, tankTexture);
        Model projectileModel = ContentLoader.GetModel("projectile", 0);
        _tank.SetProjectileModel(projectileModel);
        SoundEffect shootTank = ContentLoader.GetSoundEffect("shoot");
        _tank.SetShootSound(shootTank);
        _tank.SetIsPlayer(true);
        Texture2D tankNormal = ContentLoader.GetNormal("tank", 0);
        _tank.SetNormal(tankNormal);
        Texture2D treadmillsTexture = ContentLoader.GetTexture("tank-treadmills", 0);
        _tank.SetTreadmillTexture(treadmillsTexture);
        Texture2D treadmillsNormal = ContentLoader.GetNormal("tank-treadmills", 0);
        _tank.SetTreadmillNormal(treadmillsNormal);
        Texture2D projectileTexture = ContentLoader.GetTexture("projectile", 0);
        _tank.SetProjectileTexture(projectileTexture);
        Texture2D projectileNormal = ContentLoader.GetNormal("projectile", 0);
        _tank.SetProjectileNormal(projectileNormal);
    }
}
