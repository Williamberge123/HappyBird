using FlappyBird;

namespace FlappyBird.Tests;

public class DifficultyManagerTests
{
    [Fact]
    public void StartsWithCorrectSpeed()
    {
        // Arrange
        DifficultyManager difficultyManager =
            new DifficultyManager();

        // Assert
        Assert.Equal(
            180f,
            difficultyManager.CurrentSpeed);
    }

    [Fact]
    public void IncreaseDifficultyIncreasesSpeed()
    {
        // Arrange
        DifficultyManager difficultyManager =
            new DifficultyManager();

        // Act
        difficultyManager.IncreaseDifficulty();

        // Assert
        Assert.Equal(
            185f,
            difficultyManager.CurrentSpeed);
    }

    [Fact]
    public void DifficultyDoesNotExceedMaximumSpeed()
    {
        // Arrange
        DifficultyManager difficultyManager =
            new DifficultyManager();

        // Act
        for (int i = 0; i < 100; i++)
        {
            difficultyManager.IncreaseDifficulty();
        }

        // Assert
        Assert.Equal(
            260f,
            difficultyManager.CurrentSpeed);
    }

    [Fact]
    public void ResetRestoresStartSpeed()
    {
        // Arrange
        DifficultyManager difficultyManager =
            new DifficultyManager();

        difficultyManager.IncreaseDifficulty();
        difficultyManager.IncreaseDifficulty();

        // Act
        difficultyManager.Reset();

        // Assert
        Assert.Equal(
            180f,
            difficultyManager.CurrentSpeed);
    }
}