using System.Collections.Generic;

namespace pokemon_game.Data;

public static class GameData
{
    public static readonly Dictionary<string, TrainerData> Trainers = new Dictionary<
        string,
        TrainerData
    >
    {
        {
            "o1",
            new TrainerData(
                new Dictionary<int, (string, int)> { { 0, ("Jacana", 14) }, { 1, ("Cleaf", 15) } },
                new Dictionary<string, List<string>>
                {
                    {
                        "default",
                        new List<string>
                        {
                            "Hey, how are you?",
                            "Oh, so you want to fight?",
                            "FIGHT!",
                        }
                    },
                    {
                        "defeated",
                        new List<string> { "You are very strong!", "Let's fight again sometime?" }
                    },
                },
                new List<string> { "down" },
                true,
                false,
                "forest"
            )
        },
        {
            "o2",
            new TrainerData(
                new Dictionary<int, (string, int)>
                {
                    { 0, ("Atrox", 14) },
                    { 1, ("Pouch", 15) },
                    { 2, ("Draem", 13) },
                    { 3, ("Cindrill", 13) },
                },
                new Dictionary<string, List<string>>
                {
                    {
                        "default",
                        new List<string>
                        {
                            "I don't like sand",
                            "It's coarse and rough",
                            "oh god, fight",
                        }
                    },
                    {
                        "defeated",
                        new List<string> { "May the force be with you" }
                    },
                },
                new List<string> { "left", "down" },
                false,
                false,
                "sand"
            )
        },
        {
            "o3",
            new TrainerData(
                new Dictionary<int, (string, int)>
                {
                    { 0, ("Atrox", 14) },
                    { 1, ("Pouch", 15) },
                    { 2, ("Draem", 13) },
                    { 3, ("Cindrill", 13) },
                },
                new Dictionary<string, List<string>>
                {
                    {
                        "default",
                        new List<string> { "I love skating!", "FIGHT!" }
                    },
                    {
                        "defeated",
                        new List<string> { "Good luck with the boss", "It's so cold in here" }
                    },
                },
                new List<string> { "left", "right", "up", "down" },
                true,
                false,
                "sand"
            )
        },
        {
            "o4",
            new TrainerData(
                new Dictionary<int, (string, int)>
                {
                    { 0, ("Friolera", 25) },
                    { 1, ("Gulfin", 20) },
                    { 2, ("Atrox", 24) },
                    { 3, ("Finiette", 30) },
                },
                new Dictionary<string, List<string>>
                {
                    {
                        "default",
                        new List<string> { "I love skating!", "FIGHT!" }
                    },
                    {
                        "defeated",
                        new List<string> { "Good luck with the boss", "It's so cold in here" }
                    },
                },
                new List<string> { "right" },
                true,
                false,
                "forest"
            )
        },
        {
            "o5",
            new TrainerData(
                new Dictionary<int, (string, int)>
                {
                    { 0, ("Plumette", 20) },
                    { 1, ("Ivieron", 22) },
                    { 2, ("Atrox", 24) },
                    { 3, ("Pouch", 19) },
                },
                new Dictionary<string, List<string>>
                {
                    {
                        "default",
                        new List<string> { "I love skating!", "FIGHT!" }
                    },
                    {
                        "defeated",
                        new List<string> { "Good luck with the boss", "It's so cold in here" }
                    },
                },
                new List<string> { "up", "right" },
                true,
                false,
                "forest"
            )
        },
        {
            "o6",
            new TrainerData(
                new Dictionary<int, (string, int)>
                {
                    { 0, ("Finsta", 15) },
                    { 1, ("Finsta", 15) },
                    { 2, ("Finsta", 15) },
                },
                new Dictionary<string, List<string>>
                {
                    {
                        "default",
                        new List<string> { "I love skating!", "FIGHT!" }
                    },
                    {
                        "defeated",
                        new List<string> { "Good luck with the boss", "It's so cold in here" }
                    },
                },
                new List<string> { "down" },
                false,
                false,
                "ice"
            )
        },
        {
            "o7",
            new TrainerData(
                new Dictionary<int, (string, int)>
                {
                    { 0, ("Friolera", 25) },
                    { 1, ("Gulfin", 20) },
                    { 2, ("Atrox", 24) },
                    { 3, ("Finiette", 30) },
                },
                new Dictionary<string, List<string>>
                {
                    {
                        "default",
                        new List<string> { "There are no bugs in the snow!" }
                    },
                    {
                        "defeated",
                        new List<string>
                        {
                            "Maybe I should check a vulcano...",
                            "It's so cold in here",
                        }
                    },
                },
                new List<string> { "right" },
                false,
                false,
                "ice"
            )
        },
        {
            "p1",
            new TrainerData(
                new Dictionary<int, (string, int)>
                {
                    { 0, ("Friolera", 25) },
                    { 1, ("Gulfin", 20) },
                    { 2, ("Atrox", 24) },
                    { 3, ("Finiette", 30) },
                },
                new Dictionary<string, List<string>>
                {
                    {
                        "default",
                        new List<string> { "I love trees", "and fights" }
                    },
                    {
                        "defeated",
                        new List<string> { "Good luck with the boss!" }
                    },
                },
                new List<string> { "right" },
                false,
                false,
                "forest"
            )
        },
        {
            "p2",
            new TrainerData(
                new Dictionary<int, (string, int)>
                {
                    { 0, ("Friolera", 25) },
                    { 1, ("Gulfin", 20) },
                    { 2, ("Atrox", 24) },
                    { 3, ("Finiette", 30) },
                },
                new Dictionary<string, List<string>>
                {
                    {
                        "default",
                        new List<string> { "I love trees", "and fights" }
                    },
                    {
                        "defeated",
                        new List<string> { "Good luck with the boss!" }
                    },
                },
                new List<string> { "right" },
                false,
                false,
                "forest"
            )
        },
        {
            "p3",
            new TrainerData(
                new Dictionary<int, (string, int)>
                {
                    { 0, ("Friolera", 25) },
                    { 1, ("Gulfin", 20) },
                    { 2, ("Atrox", 24) },
                    { 3, ("Finiette", 30) },
                },
                new Dictionary<string, List<string>>
                {
                    {
                        "default",
                        new List<string> { "I love trees", "and fights" }
                    },
                    {
                        "defeated",
                        new List<string> { "Good luck with the boss!" }
                    },
                },
                new List<string> { "right" },
                false,
                false,
                "forest"
            )
        },
        {
            "p4",
            new TrainerData(
                new Dictionary<int, (string, int)>
                {
                    { 0, ("Friolera", 25) },
                    { 1, ("Gulfin", 20) },
                    { 2, ("Atrox", 24) },
                    { 3, ("Finiette", 30) },
                },
                new Dictionary<string, List<string>>
                {
                    {
                        "default",
                        new List<string> { "I love trees", "and fights" }
                    },
                    {
                        "defeated",
                        new List<string> { "Good luck with the boss!" }
                    },
                },
                new List<string> { "right" },
                false,
                false,
                "forest"
            )
        },
        {
            "px",
            new TrainerData(
                new Dictionary<int, (string, int)>
                {
                    { 0, ("Friolera", 25) },
                    { 1, ("Gulfin", 20) },
                    { 2, ("Atrox", 24) },
                    { 3, ("Finiette", 30) },
                },
                new Dictionary<string, List<string>>
                {
                    {
                        "default",
                        new List<string> { "I love trees", "and fights" }
                    },
                    {
                        "defeated",
                        new List<string> { "Good luck with the boss!" }
                    },
                },
                new List<string> { "right" },
                false,
                false,
                "forest"
            )
        },
        {
            "w1",
            new TrainerData(
                new Dictionary<int, (string, int)>
                {
                    { 0, ("Friolera", 25) },
                    { 1, ("Gulfin", 20) },
                    { 2, ("Draem", 24) },
                    { 3, ("Finiette", 30) },
                },
                new Dictionary<string, List<string>>
                {
                    {
                        "default",
                        new List<string> { "It's so cold in here", "maybe a fight will warm me up" }
                    },
                    {
                        "defeated",
                        new List<string> { "Good luck with the boss!" }
                    },
                },
                new List<string> { "left" },
                true,
                false,
                "ice"
            )
        },
        {
            "w2",
            new TrainerData(
                new Dictionary<int, (string, int)>
                {
                    { 0, ("Friolera", 25) },
                    { 1, ("Gulfin", 20) },
                    { 2, ("Draem", 24) },
                    { 3, ("Finiette", 30) },
                },
                new Dictionary<string, List<string>>
                {
                    {
                        "default",
                        new List<string> { "It's so cold in here", "maybe a fight will warm me up" }
                    },
                    {
                        "defeated",
                        new List<string> { "Good luck with the boss!" }
                    },
                },
                new List<string> { "right" },
                true,
                false,
                "ice"
            )
        },
        {
            "w3",
            new TrainerData(
                new Dictionary<int, (string, int)>
                {
                    { 0, ("Friolera", 25) },
                    { 1, ("Gulfin", 20) },
                    { 2, ("Draem", 24) },
                    { 3, ("Finiette", 30) },
                },
                new Dictionary<string, List<string>>
                {
                    {
                        "default",
                        new List<string> { "It's so cold in here", "maybe a fight will warm me up" }
                    },
                    {
                        "defeated",
                        new List<string> { "Good luck with the boss!" }
                    },
                },
                new List<string> { "left" },
                true,
                false,
                "ice"
            )
        },
        {
            "w4",
            new TrainerData(
                new Dictionary<int, (string, int)>
                {
                    { 0, ("Friolera", 25) },
                    { 1, ("Gulfin", 20) },
                    { 2, ("Draem", 24) },
                    { 3, ("Finiette", 30) },
                },
                new Dictionary<string, List<string>>
                {
                    {
                        "default",
                        new List<string> { "It's so cold in here", "maybe a fight will warm me up" }
                    },
                    {
                        "defeated",
                        new List<string> { "Good luck with the boss!" }
                    },
                },
                new List<string> { "right" },
                true,
                false,
                "ice"
            )
        },
        {
            "w5",
            new TrainerData(
                new Dictionary<int, (string, int)>
                {
                    { 0, ("Friolera", 25) },
                    { 1, ("Gulfin", 20) },
                    { 2, ("Draem", 24) },
                    { 3, ("Finiette", 30) },
                },
                new Dictionary<string, List<string>>
                {
                    {
                        "default",
                        new List<string> { "It's so cold in here", "maybe a fight will warm me up" }
                    },
                    {
                        "defeated",
                        new List<string> { "Good luck with the boss!" }
                    },
                },
                new List<string> { "left" },
                true,
                false,
                "ice"
            )
        },
        {
            "wx",
            new TrainerData(
                new Dictionary<int, (string, int)>
                {
                    { 0, ("Friolera", 25) },
                    { 1, ("Gulfin", 20) },
                    { 2, ("Draem", 24) },
                    { 3, ("Finiette", 30) },
                },
                new Dictionary<string, List<string>>
                {
                    {
                        "default",
                        new List<string> { "This place feels kinda warm...", "fight!" }
                    },
                    {
                        "defeated",
                        new List<string> { "Congratultion!" }
                    },
                },
                new List<string> { "down" },
                true,
                false,
                "ice"
            )
        },
        {
            "f1",
            new TrainerData(
                new Dictionary<int, (string, int)>
                {
                    { 0, ("Cindrill", 15) },
                    { 1, ("Jacana", 20) },
                    { 2, ("Draem", 24) },
                    { 3, ("Atrox", 30) },
                },
                new Dictionary<string, List<string>>
                {
                    {
                        "default",
                        new List<string> { "This place feels kinda warm...", "fight!" }
                    },
                    {
                        "defeated",
                        new List<string> { "Congratultion!" }
                    },
                },
                new List<string> { "right" },
                true,
                false,
                "sand"
            )
        },
        {
            "f2",
            new TrainerData(
                new Dictionary<int, (string, int)>
                {
                    { 0, ("Cindrill", 15) },
                    { 1, ("Jacana", 20) },
                    { 2, ("Draem", 24) },
                    { 3, ("Atrox", 30) },
                },
                new Dictionary<string, List<string>>
                {
                    {
                        "default",
                        new List<string> { "This place feels kinda warm...", "fight!" }
                    },
                    {
                        "defeated",
                        new List<string> { "Congratultion!" }
                    },
                },
                new List<string> { "right", "left" },
                false,
                false,
                "sand"
            )
        },
        {
            "f3",
            new TrainerData(
                new Dictionary<int, (string, int)>
                {
                    { 0, ("Cindrill", 15) },
                    { 1, ("Jacana", 20) },
                    { 2, ("Draem", 24) },
                    { 3, ("Atrox", 30) },
                },
                new Dictionary<string, List<string>>
                {
                    {
                        "default",
                        new List<string> { "This place feels kinda warm...", "fight!" }
                    },
                    {
                        "defeated",
                        new List<string> { "Congratultion!" }
                    },
                },
                new List<string> { "right", "left" },
                true,
                false,
                "sand"
            )
        },
        {
            "f4",
            new TrainerData(
                new Dictionary<int, (string, int)>
                {
                    { 0, ("Cindrill", 15) },
                    { 1, ("Jacana", 20) },
                    { 2, ("Draem", 24) },
                    { 3, ("Atrox", 30) },
                },
                new Dictionary<string, List<string>>
                {
                    {
                        "default",
                        new List<string> { "This place feels kinda warm...", "fight!" }
                    },
                    {
                        "defeated",
                        new List<string> { "Congratultion!" }
                    },
                },
                new List<string> { "up", "right" },
                true,
                false,
                "sand"
            )
        },
        {
            "f5",
            new TrainerData(
                new Dictionary<int, (string, int)>
                {
                    { 0, ("Cindrill", 15) },
                    { 1, ("Jacana", 20) },
                    { 2, ("Draem", 24) },
                    { 3, ("Atrox", 30) },
                },
                new Dictionary<string, List<string>>
                {
                    {
                        "default",
                        new List<string> { "This place feels kinda warm...", "fight!" }
                    },
                    {
                        "defeated",
                        new List<string> { "Congratultion!" }
                    },
                },
                new List<string> { "left" },
                true,
                false,
                "sand"
            )
        },
        {
            "f6",
            new TrainerData(
                new Dictionary<int, (string, int)>
                {
                    { 0, ("Cindrill", 15) },
                    { 1, ("Jacana", 20) },
                    { 2, ("Draem", 24) },
                    { 3, ("Atrox", 30) },
                },
                new Dictionary<string, List<string>>
                {
                    {
                        "default",
                        new List<string> { "This place feels kinda warm...", "fight!" }
                    },
                    {
                        "defeated",
                        new List<string> { "Congratultion!" }
                    },
                },
                new List<string> { "right" },
                true,
                false,
                "sand"
            )
        },
        {
            "fx",
            new TrainerData(
                new Dictionary<int, (string, int)>
                {
                    { 0, ("Cindrill", 15) },
                    { 1, ("Jacana", 20) },
                    { 2, ("Draem", 24) },
                    { 3, ("Atrox", 30) },
                },
                new Dictionary<string, List<string>>
                {
                    {
                        "default",
                        new List<string> { "Time to bring the heat", "fight!" }
                    },
                    {
                        "defeated",
                        new List<string> { "Congratultion!" }
                    },
                },
                new List<string> { "down" },
                false,
                false,
                "sand"
            )
        },
        {
            "Nurse",
            new TrainerData(
                new Dictionary<int, (string, int)>(),
                new Dictionary<string, List<string>>
                {
                    {
                        "default",
                        new List<string>
                        {
                            "Welcome to the hospital",
                            "Your monsters have been healed",
                        }
                    },
                },
                new List<string> { "down" },
                false,
                false,
                null
            )
        },
    };

    public static readonly Dictionary<string, MonsterData> Monsters = new Dictionary<
        string,
        MonsterData
    >
    {
        {
            "Bulbasaur",
            new MonsterData(
                1,
                "Grass",
                "Base",
                new MonsterStats(7, 2, 3, 3),
                new SecondaryAction(
                    "Leech Seed",
                    "Target takes 1 damage at the start of its next turn. Bulbasaur heals 1 HP.",
                    3
                ),
                2
            )
        },
        {
            "Ivysaur",
            new MonsterData(
                2,
                "Grass",
                "Stage1",
                new MonsterStats(8, 3, 3, 3),
                new SecondaryAction(
                    "Vine Grab",
                    "Deal 1 damage and pull the target 1 tile closer.",
                    3
                ),
                3
            )
        },
        {
            "Venusaur",
            new MonsterData(
                3,
                "Grass",
                "Stage2",
                new MonsterStats(10, 4, 3, 2),
                new SecondaryAction(
                    "Growth",
                    "Heal 2 HP and gain +1 attack until end of next turn.",
                    0
                ),
                null
            )
        },
        {
            "Charmander",
            new MonsterData(
                4,
                "Fire",
                "Base",
                new MonsterStats(6, 3, 3, 3),
                new SecondaryAction(
                    "Ember Mark",
                    "Deal 1 damage. Target takes +1 damage from the next attack this round.",
                    3
                ),
                5
            )
        },
        {
            "Charmeleon",
            new MonsterData(
                5,
                "Fire",
                "Stage1",
                new MonsterStats(8, 3, 3, 3),
                new SecondaryAction(
                    "Flame Dash",
                    "Move up to 2 tiles, then deal 2 damage to a Pokémon in range.",
                    3
                ),
                6
            )
        },
        {
            "Charizard",
            new MonsterData(
                6,
                "Fire",
                "Stage2",
                new MonsterStats(9, 4, 4, 4),
                new SecondaryAction("Fire Sweep", "Deal 2 damage to up to 2 Pokémon in range.", 4),
                null
            )
        },
        {
            "Squirtle",
            new MonsterData(
                7,
                "Water",
                "Base",
                new MonsterStats(8, 2, 3, 2),
                new SecondaryAction("Withdraw", "Reduce all damage taken by 2 until next turn.", 0),
                8
            )
        },
        {
            "Wartortle",
            new MonsterData(
                8,
                "Water",
                "Stage1",
                new MonsterStats(9, 3, 3, 3),
                new SecondaryAction(
                    "Water Push",
                    "Deal 1 damage and push the target 1 tile away.",
                    3
                ),
                9
            )
        },
        {
            "Blastoise",
            new MonsterData(
                9,
                "Water",
                "Stage2",
                new MonsterStats(10, 4, 4, 2),
                new SecondaryAction(
                    "Hydro Barrage",
                    "Cannot move this turn. Deal 3 damage to all Pokémon within range 2.",
                    0
                ),
                null
            )
        },
    };

    public static readonly Dictionary<string, AttackData> Attacks = new Dictionary<
        string,
        AttackData
    >
    {
        { "burn", new AttackData("opponent", 2f, 15, "fire", "fire") },
        { "heal", new AttackData("player", -1.2f, 15, "plant", "green") },
        { "battlecry", new AttackData("player", 1.4f, 20, "normal", "green") },
        { "spark", new AttackData("opponent", 1.1f, 20, "fire", "fire") },
        { "scratch", new AttackData("opponent", 1.2f, 20, "normal", "scratch") },
        { "splash", new AttackData("opponent", 2f, 15, "water", "splash") },
        { "fire", new AttackData("opponent", 2f, 15, "fire", "fire") },
        { "explosion", new AttackData("opponent", 2f, 90, "fire", "explosion") },
        { "annihilate", new AttackData("opponent", 2f, 15, "fire", "explosion") },
        { "ice", new AttackData("opponent", 2f, 15, "water", "ice") },
    };
}
