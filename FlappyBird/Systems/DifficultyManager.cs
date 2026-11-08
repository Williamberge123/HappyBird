namespace FlappyBird;

public class DifficultyManager
{
    private const float StartSpeed = 180f;
    private const float MaxSpeed = 260f;
    private const float SpeedIncreasePerPipe = 5f;

    public float CurrentSpeed { get; private set; }

    public DifficultyManager()
    {
        Reset();
    }

    public void IncreaseDifficulty()
    {
        CurrentSpeed += SpeedIncreasePerPipe;

        if (CurrentSpeed > MaxSpeed)
        {
            CurrentSpeed = MaxSpeed;
        }
    }

    public void Reset()
    {
        CurrentSpeed = StartSpeed;
    }
}