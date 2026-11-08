using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace FlappyBird;

public class PipeManager
{
    // Alla pipes som finns ute i spelvärlden.
    private readonly List<Pipe> _pipes =
        new List<Pipe>();

    private readonly Random _random;

    private const int PipeSpacing = 350;

    // Game1 får läsa listan men inte ersätta den.
    public IReadOnlyList<Pipe> Pipes =>
        _pipes;

    // Event som skickas när spelaren passerar en pipe.
    public event Action? PipePassed;

    // Event som skickas när en pipe återanvänds
    // och flyttas till höger sida igen.
    public event Action<Pipe>? PipeRecycled;

    public PipeManager(
        Random random)
    {
        _random = random;
    }

    public void Initialize(
        int screenHeight)
    {
        // Börja alltid med en tom lista.
        _pipes.Clear();

        // Skapa tre pipes med jämnt avstånd.
        for (int i = 0; i < 3; i++)
        {
            float startX =
                700 +
                i * PipeSpacing;

            Pipe pipe =
                new Pipe(
                    startX,
                    _random,
                    screenHeight);

            _pipes.Add(
                pipe);
        }
    }

    public void Update(
        float deltaTime,
        float speed,
        Bird bird,
        int screenHeight)
    {
        // -------------------------
        // Flytta pipes och kontrollera score
        // -------------------------

        foreach (Pipe pipe in _pipes)
        {
            pipe.Update(
                deltaTime,
                speed);

            // När hela pipen har passerat fågeln
            // får spelaren poäng.
            if (!pipe.HasScored &&
                pipe.X +
                pipe.GetWidth() <
                bird.Position.X)
            {
                pipe.HasScored =
                    true;

                PipePassed?.Invoke();
            }
        }

        // -------------------------
        // Återanvänd pipes
        // -------------------------

        foreach (Pipe pipe in _pipes)
        {
            if (!pipe.IsOffScreen())
            {
                continue;
            }

            float farthestPipeX =
                GetFarthestPipeX();

            pipe.Reset(
                farthestPipeX +
                PipeSpacing,
                screenHeight);

            // Meddela att pipen har flyttats.
            // CoinManager kan använda detta för coin-spawn.
            PipeRecycled?.Invoke(
                pipe);
        }
    }

    public bool CheckCollision(
        Bird bird,
        int screenHeight)
    {
        Rectangle birdBounds =
            bird.GetBounds();

        foreach (Pipe pipe in _pipes)
        {
            Rectangle topPipeBounds =
                pipe.GetTopBounds();

            Rectangle bottomPipeBounds =
                pipe.GetBottomBounds(
                    screenHeight);

            if (birdBounds.Intersects(
                    topPipeBounds) ||
                birdBounds.Intersects(
                    bottomPipeBounds))
            {
                return true;
            }
        }

        return false;
    }

    public void Reset(
        int screenHeight)
    {
        // Flytta tillbaka alla pipes till
        // sina ursprungliga startpositioner.
        for (int i = 0;
             i < _pipes.Count;
             i++)
        {
            float startX =
                700 +
                i * PipeSpacing;

            _pipes[i].Reset(
                startX,
                screenHeight);
        }
    }

    private float GetFarthestPipeX()
    {
        float farthestX = 0;

        // Hitta pipen som ligger längst åt höger.
        foreach (Pipe pipe in _pipes)
        {
            if (pipe.X >
                farthestX)
            {
                farthestX =
                    pipe.X;
            }
        }

        return farthestX;
    }
}