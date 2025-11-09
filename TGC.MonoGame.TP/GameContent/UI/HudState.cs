#region File Description
/// La idea de esta interfaz es que la implementen clases que se van a encargar de dibujar
/// el HUD en la pantalla según el estado del juego. Por ejemplo, si el estado del juego
/// es 'Playing', entonces se va a setear una clase HudPlaying que se va a encargar
/// de dibujar las cosas en la pantalla. La clase HUD va a tener un atributo del tipo
/// HudState.
#endregion

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP;

public abstract class HudState
{
    protected GraphicsDevice _graphicsDevice;
    protected SpriteBatch _spriteBatch;
    protected SpriteFont _font;
    protected Texture2D _pixel;
    protected HudState(GraphicsDevice graphicsDevice)
    {
        _graphicsDevice = graphicsDevice;
        _spriteBatch = new SpriteBatch(graphicsDevice);
        _font = ContentLoader.GetSpriteFont();
        // ?????????????
        _pixel = new Texture2D(graphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });
    }
    public abstract void Update(GameTime gameTime);
    public abstract void Draw();
}