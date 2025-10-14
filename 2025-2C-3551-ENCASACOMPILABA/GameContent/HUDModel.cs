using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP;

internal class Hud
{
    private readonly SpriteFont _font;
    private readonly Texture2D _lifeBarTexture;

    private int _score;
    private float _life;

    public Hud(ContentManager content)
    {
        _font = content.Load<SpriteFont>("hud/DefaultFont");

        _lifeBarTexture = content.Load<Texture2D>("hud/health");

        // Valores iniciales
        _score = 0;
        _life = 1f;  // de 0 a 1
    }

    public void Update(int score, float life)
    {
        _score = score;
        _life = MathHelper.Clamp(life, 0f,1f);
    }

    public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, float x, float y, float z)
    {
        var viewport = graphicsDevice.Viewport;
        var screenWidth = viewport.Width;
        var screenHeight = viewport.Height;

        spriteBatch.Begin();

        // Score en la esquina superior izquierda
        spriteBatch.DrawString(_font, $"Bajas: {_score}", new Vector2(20, 20), Color.White);
        spriteBatch.DrawString(_font, $"Muertes: {_score}", new Vector2(20, 40), Color.White);
        spriteBatch.DrawString(_font, $"X: {x}", new Vector2(20, 60), Color.White);
        spriteBatch.DrawString(_font, $"Y: {y}", new Vector2(20, 80), Color.White);
        spriteBatch.DrawString(_font, $"Z: {z}", new Vector2(20, 100), Color.White);

        float lifeBarWidthPercent = 0.25f;   // 25% del ancho de la pantalla
        float lifeBarHeightPercent = 0.04f;  // 4% de la altura de la pantalla
        float paddingPercent = 0.02f;        // 2% de padding desde los bordes

        int lifeBarWidth = (int)(screenWidth * lifeBarWidthPercent);
        int lifeBarHeight = (int)(screenHeight * lifeBarHeightPercent);
        int padding = (int)(screenHeight * paddingPercent);

        // Texto "Salud" encima de la barra
        spriteBatch.DrawString(_font, "Salud", new Vector2(screenWidth * 0.02f, screenHeight - lifeBarHeight - padding - _font.MeasureString("Salud").Y), Color.White);

        // Barra de vida
        spriteBatch.Draw(
            _lifeBarTexture,
            new Rectangle((int)(screenWidth * 0.02f), screenHeight - lifeBarHeight - padding, (int)(lifeBarWidth * _life), lifeBarHeight),
            Color.Red
        );

        spriteBatch.End();
    }
}