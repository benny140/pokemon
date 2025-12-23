using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using pokemon_game.Core;
using pokemon_game.Entities;

namespace pokemon_game.UI;

public class BattleScene
{
    private const int BOARD_SIZE = 7;
    private const int CELL_SIZE = 120; // Larger cells for more screen space

    private Texture2D _pixelTexture;
    private SpriteFont _font;
    private Player _player;
    private NPC _opponent;
    private bool _battleActive;
    private KeyboardState _previousKeyboardState;

    private enum BattlePhase
    {
        Positioning,
        Fighting,
    }

    private BattlePhase _currentPhase;
    private int _selectedMonsterIndex;
    private List<int?> _playerPositions; // Row index for each monster (0-6), null if not placed
    private Dictionary<string, Texture2D> _pokemonTextures;
    private ContentManager _content;

    public bool IsBattleActive => _battleActive;
    public bool IsBattleWon { get; private set; }

    public BattleScene(GraphicsDevice graphicsDevice, SpriteFont font, ContentManager content)
    {
        _font = font;
        _content = content;
        _pokemonTextures = new Dictionary<string, Texture2D>();

        // Create a 1x1 white pixel texture for drawing rectangles
        _pixelTexture = new Texture2D(graphicsDevice, 1, 1);
        _pixelTexture.SetData(new[] { Color.White });

        _previousKeyboardState = Keyboard.GetState();
    }

    public void StartBattle(Player player, NPC opponent)
    {
        _player = player;
        _opponent = opponent;
        _currentPhase = BattlePhase.Positioning;
        _selectedMonsterIndex = 0;
        _playerPositions = new List<int?> { null, null, null }; // Initialize with 3 positions

        // Load pokemon textures
        _pokemonTextures.Clear();
        foreach (var monster in _player.Monsters)
        {
            string textureName = monster.Name.ToLower();
            try
            {
                if (!_pokemonTextures.ContainsKey(textureName))
                {
                    _pokemonTextures[textureName] = _content.Load<Texture2D>(
                        $"graphics/pokemon/{textureName}"
                    );
                }
            }
            catch
            {
                // Texture not found, will skip drawing
            }
        }
        _battleActive = true;
        IsBattleWon = false;
    }

    public void Update(GameTime gameTime)
    {
        if (!_battleActive)
            return;

        var keyboardState = Keyboard.GetState();

        if (_currentPhase == BattlePhase.Positioning)
        {
            // Handle positioning phase input

            // Navigate between monsters
            if (keyboardState.IsKeyDown(Keys.Left) && _previousKeyboardState.IsKeyUp(Keys.Left))
            {
                _selectedMonsterIndex =
                    (_selectedMonsterIndex - 1 + _player.Monsters.Count) % _player.Monsters.Count;
            }
            if (keyboardState.IsKeyDown(Keys.Right) && _previousKeyboardState.IsKeyUp(Keys.Right))
            {
                _selectedMonsterIndex = (_selectedMonsterIndex + 1) % _player.Monsters.Count;
            }

            // Navigate row position for selected monster
            if (keyboardState.IsKeyDown(Keys.Up) && _previousKeyboardState.IsKeyUp(Keys.Up))
            {
                if (_playerPositions[_selectedMonsterIndex] == null)
                    _playerPositions[_selectedMonsterIndex] = 0;
                else
                    _playerPositions[_selectedMonsterIndex] =
                        (_playerPositions[_selectedMonsterIndex].Value - 1 + BOARD_SIZE)
                        % BOARD_SIZE;
            }
            if (keyboardState.IsKeyDown(Keys.Down) && _previousKeyboardState.IsKeyUp(Keys.Down))
            {
                if (_playerPositions[_selectedMonsterIndex] == null)
                    _playerPositions[_selectedMonsterIndex] = 0;
                else
                    _playerPositions[_selectedMonsterIndex] =
                        (_playerPositions[_selectedMonsterIndex].Value + 1) % BOARD_SIZE;
            }

            // Confirm positioning and start battle
            if (keyboardState.IsKeyDown(Keys.Enter) && _previousKeyboardState.IsKeyUp(Keys.Enter))
            {
                // Check if all monsters are positioned
                if (_playerPositions.All(p => p != null))
                {
                    _currentPhase = BattlePhase.Fighting;
                }
            }
        }
        else if (_currentPhase == BattlePhase.Fighting)
        {
            // Check for F9 key press to win battle (placeholder for testing)
            if (keyboardState.IsKeyDown(Keys.F9) && _previousKeyboardState.IsKeyUp(Keys.F9))
            {
                IsBattleWon = true;
                _battleActive = false;
            }
        }

        _previousKeyboardState = keyboardState;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (!_battleActive)
            return;

        // Draw semi-transparent background
        spriteBatch.Draw(
            _pixelTexture,
            new Rectangle(0, 0, Settings.WINDOW_WIDTH, Settings.WINDOW_HEIGHT),
            Color.Black * 0.7f
        );

        int totalBoardSize = BOARD_SIZE * CELL_SIZE;
        int centerX = Settings.WINDOW_WIDTH / 2;
        int centerY = Settings.WINDOW_HEIGHT / 2;

        // Calculate positions to center everything
        int boardX = centerX - totalBoardSize / 2;
        int boardY = centerY - totalBoardSize / 2;
        int characterSpacing = 180; // Distance from board edge to character

        if (_currentPhase == BattlePhase.Positioning)
        {
            DrawPositioningPhase(spriteBatch, boardX, boardY, centerX);
        }
        else
        {
            // Draw player character on the left (facing right = direction 2)
            if (_player != null)
            {
                int playerX = boardX - characterSpacing;
                int playerY = centerY;
                DrawCharacterInfo(
                    spriteBatch,
                    "PLAYER",
                    playerX - 60,
                    playerY - 150,
                    Color.LightBlue
                );
                // Draw player sprite facing right (direction 2)
                _player.DrawStatic(spriteBatch, new Vector2(playerX, playerY), 0.8f, 2);
            }

            // Draw opponent character on the right (facing left = direction 1)
            if (_opponent != null)
            {
                int enemyX = boardX + totalBoardSize + characterSpacing;
                int enemyY = centerY;
                DrawCharacterInfo(
                    spriteBatch,
                    _opponent.CharacterId.ToUpper(),
                    enemyX - 60,
                    enemyY - 150,
                    Color.LightCoral
                );
                // Draw opponent sprite facing left (direction 1)
                _opponent.DrawStatic(spriteBatch, new Vector2(enemyX, enemyY), 0.8f, 1);
            }

            // Draw 7x7 board in the middle
            DrawBoard(spriteBatch, boardX, boardY);

            // Draw positioned Pokemon on the board
            DrawPokemonOnBoard(spriteBatch, boardX, boardY);

            // Draw instructions at the bottom
            string instructions = "Battle in Progress - Press F9 to win (placeholder for testing)";
            Vector2 instructionsSize = _font.MeasureString(instructions);
            spriteBatch.DrawString(
                _font,
                instructions,
                new Vector2(centerX - instructionsSize.X / 2, Settings.WINDOW_HEIGHT - 40),
                Color.Yellow
            );
        }
    }

    private void DrawPositioningPhase(SpriteBatch spriteBatch, int boardX, int boardY, int centerX)
    {
        // Draw title
        string title = "SETUP PHASE: Position Your Pokemon";
        Vector2 titleSize = _font.MeasureString(title);
        spriteBatch.DrawString(
            _font,
            title,
            new Vector2(centerX - titleSize.X / 2, 40),
            Color.Yellow
        );

        // Draw subtitle with clearer instructions
        string subtitle = "Place each Pokemon in a row of Column 1";
        Vector2 subtitleSize = _font.MeasureString(subtitle);
        spriteBatch.DrawString(
            _font,
            subtitle,
            new Vector2(centerX - subtitleSize.X / 2, 70),
            Color.White
        );

        // Draw the board
        DrawBoard(spriteBatch, boardX, boardY);

        // Draw monsters and their positions
        int startY = 150;
        for (int i = 0; i < _player.Monsters.Count; i++)
        {
            var monster = _player.Monsters[i];
            bool isSelected = i == _selectedMonsterIndex;
            Color textColor = isSelected ? Color.Yellow : Color.White;

            int yPos = startY + i * 80;

            // Draw monster name
            string monsterText = $"{i + 1}. {monster.Name} (Lv.{monster.Level})";
            spriteBatch.DrawString(_font, monsterText, new Vector2(50, yPos), textColor);

            // Draw position indicator
            string posText =
                _playerPositions[i] == null ? "Not placed" : $"Row {_playerPositions[i].Value + 1}";
            spriteBatch.DrawString(_font, posText, new Vector2(50, yPos + 25), textColor);

            // Draw Pokemon sprite if available
            string textureName = monster.Name.ToLower();
            if (_pokemonTextures.ContainsKey(textureName))
            {
                spriteBatch.Draw(
                    _pokemonTextures[textureName],
                    new Vector2(250, yPos),
                    null,
                    Color.White,
                    0f,
                    Vector2.Zero,
                    0.5f,
                    SpriteEffects.None,
                    0f
                );
            }

            // Draw on board if positioned
            if (_playerPositions[i] != null)
            {
                int row = _playerPositions[i].Value;
                int cellX = boardX + 2; // Column 0 + border
                int cellY = boardY + row * CELL_SIZE + 2;

                // Highlight the cell
                spriteBatch.Draw(
                    _pixelTexture,
                    new Rectangle(cellX, cellY, CELL_SIZE - 4, CELL_SIZE - 4),
                    Color.LightGreen * 0.5f
                );

                // Draw Pokemon on board
                if (_pokemonTextures.ContainsKey(textureName))
                {
                    var pokemonTexture = _pokemonTextures[textureName];
                    float scale = (CELL_SIZE - 20) / (float)pokemonTexture.Width;
                    if (scale > 1f)
                        scale = 1f;

                    spriteBatch.Draw(
                        pokemonTexture,
                        new Vector2(cellX + CELL_SIZE / 2, cellY + CELL_SIZE / 2),
                        null,
                        Color.White,
                        0f,
                        new Vector2(pokemonTexture.Width / 2, pokemonTexture.Height / 2),
                        scale,
                        SpriteEffects.None,
                        0f
                    );
                }
            }
        }

        // Draw instructions
        string instructions =
            "LEFT/RIGHT: Select Pokemon | UP/DOWN: Choose Row | ENTER: Start Battle";
        Vector2 instrSize = _font.MeasureString(instructions);
        spriteBatch.DrawString(
            _font,
            instructions,
            new Vector2(centerX - instrSize.X / 2, Settings.WINDOW_HEIGHT - 40),
            Color.White
        );

        // Draw additional help text
        string helpText = "Place all your Pokemon in Column 1 before starting the battle";
        Vector2 helpSize = _font.MeasureString(helpText);
        spriteBatch.DrawString(
            _font,
            helpText,
            new Vector2(centerX - helpSize.X / 2, Settings.WINDOW_HEIGHT - 70),
            Color.LightGray
        );

        // Draw positioning status
        int positioned = _playerPositions.Count(p => p != null);
        string status = $"Positioned: {positioned}/{_player.Monsters.Count}";
        Color statusColor = positioned == _player.Monsters.Count ? Color.LightGreen : Color.Cyan;
        spriteBatch.DrawString(
            _font,
            status,
            new Vector2(50, Settings.WINDOW_HEIGHT - 80),
            statusColor
        );
    }

    private void DrawPokemonOnBoard(SpriteBatch spriteBatch, int boardX, int boardY)
    {
        for (int i = 0; i < _player.Monsters.Count; i++)
        {
            if (_playerPositions[i] == null)
                continue;

            var monster = _player.Monsters[i];
            string textureName = monster.Name.ToLower();

            if (!_pokemonTextures.ContainsKey(textureName))
                continue;

            int row = _playerPositions[i].Value;
            int cellX = boardX + 2;
            int cellY = boardY + row * CELL_SIZE + 2;

            var pokemonTexture = _pokemonTextures[textureName];
            float scale = (CELL_SIZE - 20) / (float)pokemonTexture.Width;
            if (scale > 1f)
                scale = 1f;

            spriteBatch.Draw(
                pokemonTexture,
                new Vector2(cellX + CELL_SIZE / 2, cellY + CELL_SIZE / 2),
                null,
                Color.White,
                0f,
                new Vector2(pokemonTexture.Width / 2, pokemonTexture.Height / 2),
                scale,
                SpriteEffects.None,
                0f
            );
        }
    }

    private void DrawCharacterInfo(SpriteBatch spriteBatch, string name, int x, int y, Color color)
    {
        // Draw character name
        spriteBatch.DrawString(_font, name, new Vector2(x, y), color);
    }

    private void DrawBoard(SpriteBatch spriteBatch, int boardX, int boardY)
    {
        int totalBoardSize = BOARD_SIZE * CELL_SIZE;

        // Draw board background
        spriteBatch.Draw(
            _pixelTexture,
            new Rectangle(boardX, boardY, totalBoardSize, totalBoardSize),
            Color.DarkSlateGray * 0.8f
        );

        // Draw grid cells
        for (int row = 0; row < BOARD_SIZE; row++)
        {
            for (int col = 0; col < BOARD_SIZE; col++)
            {
                int x = boardX + col * CELL_SIZE;
                int y = boardY + row * CELL_SIZE;

                // Check if this is the capture zone cell (center-most square)
                bool isCaptureZone = row == 3 && col == 3;

                // Draw cell background
                Color cellColor;
                if (isCaptureZone)
                {
                    // Capture zone color - golden/orange tint
                    cellColor = Color.Gold * 0.4f;
                }
                else
                {
                    // Normal cells - alternating colors for checkerboard pattern
                    cellColor = (row + col) % 2 == 0 ? Color.White * 0.2f : Color.White * 0.1f;
                }

                spriteBatch.Draw(
                    _pixelTexture,
                    new Rectangle(x + 2, y + 2, CELL_SIZE - 4, CELL_SIZE - 4),
                    cellColor
                );

                // Draw cell border with different color for capture zone
                Color borderColor = isCaptureZone ? Color.Gold : Color.Gray;
                DrawRectangleBorder(
                    spriteBatch,
                    new Rectangle(x, y, CELL_SIZE, CELL_SIZE),
                    borderColor,
                    2
                );
            }
        }

        // Draw board title
        string boardTitle = "BATTLE BOARD";
        Vector2 titleSize = _font.MeasureString(boardTitle);
        spriteBatch.DrawString(
            _font,
            boardTitle,
            new Vector2(boardX + totalBoardSize / 2 - titleSize.X / 2, boardY - 40),
            Color.White
        );
    }

    private void DrawRectangleBorder(
        SpriteBatch spriteBatch,
        Rectangle rect,
        Color color,
        int thickness
    )
    {
        // Top
        spriteBatch.Draw(
            _pixelTexture,
            new Rectangle(rect.X, rect.Y, rect.Width, thickness),
            color
        );
        // Bottom
        spriteBatch.Draw(
            _pixelTexture,
            new Rectangle(rect.X, rect.Y + rect.Height - thickness, rect.Width, thickness),
            color
        );
        // Left
        spriteBatch.Draw(
            _pixelTexture,
            new Rectangle(rect.X, rect.Y, thickness, rect.Height),
            color
        );
        // Right
        spriteBatch.Draw(
            _pixelTexture,
            new Rectangle(rect.X + rect.Width - thickness, rect.Y, thickness, rect.Height),
            color
        );
    }

    public void EndBattle()
    {
        _battleActive = false;
        _player = null;
        _opponent = null;
    }
}
