using System.Text.Json.Serialization;
using System.Text.Json;

namespace DnDBeyondChallenge.Models;
public static class CharacterParser
{
    public static Character ParseCharactersFromJson(string json)
    {
        var character = JsonSerializer.Deserialize<Character>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, true) }

        });

        // Parse the defenses and map the string types to the correct enums
        foreach (var defense in character.Defenses)
        {
            defense.DamageType = CharacterParser.ParseDamageType(defense.DamageType.ToString());
            defense.DefenseType = CharacterParser.ParseDefenseType(defense.DefenseType.ToString());
        }
        return character;
    }
    public static DamageType ParseDamageType(string type)
    {
        return type.ToLower() switch
        {
            "bludgeoning" => DamageType.Bludgeoning,
            "piercing" => DamageType.Piercing,
            "slashing" => DamageType.Slashing,
            "fire" => DamageType.Fire,
            "cold" => DamageType.Cold,
            "acid" => DamageType.Acid,
            "thunder" => DamageType.Thunder,
            "lightning" => DamageType.Lightning,
            "poison" => DamageType.Poison,
            "radiant" => DamageType.Radiant,
            "necrotic" => DamageType.Necrotic,
            "psychic" => DamageType.Psychic,
            "force" => DamageType.Force,
            _ => throw new ArgumentOutOfRangeException(nameof(type), $"Invalid damage type: {type}")
        };
    }
    public static DefenseType ParseDefenseType(string type)
    {
        return type.ToLower() switch
        {
            "immunity" => DefenseType.Immunity,
            "resistance" => DefenseType.Resistance,
            _ => throw new ArgumentOutOfRangeException(nameof(type), $"Invalid defense type: {type}")
        };
    }
}
