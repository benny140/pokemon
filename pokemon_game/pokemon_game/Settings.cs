using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace pokemon_game;

public static class Settings
{
    // Window and Game Constants
    public const int WINDOW_WIDTH = 1920;
    public const int WINDOW_HEIGHT = 1080;
    public const float ZOOM_SCALE = 0.5f; // 2x more compact (shows 4x the area)
    public const int TILE_SIZE = 64;
    public const int ANIMATION_SPEED = 6;
    public const int BATTLE_OUTLINE_WIDTH = 4;

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
    }

    // Battle position data structures
    public struct PositionSet
    {
        public Vector2 Top;
        public Vector2 Center;
        public Vector2 Bottom;

        public PositionSet(Vector2 top, Vector2 center, Vector2 bottom)
        {
            Top = top;
            Center = center;
            Bottom = bottom;
        }
    }

    public static class BattlePositions
    {
        public static readonly PositionSet Left = new PositionSet(
            new Vector2(360, 260),
            new Vector2(190, 400),
            new Vector2(410, 520)
        );

        public static readonly PositionSet Right = new PositionSet(
            new Vector2(900, 260),
            new Vector2(1110, 390),
            new Vector2(900, 550)
        );
    }

    // Battle choice data structures
    public struct BattleChoice
    {
        public Vector2 Position;
        public string Icon;

        public BattleChoice(Vector2 position, string icon)
        {
            Position = position;
            Icon = icon;
        }
    }

    public struct BattleChoiceSet
    {
        public BattleChoice Fight;
        public BattleChoice Defend;
        public BattleChoice Switch;
        public BattleChoice? Catch; // Nullable since it's not in limited set

        public BattleChoiceSet(
            BattleChoice fight,
            BattleChoice defend,
            BattleChoice switchChoice,
            BattleChoice? catchChoice = null
        )
        {
            Fight = fight;
            Defend = defend;
            Switch = switchChoice;
            Catch = catchChoice;
        }
    }

    public static class BattleChoices
    {
        public static readonly BattleChoiceSet Full = new BattleChoiceSet(
            new BattleChoice(new Vector2(30, -60), "sword"),
            new BattleChoice(new Vector2(40, -20), "shield"),
            new BattleChoice(new Vector2(40, 20), "arrows"),
            new BattleChoice(new Vector2(30, 60), "hand")
        );

        public static readonly BattleChoiceSet Limited = new BattleChoiceSet(
            new BattleChoice(new Vector2(30, -40), "sword"),
            new BattleChoice(new Vector2(40, 0), "shield"),
            new BattleChoice(new Vector2(30, 40), "arrows")
        );
    }
}
