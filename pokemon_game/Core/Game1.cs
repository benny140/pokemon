using Microsoft.Xna.Framework;
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
    private DialogBox _dialogBox;
    private SpriteFont _font;
    private int _mapWidth;
    private int _mapHeight;
    private KeyboardState _previousKeyboardState;
    private string _previousDialogLine;

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

        // Load the Tiled map (path relative to Content output directory, without .xnb extension)
        _tiledMap = Content.Load<TiledMap>("data/maps/world");
        _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);

        // Store map dimensions for zoom calculations
        _mapWidth = _tiledMap.WidthInPixels;
        _mapHeight = _tiledMap.HeightInPixels;

        // Load collisions
        _collisionManager = new CollisionManager();
        _collisionManager.LoadCollisions(_tiledMap);

        // Load objects from the Objects layer
        _objectManager = new ObjectManager();
        _objectManager.LoadObjects(_tiledMap);

        // Load player
        var playerTexture = Content.Load<Texture2D>("graphics/characters/player");
        _player = new Player(playerTexture, new Vector2(2560, 2560), _collisionManager); // Start in middle of map

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
        _characterManager.LoadCharacters(_tiledMap, Content);

        // Load notice icon for NPCs
        var noticeIcon = Content.Load<Texture2D>("graphics/ui/notice");
        NPC.SetNoticeIcon(noticeIcon);
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

        // Update character manager first to check for interactions
        _characterManager.Update(gameTime, _player.Position);

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

        _previousKeyboardState = keyboardState;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Settings.Colors.Gray);
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

        base.Draw(gameTime);
    }
}
