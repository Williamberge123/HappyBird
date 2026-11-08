using System.Collections.Generic;

namespace FlappyBird;

public class BirdManager
{
    // Alla fåglar som finns i spelet.
    public List<BirdSkin> BirdSkins { get; }

    // Fågeln som används i spelet.
    public int SelectedBirdIndex { get; private set; }

    // Fågeln som är markerad i Birds-menyn.
    public int MenuBirdIndex { get; private set; }

    public BirdManager(
        PlayerData playerData)
    {
        // Skapa alla tillgängliga bird skins.
        BirdSkins =
            new List<BirdSkin>
            {
                new BirdSkin(
                    "Yellow Bird",
                    0,
                    true),

                new BirdSkin(
                    "Blue Bird",
                    25,
                    false),

                new BirdSkin(
                    "Golden Bird",
                    150,
                    false)
            };

        // Återställ vilka fåglar som tidigare
        // har blivit upplåsta.
        for (int i = 0;
             i < BirdSkins.Count;
             i++)
        {
            BirdSkins[i].IsUnlocked =
                playerData
                    .UnlockedBirds
                    .Contains(i);
        }

        // Yellow Bird ska alltid vara upplåst.
        BirdSkins[0].IsUnlocked = true;

        // Kontrollera att det sparade indexet
        // fortfarande är giltigt och upplåst.
        if (playerData.SelectedBirdIndex >= 0 &&
            playerData.SelectedBirdIndex <
            BirdSkins.Count &&
            BirdSkins[
                playerData.SelectedBirdIndex]
                .IsUnlocked)
        {
            SelectedBirdIndex =
                playerData.SelectedBirdIndex;
        }
        else
        {
            SelectedBirdIndex = 0;
        }

        // När Birds-menyn öppnas börjar markeringen
        // på den fågel som redan är vald.
        MenuBirdIndex =
            SelectedBirdIndex;
    }

    public void MoveSelectionUp()
    {
        MenuBirdIndex--;

        // Om vi går över toppen hoppar vi
        // till sista fågeln i listan.
        if (MenuBirdIndex < 0)
        {
            MenuBirdIndex =
                BirdSkins.Count - 1;
        }
    }

    public void MoveSelectionDown()
    {
        MenuBirdIndex++;

        // Om vi går förbi sista fågeln
        // hoppar vi tillbaka till början.
        if (MenuBirdIndex >=
            BirdSkins.Count)
        {
            MenuBirdIndex = 0;
        }
    }

    public void ResetMenuSelection()
    {
        // När Birds-menyn öppnas markeras
        // den fågel som används just nu.
        MenuBirdIndex =
            SelectedBirdIndex;
    }

    public bool TrySelectBird(
        int coins)
    {
        BirdSkin birdSkin =
            BirdSkins[
                MenuBirdIndex];

        // Om fågeln är låst men spelaren
        // har tillräckligt många coins
        // blir den permanent upplåst.
        if (!birdSkin.IsUnlocked &&
            coins >=
            birdSkin.RequiredCoins)
        {
            birdSkin.IsUnlocked =
                true;
        }

        // En låst fågel kan inte väljas.
        if (!birdSkin.IsUnlocked)
        {
            return false;
        }

        SelectedBirdIndex =
            MenuBirdIndex;

        return true;
    }

    public List<int> GetUnlockedBirdIndexes()
    {
        List<int> unlockedBirds =
            new List<int>();

        // Samla index för alla upplåsta fåglar.
        for (int i = 0;
             i < BirdSkins.Count;
             i++)
        {
            if (BirdSkins[i].IsUnlocked)
            {
                unlockedBirds.Add(i);
            }
        }

        return unlockedBirds;
    }
}