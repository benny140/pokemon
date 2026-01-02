using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using pokemon_game.Core;
using pokemon_game.Data;
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
    private Texture2D _currentBackground;
    private Dictionary<string, Texture2D> _backgroundTextures;
    private KeyboardState _previousKeyboardState;
    private SoundEffect _battleMusic;
    private SoundEffectInstance _battleMusicInstance;

    private enum BattlePhase
    {
        Positioning,
        Fighting,
    }

    private enum BattleAction
    {
        SelectingPokemon,
        SelectingAction,
        SelectingTarget,
    }

    private BattlePhase _currentPhase;
    private int _selectedMonsterIndex;
    private int _selectedRow;
    private List<int?> _playerPositions; // Row index for each monster (0-6), null if not placed
    private Dictionary<string, Texture2D> _pokemonTextures;
    private ContentManager _content;

    // Fighting phase state
    private BattleAction _currentAction;
    private int _fightingSelectedPokemon; // Index of selected pokemon during fighting
    private int _selectedActionOption; // 0 = Move, 1 = Attack

    public bool IsBattleActive => _battleActive;
    public bool IsBattleWon { get; private set; }

    public BattleScene(GraphicsDevice graphicsDevice, SpriteFont font, ContentManager content)
    {
        _font = font;
        _content = content;
        _pokemonTextures = new Dictionary<string, Texture2D>();
        _backgroundTextures = new Dictionary<string, Texture2D>();

        // Create a 1x1 white pixel texture for drawing rectangles
        _pixelTexture = new Texture2D(graphicsDevice, 1, 1);
        _pixelTexture.SetData(new[] { Color.White });

        // Load background textures
        LoadBackgroundTextures();

        // Load battle music
        try
        {
            _battleMusic = content.Load<SoundEffect>("audio/battle");
        }
        catch
        {
            // Battle music not found
        }

        _previousKeyboardState = Keyboard.GetState();
    }

    private void LoadBackgroundTextures()
    {
        string[] biomes = { "forest", "ice", "sand" };
        foreach (var biome in biomes)
        {
            try
            {
                _backgroundTextures[biome] = _content.Load<Texture2D>(
                    $"graphics/backgrounds/{biome}"
                );
            }
            catch
            {
                // Background texture not found
            }
        }
    }

    public void StartBattle(Player player, NPC opponent)
    {
        _player = player;
        _opponent = opponent;

        // Set background based on opponent's biome
        _currentBackground = null;
        if (
            opponent?.TrainerData?.Biome != null
            && _backgroundTextures.ContainsKey(opponent.TrainerData.Biome)
        )
        {
            _currentBackground = _backgroundTextures[opponent.TrainerData.Biome];
        }
        _currentPhase = BattlePhase.Positioning;
        _selectedMonsterIndex = 0;
        _selectedRow = 0;

        // Auto-place Pokemon initially in sequential rows
        _playerPositions = new List<int?>();
        for (int i = 0; i < _player.Monsters.Count && i < BOARD_SIZE; i++)
        {
            _playerPositions.Add(i); // Place in rows 0, 1, 2, etc.
        }

        // Initialize fighting phase state
        _currentAction = BattleAction.SelectingPokemon;
        _fightingSelectedPokemon = -1;
        _selectedActionOption = 0;

        // Load pokemon textures
        _pokemonTextures.Clear();
        foreach (var monsterName in _player.Monsters)
        {
            string textureName = monsterName.ToLower();
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

        // Start battle music (looped)
        if (_battleMusic != null)
        {
            _battleMusicInstance?.Stop();
            _battleMusicInstance = _battleMusic.CreateInstance();
            _battleMusicInstance.IsLooped = true;
            _battleMusicInstance.Play();
        }

        // Initialize keyboard state to prevent immediate transitions
        _previousKeyboardState = Keyboard.GetState();
    }

    public void Update(GameTime gameTime)
    {
        if (!_battleActive)
            return;

        var keyboardState = Keyboard.GetState();

        if (_currentPhase == BattlePhase.Positioning)
        {
            // Navigate between Pokemon with Left/Right
            if (keyboardState.IsKeyDown(Keys.Left) && _previousKeyboardState.IsKeyUp(Keys.Left))
            {
                _selectedMonsterIndex =
                    (_selectedMonsterIndex - 1 + _player.Monsters.Count) % _player.Monsters.Count;
            }
            if (keyboardState.IsKeyDown(Keys.Right) && _previousKeyboardState.IsKeyUp(Keys.Right))
            {
                _selectedMonsterIndex = (_selectedMonsterIndex + 1) % _player.Monsters.Count;
            }

            // Number keys 1-3 for quick Pokemon selection
            if (
                keyboardState.IsKeyDown(Keys.D1)
                && _previousKeyboardState.IsKeyUp(Keys.D1)
                && _player.Monsters.Count >= 1
            )
                _selectedMonsterIndex = 0;
            if (
                keyboardState.IsKeyDown(Keys.D2)
                && _previousKeyboardState.IsKeyUp(Keys.D2)
                && _player.Monsters.Count >= 2
            )
                _selectedMonsterIndex = 1;
            if (
                keyboardState.IsKeyDown(Keys.D3)
                && _previousKeyboardState.IsKeyUp(Keys.D3)
                && _player.Monsters.Count >= 3
            )
                _selectedMonsterIndex = 2;

            // Move selected Pokemon up/down rows
            if (keyboardState.IsKeyDown(Keys.Up) && _previousKeyboardState.IsKeyUp(Keys.Up))
            {
                if (_playerPositions[_selectedMonsterIndex] != null)
                {
                    int newRow =
                        (_playerPositions[_selectedMonsterIndex].Value - 1 + BOARD_SIZE)
                        % BOARD_SIZE;
                    _playerPositions[_selectedMonsterIndex] = newRow;
                }
            }
            if (keyboardState.IsKeyDown(Keys.Down) && _previousKeyboardState.IsKeyUp(Keys.Down))
            {
                if (_playerPositions[_selectedMonsterIndex] != null)
                {
                    int newRow = (_playerPositions[_selectedMonsterIndex].Value + 1) % BOARD_SIZE;
                    _playerPositions[_selectedMonsterIndex] = newRow;
                }
            }

            // Start battle with Enter or Space (only if no collisions)
            if (
                (keyboardState.IsKeyDown(Keys.Enter) && _previousKeyboardState.IsKeyUp(Keys.Enter))
                || (
                    keyboardState.IsKeyDown(Keys.Space)
                    && _previousKeyboardState.IsKeyUp(Keys.Space)
                )
            )
            {
                // Check for collisions before starting
                bool hasCollision = false;
                for (int i = 0; i < _playerPositions.Count; i++)
                {
                    for (int j = i + 1; j < _playerPositions.Count; j++)
                    {
                        if (_playerPositions[i] == _playerPositions[j])
                        {
                            hasCollision = true;
                            break;
                        }
                    }
                    if (hasCollision)
                        break;
                }

                // Only start if no collisions
                if (!hasCollision)
                    _currentPhase = BattlePhase.Fighting;
            }
        }
        else if (_currentPhase == BattlePhase.Fighting)
        {
            if (_currentAction == BattleAction.SelectingPokemon)
            {
                // Initialize selection if not set
                if (_fightingSelectedPokemon == -1 && _player.Monsters.Count > 0)
                {
                    _fightingSelectedPokemon = 0;
                }

                // Navigate with Up/Down arrows through board rows
                if (keyboardState.IsKeyDown(Keys.Up) && _previousKeyboardState.IsKeyUp(Keys.Up))
                {
                    if (
                        _fightingSelectedPokemon >= 0
                        && _fightingSelectedPokemon < _playerPositions.Count
                    )
                    {
                        // Find the current selected row
                        int currentRow = _playerPositions[_fightingSelectedPokemon].Value;

                        // Find the next pokemon above (lower row number)
                        int targetPokemon = -1;
                        for (int row = currentRow - 1; row >= 0; row--)
                        {
                            for (int i = 0; i < _playerPositions.Count; i++)
                            {
                                if (_playerPositions[i] == row)
                                {
                                    targetPokemon = i;
                                    break;
                                }
                            }
                            if (targetPokemon != -1)
                                break;
                        }

                        // If no pokemon above, wrap to bottom
                        if (targetPokemon == -1)
                        {
                            for (int row = BOARD_SIZE - 1; row > currentRow; row--)
                            {
                                for (int i = 0; i < _playerPositions.Count; i++)
                                {
                                    if (_playerPositions[i] == row)
                                    {
                                        targetPokemon = i;
                                        break;
                                    }
                                }
                                if (targetPokemon != -1)
                                    break;
                            }
                        }

                        if (targetPokemon != -1)
                        {
                            _fightingSelectedPokemon = targetPokemon;
                        }
                    }
                }
                if (keyboardState.IsKeyDown(Keys.Down) && _previousKeyboardState.IsKeyUp(Keys.Down))
                {
                    if (
                        _fightingSelectedPokemon >= 0
                        && _fightingSelectedPokemon < _playerPositions.Count
                    )
                    {
                        // Find the current selected row
                        int currentRow = _playerPositions[_fightingSelectedPokemon].Value;

                        // Find the next pokemon below (higher row number)
                        int targetPokemon = -1;
                        for (int row = currentRow + 1; row < BOARD_SIZE; row++)
                        {
                            for (int i = 0; i < _playerPositions.Count; i++)
                            {
                                if (_playerPositions[i] == row)
                                {
                                    targetPokemon = i;
                                    break;
                                }
                            }
                            if (targetPokemon != -1)
                                break;
                        }

                        // If no pokemon below, wrap to top
                        if (targetPokemon == -1)
                        {
                            for (int row = 0; row < currentRow; row++)
                            {
                                for (int i = 0; i < _playerPositions.Count; i++)
                                {
                                    if (_playerPositions[i] == row)
                                    {
                                        targetPokemon = i;
                                        break;
                                    }
                                }
                                if (targetPokemon != -1)
                                    break;
                            }
                        }

                        if (targetPokemon != -1)
                        {
                            _fightingSelectedPokemon = targetPokemon;
                        }
                    }
                }

                // Select pokemon with number keys (based on their row position)
                if (keyboardState.IsKeyDown(Keys.D1) && _previousKeyboardState.IsKeyUp(Keys.D1))
                {
                    // Find pokemon in row 0
                    for (int i = 0; i < _playerPositions.Count; i++)
                    {
                        if (_playerPositions[i] == 0)
                        {
                            _fightingSelectedPokemon = i;
                            break;
                        }
                    }
                }
                if (keyboardState.IsKeyDown(Keys.D2) && _previousKeyboardState.IsKeyUp(Keys.D2))
                {
                    // Find pokemon in row 1
                    for (int i = 0; i < _playerPositions.Count; i++)
                    {
                        if (_playerPositions[i] == 1)
                        {
                            _fightingSelectedPokemon = i;
                            break;
                        }
                    }
                }
                if (keyboardState.IsKeyDown(Keys.D3) && _previousKeyboardState.IsKeyUp(Keys.D3))
                {
                    // Find pokemon in row 2
                    for (int i = 0; i < _playerPositions.Count; i++)
                    {
                        if (_playerPositions[i] == 2)
                        {
                            _fightingSelectedPokemon = i;
                            break;
                        }
                    }
                }

                // Confirm selection with Enter or Space
                if (
                    (
                        keyboardState.IsKeyDown(Keys.Enter)
                        && _previousKeyboardState.IsKeyUp(Keys.Enter)
                    )
                    || (
                        keyboardState.IsKeyDown(Keys.Space)
                        && _previousKeyboardState.IsKeyUp(Keys.Space)
                    )
                )
                {
                    if (_fightingSelectedPokemon >= 0)
                    {
                        _currentAction = BattleAction.SelectingAction;
                        _selectedActionOption = 0;
                    }
                }
            }
            else if (_currentAction == BattleAction.SelectingAction)
            {
                // Navigate between Move and Attack options
                if (keyboardState.IsKeyDown(Keys.Up) && _previousKeyboardState.IsKeyUp(Keys.Up))
                {
                    _selectedActionOption = 0; // Move
                }
                if (keyboardState.IsKeyDown(Keys.Down) && _previousKeyboardState.IsKeyUp(Keys.Down))
                {
                    _selectedActionOption = 1; // Attack
                }
                if (keyboardState.IsKeyDown(Keys.D1) && _previousKeyboardState.IsKeyUp(Keys.D1))
                {
                    _selectedActionOption = 0; // Move
                }
                if (keyboardState.IsKeyDown(Keys.D2) && _previousKeyboardState.IsKeyUp(Keys.D2))
                {
                    _selectedActionOption = 1; // Attack
                }

                // Confirm action selection
                if (
                    (
                        keyboardState.IsKeyDown(Keys.Enter)
                        && _previousKeyboardState.IsKeyUp(Keys.Enter)
                    )
                    || (
                        keyboardState.IsKeyDown(Keys.Space)
                        && _previousKeyboardState.IsKeyUp(Keys.Space)
                    )
                )
                {
                    _currentAction = BattleAction.SelectingTarget;
                }

                // Cancel back to pokemon selection
                if (
                    (
                        keyboardState.IsKeyDown(Keys.Escape)
                        && _previousKeyboardState.IsKeyUp(Keys.Escape)
                    )
                    || (
                        keyboardState.IsKeyDown(Keys.Back)
                        && _previousKeyboardState.IsKeyUp(Keys.Back)
                    )
                )
                {
                    _currentAction = BattleAction.SelectingPokemon;
                }
            }
            else if (_currentAction == BattleAction.SelectingTarget)
            {
                // For now, just allow canceling back to action selection
                if (
                    (
                        keyboardState.IsKeyDown(Keys.Escape)
                        && _previousKeyboardState.IsKeyUp(Keys.Escape)
                    )
                    || (
                        keyboardState.IsKeyDown(Keys.Back)
                        && _previousKeyboardState.IsKeyUp(Keys.Back)
                    )
                )
                {
                    _currentAction = BattleAction.SelectingAction;
                }
            }

            // Check for F9 key press to win battle (placeholder for testing)
            if (keyboardState.IsKeyDown(Keys.F9) && _previousKeyboardState.IsKeyUp(Keys.F9))
            {
                IsBattleWon = true;
                _battleActive = false;
                _battleMusicInstance?.Stop();
            }
        }

        _previousKeyboardState = keyboardState;
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        if (!_battleActive)
            return;

        // Draw biome background or fallback to semi-transparent overlay
        if (_currentBackground != null)
        {
            spriteBatch.Draw(
                _currentBackground,
                new Rectangle(0, 0, Settings.WINDOW_WIDTH, Settings.WINDOW_HEIGHT),
                Color.White
            );
        }
        else
        {
            spriteBatch.Draw(
                _pixelTexture,
                new Rectangle(0, 0, Settings.WINDOW_WIDTH, Settings.WINDOW_HEIGHT),
                Color.Black * 0.7f
            );
        }

        int totalBoardSize = BOARD_SIZE * CELL_SIZE;
        int centerX = Settings.WINDOW_WIDTH / 2;
        int centerY = Settings.WINDOW_HEIGHT / 2;

        // Calculate positions to center everything
        int boardX = centerX - totalBoardSize / 2;
        int boardY = centerY - totalBoardSize / 2;
        int characterSpacing = 180; // Distance from board edge to character

        if (_currentPhase == BattlePhase.Positioning)
        {
            DrawPositioningPhase(spriteBatch, boardX, boardY, centerX, gameTime);
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

            // Draw fighting phase UI (pokemon selection, action menu, ranges)
            DrawFightingPhaseUI(spriteBatch, boardX, boardY, centerX);

            // Draw instructions at the bottom
            string instructions = GetFightingPhaseInstructions();
            Vector2 instructionsSize = _font.MeasureString(instructions);
            DrawTextWithBackground(
                spriteBatch,
                instructions,
                new Vector2(centerX - instructionsSize.X / 2, Settings.WINDOW_HEIGHT - 40),
                Color.Yellow
            );
        }
    }

    private string GetFightingPhaseInstructions()
    {
        switch (_currentAction)
        {
            case BattleAction.SelectingPokemon:
                return "UP/DOWN: Navigate Board | 1-3: Jump to Row | ENTER/SPACE: Confirm | F9: Win (test)";
            case BattleAction.SelectingAction:
                return "UP/DOWN or 1-2: Select Action | ENTER/SPACE: Confirm | BACKSPACE: Back";
            case BattleAction.SelectingTarget:
                return "Select Target (Coming Soon) | BACKSPACE: Back";
            default:
                return "Battle in Progress";
        }
    }

    private void DrawFightingPhaseUI(SpriteBatch spriteBatch, int boardX, int boardY, int centerX)
    {
        // Draw pokemon selection panel on the left
        DrawPokemonSelectionPanel(spriteBatch);

        // Draw action menu when a pokemon is selected
        if (_currentAction == BattleAction.SelectingAction)
        {
            DrawActionMenu(spriteBatch, centerX);
        }

        // Draw range indicators on board
        if (_fightingSelectedPokemon >= 0)
        {
            DrawRangeIndicators(spriteBatch, boardX, boardY);
        }
    }

    private void DrawPokemonSelectionPanel(SpriteBatch spriteBatch)
    {
        int panelX = 20;
        int panelY = 150;
        int panelWidth = 300;

        // Draw panel background
        spriteBatch.Draw(
            _pixelTexture,
            new Rectangle(panelX, panelY, panelWidth, _player.Monsters.Count * 80 + 40),
            Color.Black * 0.8f
        );

        // Draw title
        DrawTextWithBackground(
            spriteBatch,
            "YOUR POKEMON",
            new Vector2(panelX + 10, panelY + 10),
            Color.Cyan
        );

        // Draw each pokemon
        for (int i = 0; i < _player.Monsters.Count; i++)
        {
            var monsterName = _player.Monsters[i];
            bool isSelected = i == _fightingSelectedPokemon;
            int yPos = panelY + 50 + i * 80;

            // Highlight selected pokemon
            if (isSelected)
            {
                spriteBatch.Draw(
                    _pixelTexture,
                    new Rectangle(panelX + 5, yPos - 5, panelWidth - 10, 70),
                    Color.Yellow * 0.3f
                );
            }

            // Draw pokemon info
            string indicator = isSelected ? ">>>" : "   ";
            string info = $"{indicator} {i + 1}. {monsterName}";
            Color textColor = isSelected ? Color.Yellow : Color.White;

            DrawTextWithBackground(spriteBatch, info, new Vector2(panelX + 10, yPos), textColor);

            // Draw position
            if (_playerPositions[i] != null)
            {
                string posInfo = $"     Row {_playerPositions[i].Value + 1}";
                DrawTextWithBackground(
                    spriteBatch,
                    posInfo,
                    new Vector2(panelX + 10, yPos + 25),
                    Color.LightGray
                );
            }
        }
    }

    private void DrawActionMenu(SpriteBatch spriteBatch, int centerX)
    {
        int menuWidth = 300;
        int menuHeight = 200;
        int menuX = centerX - menuWidth / 2;
        int menuY = 100;

        // Draw menu background
        spriteBatch.Draw(
            _pixelTexture,
            new Rectangle(menuX, menuY, menuWidth, menuHeight),
            Color.Black * 0.9f
        );

        // Draw border
        DrawRectangleBorder(
            spriteBatch,
            new Rectangle(menuX, menuY, menuWidth, menuHeight),
            Color.Gold,
            3
        );

        // Draw title
        var monsterName = _player.Monsters[_fightingSelectedPokemon];
        string title = $"Action for {monsterName}";
        Vector2 titleSize = _font.MeasureString(title);
        DrawTextWithBackground(
            spriteBatch,
            title,
            new Vector2(menuX + menuWidth / 2 - titleSize.X / 2, menuY + 20),
            Color.Gold
        );

        // Get pokemon stats
        var monsterData = GameData.Monsters[monsterName];
        int moveRange = monsterData.Stats.Movement;
        int attackRange = monsterData.Stats.Range;

        // Draw Move option
        bool moveSelected = _selectedActionOption == 0;
        Color moveColor = moveSelected ? Color.Yellow : Color.White;
        string moveText = moveSelected ? ">>> 1. MOVE" : "    1. MOVE";
        DrawTextWithBackground(
            spriteBatch,
            moveText,
            new Vector2(menuX + 30, menuY + 70),
            moveColor
        );
        DrawTextWithBackground(
            spriteBatch,
            $"    Range: {moveRange} cells",
            new Vector2(menuX + 30, menuY + 95),
            Color.LightGray
        );

        // Draw Attack option
        bool attackSelected = _selectedActionOption == 1;
        Color attackColor = attackSelected ? Color.Yellow : Color.White;
        string attackText = attackSelected ? ">>> 2. ATTACK" : "    2. ATTACK";
        DrawTextWithBackground(
            spriteBatch,
            attackText,
            new Vector2(menuX + 30, menuY + 130),
            attackColor
        );
        DrawTextWithBackground(
            spriteBatch,
            $"    Range: {attackRange} cells",
            new Vector2(menuX + 30, menuY + 155),
            Color.LightGray
        );
    }

    private void DrawRangeIndicators(SpriteBatch spriteBatch, int boardX, int boardY)
    {
        if (_playerPositions[_fightingSelectedPokemon] == null)
            return;

        int pokemonRow = _playerPositions[_fightingSelectedPokemon].Value;
        int pokemonCol = 0; // Player pokemon are always in column 0

        // Get pokemon stats
        var monsterName = _player.Monsters[_fightingSelectedPokemon];
        var monsterData = GameData.Monsters[monsterName];
        int moveRange = monsterData.Stats.Movement;
        int attackRange = monsterData.Stats.Range;

        // Determine which range to show
        int range = 0;
        Color rangeColor = Color.White;

        if (_currentAction == BattleAction.SelectingAction)
        {
            // Show the currently selected action's range
            if (_selectedActionOption == 0)
            {
                range = moveRange;
                rangeColor = Color.Blue * 0.4f; // Blue for movement
            }
            else
            {
                range = attackRange;
                rangeColor = Color.Red * 0.4f; // Red for attack
            }
        }
        else if (_currentAction == BattleAction.SelectingTarget)
        {
            // Show range based on confirmed action
            if (_selectedActionOption == 0)
            {
                range = moveRange;
                rangeColor = Color.Blue * 0.4f;
            }
            else
            {
                range = attackRange;
                rangeColor = Color.Red * 0.4f;
            }
        }

        // Draw range indicators
        if (range > 0)
        {
            for (int row = 0; row < BOARD_SIZE; row++)
            {
                for (int col = 0; col < BOARD_SIZE; col++)
                {
                    // Calculate Manhattan distance
                    int distance = Math.Abs(row - pokemonRow) + Math.Abs(col - pokemonCol);

                    if (distance > 0 && distance <= range)
                    {
                        int x = boardX + col * CELL_SIZE;
                        int y = boardY + row * CELL_SIZE;

                        // Draw range highlight
                        spriteBatch.Draw(
                            _pixelTexture,
                            new Rectangle(x + 2, y + 2, CELL_SIZE - 4, CELL_SIZE - 4),
                            rangeColor
                        );
                    }
                }
            }
        }
    }

    private void DrawPositioningPhase(
        SpriteBatch spriteBatch,
        int boardX,
        int boardY,
        int centerX,
        GameTime gameTime
    )
    {
        // Draw title
        string title = "SETUP PHASE: Position Your Pokemon";
        Vector2 titleSize = _font.MeasureString(title);
        DrawTextWithBackground(
            spriteBatch,
            title,
            new Vector2(centerX - titleSize.X / 2, 40),
            Color.Yellow
        );

        // Draw instructions
        string subtitle = "Select a Pokemon and use UP/DOWN to reposition";
        Color subtitleColor = Color.Cyan;

        Vector2 subtitleSize = _font.MeasureString(subtitle);
        DrawTextWithBackground(
            spriteBatch,
            subtitle,
            new Vector2(centerX - subtitleSize.X / 2, 70),
            subtitleColor
        );

        // Draw the board
        DrawBoard(spriteBatch, boardX, boardY);

        // Draw monsters and their positions
        int startY = 150;
        for (int i = 0; i < _player.Monsters.Count; i++)
        {
            var monsterName = _player.Monsters[i];
            bool isSelected = i == _selectedMonsterIndex;
            bool isPlaced = _playerPositions[i] != null;

            Color textColor = isSelected ? Color.Yellow : Color.White;

            int yPos = startY + i * 80;

            // Draw monster name with visual indicator
            string indicator = isSelected ? ">>>" : "   ";

            string monsterText = $"{indicator} {i + 1}. {monsterName}";
            DrawTextWithBackground(spriteBatch, monsterText, new Vector2(50, yPos), textColor);

            // Draw position indicator
            string posText = $"Row {_playerPositions[i].Value + 1}";
            DrawTextWithBackground(spriteBatch, posText, new Vector2(50, yPos + 25), textColor);

            // Draw Pokemon sprite if available
            string textureName = monsterName.ToLower();
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

            // Draw Pokemon on board
            if (_playerPositions[i] != null)
            {
                int row = _playerPositions[i].Value;
                int cellX = boardX + 2; // Column 0 + border
                int cellY = boardY + row * CELL_SIZE + 2;

                // Check if another Pokemon is in the same row (collision)
                bool hasCollision = false;
                for (int j = 0; j < _playerPositions.Count; j++)
                {
                    if (j != i && _playerPositions[j] == row)
                    {
                        hasCollision = true;
                        break;
                    }
                }

                // Highlight the cell - brighter if selected, red if collision
                Color highlightColor;
                if (hasCollision)
                    highlightColor = Color.Red * 0.5f; // Red for collision
                else if (isSelected)
                    highlightColor = Color.Yellow * 0.6f; // Yellow for selected
                else
                    highlightColor = Color.LightGreen * 0.4f; // Green for normal

                spriteBatch.Draw(
                    _pixelTexture,
                    new Rectangle(cellX, cellY, CELL_SIZE - 4, CELL_SIZE - 4),
                    highlightColor
                );

                // Draw Pokemon on board with shake effect if collision
                if (_pokemonTextures.ContainsKey(textureName))
                {
                    var pokemonTexture = _pokemonTextures[textureName];
                    float scale = (CELL_SIZE - 20) / (float)pokemonTexture.Width;
                    if (scale > 1f)
                        scale = 1f;

                    // Add shake offset if there's a collision
                    int shakeOffsetX = 0;
                    int shakeOffsetY = 0;
                    if (hasCollision)
                    {
                        // Simple shake based on which Pokemon index is higher
                        int shakeAmount = 5;
                        shakeOffsetX = i % 2 == 0 ? shakeAmount : -shakeAmount;
                        shakeOffsetY = (int)(
                            Math.Sin(gameTime.TotalGameTime.TotalSeconds * 10) * shakeAmount
                        );
                    }

                    spriteBatch.Draw(
                        pokemonTexture,
                        new Vector2(
                            cellX + CELL_SIZE / 2 + shakeOffsetX,
                            cellY + CELL_SIZE / 2 + shakeOffsetY
                        ),
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
            "LEFT/RIGHT or 1-3: Select Pokemon | UP/DOWN: Move Row | ENTER/SPACE: Start Battle";

        // Check for collisions
        bool hasAnyCollision = false;
        for (int i = 0; i < _playerPositions.Count; i++)
        {
            for (int j = i + 1; j < _playerPositions.Count; j++)
            {
                if (_playerPositions[i] == _playerPositions[j])
                {
                    hasAnyCollision = true;
                    break;
                }
            }
            if (hasAnyCollision)
                break;
        }

        if (hasAnyCollision)
            instructions = "WARNING: Pokemon overlapping! Reposition before starting battle!";

        Vector2 instrSize = _font.MeasureString(instructions);
        Color instrColor = hasAnyCollision ? Color.Red : Color.LightGreen;

        DrawTextWithBackground(
            spriteBatch,
            instructions,
            new Vector2(centerX - instrSize.X / 2, Settings.WINDOW_HEIGHT - 40),
            instrColor
        );
    }

    private void DrawPokemonOnBoard(SpriteBatch spriteBatch, int boardX, int boardY)
    {
        for (int i = 0; i < _player.Monsters.Count; i++)
        {
            if (_playerPositions[i] == null)
                continue;

            var monsterName = _player.Monsters[i];
            string textureName = monsterName.ToLower();

            if (!_pokemonTextures.ContainsKey(textureName))
                continue;

            int row = _playerPositions[i].Value;
            int cellX = boardX + 2;
            int cellY = boardY + row * CELL_SIZE + 2;

            // Check if this pokemon is selected during fighting phase
            bool isSelectedInFighting =
                _currentPhase == BattlePhase.Fighting && i == _fightingSelectedPokemon;

            // Draw highlight if selected during fighting phase
            if (isSelectedInFighting)
            {
                // Draw a bright highlight around the selected pokemon's cell
                spriteBatch.Draw(
                    _pixelTexture,
                    new Rectangle(cellX, cellY, CELL_SIZE - 4, CELL_SIZE - 4),
                    Color.Yellow * 0.5f
                );
            }

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

    private void DrawTextWithBackground(
        SpriteBatch spriteBatch,
        string text,
        Vector2 position,
        Color textColor
    )
    {
        Vector2 size = _font.MeasureString(text);
        int padding = 10;

        // Draw semi-transparent background
        spriteBatch.Draw(
            _pixelTexture,
            new Rectangle(
                (int)position.X - padding,
                (int)position.Y - padding,
                (int)size.X + padding * 2,
                (int)size.Y + padding * 2
            ),
            Color.Black * 0.7f
        );

        // Draw text
        spriteBatch.DrawString(_font, text, position, textColor);
    }

    private void DrawCharacterInfo(SpriteBatch spriteBatch, string name, int x, int y, Color color)
    {
        // Draw character name with background
        DrawTextWithBackground(spriteBatch, name, new Vector2(x, y), color);
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
        _battleMusicInstance?.Stop();
        _player = null;
        _opponent = null;
    }
}
