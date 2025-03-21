﻿using System.Text.Json.Serialization;
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

        if (character == null)
        {
            throw new InvalidDataException("Non existent character object in file.");
        }

        if (character.Name == string.Empty)
        {
            // Adding a check for the name but name is not used as key when saving.ss
            throw new InvalidDataException("Missing or empty 'Name' field.");
        }

        if (character.HitPoints <= 0)
        {
            throw new InvalidDataException("'HitPoints'must be a non-negative non-zero integer.");
        }

        foreach (var defense in character.Defenses)
        {
            defense.DamageType = CharacterParser.ParseDamageType(defense.DamageType.ToString());
            defense.DefenseType = CharacterParser.ParseDefenseType(defense.DefenseType.ToString());
        }

        character.MaxHitPoints = character.HitPoints;

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
