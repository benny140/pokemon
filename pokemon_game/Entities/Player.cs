using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using pokemon_game.Core;
using pokemon_game.Managers;

namespace pokemon_game.Entities;

public class Player : Character
{
    private CollisionManager _collisionManager;
    private List<(string Name, int Level)> _monsters;

    public List<(string Name, int Level)> Monsters => _monsters;

    public Player(
        Texture2D texture,
        Vector2 startPosition,
        CollisionManager collisionManager = null
    )
        : base(texture, startPosition, "down")
    {
        _collisionManager = collisionManager;

        // Initialize player with starter Pokemon
        _monsters = new List<(string Name, int Level)>
        {
            ("Bulbasaur", 5),
            ("Charmander", 5),
            ("Squirtle", 5),
        };
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

    public override void Update(GameTime gameTime)
    {
        Update(gameTime, false);
    }

    public void Update(GameTime gameTime, bool isBlocked)
    {
        // Don't process input if blocked
        if (isBlocked)
        {
            _isMoving = false;
            _currentFrame = 0;
            return;
        }

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
}
