using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Tiled;

namespace pokemon_game.Managers;

public class CollisionManager
{
    private readonly List<Rectangle> _collisionRects = new List<Rectangle>();

    public void LoadCollisions(TiledMap tiledMap)
    {
        _collisionRects.Clear();

        var collisionsLayer = tiledMap.GetLayer<TiledMapObjectLayer>("Collisions");
        if (collisionsLayer == null)
            return;

        foreach (var obj in collisionsLayer.Objects)
        {
            // Convert Tiled object to collision rectangle
            var rect = new Rectangle(
                (int)obj.Position.X,
                (int)obj.Position.Y,
                (int)obj.Size.Width,
                (int)obj.Size.Height
            );
            _collisionRects.Add(rect);
        }
    }

    public bool CheckCollision(Rectangle bounds)
    {
        foreach (var collisionRect in _collisionRects)
        {
            if (bounds.Intersects(collisionRect))
            {
                return true;
            }
        }
        return false;
    }

    public bool HasLineOfSight(Vector2 from, Vector2 to, int tileSize)
    {
        // Check if there are any collision tiles between two points
        // Sample points along the line
        Vector2 direction = to - from;
        float distance = direction.Length();

        if (distance < 1)
            return true;

        direction.Normalize();

        // Sample every tile along the path
        int steps = (int)(distance / tileSize) + 1;

        for (int i = 0; i <= steps; i++)
        {
            float t = i / (float)steps;
            Vector2 checkPoint = from + direction * (distance * t);

            // Create a small rectangle at the check point
            Rectangle checkRect = new Rectangle(
                (int)checkPoint.X - tileSize / 4,
                (int)checkPoint.Y - tileSize / 4,
                tileSize / 2,
                tileSize / 2
            );

            if (CheckCollision(checkRect))
            {
                return false; // Line of sight blocked
            }
        }

        return true; // Clear line of sight
    }
}
