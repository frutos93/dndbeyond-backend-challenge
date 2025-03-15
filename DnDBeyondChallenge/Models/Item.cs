namespace DnDBeyondChallenge.Models;
public class Modifier
{
    public string AffectedObject { get; set; } = string.Empty;
    public string AffectedValue { get; set; } = string.Empty;
    public int Value { get; set; }
}

public class Item
{
    public string Name { get; set; } = string.Empty;
    public Modifier Modifier { get; set; } = new Modifier();
}
