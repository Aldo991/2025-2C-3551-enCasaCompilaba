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
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

#endregion

public enum GameState
{
    Menu, Options, Playing, Exit
}
public class GameManager
{
    private static readonly Vector3 LigthPosition = new Vector3(10000, 500, 10000);
    private static readonly Vector3 Ambientcolor = Color.LightYellow.ToVector3();
    private static readonly Vector3 SpecularColor = Color.White.ToVector3();
    private static int TotalRounds;
    private static int ActualRound;
    private static int EnemiesPerRound;
    private static float KAmbient = 0.2f;
    private static float KDiffuse = 0.6f;
    private static float KSpecular = 0.4f;
    private const float Shininess = 15f;
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
    private static ProjectileManager _projectileManager;
    private static TankManager _tankManager;
    // Indica si se está pausa, para evitar que se pause 60 veces por segundo
    private bool _isPressingPause;
    // Cámara del juego
    private static FollowCamera _camera;
    // Da la instancia del GameManager, la idea es usarlo como singleton
    private static PhysicsManager _physicManager;
    private static GraphicsDevice _graphicsDevice;
    private static ElementosLand _GameElements;
    private static Land _land;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = new GameManager();
            return instance;
        }
    }
    // Inicializa las variables independientes del GameManager
    public void InitializeIndependentContent(GraphicsDevice graphicsDevice)
    {
        TotalRounds = 3;
        ActualRound = 0;
        EnemiesPerRound = 1;
        _graphicsDevice = graphicsDevice;
        _physicManager = PhysicsManager.Instance;
        _land = new Land();
        _GameElements = new ElementosLand();
        SetScreenInfo(graphicsDevice);
        _state = GameState.Menu;
        _projectileManager = new ProjectileManager();
        InitializeCamera(graphicsDevice);
    }
    // Inicializa las variables dependientes del tanque del GameManager
    public void InitializeDependentContent(Tank player)
    {
        _tankManager = new TankManager(player);
        _hud = new Hud(_graphicsDevice, player);
        _hud.SetHudState(GameState.Menu);
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
    public static void CreateEnemies(int amount)
    {
        for (int i = 0; i < amount; i++)
        {    
            Model tankModel = ContentLoader.GetModel("tank", 0);
            Random random = new Random();
            Vector3 enemyPosition;
            if (i == 0)
                enemyPosition = new Vector3(0, 30, 0);
            else
                enemyPosition = new Vector3(-10, 30, 10);
            // Vector3 enemyPosition = new Vector3(random.NextSingle()*100, 30f, random.NextSingle() * 100);
            // float enemyRotation = random.NextSingle();
            float enemyRotation = 0f;
            int textureIndex = (int)random.NextInt64() % 3;
            textureIndex = (int)MathF.Abs(textureIndex);
            Texture2D enemyTexture = ContentLoader.GetTexture("tank", textureIndex);
            Tank enemy = new Tank(tankModel, enemyPosition, Tank.DefaultScale, enemyRotation, enemyTexture);
            Model enemyProjectileModel = ContentLoader.GetModel("projectile", 0);
            enemy.SetProjectileModel(enemyProjectileModel);
            enemy.SetIsPlayer(false);
            Texture2D tankNormal = ContentLoader.GetNormal("tank", 0);
            enemy.SetNormal(tankNormal);
            Texture2D projectileTexture = ContentLoader.GetTexture("projectile", 0);
            enemy.SetProjectileTexture(projectileTexture);
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
        if (_state != GameState.Playing)
            {
                if (MediaPlayer.State != MediaState.Playing)
                {
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Volume = 0.4f;
                    MediaPlayer.Play(ContentLoader.GetWarBackground());
                }
            }
            else
            {
                // Si está jugando -> detener la música del menú
                if (MediaPlayer.State == MediaState.Playing)
                    MediaPlayer.Stop();
            }
            
    }
    public void Draw(Tank player, GameTime gameTime)
    {
        Matrix view = _camera.ViewMatrix;
        Matrix projection = _camera.ProjectionMatrix;
        if (_state == GameState.Playing)
            player.Draw(gameTime, view, projection);
        _GameElements.Draw(gameTime, _camera);
        _projectileManager.Draw(gameTime, view, projection);
        _tankManager.Draw(gameTime, view, projection);
        _land.Draw(view, projection);
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
        float radius = 15f;
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
    #region StaticMethods
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
    public static BodyHandle AddBodyToSimulation(BodyDescription body, GameObject gameObject)
        => _physicManager.AddBody(body, gameObject);
    public static BodyReference GetBodyReference(BodyHandle bodyHandle)
        => _physicManager.GetBodyReference(bodyHandle);
    public static GraphicsDevice GetGraphicsDevice() => _graphicsDevice;
    public static void SetAwakeTrue(BodyHandle bodyHandle)
    {
        var body = _physicManager.GetBodyReference(bodyHandle);
        body.Awake = true;
    }
    public static void RemoveProjectileFromProjectileManager(Projectile projectile)
        => _projectileManager.DeleteProjectile(projectile);
    public static void RemoveTankFromTankManager(Tank tank)
    {
        _tankManager.DeleteTank(tank);
        if (TotalEnemies() == 0)
        {
            if (GetActualRound() < GetMaxRounds())
            {
                CreateEnemies(EnemiesPerRound);
                ActualRound += 1;
            }
            // else
                // _state = GameState.Win;
        }
    }
    public static void SetIluminationParameters(
        Effect effect,
        Matrix inverseTransposeWorld,
        Vector3 specularColor
    )
    {
        Vector3 eyePosition = GetCameraPosition();
        
        effect.Parameters["EyePosition"].SetValue(eyePosition);
        effect.Parameters["InverseTransposeWorld"].SetValue(inverseTransposeWorld);
        effect.Parameters["LightPosition"].SetValue(LigthPosition);
        effect.Parameters["AmbientColor"].SetValue(Ambientcolor);
        effect.Parameters["SpecularColor"].SetValue(specularColor);
        effect.Parameters["KAmbient"].SetValue(KAmbient);
        effect.Parameters["KDiffuse"].SetValue(KDiffuse);
        effect.Parameters["KSpecular"].SetValue(KSpecular);
        effect.Parameters["Shininess"].SetValue(Shininess);
    }
    public static int TotalEnemies() => _tankManager.GetSize();
    public static List<Vector3> GetEnemiesPosition() => _tankManager.GetPositions();
    public static void ModificarKAmbiente(float cant)
    {
        KAmbient += cant;
        if (KAmbient > 1)
            KAmbient = 1;
        else if (KAmbient < 0)
            KAmbient = 0;
    }
    public static void ModificarKDiffuse(float cant)
    {
        KDiffuse += cant;
        if (KDiffuse > 1)
            KDiffuse = 1;
        else if (KDiffuse < 0)
            KDiffuse = 0;
    }
    public static void ModificarKSpecular(float cant)
    {
        KSpecular += cant;
        if (KSpecular > 1)
            KSpecular = 1;
        else if (KSpecular < 0)
            KSpecular = 0;
    }
    public static void ChangeMaxRounds(int i) => TotalRounds += i;
    public static void ChangeEnemiesPerRound(int i) => EnemiesPerRound += i;
    public static int GetMaxRounds() => TotalRounds;
    public static int GetActualRound() => ActualRound;
    public static int GetEnemiesPerRound() => EnemiesPerRound;
    public static void Restart()
    {
        Tank player = GetPlayer();
        _tankManager.DeleteAll();
        ActualRound = 0;
        player.SetScore(0);
        player.SetKills(0);
        CreateEnemies(EnemiesPerRound);
    }
    public static Tank GetPlayer() => _tankManager.GetPlayer();
    #endregion
}