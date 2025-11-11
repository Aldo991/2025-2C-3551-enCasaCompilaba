using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP;
public class Menu : HudState
{
    private bool _mousePressedLast;
    private Button _playButton;
    private Button _optionButton;
    private Button _exitButton;
    private GameManager _gameManager;
    public Menu(GraphicsDevice graphicsDevice) : base(graphicsDevice)
    {
        _gameManager = GameManager.Instance;
        _mousePressedLast = false;

        int width = GameManager.GetScreenWidth();
        int height = GameManager.GetScreenHeight();
        int bw = Math.Max(220, width / 5);
        int bh = Math.Max(60, height / 12);
        var centerX = width / 2;
        var centerY = height / 2;

        var centerYPlay = height / 6;
        var centerYOptions = height * 3 / 6;
        var centerYExit = height * 5 / 6;

        var play1 = new Point(centerX - bw / 3, centerY - bh - 12);
        var play2 = new Point(play1.X + bw, play1.Y + bh);

        var options1 = new Point(centerX - bw / 3, centerY - bh + 12);
        var options2 = new Point(options1.X + bw, options1.Y + bh);

        var exit1 = new Point(centerX - bw / 3, centerY - bh + 100);
        var exit2 = new Point(exit1.X + bw, exit1.Y + bh);

        _playButton = new Button(new Point(centerX, centerYPlay));
        _optionButton = new Button(new Point(centerX, centerYOptions));
        _exitButton = new Button(new Point(centerX, centerYExit));
    }
    public override void Update(GameTime gameTime)
    {
        var mouseX = GameManager.GetMousePositionX();
        var mouseY = GameManager.GetMousePositionY();
        var point = new Point(mouseX, mouseY);
        bool leftMouseButtonPressed = GameManager.GetLeftButtonMousePressed();

        if (_playButton.Contains(point))
            _playButton.SetTextColor(Color.Blue);
        else
            _playButton.SetTextColor(Color.White);
        if (_optionButton.Contains(point))
            _optionButton.SetTextColor(Color.Blue);
        else
            _optionButton.SetTextColor(Color.White);
        if (_exitButton.Contains(point))
            _exitButton.SetTextColor(Color.Blue);
        else
            _exitButton.SetTextColor(Color.White);

        if (leftMouseButtonPressed && !_mousePressedLast)
        {
            if (_playButton.Contains(point)) {
                _gameManager.SetState(GameState.Playing);
                Mouse.SetPosition(GameManager.GetScreenCenterWidth(), GameManager.GetScreenCenterHeight());
            }
            else if (_optionButton.Contains(point)) { _gameManager.SetState(GameState.Options); }
            else if (_exitButton.Contains(point)) { _gameManager.SetState(GameState.Exit); }
        }
        _mousePressedLast = leftMouseButtonPressed;
    }
    public override void Draw()
    {
        // Ancho y altura de la pantalla
        int widht = GameManager.GetScreenWidth();
        int height = GameManager.GetScreenHeight();

        _spriteBatch.Begin();
        // Fondo del menú
        _spriteBatch.Draw(_pixel, new Rectangle(0, 0, widht, height), new Color(0, 0, 0, 180));

        string title = "TankWars";
        var titleSize = _font.MeasureString(title);
        var center = new Vector2(widht / 2f, height / 5f);

        _spriteBatch.DrawString(_font, title, center - titleSize / 2f, Color.Yellow);

        _spriteBatch.Draw(_pixel, _playButton.GetRectangle(), _playButton.GetBackgroundColor());
        _spriteBatch.Draw(_pixel, _optionButton.GetRectangle(), _optionButton.GetBackgroundColor());
        _spriteBatch.Draw(_pixel, _exitButton.GetRectangle(), _exitButton.GetBackgroundColor());
        var jugarSize = _font.MeasureString("Jugar");
        var opcSize = _font.MeasureString("Opciones");
        var exitSize = _font.MeasureString("Salir");
        _spriteBatch.DrawString(_font, "Jugar", _playButton.GetCenter() - jugarSize/2f, _playButton.GetTextColor());
        _spriteBatch.DrawString(_font, "Opciones", _optionButton.GetCenter() - opcSize/2f, _optionButton.GetTextColor());
        _spriteBatch.DrawString(_font, "Salir", _exitButton.GetCenter() - exitSize / 2f, _exitButton.GetTextColor());

        _spriteBatch.End();
    }
}