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

    public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
    {
        var viewport = graphicsDevice.Viewport;
        var screenWidth = viewport.Width;

        spriteBatch.Begin();

        // Score en la esquina superior izquierda
        spriteBatch.DrawString(_font, $"Score: {_score}", new Vector2(20, 20), Color.White);

        // Barra de vida
        spriteBatch.DrawString(_font, "Life", new Vector2(20, 60), Color.White);
        spriteBatch.Draw(_lifeBarTexture,
            new Rectangle(100, 60, (int)(200 * _life), 20), Color.Red);


        spriteBatch.End();
    }
}