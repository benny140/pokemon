using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using pokemon_game.Core;

namespace pokemon_game.UI;

public class DialogBox
{
    private SpriteFont _font;
    private Texture2D _boxTexture;
    private const int PADDING = 15;
    private const int BOX_WIDTH = 400;
    private const int BOX_HEIGHT = 80;
    private const float CHARS_PER_SECOND = 30f;

    private string _fullText;
    private float _textTimer;
    private int _visibleChars;

    public DialogBox(GraphicsDevice graphicsDevice, SpriteFont font)
    {
        _font = font;

        // Create a simple white rectangle texture for the dialog box
        _boxTexture = new Texture2D(graphicsDevice, 1, 1);
        _boxTexture.SetData(new[] { Color.White });

        _fullText = "";
        _textTimer = 0;
        _visibleChars = 0;
    }

    public void SetText(string text)
    {
        _fullText = text ?? "";
        _textTimer = 0;
        _visibleChars = 0;
    }

    public void Update(GameTime gameTime)
    {
        if (_visibleChars < _fullText.Length)
        {
            _textTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            _visibleChars = (int)(_textTimer * CHARS_PER_SECOND);

            if (_visibleChars > _fullText.Length)
            {
                _visibleChars = _fullText.Length;
            }
        }
    }

    public bool IsTextComplete()
    {
        return _visibleChars >= _fullText.Length;
    }

    public void CompleteText()
    {
        _visibleChars = _fullText.Length;
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 characterPosition, Matrix cameraTransform)
    {
        if (string.IsNullOrEmpty(_fullText))
            return;

        // Transform character position to screen space
        Vector3 screenPos = Vector3.Transform(new Vector3(characterPosition, 0), cameraTransform);

        // Position box above character
        int boxX = (int)screenPos.X - BOX_WIDTH / 2;
        int boxY = (int)screenPos.Y - 150; // Above character

        // Clamp to screen bounds
        boxX = MathHelper.Clamp(boxX, 10, Settings.WINDOW_WIDTH - BOX_WIDTH - 10);
        boxY = MathHelper.Clamp(boxY, 10, Settings.WINDOW_HEIGHT - BOX_HEIGHT - 10);

        Rectangle boxRect = new Rectangle(boxX, boxY, BOX_WIDTH, BOX_HEIGHT);

        // Draw background box with border (light background)
        spriteBatch.Draw(_boxTexture, boxRect, Settings.Colors.Dark);
        spriteBatch.Draw(
            _boxTexture,
            new Rectangle(boxRect.X + 3, boxRect.Y + 3, boxRect.Width - 6, boxRect.Height - 6),
            Settings.Colors.White
        );

        // Draw text (only visible characters)
        string visibleText = _fullText.Substring(0, _visibleChars);
        Vector2 textPosition = new Vector2(boxRect.X + PADDING, boxRect.Y + PADDING);
        spriteBatch.DrawString(_font, visibleText, textPosition, Settings.Colors.Dark);

        // Draw continue indicator only when text is complete
        if (IsTextComplete())
        {
            string continueText = "SPACE";
            Vector2 continueSize = _font.MeasureString(continueText);
            Vector2 continuePosition = new Vector2(
                boxRect.X + boxRect.Width - continueSize.X - PADDING,
                boxRect.Y + boxRect.Height - continueSize.Y - PADDING
            );
            spriteBatch.DrawString(_font, continueText, continuePosition, Settings.Colors.Light);
        }
    }
}
