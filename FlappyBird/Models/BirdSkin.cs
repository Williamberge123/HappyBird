namespace FlappyBird;

public class BirdSkin
{
    public string Name { get; }
    public int RequiredCoins { get; }

    public bool IsUnlocked { get; set; }

    public BirdSkin(
        string name,
        int requiredCoins,
        bool isUnlocked)
    {
        Name = name;
        RequiredCoins = requiredCoins;
        IsUnlocked = isUnlocked;
    }
}