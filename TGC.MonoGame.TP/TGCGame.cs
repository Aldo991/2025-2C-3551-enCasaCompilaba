#region Using Statements
using System;
using System.IO;
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
    private Tank _tank;
    private FollowCamera _camera;
    private ElementosLand _elementosLand;
    private GameManager _gameManager;
    private ProjectileManager _projectileManager;
    private TankManager _tankManager;
    private bool _pressingF9;

    private enum GameState { Menu, Options, Playing }
    private GameState _state = GameState.Menu;
    private SpriteFont _menuFont;
    private Rectangle _btnJugar, _btnOpciones;
    private bool _mousePressedLast;
    private Texture2D _pixel;
    private bool _showScoreboard;

    public TGCGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Window.Title = "TankWars";
        _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100;
        _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;
        Content.RootDirectory = "Content";
        IsMouseVisible = false;
    }

    protected override void Initialize()
    {
        _gameManager = new GameManager();
        _gameManager.Initialize();
        _projectileManager = new ProjectileManager();
        _tankManager = new TankManager();

        int centerX = GraphicsDevice.Viewport.Width / 2;
        int centerY = GraphicsDevice.Viewport.Height / 2;
        float radius = 50000;
        _camera = new FollowCamera(GraphicsDevice.Viewport.AspectRatio, centerX, centerY, radius);
        _camera.SetLockToGun(false);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _gameManager.LoadModels(Content);
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _hud = new Hud(Content);
        _menuFont = Content.Load<SpriteFont>("hud/DefaultFont");
        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });

        _elementosLand = new ElementosLand(Content, ContentFolder3D, ContentFolderEffects, _gameManager);
        // acá cargamos TODOS los elementos del escenario

        var tankModel = _gameManager.GetModel("tank", 0);
        var tankPosition = new Vector3(320, 490, 300);
        var tankTexture = _gameManager.GetTexture("tank", 0);
        _tank = new Tank(tankModel, tankPosition, 1f, 0f, tankTexture);
        _tank.SetGround(_elementosLand);
        var projectileModel = _gameManager.GetModel("projectile", 0);
        _tank.SetProjectileModel(projectileModel);
        // Initialize menu button rectangles (centered)
        var vp = GraphicsDevice.Viewport;
        int vw = vp.Width;
        int vh = vp.Height;
        int bw = Math.Max(220, vw / 5);
        int bh = Math.Max(60, vh / 12);
        int cx = vw / 2;
        int cy = vh / 2;
        _btnJugar = new Rectangle(cx - bw / 2, cy - bh - 12, bw, bh);
        _btnOpciones = new Rectangle(cx - bw / 2, cy + 12, bw, bh);

        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        float dt = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
        var kb = Keyboard.GetState();
        if (kb.IsKeyDown(Keys.Escape)) Exit();
        _showScoreboard = kb.IsKeyDown(Keys.Tab);

        if (_state == GameState.Playing && kb.IsKeyDown(Keys.W)) _tank.MoveForwardTank(gameTime);
        if (_state == GameState.Playing && kb.IsKeyUp(Keys.W) && kb.IsKeyUp(Keys.S)) _tank.DecelerateTank(gameTime);
        if (_state == GameState.Playing && kb.IsKeyDown(Keys.A) && _tank.HasVelocity()) _tank.RotateTankLeft(gameTime);
        if (_state == GameState.Playing && kb.IsKeyDown(Keys.S)) _tank.MoveBackwardTank(gameTime);
        if (_state == GameState.Playing && kb.IsKeyDown(Keys.D) && _tank.HasVelocity()) _tank.RotateTankRight(gameTime);

        if (kb.IsKeyDown(Keys.P) && !_gameManager.IsPressingPause)
        {
            IsMouseVisible = Pause();
            _gameManager.IsPressingPause = true;
        }
        if (kb.IsKeyUp(Keys.P) && _gameManager.IsPressingPause) _gameManager.IsPressingPause = false;

        if (kb.IsKeyDown(Keys.F) && !_tank.IsShooting)
        {
            Projectile p = _tank.Shoot();
            _projectileManager.AddProjectile(p);
            _tank.IsShooting = true;
        }
        if (kb.IsKeyUp(Keys.F) && _tank.IsShooting) _tank.IsShooting = false;

        if (!_gameManager.IsPause)
        {
            var ms = Mouse.GetState();
            int mouseX = ms.X;
            int mouseY = ms.Y;
            int width = GraphicsDevice.Viewport.Width;
            int height = GraphicsDevice.Viewport.Height;

            if (_state == GameState.Menu || _state == GameState.Options)
            {
                _camera.UpdateOrbitAuto(_tank.Position, dt, 0.35f, 0.25f);
                IsMouseVisible = true;
                bool pressed = ms.LeftButton == ButtonState.Pressed;
                if (pressed && !_mousePressedLast)
                {
                    var pt = new Point(ms.X, ms.Y);
                    if (_btnJugar.Contains(pt)) { _state = GameState.Playing; _camera.ResetOrbitBehind(); }
                    else if (_btnOpciones.Contains(pt)) _state = GameState.Options;
                }
                _mousePressedLast = pressed;
            }
            else
            {
                _tank.Update(gameTime);
                var bodyForward = Vector3.Transform(Vector3.Forward, Matrix.CreateRotationY(_tank.Rotation));
                _camera.UpdateOrbitBehind(_tank.Position, bodyForward, mouseX, mouseY);

                var camFwd = _camera.Forward;
                if (camFwd.LengthSquared() > 1e-6f)
                {
                    camFwd.Normalize();
                    float yawAbs = !_tank.ModelZUp ? (float)Math.Atan2(camFwd.X, camFwd.Z)
                                                  : (float)Math.Atan2(camFwd.X, camFwd.Y);
                    float yawRel = MathHelper.WrapAngle(yawAbs - _tank.Rotation + MathHelper.Pi);
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
                }
                IsMouseVisible = false;
                Mouse.SetPosition(width / 2, height / 2);
            }

            _projectileManager.Update(gameTime);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        GraphicsDevice.DepthStencilState = DepthStencilState.Default;

        _elementosLand.Draw(gameTime, _camera.ViewMatrix, _camera.ProjectionMatrix);
        _tank.Draw(gameTime, _camera.ViewMatrix, _camera.ProjectionMatrix);
        _projectileManager.Draw(gameTime, _camera.ViewMatrix, _camera.ProjectionMatrix);
        _tankManager.Draw(gameTime, _camera.ViewMatrix, _camera.ProjectionMatrix);

        if (_state == GameState.Playing)
        {
            _hud.Draw(_spriteBatch, GraphicsDevice, _tank);
            if (_showScoreboard)
            {
                var vpSB = GraphicsDevice.Viewport;
                int pw = (int)(vpSB.Width * 0.55f);
                int ph = (int)(vpSB.Height * 0.55f);
                int px = (vpSB.Width - pw) / 2;
                int py = (vpSB.Height - ph) / 2;
                var panel = new Rectangle(px, py, pw, ph);

                _spriteBatch.Begin();
                _spriteBatch.Draw(_pixel, panel, new Color(0, 0, 0, 200));
                int b = 3;
                _spriteBatch.Draw(_pixel, new Rectangle(panel.X, panel.Y, panel.Width, b), Color.White * 0.7f);
                _spriteBatch.Draw(_pixel, new Rectangle(panel.X, panel.Bottom - b, panel.Width, b), Color.White * 0.7f);
                _spriteBatch.Draw(_pixel, new Rectangle(panel.X, panel.Y, b, panel.Height), Color.White * 0.7f);
                _spriteBatch.Draw(_pixel, new Rectangle(panel.Right - b, panel.Y, b, panel.Height), Color.White * 0.7f);

                string title = "Puntajes";
                var titleSize = _menuFont.MeasureString(title);
                var titlePos = new Vector2(panel.X + (panel.Width - titleSize.X) / 2f, panel.Y + 12);
                _spriteBatch.DrawString(_menuFont, title, titlePos, Color.Yellow);

                float colLeft = panel.X + 24f;
                float colMid = panel.X + panel.Width * 0.65f;
                float row = titlePos.Y + titleSize.Y + 18f;
                _spriteBatch.DrawString(_menuFont, "Jugador", new Vector2(colLeft, row), Color.LightGray);
                _spriteBatch.DrawString(_menuFont, "Puntos", new Vector2(colMid, row), Color.LightGray);

                row += _menuFont.LineSpacing * 1.2f;
                string playerName = "Tú";
                _spriteBatch.DrawString(_menuFont, playerName, new Vector2(colLeft, row), Color.White);
                _spriteBatch.DrawString(_menuFont, _tank.Score.ToString(), new Vector2(colMid, row), Color.White);

                _spriteBatch.End();
            }
        }
        else
        {
            var vp = GraphicsDevice.Viewport;
            _spriteBatch.Begin();
            _spriteBatch.Draw(_pixel, new Rectangle(0, 0, vp.Width, vp.Height), new Color(0, 0, 0, 180));
            string title = _state == GameState.Menu ? "TankWars" : "Opciones";
            var titleSize = _menuFont.MeasureString(title);
            var center = new Vector2(vp.Width / 2f, vp.Height / 5f);
            _spriteBatch.DrawString(_menuFont, title, center - titleSize / 2f, Color.Yellow);

            if (_state == GameState.Menu)
            {
                _spriteBatch.Draw(_pixel, _btnJugar, new Color(20, 20, 20, 220));
                _spriteBatch.Draw(_pixel, _btnOpciones, new Color(20, 20, 20, 220));
                var jugarSize = _menuFont.MeasureString("Jugar");
                var opcSize = _menuFont.MeasureString("Opciones");
                _spriteBatch.DrawString(_menuFont, "Jugar", new Vector2(_btnJugar.X + (_btnJugar.Width - jugarSize.X) / 2f, _btnJugar.Y + (_btnJugar.Height - jugarSize.Y) / 2f), Color.White);
                _spriteBatch.DrawString(_menuFont, "Opciones", new Vector2(_btnOpciones.X + (_btnOpciones.Width - opcSize.X) / 2f, _btnOpciones.Y + (_btnOpciones.Height - opcSize.Y) / 2f), Color.White);
            }
            else
            {
                _spriteBatch.DrawString(_menuFont, "Sensibilidad: mover mouse", new Vector2(40, vp.Height * 0.6f), Color.White);
                _spriteBatch.DrawString(_menuFont, "Volumen: (placeholder)", new Vector2(40, vp.Height * 0.65f), Color.White);
                _spriteBatch.DrawString(_menuFont, "[Enter] Volver", new Vector2(40, vp.Height * 0.75f), Color.LightGray);
            }
            _spriteBatch.End();
        }
    }

    private bool Pause()
    {
        var actualPause = _gameManager.IsPause;
        _gameManager.IsPause = !_gameManager.IsPause;
        return !actualPause;
    }

    protected override void UnloadContent()
    {
        Content.Unload();
        base.UnloadContent();
    }
}
