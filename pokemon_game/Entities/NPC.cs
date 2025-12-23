using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using pokemon_game.Core;
using pokemon_game.Data;

namespace pokemon_game.Entities;

public class NPC : Character
{
    private string _characterId;
    private bool _hasTriggered;
    private bool _isApproaching;
    private Vector2 _targetPosition;
    private float _moveSpeed = 150f;
    private float _detectionRadius; // in pixels
    private TrainerData _trainerData;
    private static Texture2D _noticeIcon;
    private float _idleRotationTimer;
    private const float IDLE_ROTATION_INTERVAL = 3f; // Rotate every 3 seconds
    private int _currentDirectionIndex;
    private List<int> _allowedDirections;

    public string CharacterId => _characterId;
    public bool HasTriggered => _hasTriggered;
    public bool IsApproaching => _isApproaching;

    public static void SetNoticeIcon(Texture2D texture)
    {
        _noticeIcon = texture;
    }

    public NPC(
        Texture2D texture,
        Vector2 position,
        string direction,
        string characterId,
        float radius = 400f
    )
        : base(texture, position, direction)
    {
        _characterId = characterId;
        _hasTriggered = false;
        _isApproaching = false;
        _detectionRadius = radius;

        // Load trainer data if it exists
        if (GameData.Trainers.ContainsKey(_characterId))
        {
            _trainerData = GameData.Trainers[_characterId];

            // Convert direction strings to direction indices for look around behavior
            _allowedDirections = new List<int>();
            if (_trainerData.Directions != null)
            {
                foreach (var dir in _trainerData.Directions)
                {
                    _allowedDirections.Add(DirectionStringToInt(dir));
                }
            }

            // Find current direction index
            _currentDirectionIndex = _allowedDirections.IndexOf(_direction);
            if (_currentDirectionIndex < 0)
                _currentDirectionIndex = 0;
        }
        else
        {
            _allowedDirections = new List<int> { _direction };
            _currentDirectionIndex = 0;
        }

        _idleRotationTimer = 0;
    }

    public bool CheckPlayerInView(
        Vector2 playerPosition,
        Managers.CollisionManager collisionManager
    )
    {
        // Don't trigger if already triggered or approaching
        if (_hasTriggered || _isApproaching)
            return false;

        // Check if defeated
        if (_trainerData != null && _trainerData.Defeated)
            return false;

        // Calculate distance in pixels
        float distanceX = Math.Abs(playerPosition.X - _position.X);
        float distanceY = Math.Abs(playerPosition.Y - _position.Y);

        // Check if player is within radius
        bool inRange =
            (distanceX <= _detectionRadius && distanceY < Settings.TILE_SIZE) // Horizontal check
            || (distanceY <= _detectionRadius && distanceX < Settings.TILE_SIZE); // Vertical check

        if (!inRange)
            return false;

        // Check if player is in front of NPC based on direction
        bool isInFront = _direction switch
        {
            0 => playerPosition.Y > _position.Y, // Down: player below
            1 => playerPosition.X < _position.X, // Left: player to the left
            2 => playerPosition.X > _position.X, // Right: player to the right
            3 => playerPosition.Y < _position.Y, // Up: player above
            _ => false,
        };

        // Check if player is aligned with NPC direction
        bool isAligned = _direction switch
        {
            0 or 3 => distanceX < Settings.TILE_SIZE, // Vertical: check horizontal alignment
            1 or 2 => distanceY < Settings.TILE_SIZE, // Horizontal: check vertical alignment
            _ => false,
        };

        if (!isInFront || !isAligned)
            return false;

        // Check line of sight - ensure no collision tiles block the view
        if (collisionManager != null)
        {
            bool hasLineOfSight = collisionManager.HasLineOfSight(
                _position,
                playerPosition,
                Settings.TILE_SIZE
            );
            if (!hasLineOfSight)
                return false;
        }

        return true;
    }

    public void StartApproaching(Vector2 playerPosition)
    {
        _isApproaching = true;
        _isMoving = true;

        // Set target position to one tile away from player
        _targetPosition = _direction switch
        {
            0 => new Vector2(playerPosition.X, playerPosition.Y - Settings.TILE_SIZE), // Down
            1 => new Vector2(playerPosition.X + Settings.TILE_SIZE, playerPosition.Y), // Left
            2 => new Vector2(playerPosition.X - Settings.TILE_SIZE, playerPosition.Y), // Right
            3 => new Vector2(playerPosition.X, playerPosition.Y + Settings.TILE_SIZE), // Up
            _ => playerPosition,
        };
    }

    public bool HasReachedPlayer()
    {
        if (!_isApproaching)
            return false;

        float distance = Vector2.Distance(_position, _targetPosition);
        return distance < 10f; // Within 10 pixels
    }

    public void TriggerDialog()
    {
        _hasTriggered = true;
        _isApproaching = false;
        _isMoving = false;
    }

    public List<string> GetDialog()
    {
        if (_trainerData == null)
            return new List<string> { "..." };

        if (_trainerData.Defeated && _trainerData.Dialog.ContainsKey("defeated"))
        {
            return _trainerData.Dialog["defeated"];
        }

        if (_trainerData.Dialog.ContainsKey("default"))
        {
            return _trainerData.Dialog["default"];
        }

        return new List<string> { "..." };
    }

    public void MarkAsDefeated()
    {
        if (_trainerData != null)
        {
            _trainerData.Defeated = true;
        }
    }

    public override void Update(GameTime gameTime)
    {
        // Handle idle rotation when not triggered or approaching
        if (!_hasTriggered && !_isApproaching && _trainerData != null && _trainerData.LookAround)
        {
            _idleRotationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_idleRotationTimer >= IDLE_ROTATION_INTERVAL)
            {
                _idleRotationTimer = 0;

                // Cycle to next direction in allowed directions
                if (_allowedDirections.Count > 1)
                {
                    _currentDirectionIndex =
                        (_currentDirectionIndex + 1) % _allowedDirections.Count;
                    _direction = _allowedDirections[_currentDirectionIndex];
                }
            }
        }

        // Move towards target if approaching
        if (_isApproaching && !HasReachedPlayer())
        {
            Vector2 direction = _targetPosition - _position;
            if (direction.Length() > 0)
            {
                direction.Normalize();
                Vector2 movement =
                    direction * _moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                // Don't overshoot the target
                if (movement.Length() > Vector2.Distance(_position, _targetPosition))
                {
                    _position = _targetPosition;
                }
                else
                {
                    _position += movement;
                }
            }
        }

        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);

        // Draw notice icon above NPC when approaching
        if (_isApproaching && _noticeIcon != null)
        {
            Vector2 iconPosition = new Vector2(_position.X, _position.Y - _frameHeight - 20);

            spriteBatch.Draw(
                _noticeIcon,
                iconPosition,
                null,
                Color.White,
                0f,
                new Vector2(_noticeIcon.Width / 2, _noticeIcon.Height / 2),
                1f,
                SpriteEffects.None,
                (_position.Y - 1) / 10000f // Layer slightly above NPC
            );
        }
    }
}
