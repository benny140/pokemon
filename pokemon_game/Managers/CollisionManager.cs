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
}
