using System.Collections.Generic;

namespace pokemon_game.Data;

public class MonsterData
{
    public MonsterStats Stats { get; set; }
    public Dictionary<int, string> Abilities { get; set; }
    public (string Name, int Level)? Evolve { get; set; }

    public MonsterData(
        MonsterStats stats,
        Dictionary<int, string> abilities,
        (string Name, int Level)? evolve
    )
    {
        Stats = stats;
        Abilities = abilities;
        Evolve = evolve;
    }
}
