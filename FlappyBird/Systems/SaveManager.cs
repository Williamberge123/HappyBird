using System;
using System.IO;
using System.Text.Json;

namespace FlappyBird;

public static class SaveManager
{
    private static readonly string SaveFolder =
        Path.Combine(
            Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData),
            "HappyBird");

    public static void SavePlayer(
        PlayerData playerData)
    {
        // Skapa mappen om den inte redan finns
        Directory.CreateDirectory(
            SaveFolder);

        string filePath =
            GetPlayerFilePath(
                playerData.PlayerName);

        JsonSerializerOptions options =
            new JsonSerializerOptions
            {
                WriteIndented = true
            };

        string json =
            JsonSerializer.Serialize(
                playerData,
                options);

        File.WriteAllText(
            filePath,
            json);
    }

    public static PlayerData LoadPlayer(
        string playerName)
    {
        Directory.CreateDirectory(
            SaveFolder);

        string filePath =
            GetPlayerFilePath(
                playerName);

        // Om profilen inte finns skapas en ny
        if (!File.Exists(filePath))
        {
            PlayerData newPlayer =
                new PlayerData
                {
                    PlayerName =
                        playerName
                };

            SavePlayer(
                newPlayer);

            return newPlayer;
        }

        string json =
            File.ReadAllText(
                filePath);

        PlayerData? playerData =
            JsonSerializer.Deserialize<PlayerData>(
                json);

        if (playerData == null)
        {
            return new PlayerData
            {
                PlayerName =
                    playerName
            };
        }

        return playerData;
    }

    private static string GetPlayerFilePath(
        string playerName)
    {
        // Tar bort tecken som inte får finnas i filnamn
        foreach (char invalidCharacter in
                 Path.GetInvalidFileNameChars())
        {
            playerName =
                playerName.Replace(
                    invalidCharacter,
                    '_');
        }

        return Path.Combine(
            SaveFolder,
            $"{playerName}.json");
    }
}