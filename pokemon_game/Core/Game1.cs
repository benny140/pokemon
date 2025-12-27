using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using pokemon_game.Entities;
using pokemon_game.Graphics;
using pokemon_game.Managers;
using pokemon_game.UI;

namespace pokemon_game.Core;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private TiledMap _tiledMap;
    private TiledMapRenderer _tiledMapRenderer;
    private Camera _camera;
    private ObjectManager _objectManager;
    private Player _player;
    private WaterAnimationManager _waterAnimationManager;
    private CoastAnimationManager _coastAnimationManager;
    private MonsterManager _monsterManager;
    private CollisionManager _collisionManager;
    private CharacterManager _characterManager;
    private TransitionManager _transitionManager;
    private FadeEffect _fadeEffect;
    private DialogBox _dialogBox;
    private BattleScene _battleScene;
    private SpriteFont _font;
    private int _mapWidth;
    private int _mapHeight;
    private KeyboardState _previousKeyboardState;
    private string _previousDialogLine;
    private string _currentMapName;
    private string _previousMapName;
    private string _pendingMapTransition;
    private SoundEffect _overworldMusic;
    private SoundEffectInstance _overworldMusicInstance;

    private enum TransitionState
    {
        None,
        FadingOut,
        Loading,
        FadingIn,
    }

    private enum GameState
    {
        Exploration,
        Battle,
    }

    private TransitionState _transitionState = TransitionState.None;
    private GameState _gameState = GameState.Exploration;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = Settings.WINDOW_WIDTH;
        _graphics.PreferredBackBufferHeight = Settings.WINDOW_HEIGHT;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // Create camera with dead zone in the middle
        _camera = new Camera(
            Settings.WINDOW_WIDTH,
            Settings.WINDOW_HEIGHT,
            Settings.ZOOM_SCALE,
            deadZoneWidth: 400f, // Horizontal dead zone
            deadZoneHeight: 300f // Vertical dead zone
        );

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Load font
        _font = Content.Load<SpriteFont>("Consolas");
        _dialogBox = new DialogBox(GraphicsDevice, _font);
        _battleScene = new BattleScene(GraphicsDevice, _font, Content);
        _fadeEffect = new FadeEffect(GraphicsDevice);
        _transitionManager = new TransitionManager();

        // Start with world map, assume we came from house
        _previousMapName = "house";
        _currentMapName = "world";
        LoadMap(_currentMapName);

        // Load notice icon and sound for NPCs
        var noticeIcon = Content.Load<Texture2D>("graphics/ui/notice");
        NPC.SetNoticeIcon(noticeIcon);

        var noticeSound = Content.Load<SoundEffect>("audio/notice");
        CharacterManager.SetNoticeSound(noticeSound);

        // Load and play overworld music
        try
        {
            _overworldMusic = Content.Load<SoundEffect>("audio/overworld");
            _overworldMusicInstance = _overworldMusic.CreateInstance();
            _overworldMusicInstance.IsLooped = true;
            _overworldMusicInstance.Play();
            CharacterManager.SetOverworldMusic(_overworldMusicInstance);
        }
        catch
        {
            // Overworld music not found
        }
    }

    private void LoadMap(string mapName)
    {
        // Load the Tiled map
        _tiledMap = Content.Load<TiledMap>($"data/maps/{mapName}");
        _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);

        // Store map dimensions for zoom calculations
        _mapWidth = _tiledMap.WidthInPixels;
        _mapHeight = _tiledMap.HeightInPixels;

        // Load collisions
        _collisionManager = new CollisionManager();
        _collisionManager.LoadCollisions(_tiledMap);

        // Load transitions
        _transitionManager.LoadTransitions(_tiledMap);

        // Load objects from the Objects layer
        _objectManager = new ObjectManager();
        _objectManager.LoadObjects(_tiledMap);

        // Find spawn point for player based on previous map
        Vector2 spawnPosition = FindSpawnPosition(_tiledMap, _previousMapName);

        // Create or update player
        if (_player == null)
        {
            var playerTexture = Content.Load<Texture2D>("graphics/characters/player");
            _player = new Player(playerTexture, spawnPosition, _collisionManager);
        }
        else
        {
            // Update existing player position and collision manager
            typeof(Player)
                .GetField(
                    "_position",
                    System.Reflection.BindingFlags.NonPublic
                        | System.Reflection.BindingFlags.Instance
                )
                ?.SetValue(_player, spawnPosition);
            typeof(Player)
                .GetField(
                    "_collisionManager",
                    System.Reflection.BindingFlags.NonPublic
                        | System.Reflection.BindingFlags.Instance
                )
                ?.SetValue(_player, _collisionManager);
        }

        // Load water animation
        _waterAnimationManager = new WaterAnimationManager();
        _waterAnimationManager.LoadContent(GraphicsDevice, _tiledMap);
        _waterAnimationManager.LoadTextures(
            Content.Load<Texture2D>("graphics/tilesets/water/0"),
            Content.Load<Texture2D>("graphics/tilesets/water/1"),
            Content.Load<Texture2D>("graphics/tilesets/water/2"),
            Content.Load<Texture2D>("graphics/tilesets/water/3")
        );

        // Load coast animation
        _coastAnimationManager = new CoastAnimationManager();
        _coastAnimationManager.LoadContent(_tiledMap);
        _coastAnimationManager.LoadTexture(Content.Load<Texture2D>("graphics/tilesets/coast"));

        // Load monsters and grass
        _monsterManager = new MonsterManager();
        _monsterManager.LoadTextures(
            Content.Load<Texture2D>("graphics/objects/grass"),
            Content.Load<Texture2D>("graphics/objects/grass_ice")
        );
        _monsterManager.LoadMonsters(_tiledMap);

        // Load NPCs from Entities layer
        _characterManager = new CharacterManager();
        _characterManager.LoadCharacters(_tiledMap, Content, _collisionManager);

        _currentMapName = mapName;
    }

    private Vector2 FindSpawnPosition(TiledMap tiledMap, string fromMap)
    {
        var entitiesLayer = tiledMap.GetLayer<TiledMapObjectLayer>("Entities");
        if (entitiesLayer != null)
        {
            foreach (var obj in entitiesLayer.Objects)
            {
                if (obj.Name == "Player")
                {
                    // Check if this spawn point matches where we came from
                    if (obj.Properties.ContainsKey("pos"))
                    {
                        string pos = obj.Properties["pos"].ToString();
                        if (pos == fromMap)
                        {
                            return new Vector2(obj.Position.X, obj.Position.Y);
                        }
                    }
                }
            }
        }

        // Default to center of map if no spawn point found
        return new Vector2(_mapWidth / 2, _mapHeight / 2);
    }

    protected override void Update(GameTime gameTime)
    {
        if (
            GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
            || Keyboard.GetState().IsKeyDown(Keys.Escape)
        )
            Exit();

        // Handle M button for map zoom
        var keyboardState = Keyboard.GetState();
        if (keyboardState.IsKeyDown(Keys.M))
        {
            // Calculate zoom level to show entire map
            float zoomX = (float)Settings.WINDOW_WIDTH / _mapWidth;
            float zoomY = (float)Settings.WINDOW_HEIGHT / _mapHeight;
            float mapZoom = MathHelper.Min(zoomX, zoomY);
            _camera.SetTargetZoom(mapZoom);
        }
        else
        {
            _camera.ResetZoom();
        }

        // Update fade effect
        _fadeEffect.Update(gameTime);

        // Handle transition states
        if (_transitionState == TransitionState.FadingOut && _fadeEffect.IsFadeComplete)
        {
            // Fade out complete, load new map
            _transitionState = TransitionState.Loading;
            _previousMapName = _currentMapName; // Store where we're coming from
            LoadMap(_pendingMapTransition);
            _transitionState = TransitionState.FadingIn;
            _fadeEffect.StartFadeIn();
        }
        else if (_transitionState == TransitionState.FadingIn && _fadeEffect.IsFadeComplete)
        {
            // Fade in complete, transition done
            _transitionState = TransitionState.None;
        }

        // Only update game logic when not transitioning
        if (_transitionState == TransitionState.None)
        {
            // Check for map transitions
            string targetMap = _transitionManager.CheckTransition(_player.GetBounds());
            if (!string.IsNullOrEmpty(targetMap))
            {
                _pendingMapTransition = targetMap;
                _transitionState = TransitionState.FadingOut;
                _fadeEffect.StartFadeOut();
            }
        }

        // Update game when not loading
        if (_transitionState != TransitionState.Loading)
        {
            if (_gameState == GameState.Battle)
            {
                // Update battle scene
                _battleScene.Update(gameTime);

                // Check if battle is won
                if (_battleScene.IsBattleWon)
                {
                    _battleScene.EndBattle();
                    _characterManager.EndBattle();
                    _gameState = GameState.Exploration;
                    // Resume overworld music after battle
                    _overworldMusicInstance?.Resume();
                }
            }
            else if (_gameState == GameState.Exploration)
            {
                // Update character manager first to check for interactions
                _characterManager.Update(gameTime, _player.Position);

                // Check if battle should start
                if (_characterManager.ShouldStartBattle())
                {
                    _gameState = GameState.Battle;
                    // Pause overworld music during battle
                    _overworldMusicInstance?.Pause();
                    _battleScene.StartBattle(_player, _characterManager.ActiveNPC);
                }
                // Check if dialog finished without battle (like Nurse)
                else if (
                    _characterManager.ActiveNPC != null
                    && !_characterManager.HasActiveDialog()
                    && _characterManager.ActiveNPC.TrainerData != null
                    && !_characterManager.ActiveNPC.TrainerData.CanBattle
                )
                {
                    _characterManager.EndDialog();
                }

                // Update player with blocking state
                _player.Update(gameTime, _characterManager.IsPlayerBlocked);

                // Update dialog box text streaming
                if (_characterManager.HasActiveDialog())
                {
                    string currentLine = _characterManager.GetCurrentDialogLine();

                    // Set new text when dialog line changes
                    if (currentLine != _previousDialogLine)
                    {
                        _dialogBox.SetText(currentLine);
                        _previousDialogLine = currentLine;
                    }

                    _dialogBox.Update(gameTime);
                }

                // Handle dialog advancement with Space key (only on key press, not hold)
                if (
                    _characterManager.HasActiveDialog()
                    && keyboardState.IsKeyDown(Keys.Space)
                    && _previousKeyboardState.IsKeyUp(Keys.Space)
                )
                {
                    // If text is still streaming, complete it immediately
                    if (!_dialogBox.IsTextComplete())
                    {
                        _dialogBox.CompleteText();
                    }
                    else
                    {
                        // Otherwise advance to next dialog line
                        _characterManager.AdvanceDialog();
                    }
                }

                _camera.Update(gameTime);
                _camera.Follow(_player.Position);
                _waterAnimationManager.Update(gameTime);
                _coastAnimationManager.Update(gameTime);
            }
        }
        _previousKeyboardState = keyboardState;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Settings.Colors.Gray);

        if (_gameState == GameState.Battle)
        {
            // Draw battle scene
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _battleScene.Draw(_spriteBatch, gameTime);
            _spriteBatch.End();
        }
        else
        {
            // Draw exploration scene
            _tiledMapRenderer.Draw(_camera.Transform);

            // Draw water animation, object sprites and player with camera transform
            // Use SpriteSortMode.FrontToBack to sort by layer depth (Y position)
            _spriteBatch.Begin(
                sortMode: SpriteSortMode.FrontToBack,
                transformMatrix: _camera.Transform,
                samplerState: SamplerState.PointClamp
            );
            _waterAnimationManager.Draw(_spriteBatch);
            _coastAnimationManager.Draw(_spriteBatch);
            _objectManager.Draw(_spriteBatch);
            _monsterManager.Draw(_spriteBatch);
            _characterManager.Draw(_spriteBatch);
            _player.Draw(_spriteBatch);
            _objectManager.DrawTopObjects(_spriteBatch);
            _spriteBatch.End();

            // Draw dialog box on top (without camera transform for UI, but needs position)
            if (_characterManager.HasActiveDialog())
            {
                _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                _dialogBox.Draw(
                    _spriteBatch,
                    _characterManager.GetActiveNPCPosition(),
                    _camera.Transform
                );
                _spriteBatch.End();
            }
        }

        // Draw fade effect on top of everything
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _fadeEffect.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
