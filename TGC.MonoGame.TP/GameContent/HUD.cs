#region File Description
/// HUD se encarga de dibujar el HUD del juego en la pantalla
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace TGC.MonoGame.TP;

internal class Hud
{
    private readonly SpriteFont _font;
    private readonly Texture2D _lifeBarTexture;
    private SpriteBatch _spriteBatch;
    private GraphicsDevice _graphicsDevice;
    private Texture2D _pixel;
    private Rectangle _btnJugar, _btnOpciones, _btnExit;
    private bool _showScoreboard;
    // private GameManager _gameManager;
    public Hud(ContentManager content, GraphicsDevice graphicsDevice)
    {
        _font = ContentLoader.GetSpriteFont();
        _lifeBarTexture = content.Load<Texture2D>("hud/health");
        _spriteBatch = new SpriteBatch(graphicsDevice);
        _graphicsDevice = graphicsDevice;
        // _gameManager = GameManager.Instance;
        // _gameManager = new GameManager();
        _pixel = new Texture2D(graphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });
        // _showScoreboard = false;

        var vp = graphicsDevice.Viewport;
        int vw = vp.Width;
        int vh = vp.Height;
        int bw = Math.Max(220, vw / 5);
        int bh = Math.Max(60, vh / 12);
        int cx = vw / 2;
        int cy = vh / 2;
        _btnJugar = new Rectangle(cx - bw / 3, cy - bh - 12, bw, bh);
        _btnOpciones = new Rectangle(cx - bw / 3, cy + 12, bw, bh);
        _btnExit = new Rectangle(cx - bw / 3, cy + 100, bw, bh);
    }
    public void SetScoreboard(bool mode) => _showScoreboard = mode;
    public void Update(GameManager gameManager)
    {
        if (gameManager.GetState() == GameState.Menu || gameManager.GetState() == GameState.Pause)
        {
            var ms = Mouse.GetState();
            bool pressed = ms.LeftButton == ButtonState.Pressed;
            if (pressed && !gameManager.GetMousePressedLast())
            {
                var pt = new Point(ms.X, ms.Y);
                if (_btnJugar.Contains(pt)) { gameManager.SetState(GameState.Playing); }
                else if (_btnOpciones.Contains(pt)) { gameManager.SetState(GameState.Options); }
                else if (_btnExit.Contains(pt)) { gameManager.SetState(GameState.Exit); }
            }
            gameManager.SetMousePressedLast(pressed);
        }
    }
    public void Draw(Tank player)
    {
        var viewport = _graphicsDevice.Viewport;
        var screenWidth = viewport.Width;
        var screenHeight = viewport.Height;

        _spriteBatch.Begin();

        // Score en la esquina superior izquierda
        _spriteBatch.DrawString(_font, $"Bajas/Muertes: {player.GetScore()}", new Vector2(20, 20), Color.White);
        _spriteBatch.DrawString(_font, $"X: {player.GetPosition().X}", new Vector2(20, 60), Color.White);
        _spriteBatch.DrawString(_font, $"Y: {player.GetPosition().Y}", new Vector2(20, 80), Color.White);
        _spriteBatch.DrawString(_font, $"Z: {player.GetPosition().Z}", new Vector2(20, 100), Color.White);

        float lifeBarWidthPercent = 0.25f;   // 25% del ancho de la pantalla
        float lifeBarHeightPercent = 0.04f;  // 4% de la altura de la pantalla
        float paddingPercent = 0.02f;        // 2% de padding desde los bordes

        int lifeBarWidth = (int)(screenWidth * lifeBarWidthPercent);
        int lifeBarHeight = (int)(screenHeight * lifeBarHeightPercent);
        int padding = (int)(screenHeight * paddingPercent);

        // Texto "Salud" encima de la barra
        _spriteBatch.DrawString(_font, "Salud", new Vector2(screenWidth * 0.02f, screenHeight - lifeBarHeight - padding - _font.MeasureString("Salud").Y), Color.White);

        // Barra de vida
        /*
        _spriteBatch.Draw(
            _lifeBarTexture,
            new Rectangle((int)(screenWidth * 0.02f), screenHeight - lifeBarHeight - padding, (int)(lifeBarWidth * player.Life), lifeBarHeight),
            Color.Red
        );
        */

        _spriteBatch.End();
        if (_showScoreboard)
        {
            var vpSB = _graphicsDevice.Viewport;
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
            var titleSize = _font.MeasureString(title);
            var titlePos = new Vector2(panel.X + (panel.Width - titleSize.X) / 2f, panel.Y + 12);
            _spriteBatch.DrawString(_font, title, titlePos, Color.Yellow);

            float colLeft = panel.X + 24f;
            float colMid = panel.X + panel.Width * 0.65f;
            float row = titlePos.Y + titleSize.Y + 18f;
            _spriteBatch.DrawString(_font, "Jugador", new Vector2(colLeft, row), Color.LightGray);
            _spriteBatch.DrawString(_font, "Puntos", new Vector2(colMid, row), Color.LightGray);

            row += _font.LineSpacing * 1.2f;
            string playerName = "Tú";
            _spriteBatch.DrawString(_font, playerName, new Vector2(colLeft, row), Color.White);
            _spriteBatch.DrawString(_font, player.GetScore().ToString(), new Vector2(colMid, row), Color.White);

            _spriteBatch.End();
        }
        _graphicsDevice.DepthStencilState = DepthStencilState.Default;
    }
    public void DrawMenu()
    {
        var vp = _graphicsDevice.Viewport;
        Color gray = new Color(20, 20, 20, 220);
        _spriteBatch.Begin();

        _spriteBatch.Draw(_pixel, new Rectangle(0, 0, vp.Width, vp.Height), new Color(0, 0, 0, 180));
        string title = "TankWars";
        var titleSize = _font.MeasureString(title);
        var center = new Vector2(vp.Width / 2f, vp.Height / 5f);
        _spriteBatch.DrawString(_font, title, center - titleSize / 2f, Color.Yellow);
        _spriteBatch.Draw(_pixel, _btnJugar, gray);
        _spriteBatch.Draw(_pixel, _btnOpciones, gray);
        _spriteBatch.Draw(_pixel, _btnExit, gray);
        var jugarSize = _font.MeasureString("Jugar");
        var opcSize = _font.MeasureString("Opciones");
        var exitSize = _font.MeasureString("Salir");
        _spriteBatch.DrawString(_font, "Jugar", new Vector2(_btnJugar.X + (_btnJugar.Width - jugarSize.X) / 2f, _btnJugar.Y + (_btnJugar.Height - jugarSize.Y) / 2f), Color.White);
        _spriteBatch.DrawString(_font, "Opciones", new Vector2(_btnOpciones.X + (_btnOpciones.Width - opcSize.X) / 2f, _btnOpciones.Y + (_btnOpciones.Height - opcSize.Y) / 2f), Color.White);
        _spriteBatch.DrawString(_font, "Salir", new Vector2(_btnExit.X + (_btnExit.Width - exitSize.X) / 2f, _btnExit.Y + (_btnExit.Height - exitSize.Y) / 2f), Color.White);

        _spriteBatch.End();
    }
    public void DrawOptions()
    {
        var vp = _graphicsDevice.Viewport;
        _spriteBatch.Begin();

        _spriteBatch.Draw(_pixel, new Rectangle(0, 0, vp.Width, vp.Height), new Color(0, 0, 0, 180));
        string title = "Opciones";
        var titleSize = _font.MeasureString(title);
        var center = new Vector2(vp.Width / 2f, vp.Height / 5f);
        _spriteBatch.DrawString(_font, title, center - titleSize / 2f, Color.Yellow);

        _spriteBatch.DrawString(_font, "Sensibilidad: mover mouse", new Vector2(40, vp.Height * 0.6f), Color.White);
        _spriteBatch.DrawString(_font, "Volumen: (placeholder)", new Vector2(40, vp.Height * 0.65f), Color.White);
        _spriteBatch.DrawString(_font, "[Enter] Volver", new Vector2(40, vp.Height * 0.75f), Color.LightGray);

        _spriteBatch.End();
    }
}