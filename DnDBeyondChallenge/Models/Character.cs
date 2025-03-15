namespace DnDBeyondChallenge.Models;
public class Character
{
    public string Name { get; set; }
    public int Level { get; set; }
    public double HitPoints { get; set; }
    public List<CharacterClass> Classes { get; set; }
    public Stats Stats { get; set; }
    public List<Item> Items { get; set; }
    public List<Defense> Defenses { get; set; }

    public Character()
    {
        Classes = new List<CharacterClass>();
        Stats = new Stats();
        Items = new List<Item>();
        Defenses = new List<Defense>();
    }
}

public class CharacterClass
{
    public string Name { get; set; } = string.Empty;
    public int HitDiceValue { get; set; }
    public int ClassLevel { get; set; }
}

public class Stats
{
    public int Strength { get; set; }
    public int Dexterity { get; set; }
    public int Constitution { get; set; }
    public int Intelligence { get; set; }
    public int Wisdom { get; set; }
    public int Charisma { get; set; }
}
