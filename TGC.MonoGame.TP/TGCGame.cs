﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
    private int _score = 0;
    private float _life = 1f;

    // Tanque del jugador
    private Tank _tank;
    private FollowCamera _camera;
    private ElementosLand _elementosLand;

    private Vector3 CameraPosition = new Vector3(4900f, 2350f, 2200f);

    private GameManager _gameManager;

    /// <summary>
    ///     Constructor del juego.
    /// </summary>
    public TGCGame()
    {
        _gameManager = new GameManager();

        // Maneja la configuracion y la administracion del dispositivo grafico.
        _graphics = new GraphicsDeviceManager(this);
        Window.Title = "TankWars";

        _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100;
        _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;

        // Para que el juego sea pantalla completa se puede usar Graphics IsFullScreen.
        // Carpeta raiz donde va a estar toda la Media.
        Content.RootDirectory = "Content";
        // Hace que el mouse sea visible.
        IsMouseVisible = true;
    }

    /// <summary>
    ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
    ///     Escribir aqui el codigo de inicializacion: el procesamiento que podemos pre calcular para nuestro juego.
    /// </summary>
    protected override void Initialize()
    {
        // La logica de inicializacion que no depende del contenido se recomienda poner en este metodo.

        // Apago el backface culling.
        // Esto se hace por un problema en el diseno del modelo del logo de la materia.
        // Una vez que empiecen su juego, esto no es mas necesario y lo pueden sacar.
        var rasterizerState = new RasterizerState();
        rasterizerState.CullMode = CullMode.None;
        GraphicsDevice.RasterizerState = rasterizerState;
        // Seria hasta aca.

        // Vector3 cameraPosition = new Vector3(10000f, 20000f, 10000f);
        Vector3 targetPosition = new Vector3(1000f, 490f, 500f);
        _camera = new FollowCamera(GraphicsDevice.Viewport.AspectRatio, CameraPosition, targetPosition);
        
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
        float _dt = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

        // Capturar Input teclado
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            Exit();
        }

        _hud.Update(_score, _life);
        if (Keyboard.GetState().IsKeyDown(Keys.W))
            _tank.MoveForwardTank(gameTime);
        if (Keyboard.GetState().IsKeyUp(Keys.W) && Keyboard.GetState().IsKeyUp(Keys.S))
            _tank.DecelerateTank(gameTime);
        if (Keyboard.GetState().IsKeyDown(Keys.A) && _tank.HasVelocity())
        {
            _tank.RotateTankLeft(gameTime);
            _camera.RotateCameraLeft(gameTime);
        }
        if (Keyboard.GetState().IsKeyDown(Keys.S))
            _tank.MoveBackwardTank(gameTime);
        if (Keyboard.GetState().IsKeyDown(Keys.D) && _tank.HasVelocity())
        {
            _tank.RotateTankRight(gameTime);
            _camera.RotateCameraRight(gameTime);
        }

        _tank.Update(gameTime);
        _camera.UpdateCamera(_tank.Position);

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

        _elementosLand.Draw(gameTime, _camera.ViewMatrix, _camera.ProjectionMatrix);
        
        _tank.Draw(gameTime, _camera.ViewMatrix, _camera.ProjectionMatrix);

        _hud.Draw(_spriteBatch, GraphicsDevice, _tank.Position.X, _tank.Position.Y, _tank.Position.Z);
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