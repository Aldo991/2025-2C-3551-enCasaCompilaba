#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace TGC.MonoGame.TP;
public class Playing : HudState
{
    private struct EnemyArrowInfo
    {
        public Rectangle position;
        public float angle;
        public Color color;
    };
    private const float CompassScaleXPosition = .95f;
    private const float CompassScaleYPosition = .1f;
    private const float CompassWidth = .18f;
    private const float CompassHeight = .18f;
    private const float LifeBarScaleXPosition = 0.03f;
    private const float LifeBarScaleYPosition = 0.95f;
    private const float LifeBarWidth = 0.25f;
    private const float LifeBarHeight = 0.04f;
    private const float EnemyArrowsScale = 0.41f;
    private const float InfoWidthScale = 0.011f;
    private const float InfoHeightScale = 0.05f;
    private Tank _player;
    private Texture2D _lifeBarTexture;
    private Rectangle _lifeBarPosition;
    private int _originalWidthLifeBar;
    private Texture2D _compassTexture;
    private Rectangle _compassPosition;
    private float _compassAngle;
    private Texture2D _arrowTexture;
    private List<Rectangle> _arrowsPositions;
    private List<float> _arrowsAngle;
    private List<EnemyArrowInfo> _enemyArrowsInfo;
    private bool _showScoreboard;
    private int _fps;
    public Playing() : base()
    {
        
        _player = GameManager.GetPlayer();
        _compassTexture = ContentLoader.GetTexture("hud", 0);
        _lifeBarTexture = ContentLoader.GetTexture("hud", 1);
        _arrowTexture = ContentLoader.GetTexture("hud", 2);
        _showScoreboard = false;
        _arrowsPositions = new List<Rectangle>();
        _arrowsAngle = new List<float>();
        _enemyArrowsInfo = new List<EnemyArrowInfo>();
        CalculateCompassPosition(); // instancia la variable _compassPosition
        CalculateLifeBarPosition(); // instancia la variable _lifeBarPosition
    }
    private void CalculateCompassPosition()
    {
        var screenWidth = GameManager.GetScreenWidth();
        var screenHeight = GameManager.GetScreenHeight();
        var compassX = (int)Math.Round(screenWidth * CompassScaleXPosition);
        var compassY = (int)Math.Round(screenHeight * CompassScaleYPosition);
        var compassWidth = (int)Math.Round(screenHeight * CompassWidth);
        var compassHeight = (int)Math.Round(screenHeight * CompassHeight);
        _compassPosition = new Rectangle(compassX, compassY, compassWidth, compassHeight);
    }
    private void CalculateLifeBarPosition()
    {
        var screenWidth = GameManager.GetScreenWidth();
        var screenHeight = GameManager.GetScreenHeight();
        var lifeBarX = (int)Math.Round(screenWidth * LifeBarScaleXPosition);
        var lifeBarY = (int)Math.Round(screenHeight * LifeBarScaleYPosition);
        var lifeBarWidth = (int)Math.Round(screenWidth * LifeBarWidth);
        var lifeBarHeight = (int)Math.Round(screenHeight * LifeBarHeight);
        _lifeBarPosition = new Rectangle(lifeBarX, lifeBarY, lifeBarWidth, lifeBarHeight);
        _originalWidthLifeBar = lifeBarWidth;
    }
    private void DrawString(List<string> strings)
    {
        int width = GameManager.GetScreenWidth();
        int height = GameManager.GetScreenHeight();
        // float widthScale = 0.011f;
        // float heightScale = 0.02f;
        int positionX = (int)(width * InfoWidthScale);
        int positionY = (int)(height * InfoHeightScale);
        for (int i = 0; i < strings.Count; i++)
        {
            Vector2 position = new Vector2(positionX, (i+1) * positionY);
            _spriteBatch.DrawString(_font, strings[i], position, Color.White);
        }
    }
    private void CalculateArrowsPosition()
    {
        _enemyArrowsInfo.Clear();
        // Limpio las listas para volverlas a cargar con información
        _arrowsPositions.Clear();
        _arrowsAngle.Clear();
        int height = GameManager.GetScreenHeight();
        // Obtengo la posición y dirección del tanque. Normalizo la dirección.
        Vector2 playerPosition = new Vector2(_player.GetPosition().X, _player.GetPosition().Z);
        Vector2 playerDirection = new Vector2(_player.GetCannonDirection().X, _player.GetCannonDirection().Z);
        playerDirection.Normalize();
        // Obtengo la posición de los enemigos
        List<Vector3> enemiesPosition = GameManager.GetEnemiesPosition();
        List<Vector2> enemiesPositionXZ = new List<Vector2>();
        // Paso las posiciones de los enemigos de Vector3 a Vector2
        foreach(var position in enemiesPosition)
            enemiesPositionXZ.Add(new Vector2(position.X, position.Z));
        // Obtengo los vectores que apuntan desde el jugador hacia los enemigos
        List<Vector2> enemiesDirection = new List<Vector2>();
        foreach(var position in enemiesPositionXZ)
        {
            Vector2 vectorToAdd = position - playerPosition;
            vectorToAdd.Normalize();
            enemiesDirection.Add(vectorToAdd);
        }
        // Calculo el ángulo de rotación de la flecha, está en radianes
        for (int i = 0; i < enemiesPositionXZ.Count; i++)
        {
            // Calculo la dirección del enemigo
            Vector2 enemyPosition = enemiesPositionXZ[i];
            Vector2 enemyDirection = enemyPosition - playerPosition;
            enemyDirection.Normalize();
            // Calculo la posición y el ángulo de la flecha
            float dot = Vector2.Dot(playerDirection, enemyDirection);
            float cross = playerDirection.X * enemyDirection.Y - playerDirection.Y * enemyDirection.X;
            float angleArrow = MathF.Atan2(cross, dot) + MathF.PI / 2;
            Rectangle arrowPosition = new Rectangle(
                (int)(GameManager.GetScreenCenterWidth() + height * EnemyArrowsScale * MathF.Cos(angleArrow)),
                (int)(GameManager.GetScreenCenterHeight() + height * EnemyArrowsScale * MathF.Sin(angleArrow)),
                20,
                20
            );
            // Calculo el alfa de la flecha, dependiendo de la distancia hacia el jugador
            float distance = Vector2.Distance(enemyPosition, playerPosition);
            byte distanceByte = (byte)Math.Clamp(distance, 5, 255);
            Color colorEnemyArrow = Color.White;
            colorEnemyArrow.A = (byte)(255 - distanceByte);
            EnemyArrowInfo info = new EnemyArrowInfo
            {
                position = arrowPosition,
                angle = angleArrow,
                color = colorEnemyArrow
            };
            _enemyArrowsInfo.Add(info);
        }

    }
    private void DrawScoreboard()
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
    public void SetPlayer(Tank player) => _player = player;
    public override void Update(GameTime gameTime)
    {
        Vector3 tankFrontDirection = _player.GetTankFrontDirection();
        // Paso de coordenadas cartesianas (X, Z) a coordenadas polares, donde la función
        // Atan es arcotangente, y me da los ángulos en radianes
        _compassAngle = MathF.Atan2(tankFrontDirection.Z, tankFrontDirection.X);
        float dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        _fps = (int) (1000 / dt);
        var playerLife = _player.LifePercent();
        _lifeBarPosition.Width = (int)(_originalWidthLifeBar * playerLife);

        // calculo donde ubicar las flechas, dependiendo de la ubicación de los enemigos
        CalculateArrowsPosition();
    }
    public override void Draw()
    {

        _spriteBatch.Begin();
        List<string> stringsToDraw = new List<string>();
        string score = "Puntaje: " + _player.GetScore().ToString();
        string kills = "Bajas: " + _player.GetKills().ToString();
        string positionX = "X: " + _player.GetPosition().X.ToString();
        string positionY = "Y: " + _player.GetPosition().Y.ToString();
        string positionZ = "Z: " + _player.GetPosition().Z.ToString();
        string fps = "FPS: " + _fps.ToString();
        string totalEnemies = "Enemigos Restantes: " + GameManager.TotalEnemies().ToString();
        string actualRound = "Ronda actual: " + GameManager.GetActualRound().ToString();
        string totalEnemiesPerRound = "Enemigos por ronda: " + GameManager.GetEnemiesPerRound().ToString();
        string totalRounds = "Rondas totales: " + GameManager.GetMaxRounds().ToString();

        stringsToDraw.Add(score);
        stringsToDraw.Add(kills);
        // stringsToDraw.Add(positionX);
        // stringsToDraw.Add(positionY);
        // stringsToDraw.Add(positionZ);
        stringsToDraw.Add(fps);
        stringsToDraw.Add(totalEnemies);
        stringsToDraw.Add(actualRound);
        stringsToDraw.Add(totalEnemiesPerRound);
        stringsToDraw.Add(totalRounds);

        // Score en la esquina superior izquierda
        DrawString(stringsToDraw);

        // Texto "Salud" encima de la barra
        // _spriteBatch.DrawString(_font, "Salud", new Vector2(screenWidth * 0.02f, screenHeight - lifeBarHeight - padding - _font.MeasureString("Salud").Y), Color.White);
        // Barra de vida
        // Vector2 lifeBarOrigin = new Vector2(_lifeBarTexture.Width / 2, _lifeBarTexture.Height / 2);
        _spriteBatch.Draw(
            _lifeBarTexture,
            _lifeBarPosition,
            Color.Red
        );

        Vector2 origin = new Vector2(_compassTexture.Width / 2, _compassTexture.Height / 2);
        // Dibujo la brújula
        _spriteBatch.Draw(
            _compassTexture,
            _compassPosition,
            null,
            Color.White,
            _compassAngle,
            origin,
            SpriteEffects.None,
            0f
        );

        // for (int i = 0; i < _arrowsPositions.Count; i++)
        foreach (var enemyArrowInfo in _enemyArrowsInfo)
        {
            // Dibujo las flechas que indican las posiciones de los enemigos
            Vector2 originArrow = new Vector2(_arrowTexture.Width / 2, _arrowTexture.Height / 2);
            _spriteBatch.Draw(
                _arrowTexture,
                enemyArrowInfo.position,
                null,
                enemyArrowInfo.color,
                enemyArrowInfo.angle,
                originArrow,
                SpriteEffects.None,
                0f
            );
        }

        _spriteBatch.End();

        if (_showScoreboard)
        {
            DrawScoreboard();
        }
    }
}