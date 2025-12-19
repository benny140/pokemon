using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;

namespace pokemon_game.Managers;

public class CoastAnimationManager
{
    private class CoastTile
    {
        public Vector2 Position;
        public string Terrain;
        public string Side;
    }

    private List<CoastTile> _coastTiles;
    private Texture2D _coastTexture;
    private float _animationTimer;
    private int _currentFrame;
    private const float FRAME_DURATION = 0.3f;
    private const int TILE_SIZE = 64;
    private const int FRAME_COUNT = 4;

    // Terrain types map to column groups (each group is 3 columns wide)
    private readonly Dictionary<string, int> _terrainColumns = new Dictionary<string, int>
    {
        { "grass", 0 },
        { "grass_i", 1 },
        { "sand_i", 2 },
        { "sand", 3 },
        { "rock", 4 },
        { "rock_i", 5 },
        { "ice", 6 },
        { "ice_i", 7 },
    };

    // Side positions within the 3x3 grid
    private readonly Dictionary<string, Point> _sideOffsets = new Dictionary<string, Point>
    {
        { "topleft", new Point(0, 0) },
        { "top", new Point(1, 0) },
        { "topright", new Point(2, 0) },
        { "left", new Point(0, 1) },
        { "center", new Point(1, 1) },
        { "right", new Point(2, 1) },
        { "bottomleft", new Point(0, 2) },
        { "bottom", new Point(1, 2) },
        { "bottomright", new Point(2, 2) },
    };

    public CoastAnimationManager()
    {
        _coastTiles = new List<CoastTile>();
        _animationTimer = 0f;
        _currentFrame = 0;
    }

    public void LoadContent(TiledMap map)
    {
        _coastTiles.Clear();

        // Find the Coast object layer
        TiledMapObjectLayer coastLayer = null;
        foreach (var layer in map.ObjectLayers)
        {
            if (layer.Name == "Coast")
            {
                coastLayer = layer;
                break;
            }
        }

        if (coastLayer == null)
            return;

        // Load coast tiles from objects
        foreach (var obj in coastLayer.Objects)
        {
            if (obj.Name == "coast")
            {
                string terrain = null;
                string side = null;

                // Get properties
                if (obj.Properties != null)
                {
                    foreach (var prop in obj.Properties)
                    {
                        if (prop.Key == "terrain")
                            terrain = prop.Value;
                        else if (prop.Key == "side")
                            side = prop.Value;
                    }
                }

                if (terrain != null && side != null)
                {
                    _coastTiles.Add(
                        new CoastTile
                        {
                            Position = new Vector2(obj.Position.X, obj.Position.Y),
                            Terrain = terrain,
                            Side = side,
                        }
                    );
                }
            }
        }
    }

    public void LoadTexture(Texture2D coastTexture)
    {
        _coastTexture = coastTexture;
    }

    public void Update(GameTime gameTime)
    {
        _animationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_animationTimer >= FRAME_DURATION)
        {
            _animationTimer -= FRAME_DURATION;
            _currentFrame = (_currentFrame + 1) % FRAME_COUNT;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (_coastTexture == null)
            return;

        foreach (var tile in _coastTiles)
        {
            // Get terrain column group (0-7)
            if (!_terrainColumns.TryGetValue(tile.Terrain, out int terrainGroup))
                continue;

            // Get side offset within 3x3 grid
            if (!_sideOffsets.TryGetValue(tile.Side, out Point sideOffset))
                continue;

            // Calculate source rectangle
            // Each terrain group is 3 columns wide
            // Each frame is 3 rows tall
            int sourceX = (terrainGroup * 3 + sideOffset.X) * TILE_SIZE;
            int sourceY = (_currentFrame * 3 + sideOffset.Y) * TILE_SIZE;

            var sourceRect = new Rectangle(sourceX, sourceY, TILE_SIZE, TILE_SIZE);

            spriteBatch.Draw(
                _coastTexture,
                tile.Position,
                sourceRect,
                Color.White,
                0f,
                Vector2.Zero,
                1f,
                SpriteEffects.None,
                tile.Position.Y / 10000f // Layer based on Y position for depth sorting
            );
        }
    }
}
