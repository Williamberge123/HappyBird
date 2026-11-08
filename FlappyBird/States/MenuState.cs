using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FlappyBird;

public class MenuState : IGameState
{
    // Game1 skickar in den logik som ska köras
    // när MenuState är aktivt.
    private readonly Action<KeyboardState> _updateAction;

    private readonly Action _drawAction;

    public MenuState(
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
        // när huvudmenyn öppnas.
    }

    public void Update(
        GameTime gameTime,
        KeyboardState keyboardState)
    {
        // MenuState ansvarar för att köra
        // menylogiken när detta state är aktivt.
        _updateAction(
            keyboardState);
    }

    public void Draw()
    {
        // Rita huvudmenyn.
        _drawAction();
    }
}