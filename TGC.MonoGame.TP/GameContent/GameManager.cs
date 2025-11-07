#region File Description
/// La idea de GameManager es delegar la manipulación de objetos
#endregion

#region Using Statements
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using TGC.MonoGame.TP;
#endregion

public enum GameState
{
    Menu, Options, Playing, Pause, Exit
}
public class GameManager
{
    private static GameManager instance;
    private GameState _state;
    private List<GameObject> _gameObjects;
    private Hud _hud;
    private ProjectileManager _projectileManager;
    private TankManager _tankManager;
    private bool _isPressingPause;
    private bool _mousePressedLast;
    private FollowCamera _camera;
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
    public void Initialize(ContentManager contentManager, GraphicsDevice graphicsDevice)
    {
        _gameObjects = new List<GameObject>();
        _state = GameState.Menu;
        _projectileManager = new ProjectileManager();
        _tankManager = new TankManager();
        _hud = new Hud(contentManager, graphicsDevice);
        _mousePressedLast = false;
        InitializeCamera(graphicsDevice);
    }
    // Devuelve si el estado del juego es pausa o no
    public bool IsPause() => _state == GameState.Pause;
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
    public void SetState(GameState newGameState) => _state = newGameState;
    // Devuelve el valor de la variable _mousePressedLast
    public bool GetMousePressedLast() => _mousePressedLast;
    // Setea un valor para la variable _mousePressedLast
    public void SetMousePressedLast(bool pressed) => _mousePressedLast = pressed;
    public void SetScoreboard(bool mode) => _hud.SetScoreboard(mode);
    // Update de GameManager, se lo aplica a hud, y los managers de objetos
    public void Update(GameTime gameTime)
    {
        _hud.Update(gameTime, this);
        _projectileManager.Update(gameTime);
        _tankManager.Update(gameTime);
    }
    public void Draw(ElementosLand elementosLand, Tank player, GameTime gameTime, Land land)
    {
        if (_state == GameState.Playing)
            _hud.Draw(player);
        player.Draw(gameTime, _camera.ViewMatrix, _camera.ProjectionMatrix);
        elementosLand.Draw(gameTime, _camera.ViewMatrix, _camera.ProjectionMatrix);
        _projectileManager.Draw(gameTime, _camera.ViewMatrix, _camera.ProjectionMatrix);
        _tankManager.Draw(gameTime, _camera.ViewMatrix, _camera.ProjectionMatrix);
        land.Draw(gameTime, _camera.ViewMatrix, _camera.ProjectionMatrix, Color.Green);
        
        if (_state == GameState.Playing)
            _hud.Draw(player);
        if (_state == GameState.Menu || _state == GameState.Pause)
            _hud.DrawMenu();
        if (_state == GameState.Options)
            _hud.DrawOptions();
    }
    public void UpdateOrbitBehind(Vector3 position, Vector3 bodyForward, int mouseX, int mouseY)
    {
        _camera.UpdateOrbitBehind(position, bodyForward, mouseX, mouseY);
    }
    public void UpdateOrbitAuto(Vector3 position, float dt, float angularSpeed, float fixedVerticalAngle)
    {
        _camera.UpdateOrbitAuto(position, dt, angularSpeed, fixedVerticalAngle);
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
        float radius = 500f;
        _camera = new FollowCamera(graphicsDevice.Viewport.AspectRatio, centerX, centerY, radius);
        _camera.SetLockToGun(false);
    }

    /// AUXILIAR, BORRAR
    public void CambiarBrujula(int x, int y)
    {
        _hud.CambiarPosicionBrujula(x, y);
    }
    /*
    public void VectorCamara()
    {
        _camera.UpdateLockedToGun
    }
    */
}