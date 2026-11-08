using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FlappyBird;

public class GameOverState : IGameState
{
    // Game1 skickar in logiken som ska köras
    // när spelet är slut.
    private readonly Action<KeyboardState> _updateAction;

    private readonly Action _drawAction;

    public GameOverState(
        Action<KeyboardState> updateAction,
        Action drawAction)
    {
        _updateAction =
            updateAction;

        _drawAction =
            drawAction;
    }

    public void Enter()
    {
        // Just nu behöver inget särskilt hända
        // när Game Over-state aktiveras.
    }

    public void Update(
        GameTime gameTime,
        KeyboardState keyboardState)
    {
        // GameOverState hanterar input
        // för omstart eller återgång till menyn.
        _updateAction(
            keyboardState);
    }

    public void Draw()
    {
        // Rita Game Over-vyn.
        _drawAction();
    }
}