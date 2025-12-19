using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using pokemon_game.Core;

namespace pokemon_game.Managers;

public class WaterAnimationManager
{
    private List<Rectangle> _waterTiles;
    private Texture2D[] _waterFrames;
    private float _animationTimer;
    private int _currentFrame;
    private const float FRAME_DURATION = 0.3f; // Duration per frame in seconds
    private const int TILE_SIZE = 64;

    public WaterAnimationManager()
    {
        _waterTiles = new List<Rectangle>();
        _waterFrames = new Texture2D[4];
        _animationTimer = 0f;
        _currentFrame = 0;
    }

    public void LoadContent(GraphicsDevice graphicsDevice, TiledMap map)
    {
        // Find the Water object layer
        TiledMapObjectLayer waterLayer = null;
        foreach (var layer in map.ObjectLayers)
        {
            if (layer.Name == "Water")
            {
                waterLayer = layer;
                break;
            }
        }

        if (waterLayer == null)
            return;

        // Convert water objects to tile rectangles
        foreach (var obj in waterLayer.Objects)
        {
            // Each object defines a rectangular area of water
            // We need to break it down into individual tiles
            int startX = (int)obj.Position.X;
            int startY = (int)obj.Position.Y;
            int width = (int)obj.Size.Width;
            int height = (int)obj.Size.Height;

            // Calculate how many tiles fit in this water area
            int tilesX = width / TILE_SIZE;
            int tilesY = height / TILE_SIZE;

            // Create individual tile rectangles
            for (int y = 0; y < tilesY; y++)
            {
                for (int x = 0; x < tilesX; x++)
                {
                    var tileRect = new Rectangle(
                        startX + (x * TILE_SIZE),
                        startY + (y * TILE_SIZE),
                        TILE_SIZE,
                        TILE_SIZE
                    );
                    _waterTiles.Add(tileRect);
                }
            }
        }
    }

    public void LoadTextures(Texture2D frame0, Texture2D frame1, Texture2D frame2, Texture2D frame3)
    {
        _waterFrames[0] = frame0;
        _waterFrames[1] = frame1;
        _waterFrames[2] = frame2;
        _waterFrames[3] = frame3;
    }

    public void Update(GameTime gameTime)
    {
        _animationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_animationTimer >= FRAME_DURATION)
        {
            _animationTimer -= FRAME_DURATION;
            _currentFrame = (_currentFrame + 1) % 4; // Cycle through 4 frames
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (_waterFrames[0] == null)
            return;

        var currentTexture = _waterFrames[_currentFrame];

        foreach (var tile in _waterTiles)
        {
            spriteBatch.Draw(
                currentTexture,
                new Vector2(tile.X, tile.Y),
                null,
                Color.White,
                0f,
                Vector2.Zero,
                1f,
                SpriteEffects.None,
                0f // Draw on bottom layer
            );
        }
    }
}
