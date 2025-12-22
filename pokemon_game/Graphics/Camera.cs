using Microsoft.Xna.Framework;
using pokemon_game.Core;

namespace pokemon_game.Graphics;

public class Camera
{
    private Vector2 _position;
    private readonly float _defaultZoom;
    private float _currentZoom;
    private float _targetZoom;
    private readonly int _viewportWidth;
    private readonly int _viewportHeight;
    private readonly float _zoomSpeed = 0.8f; // Speed of zoom transition

    // Dead zone settings - area in the center where player can move without camera following
    private readonly float _deadZoneWidth;
    private readonly float _deadZoneHeight;

    public Vector2 Position => _position;
    public Matrix Transform { get; private set; }

    public Camera(
        int viewportWidth,
        int viewportHeight,
        float zoom = 1f,
        float deadZoneWidth = 300f,
        float deadZoneHeight = 200f
    )
    {
        _viewportWidth = viewportWidth;
        _viewportHeight = viewportHeight;
        _defaultZoom = zoom;
        _currentZoom = zoom;
        _targetZoom = zoom;
        _deadZoneWidth = deadZoneWidth;
        _deadZoneHeight = deadZoneHeight;
        _position = Vector2.Zero;
        UpdateTransform();
    }

    public void Follow(Vector2 targetPosition)
    {
        // Calculate the camera center in world space
        var cameraCenterX = _position.X + (_viewportWidth / _currentZoom) / 2f;
        var cameraCenterY = _position.Y + (_viewportHeight / _currentZoom) / 2f;

        // Calculate the distance from camera center to target
        var deltaX = targetPosition.X - cameraCenterX;
        var deltaY = targetPosition.Y - cameraCenterY;

        // Define the dead zone bounds (half-widths from center)
        var deadZoneHalfWidth = _deadZoneWidth / 2f;
        var deadZoneHalfHeight = _deadZoneHeight / 2f;

        // Only move camera if player is outside the dead zone
        if (deltaX > deadZoneHalfWidth)
        {
            _position.X += deltaX - deadZoneHalfWidth;
        }
        else if (deltaX < -deadZoneHalfWidth)
        {
            _position.X += deltaX + deadZoneHalfWidth;
        }

        if (deltaY > deadZoneHalfHeight)
        {
            _position.Y += deltaY - deadZoneHalfHeight;
        }
        else if (deltaY < -deadZoneHalfHeight)
        {
            _position.Y += deltaY + deadZoneHalfHeight;
        }

        UpdateTransform();
    }

    private void UpdateTransform()
    {
        Transform =
            Matrix.CreateTranslation(new Vector3(-_position, 0)) * Matrix.CreateScale(_currentZoom);
    }

    public void Update(GameTime gameTime)
    {
        // Smoothly interpolate current zoom towards target zoom
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _currentZoom = MathHelper.Lerp(_currentZoom, _targetZoom, _zoomSpeed * deltaTime);

        // Update transform after zoom changes
        UpdateTransform();
    }

    public void SetTargetZoom(float zoom)
    {
        _targetZoom = zoom;
    }

    public void ResetZoom()
    {
        _targetZoom = _defaultZoom;
    }
}
