namespace pokemon_game.Data;

public class MonsterStats
{
    public int HP { get; set; }
    public int Attack { get; set; }
    public int Range { get; set; }
    public int Movement { get; set; }

    public MonsterStats(int hp, int attack, int range, int movement)
    {
        HP = hp;
        Attack = attack;
        Range = range;
        Movement = movement;
    }
}
