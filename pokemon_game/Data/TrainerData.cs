using System.Collections.Generic;

namespace pokemon_game.Data;

public class TrainerData
{
    public Dictionary<int, string> Monsters { get; set; }
    public Dictionary<string, List<string>> Dialog { get; set; }
    public List<string> Directions { get; set; }
    public bool LookAround { get; set; }
    public bool Defeated { get; set; }
    public string Biome { get; set; }
    public bool CanBattle => Biome != null;

    public TrainerData(
        Dictionary<int, string> monsters,
        Dictionary<string, List<string>> dialog,
        List<string> directions,
        bool lookAround,
        bool defeated,
        string biome
    )
    {
        Monsters = monsters;
        Dialog = dialog;
        Directions = directions;
        LookAround = lookAround;
        Defeated = defeated;
        Biome = biome;
    }
}
