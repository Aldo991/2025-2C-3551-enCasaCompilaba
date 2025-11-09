
using Microsoft.Xna.Framework;

public class Button
{
    private string _text;
    private Color _textColor;
    private Color _backgroundColor;
    private Rectangle _zoneButton;
    public Button(Point point1, Point point2, string text = null)
    {
        var buttonWidth = point2.X - point1.X;
        var buttonHeight = point2.Y - point1.Y;
        _zoneButton = new Rectangle(point1.X, point1.Y, buttonWidth, buttonHeight);
        _backgroundColor = Color.Gray;
        _text = text;
        _textColor = Color.White;
    }
    public Button(Point center, string text = null)
    {
        Point point1 = new Point(center.X - 100, center.Y - 25);
        Point point2 = new Point(center.X + 100, center.Y + 25);
        var buttonWidth = point2.X - point1.X;
        var buttonHeight = point2.Y - point1.Y;
        _zoneButton = new Rectangle(point1.X, point1.Y, buttonWidth, buttonHeight);
        _backgroundColor = Color.Gray;
        _text = text;
        _textColor = Color.White;
    }
    public bool Contains(Point point)
    {
        return _zoneButton.Contains(point);
    }
    public Rectangle GetRectangle()
    {
        return _zoneButton;
    }
    public Vector2 GetCenter()
    {
        return _zoneButton.Center.ToVector2();
    }
    public Color GetBackgroundColor() => _backgroundColor;
    public Color GetTextColor() => _textColor;
    public Color SetTextColor(Color color) => _textColor = color;
}