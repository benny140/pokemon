using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace pokemon_game;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private TiledMap _tiledMap;
    private TiledMapRenderer _tiledMapRenderer;
    private Matrix _viewMatrix;

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
    }

    protected override void Update(GameTime gameTime)
    {
        if (
            GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
            || Keyboard.GetState().IsKeyDown(Keys.Escape)
        )
            Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Settings.Colors.Black);

        // Draw the Tiled map with the scaled view matrix
        _tiledMapRenderer.Draw(_viewMatrix);

        base.Draw(gameTime);
    }
}
