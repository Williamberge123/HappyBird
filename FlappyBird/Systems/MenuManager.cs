using System.Collections.Generic;

namespace FlappyBird;

public enum MenuAction
{
    None,
    StartGame,
    OpenBirdMenu,
    Exit
}

public class MenuManager
{
    // Alternativen som visas i huvudmenyn.
    private readonly string[] _menuOptions =
    {
        "START GAME",
        "BIRDS",
        "EXIT"
    };

    // Game1 får läsa alternativen men inte ersätta listan.
    public IReadOnlyList<string> MenuOptions =>
        _menuOptions;

    // Vilket alternativ som är markerat i huvudmenyn.
    public int SelectedMenuOption { get; private set; }

    // Anger om Birds-menyn är öppen.
    public bool IsBirdMenuOpen { get; private set; }

    public MenuManager()
    {
        SelectedMenuOption = 0;
        IsBirdMenuOpen = false;
    }

    public void MoveUp()
    {
        SelectedMenuOption--;

        // Om markeringen går ovanför första alternativet
        // flyttas den till sista alternativet.
        if (SelectedMenuOption < 0)
        {
            SelectedMenuOption =
                _menuOptions.Length - 1;
        }
    }

    public void MoveDown()
    {
        SelectedMenuOption++;

        // Om markeringen går förbi sista alternativet
        // flyttas den tillbaka till början.
        if (SelectedMenuOption >=
            _menuOptions.Length)
        {
            SelectedMenuOption = 0;
        }
    }

    public MenuAction SelectCurrentOption()
    {
        // Returnera en action istället för att
        // MenuManager själv startar eller avslutar spelet.
        return SelectedMenuOption switch
        {
            0 => MenuAction.StartGame,
            1 => MenuAction.OpenBirdMenu,
            2 => MenuAction.Exit,
            _ => MenuAction.None
        };
    }

    public void OpenBirdMenu()
    {
        IsBirdMenuOpen = true;
    }

    public void CloseBirdMenu()
    {
        IsBirdMenuOpen = false;
    }

    public void Reset()
    {
        // Återställ huvudmenyn till sitt första alternativ.
        SelectedMenuOption = 0;

        IsBirdMenuOpen = false;
    }
}