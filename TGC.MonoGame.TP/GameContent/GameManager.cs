#region File Description
/// La idea de GameManager es delegar la manipulación de objetos
#endregion

#region Using Statements
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP;
using System;
using Microsoft.Xna.Framework.Input;
using BepuPhysics.Collidables;
using BepuPhysics;
#endregion

public enum GameState
{
    Menu, Options, Playing, Exit
}
public class GameManager
{
    private static GameManager instance;
    // Variables globales, posición del mouse, tamaño de pantalla, estado
    // de los botones del mouse, etc.
    private static int _mousePositionX, _mousePositionY;
    private static int _screenWidth, _screenHeight;
    private static bool _leftButtonMousePressed;
    // Estado del juego
    private GameState _state;
    // Hud y UI
    private Hud _hud;
    // ObjectManagers
    private ProjectileManager _projectileManager;
    private TankManager _tankManager;
    // Indica si se está pausa, para evitar que se pause 60 veces por segundo
    private bool _isPressingPause;
    // Cámara del juego
    private static FollowCamera _camera;
    // Da la instancia del GameManager, la idea es usarlo como singleton
    private static PhysicsManager _physicManager;
    private static GraphicsDevice _graphicsDevice;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = new GameManager();
            return instance;
        }
    }
    // Inicializa las variables del GameManager
    public void Initialize(GraphicsDevice graphicsDevice, Tank player)
    {
        _graphicsDevice = graphicsDevice;
        _physicManager = PhysicsManager.Instance;
        SetScreenInfo(graphicsDevice);
        _state = GameState.Menu;
        _projectileManager = new ProjectileManager();
        _tankManager = new TankManager();
        _hud = new Hud(graphicsDevice, player);
        _hud.SetHudState(GameState.Menu);
        InitializeCamera(graphicsDevice);
    }
    public void SetHudPlayer(Tank player) => _hud.SetPlayer(player);
    // Devuelve si el estado del juego es pausa o no
    public bool IsPause() => _state == GameState.Menu;
    // Devuelve si el estado del juego es jugando o no
    public bool IsPlaying() => _state == GameState.Playing;
    // Devuelve si el estado del juego es exit y se debe cerrar
    public bool IsExit() => _state == GameState.Exit;
    // Getter y setter de la variable _pressingPause, que indica si se está apretando la tecla de pausa o no
    public bool GetPressingPause() => _isPressingPause;
    public void SetPressingPause(bool pause) => _isPressingPause = pause;
    // Devuelve el estado del juego
    public GameState GetState() => _state;
    // Setea un estado del juego
    public void SetState(GameState newGameState)
    {
        _state = newGameState;
        _hud.SetHudState(newGameState);
    }
    // Permite mostrar el scoreboard, no está implementado
    public void SetScoreboard(bool mode) => _hud.SetScoreboard(mode);
    // Método auxiliar para crear enemigos
    public void CreateEnemies(int amount)
    {
        for (int i = 0; i < amount; i++)
        {    
            Model tankModel = ContentLoader.GetModel("tank", 1);
            Random random = new Random();
            Vector3 enemyPosition = new Vector3(random.NextSingle(), 30f, random.NextSingle());
            float enemyRotation = random.NextSingle();
            int textureIndex = (int)random.NextInt64() % 3;
            textureIndex = (int)MathF.Abs(textureIndex);
            Texture2D enemyTexture = ContentLoader.GetTexture("tank", textureIndex);
            Tank enemy = new Tank(tankModel, enemyPosition, Tank.DefaultScale, enemyRotation, enemyTexture);
            Model enemyProjectileModel = ContentLoader.GetModel("projectile", 0);
            enemy.SetProjectileModel(enemyProjectileModel);
            enemy.SetIsPlayer(false);
            _tankManager.AddTank(enemy);
        }
    }
    // Update de GameManager, se lo aplica a hud, y los managers de objetos
    public void Update(GameTime gameTime)
    {
        _hud.Update(gameTime);
        _projectileManager.Update(gameTime);
        _tankManager.Update(gameTime);
        _physicManager.Update();
    }
    public void Draw(ElementosLand elementosLand, Tank player, GameTime gameTime, Land land)
    {
        player.Draw(gameTime, _camera.ViewMatrix, _camera.ProjectionMatrix);
        elementosLand.Draw(gameTime, _camera.ViewMatrix, _camera.ProjectionMatrix);
        _projectileManager.Draw(gameTime, _camera.ViewMatrix, _camera.ProjectionMatrix);
        _tankManager.Draw(gameTime, _camera.ViewMatrix, _camera.ProjectionMatrix);
        land.Draw(gameTime, _camera.ViewMatrix, _camera.ProjectionMatrix, Color.Green);
        _hud.Draw();
    }
    // Orbita a través del tanque cuando está en pausa
    public void UpdateOrbitAuto(Vector3 position, GameTime gameTime)
    {
        _camera.UpdateOrbitAuto(position, gameTime);
    }
    // Agrega un projectil al manager
    public void AddToProjectileManager(Projectile projectile)
    {
        _projectileManager.AddProjectile(projectile);
    }
    // Setea la cámara detrás del tanque, apuntando al mismo lugar que apunta el cañón
    public void SetCameraBehindTank(Vector3 target, Vector3 direction)
    {
        _camera.SetCameraDirection(target, direction);
    }
    // Método auxiliar que inicializa la cámara
    private void InitializeCamera(GraphicsDevice graphicsDevice)
    {
        float radius = 1500f;
        _camera = new FollowCamera(graphicsDevice.Viewport.AspectRatio, radius);
    }
    // Setea la información del juego, más que nada del mouse
    public void SetGameInfo()
    {
        var ms = Mouse.GetState();
        _mousePositionX = ms.X;
        _mousePositionY = ms.Y;
        _leftButtonMousePressed = ms.LeftButton == ButtonState.Pressed;
    }
    // Setea información de la pantalla
    private void SetScreenInfo(GraphicsDevice graphicsDevice)
    {
        _screenWidth = graphicsDevice.Viewport.Width;
        _screenHeight = graphicsDevice.Viewport.Height;
    }
    // Entrega información de la posición del mouse y de la pantalla.
    public static int GetMousePositionX() => _mousePositionX;
    public static int GetMousePositionY() => _mousePositionY;
    public static int GetScreenWidth() => _screenWidth;
    public static int GetScreenHeight() => _screenHeight;
    public static int GetScreenCenterWidth() => _screenWidth / 2;
    public static int GetScreenCenterHeight() => _screenHeight / 2;
    public static bool GetLeftButtonMousePressed() => _leftButtonMousePressed;
    public static Vector3 GetCameraPosition() => _camera.GetCameraPosition();
    public static TypedIndex AddShapeToSimulation(Box shape) => _physicManager.AddShape(shape);
    public static TypedIndex AddShapeSphereToSimulation(Sphere shape) => _physicManager.AddShapeSphere(shape);
    public static BodyHandle AddBodyToSimulation(BodyDescription body)
        => _physicManager.AddBody(body);
    public static BodyReference GetBodyReference(BodyHandle bodyHandle)
        => _physicManager.GetBodyReference(bodyHandle);
    public static GraphicsDevice GetGraphicsDevice() => _graphicsDevice;
    public static void SetAwakeTrue(BodyHandle bodyHandle)
    {
        var body = _physicManager.GetBodyReference(bodyHandle);
        body.Awake = true;
    }
}