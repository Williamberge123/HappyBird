using System;
using Microsoft.Xna.Framework;

namespace FlappyBird;

public class Pipe
{
    public float X { get; private set; }

    // Håller koll på om spelaren redan fått poäng för röret
    public bool HasScored { get; set; }

    private int _topPipeHeight;

    // Hur brett röret ritas
    private const int DrawWidth = 170;

    // Smalare hitbox än grafiken
    private const int HitboxWidth = 80;

    // Mellanrummet mellan övre och nedre röret
    private const int Gap = 135;

    // Extra marginal på hitboxen mot öppningen
    private const int GapPadding = 18;

    // Gör att rören fortsätter utanför skärmen
    // så att änden inte syns vid tak eller mark
    private const int PipeOverdraw = 120;

    private readonly Random _random;

    public Pipe(
        float startX,
        Random random,
        int screenHeight)
    {
        X = startX;

        _random = random;

        GenerateHeight(
            screenHeight);

        HasScored = false;
    }

    public void Update(
        float deltaTime,
        float speed)
    {
        // Flytta röret åt vänster
        // med spelets aktuella hastighet
        X -= speed * deltaTime;
    }

    public bool IsOffScreen()
    {
        // Kontrollera om röret
        // har lämnat skärmen helt
        return X < -DrawWidth;
    }

    public void Reset(
        float newX,
        int screenHeight)
    {
        // Flytta röret tillbaka
        // till höger om skärmen
        X = newX;

        // Skapa en ny slumpad öppning
        GenerateHeight(
            screenHeight);

        // Röret kan ge poäng igen
        HasScored = false;
    }

    private void GenerateHeight(
        int screenHeight)
    {
        // Hindrar öppningen från att hamna
        // alldeles för nära tak eller mark
        int minimumHeight = 80;

        int maximumHeight =
            screenHeight -
            Gap -
            minimumHeight;

        _topPipeHeight =
            _random.Next(
                minimumHeight,
                maximumHeight);
    }

    public Rectangle GetTopBounds()
    {
        // Centrera den smalare hitboxen
        // inuti den bredare grafiken
        int hitboxOffsetX =
            (DrawWidth -
             HitboxWidth) / 2;

        // Hitboxen görs lite kortare
        // nära öppningen för rättvisare kollision
        return new Rectangle(
            (int)X +
            hitboxOffsetX,
            0,
            HitboxWidth,
            _topPipeHeight -
            GapPadding);
    }

    public Rectangle GetBottomBounds(
        int screenHeight)
    {
        int bottomPipeY =
            _topPipeHeight +
            Gap;

        int hitboxOffsetX =
            (DrawWidth -
             HitboxWidth) / 2;

        // Flytta ned hitboxen lite
        // från öppningen
        return new Rectangle(
            (int)X +
            hitboxOffsetX,
            bottomPipeY +
            GapPadding,
            HitboxWidth,
            screenHeight -
            bottomPipeY -
            GapPadding);
    }

    public Rectangle GetTopDrawRectangle()
    {
        // Röret fortsätter ovanför skärmen
        // så den yttre änden inte syns
        return new Rectangle(
            (int)X,
            -PipeOverdraw,
            DrawWidth,
            _topPipeHeight +
            PipeOverdraw);
    }

    public Rectangle GetBottomDrawRectangle(
        int screenHeight)
    {
        int bottomPipeY =
            _topPipeHeight +
            Gap;

        // Röret fortsätter nedanför skärmen
        // så det ser ut att gå ner i marken
        return new Rectangle(
            (int)X,
            bottomPipeY,
            DrawWidth,
            screenHeight -
            bottomPipeY +
            PipeOverdraw);
    }

    public int GetWidth()
    {
        return DrawWidth;
    }
}