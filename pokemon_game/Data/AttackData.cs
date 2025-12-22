namespace pokemon_game.Data;

public class AttackData
{
    public string Target { get; set; }
    public float Amount { get; set; }
    public int Cost { get; set; }
    public string Element { get; set; }
    public string Animation { get; set; }

    public AttackData(string target, float amount, int cost, string element, string animation)
    {
        Target = target;
        Amount = amount;
        Cost = cost;
        Element = element;
        Animation = animation;
    }
}
