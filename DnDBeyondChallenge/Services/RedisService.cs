using DnDBeyondChallenge.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace DnDBeyondChallenge.Services;

public class RedisService
{
    private readonly IConnectionMultiplexer connection;
    private readonly IDatabase db;
    private const string CharacterKey = "character:";

    public RedisService(IConnectionMultiplexer connectionMultiplexer)
    {
        connection = connectionMultiplexer;
        db = connection.GetDatabase();
    }

    public async Task InitializeCacheFromFiles(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            throw new DirectoryNotFoundException($"Directory not found: {directoryPath}");
        }

        foreach (var charFile in Directory.GetFiles(directoryPath, "*.json"))
        {
            var json = await File.ReadAllTextAsync(charFile);
            var character = CharacterParser.ParseCharactersFromJson(json);
            if (character != null)
            {
                string key = CharacterKey + character.Name.ToLower();
                await db.StringSetAsync(key, JsonSerializer.Serialize(character));
            }
        }
    }

    public async Task<Character?> GetCharacterAsync(string name)
    {
        string key = CharacterKey + name.ToLower();
        string? json = await db.StringGetAsync(key);

        return json != null ? JsonSerializer.Deserialize<Character>(json) : null;
    }

    public async Task SetCharacterAsync(Character character)
    {
        string key = CharacterKey + character.Name.ToLower();
        string json = JsonSerializer.Serialize(character);
        await db.StringSetAsync(key, json);
    }
}
