using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace FlappyBird;

public class CoinManager
{
    // Alla coins som för tillfället finns ute i spelvärlden.
    private readonly List<Coin> _coins =
        new List<Coin>();

    private readonly Random _random;

    // 40 % chans att en coin skapas mellan två pipes.
    private const double CoinSpawnChance = 0.40;

    // Game1 kan läsa listan men inte ersätta den.
    public IReadOnlyList<Coin> Coins =>
        _coins;

    // Event som körs när spelaren samlar en coin.
    public event Action? CoinCollected;

    public CoinManager(
        Random random)
    {
        _random = random;
    }

    public void Initialize(
        IReadOnlyList<Pipe> pipes,
        int screenHeight)
    {
        // Börja alltid med en tom coin-lista.
        _coins.Clear();

        // Försök skapa coins mellan de pipes
        // som redan finns när en runda startar.
        for (int i = 0;
             i < pipes.Count - 1;
             i++)
        {
            if (_random.NextDouble() <
                CoinSpawnChance)
            {
                CreateCoinBetweenPipes(
                    pipes[i],
                    pipes[i + 1],
                    screenHeight);
            }
        }
    }

    public void Update(
        float deltaTime,
        float speed,
        Bird bird)
    {
        foreach (Coin coin in _coins)
        {
            // Coins rör sig med samma hastighet
            // som spelets pipes.
            coin.Update(
                deltaTime,
                speed);

            // Kontrollera om fågeln plockar upp coinen.
            if (!coin.IsCollected &&
                bird
                    .GetBounds()
                    .Intersects(
                        coin.GetBounds()))
            {
                coin.Collect();

                // Meddela Game1 att en coin
                // har samlats in.
                CoinCollected?.Invoke();
            }
        }

        // Ta bort coins som redan samlats in
        // eller har lämnat skärmen.
        _coins.RemoveAll(
            coin =>
                coin.IsCollected ||
                coin.IsOffScreen());
    }

    public void TrySpawnForNewPipe(
        Pipe newPipe,
        IReadOnlyList<Pipe> pipes,
        int screenHeight)
    {
        // Alla nya pipes behöver inte få en coin.
        if (_random.NextDouble() >=
            CoinSpawnChance)
        {
            return;
        }

        Pipe? closestLeftPipe = null;

        float closestDistance =
            float.MaxValue;

        // Hitta pipen som ligger närmast
        // till vänster om den nya pipen.
        foreach (Pipe pipe in pipes)
        {
            if (pipe == newPipe)
            {
                continue;
            }

            if (pipe.X <
                newPipe.X)
            {
                float distance =
                    newPipe.X -
                    pipe.X;

                if (distance <
                    closestDistance)
                {
                    closestDistance =
                        distance;

                    closestLeftPipe =
                        pipe;
                }
            }
        }

        // Om en pipe hittades till vänster
        // skapas coinen mellan dessa två pipes.
        if (closestLeftPipe != null)
        {
            CreateCoinBetweenPipes(
                closestLeftPipe,
                newPipe,
                screenHeight);
        }
    }

    private void CreateCoinBetweenPipes(
        Pipe leftPipe,
        Pipe rightPipe,
        int screenHeight)
    {
        // Placera coinen ungefär mitt mellan pipesen.
        float coinX =
            leftPipe.X +
            (rightPipe.X -
             leftPipe.X) / 2f;

        int minimumY = 100;

        int maximumY =
            screenHeight - 160;

        // Coinens höjd slumpas så spelaren ibland
        // behöver ta en svårare väg.
        float coinY =
            _random.Next(
                minimumY,
                maximumY);

        _coins.Add(
            new Coin(
                new Vector2(
                    coinX,
                    coinY)));
    }

    public void Reset(
        IReadOnlyList<Pipe> pipes,
        int screenHeight)
    {
        // Rensa coins från föregående runda.
        _coins.Clear();

        // Skapa nya coins för den nya rundan.
        Initialize(
            pipes,
            screenHeight);
    }

    public void Clear()
    {
        _coins.Clear();
    }
}