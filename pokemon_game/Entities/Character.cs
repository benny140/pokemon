using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using pokemon_game.Core;

namespace pokemon_game.Entities;

public class Character
{
    protected Texture2D _texture;
    protected Vector2 _position;
    protected int _currentFrame;
    protected int _direction; // 0=down, 1=left, 2=right, 3=up
    protected float _animationTimer;
    protected const float ANIMATION_SPEED = 0.15f;
    protected bool _isMoving;
    protected int _frameWidth;
    protected int _frameHeight;

    public Vector2 Position => _position;
    public int Direction => _direction;

    public Character(Texture2D texture, Vector2 position, string direction = "down")
    {
        _texture = texture;
        _position = position;
        _currentFrame = 0;
        _direction = DirectionStringToInt(direction);
        _isMoving = false;

        // Calculate frame size based on texture dimensions (4x4 grid)
        _frameWidth = texture.Width / 4;
        _frameHeight = texture.Height / 4;
    }

    protected int DirectionStringToInt(string direction)
    {
        return direction.ToLower() switch
        {
            "down" => 0,
            "left" => 1,
            "right" => 2,
            "up" => 3,
            _ => 0, // Default to down
        };
    }

    public virtual void Update(GameTime gameTime)
    {
        // Update animation if moving
        if (_isMoving)
        {
            _animationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_animationTimer >= ANIMATION_SPEED)
            {
                _animationTimer = 0;
                _currentFrame = (_currentFrame + 1) % 4; // Cycle through 4 frames
            }
        }
        else
        {
            _currentFrame = 0; // Reset to idle frame when not moving
            _animationTimer = 0;
        }
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        // Calculate source rectangle based on current frame and direction
        var sourceRect = new Rectangle(
            _currentFrame * _frameWidth,
            _direction * _frameHeight,
            _frameWidth,
            _frameHeight
        );

        // Draw the character
        spriteBatch.Draw(
            _texture,
            _position,
            sourceRect,
            Color.White,
            0f,
            new Vector2(_frameWidth / 2, _frameHeight), // Origin at bottom-center for proper positioning
            1f,
            SpriteEffects.None,
            _position.Y / 10000f // Layer based on Y position for depth sorting
        );
    }
}
