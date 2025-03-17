using DnDBeyondChallenge.Models;
using StackExchange.Redis;
using System.Formats.Tar;
using System.Text.Json;

namespace DnDBeyondChallenge.Services;

public class RedisService
{
    private readonly IConnectionMultiplexer connection;
    private readonly IDatabase db;
    private const string CharacterKey = "character:";
    private readonly ILogger<RedisService> logger;

    public RedisService(IConnectionMultiplexer connectionMultiplexer, ILogger<RedisService> log)
    {
        connection = connectionMultiplexer;
        db = connection.GetDatabase();
        logger = log;
    }

    public async Task InitializeCacheFromFiles(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            logger.LogError($"Directory not found: {directoryPath}");
            return;
        }


        foreach (var charFile in Directory.GetFiles(directoryPath, "*.json"))
        {
            try
            {
                var json = await File.ReadAllTextAsync(charFile);
                //Make sure it's a valid character
                var character = CharacterParser.ParseCharactersFromJson(json);
                if (character != null)
                {
                    string key = CharacterKey + Path.GetFileNameWithoutExtension(charFile).ToLower();
                    await db.StringSetAsync(key, JsonSerializer.Serialize(character));
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning($"Skipped. Error initializing cache from file {charFile}. {ex.Message}");
                continue;
            }
        }
    }

    public async Task<Character?> GetCharacterAsync(string name)
    {
        string key = CharacterKey + name.ToLower();
        string? json = await db.StringGetAsync(key);

        return json != null ? JsonSerializer.Deserialize<Character>(json) : null;
    }

    public async Task SetCharacterAsync(string key, Character character)
    {
        // Key can be different than character.Name.
        key = CharacterKey + key.ToLower();
        string json = JsonSerializer.Serialize(character);
        await db.StringSetAsync(key, json);
    }
}
