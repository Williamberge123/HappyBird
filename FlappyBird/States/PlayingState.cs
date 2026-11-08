using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FlappyBird;

public class PlayingState : IGameState
{
    // Game1 skickar in gameplay-logiken
    // som ska köras när spelet är igång.
    private readonly Action<KeyboardState, float> _updateAction;

    private readonly Action _drawAction;

    public PlayingState(
        Action<KeyboardState, float> updateAction,
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
        // när gameplay-state blir aktivt.
    }

    public void Update(
        GameTime gameTime,
        KeyboardState keyboardState)
    {
        float deltaTime =
            (float)gameTime
                .ElapsedGameTime
                .TotalSeconds;

        // PlayingState skickar vidare både input
        // och deltaTime till gameplay-logiken.
        _updateAction(
            keyboardState,
            deltaTime);
    }

    public void Draw()
    {
        // Rita själva spelvärlden.
        _drawAction();
    }
}