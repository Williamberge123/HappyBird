using Microsoft.Xna.Framework;

namespace FlappyBird;

public class Coin
{
    public Vector2 Position { get; private set; }

    // Storleken som myntet ritas i
    private const int Size = 40;

    public bool IsCollected { get; private set; }

    public Coin(Vector2 startPosition)
    {
        Position = startPosition;
        IsCollected = false;
    }

    public void Update(
        float deltaTime,
        float speed)
    {
        // Flytta myntet åt vänster
        // med samma hastighet som pipesen
        Position = new Vector2(
            Position.X - speed * deltaTime,
            Position.Y);
    }

    public Rectangle GetBounds()
    {
        // Hitbox för myntet
        return new Rectangle(
            (int)Position.X,
            (int)Position.Y,
            Size,
            Size);
    }

    public Rectangle GetDrawRectangle()
    {
        // Rektangeln som används när myntet ritas
        return new Rectangle(
            (int)Position.X,
            (int)Position.Y,
            Size,
            Size);
    }

    public void Collect()
    {
        // Markera myntet som insamlat
        IsCollected = true;
    }

    public bool IsOffScreen()
    {
        // Kontrollera om myntet lämnat skärmen
        return Position.X < -Size;
    }
}