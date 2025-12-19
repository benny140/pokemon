using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using pokemon_game.Entities;
using pokemon_game.Graphics;
using pokemon_game.Managers;

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

        // Load the Tiled map (path relative to Content output directory, without .xnb extension)
        _tiledMap = Content.Load<TiledMap>("data/maps/world");
        _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);

        // Load objects from the Objects layer
        _objectManager = new ObjectManager();
        _objectManager.LoadObjects(_tiledMap);

        // Load player
        var playerTexture = Content.Load<Texture2D>("graphics/characters/player");
        _player = new Player(playerTexture, new Vector2(2560, 2560)); // Start in middle of map

        // Load water animation
        _waterAnimationManager = new WaterAnimationManager();
        _waterAnimationManager.LoadContent(GraphicsDevice, _tiledMap);
        _waterAnimationManager.LoadTextures(
            Content.Load<Texture2D>("graphics/tilesets/water/0"),
            Content.Load<Texture2D>("graphics/tilesets/water/1"),
            Content.Load<Texture2D>("graphics/tilesets/water/2"),
            Content.Load<Texture2D>("graphics/tilesets/water/3")
        );
    }

    protected override void Update(GameTime gameTime)
    {
        if (
            GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
            || Keyboard.GetState().IsKeyDown(Keys.Escape)
        )
            Exit();

        _player.Update(gameTime);
        _camera.Follow(_player.Position);
        _waterAnimationManager.Update(gameTime);

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
        _objectManager.Draw(_spriteBatch);
        _player.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
