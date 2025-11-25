
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP;

public class Options : HudState
{
    private struct OptionTextPositions
    {
        public Point position;
        public Func<string> text;
    };
    private List<Button> _buttons;
    private List <OptionTextPositions> _texts;
    private bool _mousePressedLast;
    private GameManager _gameManager;
    private Tank _player;
    public Options() : base()
    {
        _player = GameManager.GetPlayer();
        _gameManager = GameManager.Instance;
        _mousePressedLast = false;
        _buttons = new List<Button>();
        _texts = new List<OptionTextPositions>();

        int width = GameManager.GetScreenWidth();
        int height = GameManager.GetScreenHeight();

        // estas son las medidas de un cuadrado donde van a ir los botones
        // Por lo menos el botón de volver
        int x1 = (int)(width * 0 / 10);
        int y1 = (int)(height * 6 / 10);
        int x2 = (int)(width * 1 / 10);
        int y2 = (int)(height * 4 / 10);
        Point leftDownMenuButtons = new Point(x1, y1);
        Point rightUpMenuButtons = new Point(x2, y2);

        CreateBackButton(leftDownMenuButtons, rightUpMenuButtons);
        CreateRestartButton(leftDownMenuButtons, rightUpMenuButtons);

        // Estas van a ser las medidas donde van a ir las opciones
        int x1LeftSettings = (int)(width * 3 / 10);
        int y1BottomSettings = (int)(height * 8 / 10);
        int x2RightSettings = (int)(width * 7 / 10);
        int y2UpSettings = (int)(height * 2 / 10);

        Point leftDown = new Point(x1LeftSettings, y1BottomSettings);
        Point rightUp = new Point(x2RightSettings, y2UpSettings);

        // botonTest = new Button(punto1, punto2, "");

        CreateOptions(leftDown, rightUp);
        // CreateAddSensitivityButton();
        // CreateSubstractSensitivityButton();
    }
    private void CreateBackButton(Point leftUp, Point rightDown)
    {
        var x1BackButton = leftUp.X;
        var y1BackButton = leftUp.Y + (rightDown.Y - leftUp.Y) * 4 / 5;
        var x2BackButton = (rightDown.X + leftUp.X) / 2;
        var y2BackButton = rightDown.Y;
        Point backPoint1 = new Point(x1BackButton, y1BackButton);
        Point backPoint2 = new Point(x2BackButton, y2BackButton);
        var backButton = new Button(backPoint1, backPoint2, "Volver");
        backButton._action = () => _gameManager.SetState(GameState.Menu);
        _buttons.Add(backButton);
    }
    private void CreateRestartButton(Point leftUp, Point rightDown)
    {
        int menuHeightBotton = (leftUp.Y - rightDown.Y) / 5;
        int x1RestartButton = leftUp.X;
        int y1RestartButton = leftUp.Y - menuHeightBotton * 4;
        Point restartPoint = new Point(x1RestartButton, y1RestartButton);
        var restartButton = new Button(restartPoint, "Reiniciar");
        restartButton._action = () => GameManager.Restart();
        _buttons.Add(restartButton);
    }
    private void CreateOptions(Point leftDown, Point rightUp)
    {
        // int width = leftDown.X - rightUp.X;
        int height = rightUp.Y - leftDown.Y;
        int optionHeight = (int)(height / 5);
        Point option1 = new Point(leftDown.X, leftDown.Y + 4 * optionHeight);
        Point option11 = new Point(rightUp.X, rightUp.Y - optionHeight);

        CreateMaxRounds(option1, option11);
        option1.Y -= optionHeight;
        option11.Y -= optionHeight;
        CreateTotalEnemies(option1, option11);
        option1.Y -= optionHeight;
        option11.Y -= optionHeight;
        CreateChangeFullScreen(option1, option11);
        option1.Y -= optionHeight;
        option11.Y -= optionHeight;
        CreateChangeVolume(option1, option11);
    }
    private void CreateMaxRounds(Point leftDown, Point rightUp)
    {
        int width = rightUp.X - leftDown.X;
        int y1Buttons = leftDown.Y;

        int x1MinusButton = (int)(width * .70) + leftDown.X;
        Point leftBottomMinus = new Point(x1MinusButton, y1Buttons);
        var removeRound = new Button(leftBottomMinus, "-");
        removeRound._action = () => GameManager.ChangeMaxRounds(-1);

        int x1AddButton = (int)(width * .85) + leftDown.X;
        Point leftBottomAdd = new Point(x1AddButton, y1Buttons);
        var addRound = new Button(leftBottomAdd, "+");
        addRound._action = () => GameManager.ChangeMaxRounds(1);

        _buttons.Add(removeRound);
        _buttons.Add(addRound);
        
        string textTotalRounds = "Rondas totales: ";
        Point positionTotalRounds = leftDown;
        var optionTextPositions = new OptionTextPositions
        {
            position = positionTotalRounds,
            text = () => textTotalRounds + $"{GameManager.GetMaxRounds()}",
        };
        _texts.Add(optionTextPositions);

    }
    private void CreateTotalEnemies(Point leftDown, Point rightUp)
    {
        int width = rightUp.X - leftDown.X;
        int y1Buttons = leftDown.Y;

        int x1MinusButton = (int)(width * .70) + leftDown.X;
        Point leftBottomMinus = new Point(x1MinusButton, y1Buttons);
        var removeEnemy = new Button(leftBottomMinus, "-");
        removeEnemy._action = () => GameManager.ChangeEnemiesPerRound(-1);

        int x1AddButton = (int)(width * .85) + leftDown.X;
        Point leftBottomAdd = new Point(x1AddButton, y1Buttons);
        var addEnemy = new Button(leftBottomAdd, "+");
        addEnemy._action = () => GameManager.ChangeEnemiesPerRound(1);

        _buttons.Add(removeEnemy);
        _buttons.Add(addEnemy);

        string textTotalEnemies = "Enemigo totales por ronda: ";
        Point positionTotalEnemies = leftDown;
        var optionTextPositions = new OptionTextPositions
        {
            position = positionTotalEnemies,
            text = () => textTotalEnemies + $"{GameManager.GetEnemiesPerRound()}",
        };
        _texts.Add(optionTextPositions);        
    }
    private void CreateChangeFullScreen(Point leftDown, Point rightUp)
    {
        int width = rightUp.X - leftDown.X;
        int y1Buttons = leftDown.Y;

        int x1NoFullScreenButton = (int)(width * .7) + leftDown.X;
        Point leftBottomNoFullScreen = new Point(x1NoFullScreenButton, y1Buttons);
        var noFullScreenButton = new Button(leftBottomNoFullScreen, "No");
        noFullScreenButton._action = () => GameManager.SetFullScreen(false);

        int x1FullScreenButton = (int)(width * .85) + leftDown.X;
        Point leftBottomFullScreen = new Point(x1FullScreenButton, y1Buttons);
        var fullScreenButton = new Button(leftBottomFullScreen, "Si");
        fullScreenButton._action = () => GameManager.SetFullScreen(true);

        _buttons.Add(noFullScreenButton);
        _buttons.Add(fullScreenButton);

        string textFullScreen = "Pantalla completa";
        var optionTextPositions = new OptionTextPositions
        {
            position = leftDown,
            text = () => textFullScreen
        };
        _texts.Add(optionTextPositions);
    }
    private void CreateChangeVolume(Point leftDown, Point rightUp)
    {
        int width = rightUp.X - leftDown.X;
        int y1Buttons = leftDown.Y;

        int x1MinusVolume = (int)(width * .7) + leftDown.X;
        Point leftBottomMinusVolume = new Point(x1MinusVolume, y1Buttons);
        var minusVolume = new Button(leftBottomMinusVolume, "-");
        minusVolume._action = () => GameManager.ChangeVolume(-.1f);

        int x1AddVolume = (int)(width * .85) + leftDown.X;
        Point leftBottomAddVolume = new Point(x1AddVolume, y1Buttons);
        var addVolume = new Button(leftBottomAddVolume, "+");
        addVolume._action = () => GameManager.ChangeVolume(.1f);

        _buttons.Add(minusVolume);
        _buttons.Add(addVolume);

        string text = "Volumen general: ";
        var optionTextPosition = new OptionTextPositions
        {
            position = leftDown,
            text = () => text + $"{GameManager.GetVolume()}"
        };
        _texts.Add(optionTextPosition);
    }
    private void DrawButton(Button button)
    {
        // dibujo el botón
        _spriteBatch.Draw(_pixel, button.GetRectangle(), button.GetBackgroundColor());
        // dibujo el texto del botón
        var buttonSize = _font.MeasureString(button.GetText());
        _spriteBatch.DrawString(_font, button.GetText(), button.GetCenter() - buttonSize / 2f, button.GetTextColor());
    }
    private void DrawString(OptionTextPositions optionTextPositions)
    {
        _spriteBatch.DrawString(
            _font,
            optionTextPositions.text(),
            optionTextPositions.position.ToVector2(),
            Color.White
        );
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
        foreach(Button button in _buttons)
        {
            if (button.Contains(point))
                button.SetTextColor(Color.Blue);
            else
                button.SetTextColor(Color.White);
        }
        // reviso si se hizo click en el botón
        if (leftMouseButtonPressed && !_mousePressedLast)
        {
            foreach(Button button in _buttons)
            {
                if (button.Contains(point))
                    button._action();
            }
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

        foreach(var button in _buttons)
            DrawButton(button);
        foreach(var optionPosition in _texts)
            DrawString(optionPosition);

        _spriteBatch.End();
    }
}