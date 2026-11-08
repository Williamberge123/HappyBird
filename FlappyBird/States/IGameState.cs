using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FlappyBird;

public interface IGameState
{
    // Körs när spelet går in i detta state.
    void Enter();

    // Hanterar logiken för det aktuella state:et.
    void Update(
        GameTime gameTime,
        KeyboardState keyboardState);

    // Ritar det som hör till det aktuella state:et.
    void Draw();
}