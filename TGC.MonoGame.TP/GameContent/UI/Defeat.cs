using Microsoft.Xna.Framework;
using TGC.MonoGame.TP;

public class Defeat : HudState
{
    Vector2 screenCenter;
    public override void Update(GameTime gameTime)
    {
        var centerWidth = GameManager.GetScreenCenterWidth();
        var centerHeight = GameManager.GetScreenCenterHeight();
        screenCenter = new Vector2(centerWidth, centerHeight);
    }
    public override void Draw()
    {
        string text = "Perdiste! Tu puntuacion: " + GameManager.GetPlayer().GetScore().ToString();

        _spriteBatch.Begin();
        _spriteBatch.DrawString(_font, text, screenCenter, Color.White);
        _spriteBatch.End();
    }
}