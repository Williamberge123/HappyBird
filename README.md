# HappyBird

HappyBird är ett Flappy Bird-inspirerat spel byggt i C# med .NET 10 och MonoGame (skolprojekt). Styr fågeln mellan rören, samla coins och slå ditt high score. Coins låser upp nya fåglar (skins), och framsteg sparas lokalt.

Mer detaljer finns i [FlappyBird/README.md](FlappyBird/README.md).

## Krav
- .NET 10 SDK

## Kör spelet
```bash
dotnet run --project FlappyBird
```

## Kör testerna
```bash
dotnet test
```
Testerna använder xUnit och ligger i `FlappyBird.Tests`.
