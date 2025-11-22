using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP;

public class Options : HudState
{
    private Button _backButton;
    private Button _addEnemy;
    private Button _deleteEnemy;
    private Button _addSensitivity;
    private Button _substractSensitivity;
    private bool _mousePressedLast;
    private GameManager _gameManager;
    private Tank _player;
    public Options(GraphicsDevice graphicsDevice, Tank player) : base(graphicsDevice)
    {
        _player = player;
        _gameManager = GameManager.Instance;
        _mousePressedLast = false;

        int width = GameManager.GetScreenWidth();
        int height = GameManager.GetScreenHeight();

        // estas son las medidas de un cuadrado donde van a ir los botones
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

        var xSize = x2BackButton - x1BackButton;
        var ySize = y2BackButton - y1BackButton;

        _backButton = new Button(backPoint1, backPoint2, "Volver");

        CreateAddEnemyButton();
        CreateDeleteEnemyButton();
        CreateAddSensitivityButton();
        CreateSubstractSensitivityButton();
    }
    private void CreateAddEnemyButton()
    {
        int width = GameManager.GetScreenWidth();
        int height = GameManager.GetScreenHeight();

        // estas son las medidas de un cuadrado donde van a ir los botones
        /*
        int x1 = (int)Math.Round((double)(width * 4 / 10));
        int y1 = (int)Math.Round((double)(height * 4 / 10));
        int x2 = (int)Math.Round((double)(width * 5 / 10));
        int y2 = (int)Math.Round((double)(height * 6 / 10));

        var x1BackButton = x1;
        var y1BackButton = y1 + (y2 - y1) * 4 / 5;
        var x2BackButton = (x2 + x1) / 2;
        var y2BackButton = y2;
        */
        int x1Button = (int)(width * .60);
        int x2Button = (int)(width * .62);
        int y1Button = (int)(height * .30);
        int y2Button = (int)(height * .33);
        Point backPoint1 = new Point(x1Button, y1Button);
        Point backPoint2 = new Point(x2Button, y2Button);

        _addEnemy = new Button(backPoint1, backPoint2, "+");
        _addEnemy.SetBackgroundColor(Color.Blue);
    }
    private void CreateDeleteEnemyButton()
    {
        Point point = new Point();
        _deleteEnemy = new Button(point, "+");
    }
    private void CreateAddSensitivityButton()
    {
        Point point = new Point(1020, 200);
        _addSensitivity = new Button(point, "+");
        _addSensitivity.SetBackgroundColor(Color.Blue);
    }
    private void CreateSubstractSensitivityButton()
    {
        Point point = new Point(820, 200);
        _substractSensitivity = new Button(point, "-");
        _substractSensitivity.SetBackgroundColor(Color.Blue);
    }
    private void DrawButton(Button button)
    {
        // dibujo el botón
        _spriteBatch.Draw(_pixel, button.GetRectangle(), button.GetBackgroundColor());
        // dibujo el texto del botón
        var buttonSize = _font.MeasureString(button.GetText());
        _spriteBatch.DrawString(_font, button.GetText(), button.GetCenter() - buttonSize / 2f, button.GetTextColor());
    }
    public void SetPlayer(Tank player)
    {
        _player = player;
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
        if (_addSensitivity.Contains(point))
            _addSensitivity.SetTextColor(Color.Blue);
        else
            _addSensitivity.SetTextColor(Color.White);
        if (_substractSensitivity.Contains(point))
            _substractSensitivity.SetTextColor(Color.Blue);
        else
            _substractSensitivity.SetTextColor(Color.White);
        
        // reviso si se hizo click en el botón
        if (leftMouseButtonPressed && !_mousePressedLast)
        {
            if (_backButton.Contains(point))
                _gameManager.SetState(GameState.Menu);
            if (_addSensitivity.Contains(point))
                _player.ChangeSensitivity(0.001f);
            if (_substractSensitivity.Contains(point))
                _player.ChangeSensitivity(-0.001f);

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
        _spriteBatch.DrawString(_font, title, center - titleSize / 2f, Color.Yellow);

        DrawButton(_backButton);
        // DrawButton(_addEnemy);
        DrawButton(_addSensitivity);
        DrawButton(_substractSensitivity);
        /*
        // dibujo el botón volver
        _spriteBatch.Draw(_pixel, _backButton.GetRectangle(), _backButton.GetBackgroundColor());
        // dibujo el texto volver en el botón
        var backSize = _font.MeasureString(_backButton.GetText());
        _spriteBatch.DrawString(_font, _backButton.GetText(), _backButton.GetCenter() - backSize / 2f, _backButton.GetTextColor());
        */

        _spriteBatch.End();
    }
}