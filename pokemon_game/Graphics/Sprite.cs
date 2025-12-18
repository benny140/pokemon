using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace pokemon_game.Graphics;

public class Sprite
{
    public Texture2D Texture { get; set; }
    public Vector2 Position { get; set; }
    public Vector2 Size { get; set; }
    public float Layer { get; set; } = 0f;

    public Sprite(Texture2D texture, Vector2 position, Vector2 size)
    {
        Texture = texture;
        Position = position;
        Size = size;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            Texture,
            Position,
            null,
            Color.White,
            0f,
            Vector2.Zero,
            Size / new Vector2(Texture.Width, Texture.Height),
            SpriteEffects.None,
            Layer
        );
    }
}
