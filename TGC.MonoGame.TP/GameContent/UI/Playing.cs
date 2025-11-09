#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace TGC.MonoGame.TP;
public class Playing : HudState
{
    private Tank _player;
    private Texture2D _lifeBarTexture;
    private Texture2D _compass;
    private Rectangle _compassPosition;
    private float _compassAngle;
    private bool _showScoreboard;
    private int _posicionBrujulaX, _posicionBrujulaY;
    private int _fps;
    public Playing(GraphicsDevice graphicsDevice, Tank player) : base(graphicsDevice)
    {
        _player = player;
        _lifeBarTexture = ContentLoader.GetTexture("hud", 1);
        _compass = ContentLoader.GetTexture("hud", 0);
        _showScoreboard = false;
        _posicionBrujulaX = 1700;
        _posicionBrujulaY = 120;
        _compassPosition = new Rectangle(1700, 120, 200, 200);
    }
    public override void Update(GameTime gameTime)
    {
        int mouseX = GameManager.GetMousePositionX();
        // int mouseY = ms.Y;
        // int height = _graphicsDevice.Viewport.Height;
        int centerX = GameManager.GetScreenCenterWidth();
        // int centerY = height / 2;
        int offsetX = mouseX - centerX;
        // int offsetY = mouseY - centerY;
        _compassAngle += offsetX * 0.001f;

        // Actualizo los fps
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _fps = (int) (1 / dt);
    }
    public override void Draw()
    {
        var screenWidth = GameManager.GetScreenWidth();
        var screenHeight = GameManager.GetScreenHeight();

        _spriteBatch.Begin();

        // Score en la esquina superior izquierda
        _spriteBatch.DrawString(_font, $"Bajas/Muertes: {_player.GetScore()}", new Vector2(20, 20), Color.White);
        _spriteBatch.DrawString(_font, $"X: {_player.GetPosition().X}", new Vector2(20, 60), Color.White);
        _spriteBatch.DrawString(_font, $"Y: {_player.GetPosition().Y}", new Vector2(20, 80), Color.White);
        _spriteBatch.DrawString(_font, $"Z: {_player.GetPosition().Z}", new Vector2(20, 100), Color.White);
        _spriteBatch.DrawString(_font, $"X brujula: {_posicionBrujulaX}", new Vector2(20, 120), Color.White);
        _spriteBatch.DrawString(_font, $"Y brujula: {_posicionBrujulaY}", new Vector2(20, 140), Color.White);
        _spriteBatch.DrawString(_font, $"FPS: {_fps}", new Vector2(20, 160), Color.White);

        float lifeBarWidthPercent = 0.25f;   // 25% del ancho de la pantalla
        float lifeBarHeightPercent = 0.04f;  // 4% de la altura de la pantalla
        float paddingPercent = 0.02f;        // 2% de padding desde los bordes

        int lifeBarWidth = (int)(screenWidth * lifeBarWidthPercent);
        int lifeBarHeight = (int)(screenHeight * lifeBarHeightPercent);
        int padding = (int)(screenHeight * paddingPercent);

        // Texto "Salud" encima de la barra
        _spriteBatch.DrawString(_font, "Salud", new Vector2(screenWidth * 0.02f, screenHeight - lifeBarHeight - padding - _font.MeasureString("Salud").Y), Color.White);
        // Barra de vida
        _spriteBatch.Draw(
            _lifeBarTexture,
            new Rectangle((int)(screenWidth * 0.02f), screenHeight - lifeBarHeight - padding, (int)(lifeBarWidth * _player.LifePercent()), lifeBarHeight),
            Color.Red
        );
        /*
        Vector2 direccionBrujula = new Vector2(_player.GetTurretDirection().X, _player.GetTurretDirection().Z);
        direccionBrujula.Normalize();
        float angulosBrujula = MathF.Acos(Vector2.Dot(direccionBrujula, Vector2.UnitX)); // está en radianes
        if (direccionBrujula.Y > 0) { angulosBrujula = MathF.PI * 2 - angulosBrujula; }
        */
        Vector2 origin = new Vector2(_compass.Width / 2, _compass.Height / 2);
        // Dibujo la brújula
        _spriteBatch.Draw(
            _compass,
            _compassPosition,
            null,
            Color.White,
            _compassAngle,
            origin,
            SpriteEffects.None,
            0f
        );

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
            string _playerName = "Tu";
            _spriteBatch.DrawString(_font, _playerName, new Vector2(colLeft, row), Color.White);
            _spriteBatch.DrawString(_font, _player.GetScore().ToString(), new Vector2(colMid, row), Color.White);

            _spriteBatch.End();
        }
    }
}