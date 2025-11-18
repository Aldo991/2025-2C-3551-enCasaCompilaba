using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP;

public class Options : HudState
{
    private Button _backButton;
    private bool _mousePressedLast;
    private GameManager _gameManager;
    public Options(GraphicsDevice graphicsDevice) : base(graphicsDevice)
    {
        _gameManager = GameManager.Instance;
        _mousePressedLast = false;

        int width = GameManager.GetScreenWidth();
        int height = GameManager.GetScreenHeight();

        // estas son las medidas de un cuadradon donde van a ir los botones
        int x1 = (int)Math.Round((double)(width * 0 / 10));
        int y1 = (int)Math.Round((double)(height * 4 / 10));
        int x2 = (int)Math.Round((double)(width * 1 / 10));
        int y2 = (int)Math.Round((double)(height * 6 / 10));

        var x1BackButton = x1;
        var y1BackButton = y1 + (y2 - y1) * 4 / 5;
        var x2BackButton = (x2 + x1) / 2;
        var y2BackButton = y2;
        Point backPoint1 = new Point(x1BackButton, y1BackButton);
        Point backPoint2 = new Point(x2BackButton, y2BackButton);

        _backButton = new Button(backPoint1, backPoint2, "Volver");

    }
    public override void Update(GameTime gameTime)
    {
        var mouseX = GameManager.GetMousePositionX();
        var mouseY = GameManager.GetMousePositionY();
        var point = new Point(mouseX, mouseY);
        bool leftMouseButtonPressed = GameManager.GetLeftButtonMousePressed();
        // pinto del texto de color si el mouse está encima
        if (_backButton.Contains(point))
            _backButton.SetTextColor(Color.Blue);
        else
            _backButton.SetTextColor(Color.White);
        // reviso si se hizo click en e botón
        if (leftMouseButtonPressed && !_mousePressedLast)
        {
            if (_backButton.Contains(point))
            _gameManager.SetState(GameState.Menu);
        }
        _mousePressedLast = leftMouseButtonPressed;
    }
    public override void Draw()
    {
        var width = GameManager.GetScreenWidth();
        var height = GameManager.GetScreenHeight();
        _spriteBatch.Begin();

        // dibujo un fondo gris
        _spriteBatch.Draw(_pixel, new Rectangle(0, 0, width, height), new Color(0, 0, 0, 180));

        string title = "Opciones";
        var titleSize = _font.MeasureString(title);
        var center = new Vector2(width / 2f, height / 5f);

        // dibujo el botón donde va a estar el 
        _spriteBatch.Draw(_pixel, _backButton.GetRectangle(), _backButton.GetBackgroundColor());

        _spriteBatch.DrawString(_font, title, center - titleSize / 2f, Color.Yellow);

        var backSize = _font.MeasureString(_backButton.GetText());
        _spriteBatch.DrawString(_font, _backButton.GetText(), _backButton.GetCenter() - backSize / 2f, _backButton.GetTextColor());

        _spriteBatch.End();
    }
}