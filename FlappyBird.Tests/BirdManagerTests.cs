using System.Collections.Generic;
using FlappyBird;

namespace FlappyBird.Tests;

public class BirdManagerTests
{
    [Fact]
    public void YellowBirdIsUnlockedByDefault()
    {
        // Arrange
        PlayerData playerData =
            new PlayerData();

        // Act
        BirdManager birdManager =
            new BirdManager(
                playerData);

        // Assert
        Assert.True(
            birdManager.BirdSkins[0].IsUnlocked);
    }

    [Fact]
    public void BlueBirdUnlocksWithEnoughCoins()
    {
        // Arrange
        PlayerData playerData =
            new PlayerData();

        BirdManager birdManager =
            new BirdManager(
                playerData);

        // Flytta markeringen till Blue Bird.
        birdManager.MoveSelectionDown();

        // Act
        bool result =
            birdManager.TrySelectBird(
                25);

        // Assert
        Assert.True(result);

        Assert.True(
            birdManager.BirdSkins[1].IsUnlocked);

        Assert.Equal(
            1,
            birdManager.SelectedBirdIndex);
    }

    [Fact]
    public void BlueBirdDoesNotUnlockWithoutEnoughCoins()
    {
        // Arrange
        PlayerData playerData =
            new PlayerData();

        BirdManager birdManager =
            new BirdManager(
                playerData);

        birdManager.MoveSelectionDown();

        // Act
        bool result =
            birdManager.TrySelectBird(
                24);

        // Assert
        Assert.False(result);

        Assert.False(
            birdManager.BirdSkins[1].IsUnlocked);

        Assert.Equal(
            0,
            birdManager.SelectedBirdIndex);
    }

    [Fact]
    public void GoldenBirdUnlocksWithEnoughCoins()
    {
        // Arrange
        PlayerData playerData =
            new PlayerData();

        BirdManager birdManager =
            new BirdManager(
                playerData);

        // Yellow -> Blue -> Golden.
        birdManager.MoveSelectionDown();
        birdManager.MoveSelectionDown();

        // Act
        bool result =
            birdManager.TrySelectBird(
                150);

        // Assert
        Assert.True(result);

        Assert.True(
            birdManager.BirdSkins[2].IsUnlocked);

        Assert.Equal(
            2,
            birdManager.SelectedBirdIndex);
    }

    [Fact]
    public void SavedSelectedBirdIsRestored()
    {
        // Arrange
        PlayerData playerData =
            new PlayerData
            {
                SelectedBirdIndex = 1,

                UnlockedBirds =
                    new List<int>
                    {
                        0,
                        1
                    }
            };

        // Act
        BirdManager birdManager =
            new BirdManager(
                playerData);

        // Assert
        Assert.Equal(
            1,
            birdManager.SelectedBirdIndex);

        Assert.True(
            birdManager.BirdSkins[1].IsUnlocked);
    }

    [Fact]
    public void MenuSelectionWrapsAround()
    {
        // Arrange
        PlayerData playerData =
            new PlayerData();

        BirdManager birdManager =
            new BirdManager(
                playerData);

        // Act
        birdManager.MoveSelectionUp();

        // Assert
        Assert.Equal(
            2,
            birdManager.MenuBirdIndex);
    }
}