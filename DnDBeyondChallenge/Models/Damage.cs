using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DnDBeyondChallenge.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DamageType
{
    [EnumMember(Value = "bludgeoning")]
    Bludgeoning,
    [EnumMember(Value = "piercing")]
    Piercing,
    [EnumMember(Value = "slashing")]
    Slashing,
    [EnumMember(Value = "fire")]
    Fire,
    [EnumMember(Value = "cold")]
    Cold,
    [EnumMember(Value = "acid")]
    Acid,
    [EnumMember(Value = "thunder")]
    Thunder,
    [EnumMember(Value = "lightning")]
    Lightning,
    [EnumMember(Value = "poison")]
    Poison,
    [EnumMember(Value = "radiant")]
    Radiant,
    [EnumMember(Value = "necrotic")]
    Necrotic,
    [EnumMember(Value = "psychic")]
    Psychic,
    [EnumMember(Value = "force")]
    Force
}

[JsonConverter(typeof(JsonStringEnumConverter))]

public enum DefenseType
{
    [EnumMember(Value = "immunity")]
    Immunity,
    [EnumMember(Value = "resistance")]
    Resistance,
}

public class Defense
{
    [JsonPropertyName("type")]
    public DamageType DamageType { get; set; }
    [JsonPropertyName("defense")]
    public DefenseType DefenseType { get; set; }
}
