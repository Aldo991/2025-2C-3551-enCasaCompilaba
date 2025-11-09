#region File Description
/// La idea de GameManager es delegar la manipulación de objetos
#endregion

#region Using Statements
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP;
using System;
using Microsoft.Xna.Framework.Input;
#endregion

public enum GameState
{
    Menu, Options, Playing, Exit
}
public class GameManager
{
    private static GameManager instance;
    private GameState _state;
    private Hud _hud;
    private ProjectileManager _projectileManager;
    private TankManager _tankManager;
    private bool _isPressingPause;
    private FollowCamera _camera;
    // Variables globales, posición del mouse, tamaño de pantalla, estado
    // de los botones del mouse, etc.
    private static int _mousePositionX, _mousePositionY;
    private static int _screenWidth, _screenHeight;
    private static bool _leftButtonMousePressed;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
            }
            return instance;
        }
    }
    // Inicializa las variables del GameManager
    public void Initialize(GraphicsDevice graphicsDevice, Tank player)
    {
        SetScreenInfo(graphicsDevice);
        _state = GameState.Menu;
        _projectileManager = new ProjectileManager();
        _tankManager = new TankManager();
        _hud = new Hud(graphicsDevice, player);
        _hud.SetHudState(GameState.Menu);
        InitializeCamera(graphicsDevice);
    }
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
    public void SetScoreboard(bool mode) => _hud.SetScoreboard(mode);
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
    public void UpdateOrbitBehind(Vector3 position, Vector3 bodyForward, int mouseX, int mouseY)
    {
        _camera.UpdateOrbitBehind(position, bodyForward, mouseX, mouseY);
    }
    public void UpdateOrbitAuto(Vector3 position, GameTime gameTime)
    {
        _camera.UpdateOrbitAuto(position, gameTime);
    }
    public void AddToProjectileManager(Projectile projectile)
    {
        _projectileManager.AddProjectile(projectile);
    }
    public Vector3 GetCameraForward() => _camera.Forward;
    public float GetCameraHorizontalAngle() => _camera.GetHorizontalAngle();
    /// <summary>
    /// todo: deberíamos ver como parametrizar esto
    /// </summary>
    /// <param name="graphicsDevice"></param>
    private void InitializeCamera(GraphicsDevice graphicsDevice)
    {
        int centerX = graphicsDevice.Viewport.Width / 2;
        int centerY = graphicsDevice.Viewport.Height / 2;
        float radius = 1500f;
        _camera = new FollowCamera(graphicsDevice.Viewport.AspectRatio, centerX, centerY, radius);
        _camera.SetLockToGun(false);
    }
    public void SetGameInfo(GraphicsDevice graphicsDevice)
    {
        var ms = Mouse.GetState();
        _mousePositionX = ms.X;
        _mousePositionY = ms.Y;
        _leftButtonMousePressed = ms.LeftButton == ButtonState.Pressed;
    }
    private void SetScreenInfo(GraphicsDevice graphicsDevice)
    {
        _screenWidth = graphicsDevice.Viewport.Width;
        _screenHeight = graphicsDevice.Viewport.Height;
    }
    public static int GetMousePositionX() => _mousePositionX;
    public static int GetMousePositionY() => _mousePositionY;
    public static int GetScreenWidth() => _screenWidth;
    public static int GetScreenHeight() => _screenHeight;
    public static int GetScreenCenterWidth() => _screenWidth / 2;
    public static int GetScreenCenterHeight() => _screenHeight / 2;
    public static bool GetLeftButtonMousePressed() => _leftButtonMousePressed;
    /// AUXILIAR, BORRAR
    /*
    public void VectorCamara()
    {
        _camera.UpdateLockedToGun
    }
    */
}