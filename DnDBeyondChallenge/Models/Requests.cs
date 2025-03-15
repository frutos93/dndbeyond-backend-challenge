namespace DnDBeyondChallenge.Models;

public class DamageRequest
{
    public int Amount { get; set; }
    public string Type { get; set; } = string.Empty;

    public bool TryParseDamageType(out DamageType damageType)
    {
        return Enum.TryParse(Type, true, out damageType);
    }
}

public class HealRequest
{
    public int Amount { get; set; }
}

public class TempHPRequest
{
    public int Amount { get; set; }
}
