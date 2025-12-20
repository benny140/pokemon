using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using pokemon_game.Core;
using pokemon_game.Managers;

namespace pokemon_game.Entities;

public class Player
{
    private Texture2D _texture;
    private Vector2 _position;
    private int _currentFrame;
    private int _direction; // 0=down, 1=left, 2=right, 3=up
    private float _animationTimer;
    private const float ANIMATION_SPEED = 0.15f;
    private bool _isMoving;
    private int _frameWidth;
    private int _frameHeight;
    private CollisionManager _collisionManager;

    public Vector2 Position => _position;

    public Player(
        Texture2D texture,
        Vector2 startPosition,
        CollisionManager collisionManager = null
    )
    {
        _texture = texture;
        _position = startPosition;
        _currentFrame = 0;
        _direction = 0; // Start facing down
        _collisionManager = collisionManager;

        // Calculate frame size based on texture dimensions (4x4 grid)
        _frameWidth = texture.Width / 4;
        _frameHeight = texture.Height / 4;
    }

    public Rectangle GetBounds()
    {
        // Collision box is slightly smaller than the sprite for better feel
        // Centered on the bottom half of the sprite
        int collisionWidth = _frameWidth / 2;
        int collisionHeight = _frameHeight / 3;
        return new Rectangle(
            (int)_position.X - collisionWidth / 2,
            (int)_position.Y - collisionHeight,
            collisionWidth,
            collisionHeight
        );
    }

    public void Update(GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();
        var velocity = Vector2.Zero;
        _isMoving = false;

        // Handle input and set direction
        if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
        {
            velocity.Y = -1;
            _direction = 3; // Up
            _isMoving = true;
        }
        else if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
        {
            velocity.Y = 1;
            _direction = 0; // Down
            _isMoving = true;
        }

        if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
        {
            velocity.X = -1;
            _direction = 1; // Left
            _isMoving = true;
        }
        else if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
        {
            velocity.X = 1;
            _direction = 2; // Right
            _isMoving = true;
        }

        // Normalize diagonal movement
        if (velocity.Length() > 0)
        {
            velocity.Normalize();
        }

        // Move player with collision detection
        if (velocity.Length() > 0)
        {
            Vector2 movement =
                velocity
                * Settings.PLAYER_MOVE_SPEED
                * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 newPosition = _position + movement;

            // Check collision if collision manager is set
            if (_collisionManager != null)
            {
                // Create bounds at new position
                int collisionWidth = _frameWidth / 2;
                int collisionHeight = _frameHeight / 3;
                Rectangle newBounds = new Rectangle(
                    (int)newPosition.X - collisionWidth / 2,
                    (int)newPosition.Y - collisionHeight,
                    collisionWidth,
                    collisionHeight
                );

                // Only move if there's no collision
                if (!_collisionManager.CheckCollision(newBounds))
                {
                    _position = newPosition;
                }
            }
            else
            {
                // No collision manager, move freely
                _position = newPosition;
            }
        }

        // Update animation
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

    public void Draw(SpriteBatch spriteBatch)
    {
        // Calculate source rectangle based on current frame and direction
        var sourceRect = new Rectangle(
            _currentFrame * _frameWidth,
            _direction * _frameHeight,
            _frameWidth,
            _frameHeight
        );

        // Draw the player
        // Use scale 1f - the view matrix already applies ZOOM_SCALE to match tiles
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
