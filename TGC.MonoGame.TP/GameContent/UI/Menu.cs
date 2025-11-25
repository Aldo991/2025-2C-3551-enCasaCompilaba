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
    private Button test;
    public Menu() : base()
    {
        _mousePressedLast = false;

        int width = GameManager.GetScreenWidth();
        int height = GameManager.GetScreenHeight();

        // estas son las medidas de un cuadradon donde van a ir los botones
        int x1 = (int)Math.Round((double)(width * 0 / 10));
        int y1 = (int)Math.Round((double)(height * 6 / 10));
        int x2 = (int)Math.Round((double)(width * 1 / 10));
        int y2 = (int)Math.Round((double)(height * 4 / 10));
        var point1 = new Point(x1, y1);
        var point2 = new Point(x2, y2);
        test = new Button(point1, point2);
        test.SetBackgroundColor(Color.AliceBlue);

        var x1ExitButton = x1;
        var y1ExitButton = y1 + (y2 - y1) * 4 / 5;
        var x2ExitButton = (x2 + x1) / 2;
        var y2ExitButton = y2;
        Point playPoint1 = new Point(x1ExitButton, y1ExitButton);
        Point playPoint2 = new Point(x2ExitButton, y2ExitButton);

        var x1OptionButton = x1;
        var y1OptionButton = y1 + (y2 - y1) * 2 / 5;
        var x2OptionButton = (x2 + x1) / 2;
        var y2OptionButton = y1 + (y2 - y1) * 3 / 5;
        Point optionPoint1 = new Point(x1OptionButton, y1OptionButton);
        Point optionPoint2 = new Point(x2OptionButton, y2OptionButton);

        var x1PlayButton = x1;
        var y1PlayButton = y1;
        var x2PlayButton = (x2 + x1) / 2;
        var y2PlayButton = y1 + (y2 - y1) / 5;
        Point exitPoint1 = new Point(x1PlayButton, y1PlayButton);
        Point exitPoint2 = new Point(x2PlayButton, y2PlayButton);

        _playButton = new Button(playPoint1, playPoint2, "Jugar");
        _optionButton = new Button(optionPoint1, optionPoint2, "Opciones");
        _exitButton = new Button(exitPoint1, exitPoint2, "Salir");

    }
    public override void Update(GameTime gameTime)
    {
        var mouseX = GameManager.GetMousePositionX();
        var mouseY = GameManager.GetMousePositionY();
        var point = new Point(mouseX, mouseY);
        bool leftMouseButtonPressed = GameManager.GetLeftButtonMousePressed();
        if (!GameManager.WasDefeated())
        {
            if (_playButton.Contains(point))
                _playButton.SetTextColor(Color.Blue);
            else
                _playButton.SetTextColor(Color.White);
        }
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
            if (!GameManager.WasDefeated())
            {
                if (_playButton.Contains(point))
                {
                    GameManager.SetState(GameState.Playing);
                    Mouse.SetPosition(GameManager.GetScreenCenterWidth(), GameManager.GetScreenCenterHeight());
                }
            }
            if (_optionButton.Contains(point)) { GameManager.SetState(GameState.Options); }
            else if (_exitButton.Contains(point)) { GameManager.SetState(GameState.Exit); }
        }
        _mousePressedLast = leftMouseButtonPressed;
    }
    public override void Draw()
    {
        // Ancho y altura de la pantalla
        int width = GameManager.GetScreenWidth();
        int height = GameManager.GetScreenHeight();

        _spriteBatch.Begin();
        // Fondo del menú
        _spriteBatch.Draw(_pixel, new Rectangle(0, 0, width, height), new Color(0, 0, 0, 180));

        string title = "TankWars";
        var titleSize = _font.MeasureString(title);
        var center = new Vector2(width / 2f, height / 5f);

        _spriteBatch.DrawString(_font, title, center - titleSize / 2f, Color.Yellow);

        if (!GameManager.WasDefeated())
            _spriteBatch.Draw(_pixel, _playButton.GetRectangle(), _playButton.GetBackgroundColor());
        _spriteBatch.Draw(_pixel, _optionButton.GetRectangle(), _optionButton.GetBackgroundColor());
        _spriteBatch.Draw(_pixel, _exitButton.GetRectangle(), _exitButton.GetBackgroundColor());
        // _spriteBatch.Draw(_pixel, test.GetRectangle(), test.GetBackgroundColor());
        var jugarSize = _font.MeasureString(_playButton.GetText());
        var opcSize = _font.MeasureString(_optionButton.GetText());
        var exitSize = _font.MeasureString(_exitButton.GetText());
        if (!GameManager.WasDefeated())
            _spriteBatch.DrawString(_font, _playButton.GetText(), _playButton.GetCenter() - jugarSize/2f, _playButton.GetTextColor());
        _spriteBatch.DrawString(_font, _optionButton.GetText(), _optionButton.GetCenter() - opcSize/2f, _optionButton.GetTextColor());
        _spriteBatch.DrawString(_font, _exitButton.GetText(), _exitButton.GetCenter() - exitSize / 2f, _exitButton.GetTextColor());

        _spriteBatch.End();
    }
}