# HAPPYBIRD

HAPPYBIRD är ett Flappy Bird-inspirerat spel utvecklat i C# med MonoGame.

Spelaren styr en fågel genom att hoppa mellan pipes, samla coins och försöka slå sitt high score. Coins används för att låsa upp nya bird skins.

## Funktioner

- Flappy Bird-inspirerad gameplay
- Pipes med slumpmässiga öppningar
- Gradvis ökande svårighetsgrad
- Coins som kan samlas under spelet
- Upplåsningsbara bird skins
- Sparning av coins, high score och vald bird
- Huvudmeny och bird-meny
- Lokalt sparade spelarprofiler
- Event och delegates för bland annat coins och pipes
- Unit tests med xUnit

## Teknik

Projektet använder:

- C#
- .NET 10
- MonoGame DesktopGL
- xUnit

## Projektstruktur

Projektet är uppdelat i flera delar för att separera ansvar.

### Entities

Innehåller spelobjekt:

- `Bird`
- `Pipe`
- `Coin`

### Models

Innehåller datamodeller:

- `PlayerData`
- `BirdSkin`

### Systems

Innehåller system som ansvarar för olika delar av spelet:

- `BirdManager`
- `CoinManager`
- `PipeManager`
- `DifficultyManager`
- `MenuManager`
- `SaveManager`

`Game1` ansvarar främst för att koppla ihop systemen, hantera MonoGames game loop och rita spelet.

## Design

Projektet använder events och delegates för att minska kopplingen mellan olika system.

Exempel:

- `CoinManager` skickar ett event när en coin samlas in.
- `PipeManager` skickar events när en pipe passeras eller återanvänds.

Event-prenumerationerna tas bort när spelet avslutas för att undvika onödiga referenser.

Projektet använder även collections och generics, bland annat `List<T>` och `IReadOnlyList<T>`.

## Sparning

Spelarens progression sparas lokalt som JSON.

Följande sparas:

- Totalt antal coins
- High score
- Vald bird
- Upplåsta birds

Sparfilen lagras i användarens Application Data-mapp under mappen `HappyBird`.

## Bygga och köra

### Krav

- .NET 10 SDK
- MonoGame

### Köra spelet

Öppna projektet i Rider eller annan kompatibel C#-IDE och kör projektet `FlappyBird`.

Det går även att köra från terminal:

```powershell
cd C:\Users\Willg\Desktop\FlappyBird
dotnet run
```

## Tester

Testerna ligger i det separata projektet:

```text
FlappyBird.Tests
```

För att köra samtliga tester:

```powershell
cd C:\Users\Willg\Desktop
dotnet test
```

Projektet innehåller bland annat tester för:

- DifficultyManager
- BirdManager

Tester kontrollerar bland annat:

- Startvärde för spelhastighet
- Ökning av svårighetsgrad
- Maximal spelhastighet
- Reset av svårighetsgrad
- Upplåsning av birds
- Krav på coins
- Återställning av vald bird
- Menynavigering

## Kontroller

### Huvudmeny

- Pil upp / pil ner – navigera
- Enter – välj

### Birds-meny

- Pil upp / pil ner – navigera
- Enter – välj eller lås upp bird
- Backspace / Escape – tillbaka

### Gameplay

- Space – hoppa

### Game Over

- Space – starta om
- Enter – gå tillbaka till huvudmenyn

## Minneshantering

Projektet försöker undvika onödiga allokeringar under gameplay.

Exempel:

- Pipes återanvänds istället för att skapas på nytt varje gång.
- Content som laddas av MonoGames `ContentManager` återanvänds.
- Manuellt skapade resurser som `_pixelTexture` kör `Dispose()`.
- Event-prenumerationer tas bort när spelet avslutas.

## Unit Tests

Projektet innehåller för närvarande 10 unit tests med xUnit.

Alla tester kan köras med:

```powershell
dotnet test
```