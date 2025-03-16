namespace DnDBeyondChallenge.Models;

public class CharacterResponse
{
    public string Name { get; set; } = string.Empty;
    public int HitPoints { get; set; }
    public int TemporaryHitPoints { get; set; }
}

public class GetCharacterResponse : CharacterResponse
{
    public int Level { get; set; }
    public List<CharacterClass> Classes { get; set; } = new List<CharacterClass>();
    public Stats Stats { get; set; } = new Stats();
    public List<Item> Items { get; set; } = new List<Item>();
    public List<Defense> Defenses { get; set; } = new List<Defense>();
}
public class DamageResponse : CharacterResponse 
{
    public int DamageTaken { get; set; }
}

public class HealResponse : CharacterResponse
{
    public int MaxHitPoints { get; set; }
}

public class TempHPResponse : CharacterResponse { }
