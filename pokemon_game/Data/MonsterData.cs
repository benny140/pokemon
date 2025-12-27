using System.Collections.Generic;

namespace pokemon_game.Data;

public class SecondaryAction
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Range { get; set; }

    public SecondaryAction(string name, string description, int range)
    {
        Name = name;
        Description = description;
        Range = range;
    }
}

public class MonsterData
{
    public int Id { get; set; }
    public string Type { get; set; }
    public string Stage { get; set; }
    public MonsterStats Stats { get; set; }
    public SecondaryAction SecondaryAction { get; set; }
    public int? EvolvesTo { get; set; }

    public MonsterData(
        int id,
        string type,
        string stage,
        MonsterStats stats,
        SecondaryAction secondaryAction,
        int? evolvesTo
    )
    {
        Id = id;
        Type = type;
        Stage = stage;
        Stats = stats;
        SecondaryAction = secondaryAction;
        EvolvesTo = evolvesTo;
    }
}
