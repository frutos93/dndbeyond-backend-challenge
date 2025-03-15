using DnDBeyondChallenge.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DnDBeyondChallenge.Services;
public class CharacterService
{
    private readonly RedisService redis;

    public CharacterService(RedisService redisService)
    {
        redis = redisService;
    }

    public async Task<Character?> GetCharacterAsync(string name)
    {
        return await redis.GetCharacterAsync(name);
    }
    public async Task SaveCharacterAsync(Character character)
    {
         await redis.SetCharacterAsync(character);
    }
}
