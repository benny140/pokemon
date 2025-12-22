namespace pokemon_game.Data;

public class MonsterStats
{
    public string Element { get; set; }
    public int MaxHealth { get; set; }
    public int MaxEnergy { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Recovery { get; set; }
    public float Speed { get; set; }

    public MonsterStats(
        string element,
        int maxHealth,
        int maxEnergy,
        int attack,
        int defense,
        int recovery,
        float speed
    )
    {
        Element = element;
        MaxHealth = maxHealth;
        MaxEnergy = maxEnergy;
        Attack = attack;
        Defense = defense;
        Recovery = recovery;
        Speed = speed;
    }
}
