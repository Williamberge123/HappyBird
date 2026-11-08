using System.Collections.Generic;

namespace FlappyBird;

public class PlayerData
{
    public string PlayerName { get; set; } = "Player";

    public int Coins { get; set; }

    public int HighScore { get; set; }

    public int SelectedBirdIndex { get; set; }

    public List<int> UnlockedBirds { get; set; } =
        new List<int>
        {
            0
        };
}