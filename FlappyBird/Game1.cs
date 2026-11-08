using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace FlappyBird;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch = null!;

    // -------------------------
    // Bakgrund
    // -------------------------

    private Texture2D _backgroundTexture = null!;

    private float _backgroundX1 = 0f;
    private float _backgroundX2;

    private const float BackgroundSpeed = 30f;

    // -------------------------
    // UI
    // -------------------------

    private Texture2D _pixelTexture = null!;

    // -------------------------
    // Fågel
    // -------------------------

    // Gul fågel
    private Texture2D _birdNeutralTexture = null!;
    private Texture2D _birdJumpTexture = null!;
    private Texture2D _birdDeadTexture = null!;

    // Blå fågel
    private Texture2D _blueBirdNeutralTexture = null!;
    private Texture2D _blueBirdJumpTexture = null!;
    private Texture2D _blueBirdDeadTexture = null!;

    // Golden Bird
    private Texture2D _goldenBirdNeutralTexture = null!;
    private Texture2D _goldenBirdJumpTexture = null!;
    private Texture2D _goldenBirdDeadTexture = null!;

    private Bird _bird = null!;

    private BirdManager _birdManager = null!;

    // -------------------------
    // Player data / save
    // -------------------------

    private PlayerData _playerData = null!;

    private int _highScore;
    private bool _isNewHighScore;

    private const string PlayerName = "William";

    // -------------------------
    // Pipes
    // -------------------------

    private Texture2D _pipeTexture = null!;

    private PipeManager _pipeManager = null!;

    // -------------------------
    // Coins
    // -------------------------

    private Texture2D _coinTexture = null!;

    private CoinManager _coinManager = null!;

    private int _coins;

    // -------------------------
    // Difficulty
    // -------------------------

    private DifficultyManager _difficultyManager = null!;

    // -------------------------
    // Meny
    // -------------------------

    private MenuManager _menuManager = null!;

    // -------------------------
    // State Pattern
    // -------------------------

    private IGameState _currentState = null!;

    private MenuState _menuState = null!;
    private PlayingState _playingState = null!;
    private GameOverState _gameOverState = null!;

    // -------------------------
    // Fonts
    // -------------------------

    private SpriteFont _gameFont = null!;
    private SpriteFont _menuFont = null!;

    // -------------------------
    // Ljud
    // -------------------------

    // MP3-filer laddas som Song i MonoGame.
    private Song _coinSound = null!;
    private Song _deathSound = null!;
    private Song _jumpSound = null!;

    // -------------------------
    // Speldata
    // -------------------------

    private readonly Random _random =
        new Random();

    private KeyboardState _previousKeyboardState;

    private int _score;

    // -------------------------
    // Fågelanimation
    // -------------------------

    private float _jumpTextureTimer;

    private const float JumpTextureDuration = 0.20f;

    public Game1()
    {
        _graphics =
            new GraphicsDeviceManager(this);

        Content.RootDirectory =
            "Content";

        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // -------------------------
        // Ladda spelarens profil
        // -------------------------

        _playerData =
            SaveManager.LoadPlayer(
                PlayerName);

        _coins =
            _playerData.Coins;

        _highScore =
            _playerData.HighScore;

        // -------------------------
        // Skapa fågeln
        // -------------------------

        _bird =
            new Bird(
                new Vector2(
                    150,
                    200));

        // -------------------------
        // Systems
        // -------------------------

        _difficultyManager =
            new DifficultyManager();

        _birdManager =
            new BirdManager(
                _playerData);

        _pipeManager =
            new PipeManager(
                _random);

        _coinManager =
            new CoinManager(
                _random);

        _menuManager =
            new MenuManager();

        // -------------------------
        // Events
        // -------------------------

        _coinManager.CoinCollected +=
            HandleCoinCollected;

        _pipeManager.PipePassed +=
            HandlePipePassed;

        _pipeManager.PipeRecycled +=
            HandlePipeRecycled;

        // -------------------------
        // Skapa pipes
        // -------------------------

        _pipeManager.Initialize(
            GraphicsDevice
                .Viewport
                .Height);

        // -------------------------
        // Skapa start-coins
        // -------------------------

        _coinManager.Initialize(
            _pipeManager.Pipes,
            GraphicsDevice
                .Viewport
                .Height);

        // -------------------------
        // State Pattern
        // -------------------------

        _menuState =
            new MenuState(
                UpdateMenu,
                DrawMenu);

        _playingState =
            new PlayingState(
                UpdatePlaying,
                DrawPlayingState);

        _gameOverState =
            new GameOverState(
                UpdateGameOver,
                DrawGameOverState);

        _currentState =
            _menuState;

        _currentState.Enter();

        // -------------------------
        // Bakgrund
        // -------------------------

        _backgroundX2 =
            GraphicsDevice
                .Viewport
                .Width;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch =
            new SpriteBatch(
                GraphicsDevice);

        // -------------------------
        // UI
        // -------------------------

        _pixelTexture =
            new Texture2D(
                GraphicsDevice,
                1,
                1);

        _pixelTexture.SetData(
            new[]
            {
                Color.White
            });

        // -------------------------
        // Bakgrund
        // -------------------------

        _backgroundTexture =
            Content.Load<Texture2D>(
                "background1");

        // -------------------------
        // Yellow Bird
        // -------------------------

        _birdNeutralTexture =
            Content.Load<Texture2D>(
                "bird_neutral");

        _birdJumpTexture =
            Content.Load<Texture2D>(
                "bird_jump");

        _birdDeadTexture =
            Content.Load<Texture2D>(
                "bird_dead");

        // -------------------------
        // Blue Bird
        // -------------------------

        _blueBirdNeutralTexture =
            Content.Load<Texture2D>(
                "blue_neutral");

        _blueBirdJumpTexture =
            Content.Load<Texture2D>(
                "blue_jump");

        _blueBirdDeadTexture =
            Content.Load<Texture2D>(
                "blue_dead");

        // -------------------------
        // Golden Bird
        // -------------------------

        _goldenBirdNeutralTexture =
            Content.Load<Texture2D>(
                "golden_neutral");

        _goldenBirdJumpTexture =
            Content.Load<Texture2D>(
                "golden_jump");

        _goldenBirdDeadTexture =
            Content.Load<Texture2D>(
                "golden_dead");

        // -------------------------
        // Övriga assets
        // -------------------------

        _pipeTexture =
            Content.Load<Texture2D>(
                "pipe");

        _coinTexture =
            Content.Load<Texture2D>(
                "coin");

        _gameFont =
            Content.Load<SpriteFont>(
                "GameFont");

        _menuFont =
            Content.Load<SpriteFont>(
                "MenuFont");

        // ----------------------
        // Lju
        // ---------------------

        _coinSound =
            Content.Load<Song>(
                "CoinSound");

        _deathSound =
            Content.Load<Song>(
                "DeathSound");

        _jumpSound =
            Content.Load<Song>(
                "JumpSound");
        // Ljuden ska inte loopa.
        MediaPlayer.IsRepeating =
            false;

        // Justera global ljudvolym.
        MediaPlayer.Volume =
            0.6f;
    }

    protected override void Update(
        GameTime gameTime)
    {
        KeyboardState keyboardState =
            Keyboard.GetState();

        float deltaTime =
            (float)gameTime
                .ElapsedGameTime
                .TotalSeconds;

        // ----------------------
        // Escape
        // ----------------------

        if (keyboardState.IsKeyDown(Keys.Escape) &&
            _previousKeyboardState.IsKeyUp(Keys.Escape))
        {
            if (_currentState == _menuState &&
                _menuManager.IsBirdMenuOpen)
            {
                _menuManager
                    .CloseBirdMenu();
            }
            else
            {
                SaveProgress();

                Exit();

                return;
            }
        }

        // Bakgrunden uppdateras i alla states.
        UpdateBackground(
            deltaTime);

        // Det aktiva state-objektet
        // bestämmer vilken logik som körs.
        _currentState.Update(
            gameTime,
            keyboardState);

        _previousKeyboardState =
            keyboardState;

        base.Update(
            gameTime);
    }

    // ---------------
    // State byte
    // --------------

    private void ChangeState(
        IGameState newState)
    {
        _currentState =
            newState;

        _currentState.Enter();
    }

    // -------------------
    // Bakgrundslogik
    // -------------------

    private void UpdateBackground(
        float deltaTime)
    {
        _backgroundX1 -=
            BackgroundSpeed *
            deltaTime;

        _backgroundX2 -=
            BackgroundSpeed *
            deltaTime;

        if (_backgroundX1 <=
            -GraphicsDevice
                .Viewport
                .Width)
        {
            _backgroundX1 =
                _backgroundX2 +
                GraphicsDevice
                    .Viewport
                    .Width;
        }

        if (_backgroundX2 <=
            -GraphicsDevice
                .Viewport
                .Width)
        {
            _backgroundX2 =
                _backgroundX1 +
                GraphicsDevice
                    .Viewport
                    .Width;
        }
    }

    // -------------------------
    // Gameplay
    // -------------------------

    private void UpdatePlaying(
        KeyboardState keyboardState,
        float deltaTime)
    {
        // -------------------------
        // Fågel
        // -------------------------

        if (keyboardState.IsKeyDown(Keys.Space) &&
            _previousKeyboardState.IsKeyUp(Keys.Space))
        {
            _bird.Jump();

            _jumpTextureTimer =
                JumpTextureDuration;

            // Spela hoppljudet.
            MediaPlayer.Play(
                _jumpSound);
        }

        if (_jumpTextureTimer > 0)
        {
            _jumpTextureTimer -=
                deltaTime;
        }

        _bird.Update(
            deltaTime);

        // -------------------------
        // Pipes
        // -------------------------

        _pipeManager.Update(
            deltaTime,
            _difficultyManager
                .CurrentSpeed,
            _bird,
            GraphicsDevice
                .Viewport
                .Height);

        // -------------------------
        // Coins
        // -------------------------

        _coinManager.Update(
            deltaTime,
            _difficultyManager
                .CurrentSpeed,
            _bird);

        // -------------------------
        // Pipe-kollision
        // -------------------------

        if (_pipeManager.CheckCollision(
                _bird,
                GraphicsDevice
                    .Viewport
                    .Height))
        {
            EndGame();

            return;
        }

        // -------------------------
        // Skärmens kanter
        // -------------------------

        if (_bird.Position.Y < 0 ||
            _bird.Position.Y +
            _bird.GetHeight() >
            GraphicsDevice
                .Viewport
                .Height)
        {
            EndGame();
        }
    }

    // -------------------------
    // Game Over
    // -------------------------

    private void UpdateGameOver(
        KeyboardState keyboardState)
    {
        if (keyboardState.IsKeyDown(Keys.Space) &&
            _previousKeyboardState.IsKeyUp(Keys.Space))
        {
            RestartGame();

            ChangeState(
                _playingState);

            return;
        }

        if (keyboardState.IsKeyDown(Keys.Enter) &&
            _previousKeyboardState.IsKeyUp(Keys.Enter))
        {
            RestartGame();

            _menuManager.Reset();

            ChangeState(
                _menuState);
        }
    }

    // ----------------------
    // Events
    // ------------------------

    private void HandleCoinCollected()
    {
        _coins++;

        // Spela coin-ljudet.
        MediaPlayer.Play(
            _coinSound);

        SaveProgress();
    }

    private void HandlePipePassed()
    {
        _score++;

        _difficultyManager
            .IncreaseDifficulty();
    }

    private void HandlePipeRecycled(
        Pipe pipe)
    {
        _coinManager
            .TrySpawnForNewPipe(
                pipe,
                _pipeManager.Pipes,
                GraphicsDevice
                    .Viewport
                    .Height);
    }

    // ----------------------
    // Menyinput
    // ----------------------

    private void UpdateMenu(
        KeyboardState keyboardState)
    {
        if (_menuManager.IsBirdMenuOpen)
        {
            if (keyboardState.IsKeyDown(Keys.Back) &&
                _previousKeyboardState.IsKeyUp(Keys.Back))
            {
                _menuManager
                    .CloseBirdMenu();

                return;
            }

            if (keyboardState.IsKeyDown(Keys.Up) &&
                _previousKeyboardState.IsKeyUp(Keys.Up))
            {
                _birdManager
                    .MoveSelectionUp();
            }

            if (keyboardState.IsKeyDown(Keys.Down) &&
                _previousKeyboardState.IsKeyUp(Keys.Down))
            {
                _birdManager
                    .MoveSelectionDown();
            }

            if (keyboardState.IsKeyDown(Keys.Enter) &&
                _previousKeyboardState.IsKeyUp(Keys.Enter))
            {
                bool birdWasSelected =
                    _birdManager
                        .TrySelectBird(
                            _coins);

                if (birdWasSelected)
                {
                    SaveProgress();
                }
            }

            return;
        }

        if (keyboardState.IsKeyDown(Keys.Up) &&
            _previousKeyboardState.IsKeyUp(Keys.Up))
        {
            _menuManager
                .MoveUp();
        }

        if (keyboardState.IsKeyDown(Keys.Down) &&
            _previousKeyboardState.IsKeyUp(Keys.Down))
        {
            _menuManager
                .MoveDown();
        }

        if (keyboardState.IsKeyDown(Keys.Enter) &&
            _previousKeyboardState.IsKeyUp(Keys.Enter))
        {
            MenuAction action =
                _menuManager
                    .SelectCurrentOption();

            HandleMenuAction(
                action);
        }
    }

    private void HandleMenuAction(
        MenuAction action)
    {
        switch (action)
        {
            case MenuAction.StartGame:

                RestartGame();

                ChangeState(
                    _playingState);

                break;

            case MenuAction.OpenBirdMenu:

                _menuManager
                    .OpenBirdMenu();

                _birdManager
                    .ResetMenuSelection();

                break;

            case MenuAction.Exit:

                SaveProgress();

                Exit();

                break;

            case MenuAction.None:
            default:

                break;
        }
    }

    // ------------------------
    // Game Over
    // -----------------------

    private void EndGame()
    {
        if (_currentState ==
            _gameOverState)
        {
            return;
        }

        _isNewHighScore =
            false;

        if (_score >
            _highScore)
        {
            _highScore =
                _score;

            _isNewHighScore =
                true;
        }

        // Spela death-ljudet.
        MediaPlayer.Play(
            _deathSound);

        SaveProgress();

        ChangeState(
            _gameOverState);
    }

    // ------------------------
    // Save
    // -------------------------

    private void SaveProgress()
    {
        _playerData.Coins =
            _coins;

        _playerData.HighScore =
            _highScore;

        _playerData.SelectedBirdIndex =
            _birdManager
                .SelectedBirdIndex;

        _playerData.UnlockedBirds =
            _birdManager
                .GetUnlockedBirdIndexes();

        SaveManager.SavePlayer(
            _playerData);
    }

    // -------------------
    // Restart
    // -------------------

    private void RestartGame()
    {
        _bird.Reset(
            new Vector2(
                150,
                200));

        _difficultyManager
            .Reset();

        _isNewHighScore =
            false;

        _pipeManager.Reset(
            GraphicsDevice
                .Viewport
                .Height);

        _coinManager.Reset(
            _pipeManager.Pipes,
            GraphicsDevice
                .Viewport
                .Height);

        _score =
            0;

        _jumpTextureTimer =
            0f;
    }

    // ----------------------
    // Draw
    // --------------------

    protected override void Draw(
        GameTime gameTime)
    {
        GraphicsDevice.Clear(
            Color.CornflowerBlue);

        _spriteBatch.Begin();

        DrawBackground();

        _currentState.Draw();

        _spriteBatch.End();

        base.Draw(
            gameTime);
    }

    // ------------------------
    // State Draw
    // -----------------------

    private void DrawPlayingState()
    {
        DrawGameWorld(
            false);
    }

    private void DrawGameOverState()
    {
        DrawGameWorld(
            true);

        DrawGameOver();
    }

    // ------------------------
    // Spelvärld
    // ------------------------

    private void DrawGameWorld(
        bool showDeadBird)
    {
        // Coins
        foreach (Coin coin in
                 _coinManager.Coins)
        {
            if (!coin.IsCollected)
            {
                _spriteBatch.Draw(
                    _coinTexture,
                    coin.GetDrawRectangle(),
                    Color.White);
            }
        }

        // Fågel
        Texture2D currentBirdTexture;

        if (showDeadBird)
        {
            currentBirdTexture =
                GetSelectedBirdDeadTexture();
        }
        else if (_jumpTextureTimer > 0)
        {
            currentBirdTexture =
                GetSelectedBirdJumpTexture();
        }
        else
        {
            currentBirdTexture =
                GetSelectedBirdNeutralTexture();
        }

        DrawBird(
            currentBirdTexture);

        // Pipes
        foreach (Pipe pipe in
                 _pipeManager.Pipes)
        {
            Rectangle topPipeDrawRect =
                pipe.GetTopDrawRectangle();

            Rectangle bottomPipeDrawRect =
                pipe.GetBottomDrawRectangle(
                    GraphicsDevice
                        .Viewport
                        .Height);

            _spriteBatch.Draw(
                _pipeTexture,
                topPipeDrawRect,
                null,
                Color.White,
                0f,
                Vector2.Zero,
                SpriteEffects.FlipVertically,
                0f);

            _spriteBatch.Draw(
                _pipeTexture,
                bottomPipeDrawRect,
                Color.White);
        }

        // HUD
        _spriteBatch.DrawString(
            _gameFont,
            $"Score: {_score}",
            new Vector2(
                20,
                20),
            Color.White);

        _spriteBatch.Draw(
            _coinTexture,
            new Rectangle(
                20,
                55,
                28,
                28),
            Color.White);

        _spriteBatch.DrawString(
            _gameFont,
            _coins.ToString(),
            new Vector2(
                55,
                58),
            Color.Gold);
    }

    // ------------------------
    // Fågelritning
    // -----------------------

    private void DrawBird(
        Texture2D currentBirdTexture)
    {
        float birdRotation;

        if (_bird.GetVelocity() < 0)
        {
            birdRotation =
                -0.25f;
        }
        else
        {
            birdRotation =
                MathHelper.Clamp(
                    _bird.GetVelocity() /
                    900f,
                    0f,
                    1.2f);
        }

        Rectangle birdRectangle =
            _bird.GetDrawRectangle();

        Vector2 birdPosition =
            new Vector2(
                birdRectangle.Center.X,
                birdRectangle.Center.Y);

        Vector2 birdOrigin =
            new Vector2(
                currentBirdTexture.Width /
                2f,
                currentBirdTexture.Height /
                2f);

        Vector2 birdScale =
            new Vector2(
                birdRectangle.Width /
                (float)currentBirdTexture.Width,

                birdRectangle.Height /
                (float)currentBirdTexture.Height);

        _spriteBatch.Draw(
            currentBirdTexture,
            birdPosition,
            null,
            Color.White,
            birdRotation,
            birdOrigin,
            birdScale,
            SpriteEffects.None,
            0f);
    }

    // ------------
    // Bakgrund
    // ----------------

    private void DrawBackground()
    {
        _spriteBatch.Draw(
            _backgroundTexture,
            new Rectangle(
                (int)_backgroundX1,
                0,
                GraphicsDevice
                    .Viewport
                    .Width,
                GraphicsDevice
                    .Viewport
                    .Height),
            Color.White);

        _spriteBatch.Draw(
            _backgroundTexture,
            new Rectangle(
                (int)_backgroundX2,
                0,
                GraphicsDevice
                    .Viewport
                    .Width,
                GraphicsDevice
                    .Viewport
                    .Height),
            Color.White);
    }

    // -------------
    // Huvudmeny
    // ---------------

    private void DrawMenu()
    {
        int screenWidth =
            GraphicsDevice
                .Viewport
                .Width;

        int screenHeight =
            GraphicsDevice
                .Viewport
                .Height;

        float centerX =
            screenWidth / 2f;

        if (_menuManager.IsBirdMenuOpen)
        {
            DrawBirdMenu();

            return;
        }

        Rectangle panelRectangle =
            new Rectangle(
                (int)centerX - 190,
                45,
                380,
                screenHeight - 90);

        DrawPanel(
            panelRectangle,
            new Color(
                15,
                25,
                35,
                205));

        string titleText =
            "HAPPYBIRD";

        Vector2 titleSize =
            _gameFont
                .MeasureString(
                    titleText);

        Vector2 titlePosition =
            new Vector2(
                centerX -
                titleSize.X / 2f,
                68);

        _spriteBatch.DrawString(
            _gameFont,
            titleText,
            titlePosition +
            new Vector2(
                3,
                3),
            Color.Black);

        _spriteBatch.DrawString(
            _gameFont,
            titleText,
            titlePosition,
            Color.Gold);

        Rectangle menuBirdRectangle =
            new Rectangle(
                (int)centerX - 43,
                112,
                86,
                72);

        _spriteBatch.Draw(
            GetSelectedBirdNeutralTexture(),
            menuBirdRectangle,
            Color.White);

        DrawCenteredMenuText(
            $"BEST: {_highScore}",
            centerX,
            180,
            Color.Gold);

        int buttonWidth =
            250;

        int buttonHeight =
            46;

        int buttonX =
            (int)centerX -
            buttonWidth / 2;

        int firstButtonY =
            215;

        for (int i = 0;
             i <
             _menuManager
                 .MenuOptions
                 .Count;
             i++)
        {
            int buttonY =
                firstButtonY +
                i * 57;

            Rectangle buttonRectangle =
                new Rectangle(
                    buttonX,
                    buttonY,
                    buttonWidth,
                    buttonHeight);

            bool isSelected =
                i ==
                _menuManager
                    .SelectedMenuOption;

            if (isSelected)
            {
                Rectangle borderRectangle =
                    new Rectangle(
                        buttonRectangle.X - 3,
                        buttonRectangle.Y - 3,
                        buttonRectangle.Width + 6,
                        buttonRectangle.Height + 6);

                _spriteBatch.Draw(
                    _pixelTexture,
                    borderRectangle,
                    Color.Gold);
            }

            Color buttonColor =
                isSelected
                    ? new Color(
                        55,
                        70,
                        55,
                        245)
                    : new Color(
                        25,
                        35,
                        45,
                        225);

            _spriteBatch.Draw(
                _pixelTexture,
                buttonRectangle,
                buttonColor);

            string optionText =
                _menuManager
                    .MenuOptions[i];

            Vector2 optionSize =
                _gameFont
                    .MeasureString(
                        optionText);

            Vector2 optionPosition =
                new Vector2(
                    buttonRectangle.Center.X -
                    optionSize.X / 2f,
                    buttonRectangle.Center.Y -
                    optionSize.Y / 2f);

            Color optionColor =
                isSelected
                    ? Color.Gold
                    : Color.White;

            _spriteBatch.DrawString(
                _gameFont,
                optionText,
                optionPosition,
                optionColor);
        }

        Rectangle coinBox =
            new Rectangle(
                (int)centerX - 70,
                screenHeight - 70,
                140,
                42);

        _spriteBatch.Draw(
            _pixelTexture,
            coinBox,
            new Color(
                20,
                28,
                38,
                220));

        _spriteBatch.Draw(
            _coinTexture,
            new Rectangle(
                coinBox.X + 15,
                coinBox.Y + 7,
                28,
                28),
            Color.White);

        string coinText =
            _coins.ToString();

        Vector2 coinTextSize =
            _gameFont
                .MeasureString(
                    coinText);

        _spriteBatch.DrawString(
            _gameFont,
            coinText,
            new Vector2(
                coinBox.X + 55,
                coinBox.Center.Y -
                coinTextSize.Y / 2f),
            Color.Gold);
    }

    // ---------------------
    // Birds-menyn
    // ---------------------

    private void DrawBirdMenu()
    {
        int screenWidth =
            GraphicsDevice
                .Viewport
                .Width;

        int screenHeight =
            GraphicsDevice
                .Viewport
                .Height;

        float centerX =
            screenWidth / 2f;

        Rectangle panelRectangle =
            new Rectangle(
                (int)centerX - 230,
                35,
                460,
                screenHeight - 70);

        DrawPanel(
            panelRectangle,
            new Color(
                15,
                25,
                35,
                220));

        DrawCenteredText(
            "BIRDS",
            centerX,
            55,
            Color.Gold);

        _spriteBatch.Draw(
            _coinTexture,
            new Rectangle(
                (int)centerX - 42,
                95,
                28,
                28),
            Color.White);

        _spriteBatch.DrawString(
            _menuFont,
            _coins.ToString(),
            new Vector2(
                centerX - 5,
                99),
            Color.Gold);

        int firstBirdY =
            145;

        for (int i = 0;
             i <
             _birdManager
                 .BirdSkins
                 .Count;
             i++)
        {
            BirdSkin birdSkin =
                _birdManager
                    .BirdSkins[i];

            bool isHighlighted =
                i ==
                _birdManager
                    .MenuBirdIndex;

            bool isSelected =
                i ==
                _birdManager
                    .SelectedBirdIndex;

            Rectangle birdRow =
                new Rectangle(
                    (int)centerX - 185,
                    firstBirdY +
                    i * 58,
                    370,
                    48);

            if (isHighlighted)
            {
                _spriteBatch.Draw(
                    _pixelTexture,
                    new Rectangle(
                        birdRow.X - 3,
                        birdRow.Y - 3,
                        birdRow.Width + 6,
                        birdRow.Height + 6),
                    Color.Gold);
            }

            Color rowColor =
                isHighlighted
                    ? new Color(
                        55,
                        70,
                        55,
                        245)
                    : new Color(
                        18,
                        26,
                        34,
                        235);

            _spriteBatch.Draw(
                _pixelTexture,
                birdRow,
                rowColor);

            _spriteBatch.DrawString(
                _menuFont,
                birdSkin
                    .Name
                    .ToUpper(),
                new Vector2(
                    birdRow.X + 15,
                    birdRow.Y + 11),
                Color.White);

            string statusText;

            if (isSelected)
            {
                statusText =
                    "SELECTED";
            }
            else if (birdSkin.IsUnlocked)
            {
                statusText =
                    "UNLOCKED";
            }
            else
            {
                statusText =
                    $"{birdSkin.RequiredCoins} COINS";
            }

            Vector2 statusSize =
                _menuFont
                    .MeasureString(
                        statusText);

            _spriteBatch.DrawString(
                _menuFont,
                statusText,
                new Vector2(
                    birdRow.Right -
                    statusSize.X -
                    15,
                    birdRow.Y + 11),
                Color.Gold);
        }

        DrawCenteredMenuText(
            "ENTER - SELECT / UNLOCK",
            centerX,
            screenHeight - 80,
            Color.Gold);

        DrawCenteredMenuText(
            "BACKSPACE - BACK",
            centerX,
            screenHeight - 48,
            Color.White);
    }

    // -------------------------
    // Game Over
    // -------------------------

    private void DrawGameOver()
    {
        int screenWidth =
            GraphicsDevice
                .Viewport
                .Width;

        int screenHeight =
            GraphicsDevice
                .Viewport
                .Height;

        float centerX =
            screenWidth / 2f;

        float centerY =
            screenHeight / 2f;

        Rectangle panel =
            new Rectangle(
                (int)centerX - 190,
                (int)centerY - 145,
                380,
                300);

        DrawPanel(
            panel,
            new Color(
                15,
                20,
                30,
                220));

        DrawCenteredText(
            "GAME OVER",
            centerX,
            centerY - 115,
            Color.Gold);

        if (_isNewHighScore)
        {
            DrawCenteredMenuText(
                "NEW HIGH SCORE!",
                centerX,
                centerY - 70,
                Color.Gold);
        }

        DrawCenteredMenuText(
            $"SCORE: {_score}",
            centerX,
            centerY - 25,
            Color.White);

        DrawCenteredMenuText(
            $"BEST: {_highScore}",
            centerX,
            centerY + 10,
            Color.Gold);

        DrawCenteredMenuText(
            "SPACE - RESTART",
            centerX,
            centerY + 65,
            Color.White);

        DrawCenteredMenuText(
            "ENTER - MAIN MENU",
            centerX,
            centerY + 105,
            Color.White);
    }

    // -------------------------
    // Bird textures
    // -------------------------

    private Texture2D GetSelectedBirdNeutralTexture()
    {
        return _birdManager
            .SelectedBirdIndex switch
        {
            1 =>
                _blueBirdNeutralTexture,

            2 =>
                _goldenBirdNeutralTexture,

            _ =>
                _birdNeutralTexture
        };
    }

    private Texture2D GetSelectedBirdJumpTexture()
    {
        return _birdManager
            .SelectedBirdIndex switch
        {
            1 =>
                _blueBirdJumpTexture,

            2 =>
                _goldenBirdJumpTexture,

            _ =>
                _birdJumpTexture
        };
    }

    private Texture2D GetSelectedBirdDeadTexture()
    {
        return _birdManager
            .SelectedBirdIndex switch
        {
            1 =>
                _blueBirdDeadTexture,

            2 =>
                _goldenBirdDeadTexture,

            _ =>
                _birdDeadTexture
        };
    }

    // -------------------------
    // UI-hjälpmetoder
    // -------------------------

    private void DrawPanel(
        Rectangle rectangle,
        Color color)
    {
        Rectangle shadow =
            new Rectangle(
                rectangle.X + 6,
                rectangle.Y + 6,
                rectangle.Width,
                rectangle.Height);

        _spriteBatch.Draw(
            _pixelTexture,
            shadow,
            new Color(
                0,
                0,
                0,
                100));

        _spriteBatch.Draw(
            _pixelTexture,
            rectangle,
            color);

        _spriteBatch.Draw(
            _pixelTexture,
            new Rectangle(
                rectangle.X,
                rectangle.Y,
                rectangle.Width,
                2),
            new Color(
                255,
                255,
                255,
                45));
    }

    private void DrawCenteredText(
        string text,
        float centerX,
        float y,
        Color color)
    {
        Vector2 textSize =
            _gameFont
                .MeasureString(
                    text);

        _spriteBatch.DrawString(
            _gameFont,
            text,
            new Vector2(
                centerX -
                textSize.X / 2f,
                y),
            color);
    }

    private void DrawCenteredMenuText(
        string text,
        float centerX,
        float y,
        Color color)
    {
        Vector2 textSize =
            _menuFont
                .MeasureString(
                    text);

        _spriteBatch.DrawString(
            _menuFont,
            text,
            new Vector2(
                centerX -
                textSize.X / 2f,
                y),
            color);
    }

    protected override void UnloadContent()
    {
        if (_playerData != null &&
            _birdManager != null)
        {
            SaveProgress();
        }

        if (_coinManager != null)
        {
            _coinManager.CoinCollected -=
                HandleCoinCollected;
        }

        if (_pipeManager != null)
        {
            _pipeManager.PipePassed -=
                HandlePipePassed;

            _pipeManager.PipeRecycled -=
                HandlePipeRecycled;
        }

        // Stoppa eventuellt ljud när spelet stängs.
        MediaPlayer.Stop();

        _pixelTexture?.Dispose();

        base.UnloadContent();
    }
}