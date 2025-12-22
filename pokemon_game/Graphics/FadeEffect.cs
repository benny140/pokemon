using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using pokemon_game.Core;

namespace pokemon_game.Graphics;

public class FadeEffect
{
    private Texture2D _fadeTexture;
    private float _fadeAlpha;
    private bool _isFading;
    private bool _isFadingOut;
    private float _fadeSpeed = 2f;
    private bool _fadeComplete;

    public bool IsFading => _isFading;
    public bool IsFadeComplete => _fadeComplete;

    public FadeEffect(GraphicsDevice graphicsDevice)
    {
        _fadeTexture = new Texture2D(graphicsDevice, 1, 1);
        _fadeTexture.SetData(new[] { Color.White });
        _fadeAlpha = 0f;
        _isFading = false;
        _fadeComplete = false;
    }

    public void StartFadeOut()
    {
        _isFading = true;
        _isFadingOut = true;
        _fadeAlpha = 0f;
        _fadeComplete = false;
    }

    public void StartFadeIn()
    {
        _isFading = true;
        _isFadingOut = false;
        _fadeAlpha = 1f;
        _fadeComplete = false;
    }

    public void Update(GameTime gameTime)
    {
        if (!_isFading)
            return;

        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_isFadingOut)
        {
            _fadeAlpha += _fadeSpeed * deltaTime;
            if (_fadeAlpha >= 1f)
            {
                _fadeAlpha = 1f;
                _fadeComplete = true;
            }
        }
        else
        {
            _fadeAlpha -= _fadeSpeed * deltaTime;
            if (_fadeAlpha <= 0f)
            {
                _fadeAlpha = 0f;
                _isFading = false;
                _fadeComplete = true;
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (_fadeAlpha > 0f)
        {
            spriteBatch.Draw(
                _fadeTexture,
                new Rectangle(0, 0, Settings.WINDOW_WIDTH, Settings.WINDOW_HEIGHT),
                Color.Black * _fadeAlpha
            );
        }
    }
}
