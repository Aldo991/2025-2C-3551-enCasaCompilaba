﻿#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace TGC.MonoGame.TP;

public class TGCGame : Game
{
    public const string ContentFolder3D = "Models/";
    public const string ContentFolderEffects = "Effects/";
    public const string ContentFolderMusic = "Music/";
    public const string ContentFolderSounds = "Sounds/";
    public const string ContentFolderSpriteFonts = "SpriteFonts/";
    public const string ContentFolderTextures = "Textures/";
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Hud _hud;
    // Tanque del jugador
    private Tank _tank;
    private FollowCamera _camera;
    private ElementosLand _elementosLand;
    private GameManager _gameManager;
    private ProjectileManager _projectileManager;
    private TankManager _tankManager;

    /// <summary>
    ///     Constructor del juego.
    /// </summary>
    public TGCGame()
    {
        // Maneja la configuracion y la administracion del dispositivo grafico.
        _graphics = new GraphicsDeviceManager(this);
        Window.Title = "TankWars";

        _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100;
        _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;

        // Para que el juego sea pantalla completa se puede usar Graphics IsFullScreen.
        // Carpeta raiz donde va a estar toda la Media.
        Content.RootDirectory = "Content";
        // Hace que el mouse sea visible.
        IsMouseVisible = false;
    }

    /// <summary>
    ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
    ///     Escribir aqui el codigo de inicializacion: el procesamiento que podemos pre calcular para nuestro juego.
    /// </summary>
    protected override void Initialize()
    {
        _gameManager = new GameManager();
        _projectileManager = new ProjectileManager();
        _tankManager = new TankManager();

        int centerX = GraphicsDevice.Viewport.Width / 2;
        int centerY = GraphicsDevice.Viewport.Height / 2;
        float radius = 50000;
        _camera = new FollowCamera(GraphicsDevice.Viewport.AspectRatio, centerX, centerY, radius);
        
        base.Initialize();
    }

    /// <summary>
    ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo, despues de Initialize.
    ///     Escribir aqui el codigo de inicializacion: cargar modelos, texturas, estructuras de optimizacion, el procesamiento
    ///     que podemos pre calcular para nuestro juego.
    /// </summary>
    protected override void LoadContent()
    {
        _gameManager.LoadModels(Content);

        // Aca es donde deberiamos cargar todos los contenido necesarios antes de iniciar el juego.
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _hud = new Hud(Content);

        var tankModel = _gameManager.GetModel("tank", 0);
        var tankPosition = new Vector3(1000, 490, 500);
        var tankTexture = _gameManager.GetTexture("tank", 0);
        _tank = new Tank(tankModel, tankPosition, 1f, 0f, tankTexture);
        var projectileModel = _gameManager.GetModel("projectile", 0);
        _tank.SetProjectileModel(projectileModel);

        _elementosLand = new ElementosLand(Content, ContentFolder3D, ContentFolderEffects, _gameManager);
        // acá cargamos TODOS los elementos del escenario

        base.LoadContent();
    }

    /// <summary>
    ///     Se llama en cada frame.
    ///     Se debe escribir toda la logica de computo del modelo, asi como tambien verisficar entradas del usuario y reacciones
    ///     ante ellas.
    /// </summary>
    protected override void Update(GameTime gameTime)
    {
        // Aca deberiamos poner toda la logica de actualizacion del juego.
        float dt = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
        float totalGameTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

        // Capturar Input teclado
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        if (Keyboard.GetState().IsKeyDown(Keys.W))
            _tank.MoveForwardTank(gameTime);
        if (Keyboard.GetState().IsKeyUp(Keys.W) && Keyboard.GetState().IsKeyUp(Keys.S))
            _tank.DecelerateTank(gameTime);
        if (Keyboard.GetState().IsKeyDown(Keys.A) && _tank.HasVelocity())
            _tank.RotateTankLeft(gameTime);
        if (Keyboard.GetState().IsKeyDown(Keys.S))
            _tank.MoveBackwardTank(gameTime);
        if (Keyboard.GetState().IsKeyDown(Keys.D) && _tank.HasVelocity())
            _tank.RotateTankRight(gameTime);
        if (Keyboard.GetState().IsKeyDown(Keys.P) && !_gameManager.IsPressingPause)
        {
            IsMouseVisible = Pause();
            _gameManager.IsPressingPause = true;
        }
        if (Keyboard.GetState().IsKeyUp(Keys.P) && _gameManager.IsPressingPause)
            _gameManager.IsPressingPause = false;
        if (Keyboard.GetState().IsKeyDown(Keys.F) && !_tank.IsShooting)
        {
            Projectile p = _tank.Shoot();
            _projectileManager.AddProjectile(p);
            _tank.IsShooting = true;
        }
        if (Keyboard.GetState().IsKeyUp(Keys.F) && _tank.IsShooting)
            _tank.IsShooting = false;

        /************************ BORRAR ************************
        if (Keyboard.GetState().IsKeyDown(Keys.Up) && !_pressingUp)
        {
            _tank.AumentarNumeroEnUno();
            _pressingUp = true;
        }
        if (Keyboard.GetState().IsKeyUp(Keys.Up) && _pressingUp)
            _pressingUp = false;
        if (Keyboard.GetState().IsKeyDown(Keys.Down) && !_pressingDown)
        {
            _tank.DisminuirNumeroEnUno();
            _pressingDown = true;
        }
        if (Keyboard.GetState().IsKeyUp(Keys.Down) && _pressingDown)
            _pressingDown = false;
        ************************ BORRAR ************************/

        if (!_gameManager.IsPause)
        {
            int mousePositionX = Mouse.GetState().X;
            int mousePositionY = Mouse.GetState().Y;
            _tank.Update(gameTime);
            _camera.UpdateCamera(_tank.Position, mousePositionX, mousePositionY);
            _projectileManager.Update(gameTime);
            // Window.
            int width = GraphicsDevice.Viewport.Width;
            int height = GraphicsDevice.Viewport.Height;
            Mouse.SetPosition(width / 2, height / 2);
        }
        base.Update(gameTime);
    }

    /// <summary>
    ///     Se llama cada vez que hay que refrescar la pantalla.
    ///     Escribir aqui el codigo referido al renderizado.
    /// </summary>
    protected override void Draw(GameTime gameTime)
    {
        // Aca deberiamos poner toda la logia de renderizado del juego.
        GraphicsDevice.Clear(Color.CornflowerBlue);
        GraphicsDevice.DepthStencilState = DepthStencilState.Default;

        // Mesh

        _elementosLand.Draw(gameTime, _camera.ViewMatrix, _camera.ProjectionMatrix);

        _tank.Draw(gameTime, _camera.ViewMatrix, _camera.ProjectionMatrix);

        _projectileManager.Draw(gameTime, _camera.ViewMatrix, _camera.ProjectionMatrix);

        _tankManager.Draw(gameTime, _camera.ViewMatrix, _camera.ProjectionMatrix);

        _hud.Draw(_spriteBatch, GraphicsDevice, _tank);
    }

    private bool Pause()
    {
        var actualPause = _gameManager.IsPause;
        _gameManager.IsPause = !_gameManager.IsPause;
        if (actualPause)
            return false;
        else
            return true;
    }

    /// <summary>
    ///     Libero los recursos que se cargaron en el juego.
    /// </summary>
    protected override void UnloadContent()
    {
        // Libero los recursos.
        Content.Unload();

        base.UnloadContent();
    }
}