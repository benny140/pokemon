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
                new MonsterStats("plant", 20, 22, 5, 10, 6, 1.2f),
                new Dictionary<int, string>
                {
                    { 0, "scratch" },
                    { 5, "spark" },
                    { 10, "heal" },
                },
                ("Ivysaur", 16)
            )
        },
        {
            "Ivysaur",
            new MonsterData(
                new MonsterStats("plant", 28, 26, 6, 12, 8, 1.4f),
                new Dictionary<int, string>
                {
                    { 0, "scratch" },
                    { 5, "spark" },
                    { 10, "heal" },
                },
                ("Venusaur", 32)
            )
        },
        {
            "Venusaur",
            new MonsterData(
                new MonsterStats("plant", 35, 32, 8, 15, 10, 1.6f),
                new Dictionary<int, string>
                {
                    { 0, "scratch" },
                    { 5, "spark" },
                    { 10, "heal" },
                },
                null
            )
        },
        {
            "Charmander",
            new MonsterData(
                new MonsterStats("fire", 18, 24, 6, 8, 5, 1.3f),
                new Dictionary<int, string>
                {
                    { 0, "scratch" },
                    { 5, "spark" },
                    { 10, "fire" },
                },
                ("Charmeleon", 16)
            )
        },
        {
            "Charmeleon",
            new MonsterData(
                new MonsterStats("fire", 26, 28, 8, 10, 7, 1.5f),
                new Dictionary<int, string>
                {
                    { 0, "scratch" },
                    { 5, "spark" },
                    { 10, "fire" },
                },
                ("Charizard", 36)
            )
        },
        {
            "Charizard",
            new MonsterData(
                new MonsterStats("fire", 34, 34, 10, 12, 9, 1.8f),
                new Dictionary<int, string>
                {
                    { 0, "scratch" },
                    { 5, "spark" },
                    { 10, "fire" },
                    { 20, "explosion" },
                },
                null
            )
        },
        {
            "Squirtle",
            new MonsterData(
                new MonsterStats("water", 19, 22, 5, 12, 6, 1.1f),
                new Dictionary<int, string>
                {
                    { 0, "scratch" },
                    { 5, "spark" },
                    { 10, "splash" },
                },
                ("Wartortle", 16)
            )
        },
        {
            "Wartortle",
            new MonsterData(
                new MonsterStats("water", 27, 26, 6, 14, 8, 1.3f),
                new Dictionary<int, string>
                {
                    { 0, "scratch" },
                    { 5, "spark" },
                    { 10, "splash" },
                },
                ("Blastoise", 36)
            )
        },
        {
            "Blastoise",
            new MonsterData(
                new MonsterStats("water", 36, 30, 8, 16, 10, 1.5f),
                new Dictionary<int, string>
                {
                    { 0, "scratch" },
                    { 5, "spark" },
                    { 10, "splash" },
                    { 20, "ice" },
                },
                null
            )
        },
        {
            "Plumette",
            new MonsterData(
                new MonsterStats("plant", 15, 17, 4, 8, 5, 1f),
                new Dictionary<int, string> { { 0, "scratch" }, { 5, "spark" } },
                ("Ivieron", 15)
            )
        },
        {
            "Ivieron",
            new MonsterData(
                new MonsterStats("plant", 18, 20, 5, 10, 6, 1.2f),
                new Dictionary<int, string> { { 0, "scratch" }, { 5, "spark" } },
                ("Pluma", 32)
            )
        },
        {
            "Pluma",
            new MonsterData(
                new MonsterStats("plant", 23, 25, 6, 12, 7, 1.8f),
                new Dictionary<int, string> { { 0, "scratch" }, { 5, "spark" } },
                null
            )
        },
        {
            "Sparchu",
            new MonsterData(
                new MonsterStats("fire", 15, 17, 3, 8, 5, 1f),
                new Dictionary<int, string> { { 0, "scratch" }, { 5, "spark" } },
                ("Cindrill", 15)
            )
        },
        {
            "Cindrill",
            new MonsterData(
                new MonsterStats("fire", 18, 20, 4, 10, 6, 1.2f),
                new Dictionary<int, string> { { 0, "scratch" }, { 5, "spark" } },
                ("Charmadillo", 33)
            )
        },
        {
            "Charmadillo",
            new MonsterData(
                new MonsterStats("fire", 27, 23, 6, 17, 7, 1.5f),
                new Dictionary<int, string>
                {
                    { 0, "scratch" },
                    { 5, "fire" },
                    { 10, "explosion" },
                    { 12, "battlecry" },
                    { 20, "annihilate" },
                },
                null
            )
        },
        {
            "Finsta",
            new MonsterData(
                new MonsterStats("water", 13, 17, 2, 8, 5, 1.8f),
                new Dictionary<int, string> { { 0, "scratch" }, { 5, "spark" } },
                ("Gulfin", 34)
            )
        },
        {
            "Gulfin",
            new MonsterData(
                new MonsterStats("water", 18, 20, 3, 10, 6, 2f),
                new Dictionary<int, string> { { 0, "scratch" }, { 5, "spark" } },
                ("Finiette", 32)
            )
        },
        {
            "Finiette",
            new MonsterData(
                new MonsterStats("water", 27, 23, 4, 17, 7, 2.5f),
                new Dictionary<int, string> { { 0, "scratch" }, { 5, "spark" } },
                null
            )
        },
        {
            "Atrox",
            new MonsterData(
                new MonsterStats("fire", 18, 20, 3, 10, 6, 1.9f),
                new Dictionary<int, string> { { 0, "scratch" }, { 5, "spark" } },
                null
            )
        },
        {
            "Pouch",
            new MonsterData(
                new MonsterStats("plant", 23, 25, 4, 12, 7, 1.5f),
                new Dictionary<int, string> { { 0, "scratch" }, { 5, "spark" } },
                null
            )
        },
        {
            "Draem",
            new MonsterData(
                new MonsterStats("plant", 23, 25, 4, 12, 7, 1.4f),
                new Dictionary<int, string> { { 0, "scratch" }, { 5, "spark" } },
                null
            )
        },
        {
            "Larvea",
            new MonsterData(
                new MonsterStats("plant", 15, 17, 1, 8, 5, 1f),
                new Dictionary<int, string> { { 0, "scratch" }, { 5, "spark" } },
                ("Cleaf", 4)
            )
        },
        {
            "Cleaf",
            new MonsterData(
                new MonsterStats("plant", 18, 20, 3, 10, 6, 1.6f),
                new Dictionary<int, string> { { 0, "scratch" }, { 5, "spark" } },
                null
            )
        },
        {
            "Jacana",
            new MonsterData(
                new MonsterStats("fire", 12, 19, 3, 10, 6, 2.6f),
                new Dictionary<int, string> { { 0, "scratch" }, { 5, "spark" } },
                null
            )
        },
        {
            "Friolera",
            new MonsterData(
                new MonsterStats("water", 27, 23, 4, 17, 7, 2f),
                new Dictionary<int, string>
                {
                    { 0, "scratch" },
                    { 5, "spark" },
                    { 15, "splash" },
                    { 20, "ice" },
                    { 25, "heal" },
                },
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
