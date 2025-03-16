namespace DnDBeyondChallenge.Models;
public class Character
{
    public string Name { get; set; } = string.Empty;
    public int Level { get; set; }
    public int HitPoints { get; set; }
    public int MaxHitPoints { get; set; }
    public List<CharacterClass> Classes { get; set; }
    public Stats Stats { get; set; }
    public List<Item> Items { get; set; }
    public List<Defense> Defenses { get; set; }
    public int TemporaryHitPoints { get; set; }

    public Character()
    {
        Classes = new List<CharacterClass>();
        Stats = new Stats();
        Items = new List<Item>();
        Defenses = new List<Defense>();
    }

    public bool IsImmune(DamageType damageType)
    {
        return Defenses.Any(d => d.DamageType == damageType && d.DefenseType == DefenseType.Immunity);
    }

    public bool HasResistance(DamageType damageType)
    {
        return Defenses.Any(d => d.DamageType == damageType && d.DefenseType == DefenseType.Resistance);
    }

    public int TakeDamage(DamageType damageType, int amount)
    {
        if (HasResistance(damageType))
        {
            amount = (int)Math.Floor(amount / 2.0);
        }

        var damage = amount;

        if (TemporaryHitPoints > 0)
        {
            int tempHPUsed = Math.Min(damage, TemporaryHitPoints);
            TemporaryHitPoints -= tempHPUsed;
            damage -= tempHPUsed;
        }

        HitPoints = Math.Max(0, HitPoints -= damage);

        // Returning amount instead of damage, because damage is modified by tempHP if applicable.
        return amount;
    }
    public void Heal(int amount)
    {
        HitPoints = Math.Min(MaxHitPoints, HitPoints += amount);
    }

    public void AddTemporaryHitPoints(int tempHP)
    {
        if (tempHP > TemporaryHitPoints)
        {
            TemporaryHitPoints = tempHP;
        }
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
