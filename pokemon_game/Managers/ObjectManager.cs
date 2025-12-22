using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using pokemon_game.Graphics;

namespace pokemon_game.Managers;

public class ObjectManager
{
    private readonly List<Sprite> _sprites = new List<Sprite>();
    private readonly List<Sprite> _topSprites = new List<Sprite>();

    public void LoadObjects(TiledMap tiledMap)
    {
        _sprites.Clear();
        _topSprites.Clear();

        var objectsLayer = tiledMap.GetLayer<TiledMapObjectLayer>("Objects");
        if (objectsLayer == null)
            return;

        foreach (var obj in objectsLayer.Objects)
        {
            if (obj is TiledMapTileObject tileObject && tileObject.Tile != null)
            {
                // Load the texture from the tile's tileset
                var texture = tileObject.Tile.Texture;

                // Position needs to account for Tiled's coordinate system (y-up, origin at bottom-left of object)
                var position = new Vector2(obj.Position.X, obj.Position.Y - obj.Size.Height);

                var sprite = new Sprite(texture, position, obj.Size);

                // Check if this is a "top" object that should render above everything
                bool isTopObject = obj.Name?.ToLower() == "top";

                if (isTopObject)
                {
                    // Top objects use max layer to always render on top
                    sprite.Layer = 0.9999f;
                    _topSprites.Add(sprite);
                }
                else
                {
                    // Layer based on bottom of sprite (obj.Position.Y) for depth sorting
                    sprite.Layer = obj.Position.Y / 10000f;
                    _sprites.Add(sprite);
                }
            }
        }

        // Sort sprites by layer (Y position) for proper draw order
        _sprites.Sort((a, b) => a.Layer.CompareTo(b.Layer));
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var sprite in _sprites)
        {
            sprite.Draw(spriteBatch);
        }
    }

    public void DrawTopObjects(SpriteBatch spriteBatch)
    {
        foreach (var sprite in _topSprites)
        {
            sprite.Draw(spriteBatch);
        }
    }
}
