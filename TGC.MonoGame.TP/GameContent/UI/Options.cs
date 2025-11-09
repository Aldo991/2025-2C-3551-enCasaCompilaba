using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP;

public class Options : HudState
{
    public Options(GraphicsDevice graphicsDevice) : base(graphicsDevice)
    {
        
    }
    public override void Update(GameTime gameTime)
    {
        throw new System.NotImplementedException();
    }
    public override void Draw()
    {
        var width = GameManager.GetScreenWidth();
        var height = GameManager.GetScreenHeight();
        _spriteBatch.Begin();

        _spriteBatch.Draw(_pixel, new Rectangle(0, 0, width, height), new Color(0, 0, 0, 180));
        string title = "Opciones";
        var titleSize = _font.MeasureString(title);
        var center = new Vector2(width / 2f, height / 5f);
        _spriteBatch.DrawString(_font, title, center - titleSize / 2f, Color.Yellow);

        _spriteBatch.DrawString(_font, "Sensibilidad: mover mouse", new Vector2(40, height * 0.6f), Color.White);
        _spriteBatch.DrawString(_font, "Volumen: (placeholder)", new Vector2(40, height * 0.65f), Color.White);
        _spriteBatch.DrawString(_font, "[Esc] Volver", new Vector2(40, height * 0.75f), Color.LightGray);

        _spriteBatch.End();
    }
}