using Microsoft.Xna.Framework;

namespace FlappyBird;

public class Bird
{
    public Vector2 Position { get; private set; }

    private float _velocity;

    // Storleken som fågelbilden ritas i
    private const int Width = 90;
    private const int Height = 75;

    private const float Gravity = 900f;
    private const float JumpStrength = -350f;

    public Bird(Vector2 startPosition)
    {
        Position = startPosition;
    }

    public void Update(float deltaTime)
    {
        // Lägg på gravitation så att fågeln accelererar nedåt
        _velocity += Gravity * deltaTime;

        // Flytta fågeln utifrån dess nuvarande hastighet
        Position = new Vector2(
            Position.X,
            Position.Y + _velocity * deltaTime);
    }

    public void Jump()
    {
        // Negativ hastighet gör att fågeln rör sig uppåt
        _velocity = JumpStrength;
    }

    public Rectangle GetDrawRectangle()
    {
        // Rektangeln som används när fågelbilden ritas
        return new Rectangle(
            (int)Position.X,
            (int)Position.Y,
            Width,
            Height);
    }

    public Rectangle GetBounds()
    {
        // Mindre hitbox än själva bilden
        // så att kollisionerna känns mer rättvisa
        int hitboxWidth = 60;
        int hitboxHeight = 45;

        // Centrerar hitboxen ungefär i fågelns kropp
        int offsetX = (Width - hitboxWidth) / 2;
        int offsetY = (Height - hitboxHeight) / 2;

        return new Rectangle(
            (int)Position.X + offsetX,
            (int)Position.Y + offsetY,
            hitboxWidth,
            hitboxHeight);
    }

    public void Reset(Vector2 startPosition)
    {
        // Återställ position och hastighet
        Position = startPosition;
        _velocity = 0f;
    }

    public float GetVelocity()
    {
        // Används bland annat för att rotera fågeln
        // beroende på om den stiger eller faller
        return _velocity;
    }

    public int GetWidth()
    {
        return Width;
    }

    public int GetHeight()
    {
        return Height;
    }
}