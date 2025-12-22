using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace pokemon_game.Core;

public static class Settings
{
    // Window and Game Constants
    public const int WINDOW_WIDTH = 1920;
    public const int WINDOW_HEIGHT = 1080;
    public const float ZOOM_SCALE = 0.8f; // Scale down for better visibility
    public const int TILE_SIZE = 64;
    public const int ANIMATION_SPEED = 6;
    public const int BATTLE_OUTLINE_WIDTH = 4;
    public const float PLAYER_MOVE_SPEED = 200f;

    // Colors - converted from hex to MonoGame Color
    public static class Colors
    {
        public static readonly Color White = new Color(244, 254, 250);
        public static readonly Color PureWhite = Color.White;
        public static readonly Color Dark = new Color(43, 41, 44);
        public static readonly Color Light = new Color(200, 200, 200);
        public static readonly Color Gray = new Color(58, 55, 59);
        public static readonly Color Gold = new Color(255, 215, 0);
        public static readonly Color LightGray = new Color(75, 72, 77);
        public static readonly Color Fire = new Color(248, 160, 96);
        public static readonly Color Water = new Color(80, 176, 216);
        public static readonly Color Plant = new Color(100, 169, 144);
        public static readonly Color Black = Color.Black;
        public static readonly Color Red = new Color(240, 49, 49);
        public static readonly Color Blue = new Color(102, 215, 238);
    }

    // Layer enums for type safety
    public enum WorldLayer
    {
        Water = 0,
        Bg = 1,
        Shadow = 2,
        Main = 3,
        Top = 4,
    }

    public enum BattleLayer
    {
        Outline = 0,
        Name = 1,
        Monster = 2,
        Effects = 3,
        Overlay = 4,
        Opponent = 5,
        Player = 6,
    }

    // Monster Data
    public static class MonsterData
    {
        public static readonly Dictionary<string, MonsterInfo> Monsters = new Dictionary<
            string,
            MonsterInfo
        >
        {
            {
                "Plumette",
                new MonsterInfo
                {
                    Health = 22,
                    Energy = 5,
                    Attack = 8,
                    Defense = 8,
                    Speed = 7,
                    Element = "plant",
                    Attacks = new[] { "Scratch", "Spark", "Fire" },
                }
            },
            {
                "Sparchu",
                new MonsterInfo
                {
                    Health = 18,
                    Energy = 7,
                    Attack = 7,
                    Defense = 6,
                    Speed = 9,
                    Element = "fire",
                    Attacks = new[] { "Scratch", "Spark", "Fire" },
                }
            },
            {
                "Charmadillo",
                new MonsterInfo
                {
                    Health = 20,
                    Energy = 6,
                    Attack = 9,
                    Defense = 7,
                    Speed = 6,
                    Element = "fire",
                    Attacks = new[] { "Scratch", "Spark", "Fire" },
                }
            },
            {
                "Finsta",
                new MonsterInfo
                {
                    Health = 24,
                    Energy = 5,
                    Attack = 6,
                    Defense = 9,
                    Speed = 5,
                    Element = "water",
                    Attacks = new[] { "Scratch", "Spark", "Ice" },
                }
            },
            {
                "Gulfin",
                new MonsterInfo
                {
                    Health = 21,
                    Energy = 6,
                    Attack = 7,
                    Defense = 7,
                    Speed = 8,
                    Element = "water",
                    Attacks = new[] { "Scratch", "Spark", "Ice" },
                }
            },
            {
                "Pouch",
                new MonsterInfo
                {
                    Health = 26,
                    Energy = 4,
                    Attack = 7,
                    Defense = 10,
                    Speed = 4,
                    Element = "plant",
                    Attacks = new[] { "Scratch", "Spark", "Explosion" },
                }
            },
            {
                "Draem",
                new MonsterInfo
                {
                    Health = 19,
                    Energy = 8,
                    Attack = 6,
                    Defense = 5,
                    Speed = 11,
                    Element = "normal",
                    Attacks = new[] { "Scratch", "Spark", "Explosion" },
                }
            },
            {
                "Cindrill",
                new MonsterInfo
                {
                    Health = 23,
                    Energy = 6,
                    Attack = 10,
                    Defense = 6,
                    Speed = 7,
                    Element = "fire",
                    Attacks = new[] { "Scratch", "Spark", "Fire" },
                }
            },
            {
                "Jacana",
                new MonsterInfo
                {
                    Health = 17,
                    Energy = 7,
                    Attack = 5,
                    Defense = 7,
                    Speed = 10,
                    Element = "water",
                    Attacks = new[] { "Scratch", "Spark", "Ice" },
                }
            },
            {
                "Atrox",
                new MonsterInfo
                {
                    Health = 25,
                    Energy = 5,
                    Attack = 11,
                    Defense = 8,
                    Speed = 5,
                    Element = "normal",
                    Attacks = new[] { "Scratch", "Spark", "Explosion" },
                }
            },
            {
                "Cleaf",
                new MonsterInfo
                {
                    Health = 20,
                    Energy = 7,
                    Attack = 5,
                    Defense = 8,
                    Speed = 8,
                    Element = "plant",
                    Attacks = new[] { "Scratch", "Spark", "Splash" },
                }
            },
            {
                "Finiette",
                new MonsterInfo
                {
                    Health = 22,
                    Energy = 6,
                    Attack = 6,
                    Defense = 9,
                    Speed = 7,
                    Element = "water",
                    Attacks = new[] { "Scratch", "Spark", "Ice" },
                }
            },
            {
                "Ivieron",
                new MonsterInfo
                {
                    Health = 28,
                    Energy = 3,
                    Attack = 8,
                    Defense = 12,
                    Speed = 3,
                    Element = "plant",
                    Attacks = new[] { "Scratch", "Spark", "Splash" },
                }
            },
            {
                "Bronyx",
                new MonsterInfo
                {
                    Health = 16,
                    Energy = 9,
                    Attack = 4,
                    Defense = 4,
                    Speed = 12,
                    Element = "normal",
                    Attacks = new[] { "Scratch", "Spark", "Explosion" },
                }
            },
        };

        public static readonly Dictionary<string, AttackInfo> Attacks = new Dictionary<
            string,
            AttackInfo
        >
        {
            {
                "Scratch",
                new AttackInfo
                {
                    Element = "normal",
                    Amount = 20,
                    Cost = 1,
                    Animation = "scratch",
                }
            },
            {
                "Spark",
                new AttackInfo
                {
                    Element = "fire",
                    Amount = 30,
                    Cost = 2,
                    Animation = "explosion",
                }
            },
            {
                "Fire",
                new AttackInfo
                {
                    Element = "fire",
                    Amount = 40,
                    Cost = 3,
                    Animation = "fire",
                }
            },
            {
                "Ice",
                new AttackInfo
                {
                    Element = "water",
                    Amount = 40,
                    Cost = 3,
                    Animation = "ice",
                }
            },
            {
                "Splash",
                new AttackInfo
                {
                    Element = "plant",
                    Amount = 40,
                    Cost = 3,
                    Animation = "splash",
                }
            },
            {
                "Explosion",
                new AttackInfo
                {
                    Element = "normal",
                    Amount = 50,
                    Cost = 4,
                    Animation = "explosion",
                }
            },
        };

        public static readonly Dictionary<string, float> EffectivenessChart = new Dictionary<
            string,
            float
        >
        {
            { "fire_fire", 0.5f },
            { "fire_water", 0.5f },
            { "fire_plant", 2.0f },
            { "water_fire", 2.0f },
            { "water_water", 0.5f },
            { "water_plant", 0.5f },
            { "plant_fire", 0.5f },
            { "plant_water", 2.0f },
            { "plant_plant", 0.5f },
        };
    }

    // Helper structs
    public struct MonsterInfo
    {
        public int Health { get; set; }
        public int Energy { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Speed { get; set; }
        public string Element { get; set; }
        public string[] Attacks { get; set; }
    }

    public struct AttackInfo
    {
        public string Element { get; set; }
        public int Amount { get; set; }
        public int Cost { get; set; }
        public string Animation { get; set; }
    }
}
