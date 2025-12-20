using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using pokemon_game.Graphics;

namespace pokemon_game.Managers;

public class MonsterManager
{
    private readonly List<Sprite> _grassSprites = new List<Sprite>();
    private Texture2D _grassTexture;
    private Texture2D _grassIceTexture;

    public void LoadTextures(Texture2D grassTexture, Texture2D grassIceTexture)
    {
        _grassTexture = grassTexture;
        _grassIceTexture = grassIceTexture;
    }

    public void LoadMonsters(TiledMap tiledMap)
    {
        _grassSprites.Clear();

        var monstersLayer = tiledMap.GetLayer<TiledMapObjectLayer>("Monsters");
        if (monstersLayer == null)
            return;

        foreach (var obj in monstersLayer.Objects)
        {
            // Check if the object has a biome property
            string biome = null;
            if (obj.Properties.ContainsKey("biome"))
            {
                biome = obj.Properties["biome"];
            }

            // Choose texture based on biome
            Texture2D textureToUse = biome == "ice" ? _grassIceTexture : _grassTexture;

            // Create sprite at the object's position
            var position = new Vector2(obj.Position.X, obj.Position.Y);
            var size = new Vector2(obj.Size.Width, obj.Size.Height);

            var sprite = new Sprite(textureToUse, position, size);
            // Layer based on Y position for depth sorting
            sprite.Layer = (obj.Position.Y + obj.Size.Height) / 10000f;

            _grassSprites.Add(sprite);
        }

        // Sort sprites by layer (Y position) for proper draw order
        _grassSprites.Sort((a, b) => a.Layer.CompareTo(b.Layer));
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var sprite in _grassSprites)
        {
            sprite.Draw(spriteBatch);
        }
    }
}
