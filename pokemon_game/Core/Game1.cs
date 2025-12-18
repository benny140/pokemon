using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using pokemon_game.Entities;
using pokemon_game.Managers;

namespace pokemon_game.Core;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private TiledMap _tiledMap;
    private TiledMapRenderer _tiledMapRenderer;
    private Matrix _viewMatrix;
    private ObjectManager _objectManager;
    private Player _player;

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
        // Create a view matrix that scales down to show 4x the area
        _viewMatrix = Matrix.CreateScale(Settings.ZOOM_SCALE);

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
    }

    protected override void Update(GameTime gameTime)
    {
        if (
            GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
            || Keyboard.GetState().IsKeyDown(Keys.Escape)
        )
            Exit();

        _player.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Settings.Colors.Gray);

        // Draw the Tiled map with the scaled view matrix
        _tiledMapRenderer.Draw(_viewMatrix);

        // Draw object sprites and player
        _spriteBatch.Begin(transformMatrix: _viewMatrix, samplerState: SamplerState.PointClamp);
        _objectManager.Draw(_spriteBatch);
        _player.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
