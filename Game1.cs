using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CorporateNightmare
{
    /// <summary>
    /// Main game class for Corporate Nightmare - a corporate-themed twist on the classic Snake game.
    /// This class handles the core game loop, initialization, and rendering processes.
    /// </summary>
    public class Game1 : Game
    {
        // Constants for game configuration
        private const int WINDOW_WIDTH = 800;
        private const int WINDOW_HEIGHT = 600;
        private const string GAME_TITLE = "Corporate Nightmare";

        // Core graphics components
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        // Placeholder variables for MVP - will be replaced by proper components later
        private Texture2D _pixel;
        
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
            // Configure window properties
            _graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            _graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            _graphics.ApplyChanges();
            
            Window.Title = GAME_TITLE;
            IsMouseVisible = true;
        }

        /// <summary>
        /// Initialize the game state and variables before the game starts.
        /// </summary>
        protected override void Initialize()
        {
            // Initialize game components will be added here in future steps
            
            base.Initialize();
        }

        /// <summary>
        /// Load all game content such as textures, fonts, and sounds.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // Temporary pixel texture for placeholder rendering
            _pixel = new Texture2D(GraphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });
            
            // Future content loading will be added here
        }

        /// <summary>
        /// Update game logic such as player movement, collisions, and game state.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Check if player wants to exit the game
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || 
                Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            // Game state updates will be added here
            
            base.Update(gameTime);
        }

        /// <summary>
        /// Draw the game's visual elements to the screen.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Set the background color to a corporate-friendly light blue
            GraphicsDevice.Clear(new Color(240, 248, 255)); // AliceBlue
            
            _spriteBatch.Begin();
            
            // Placeholder drawing - this will be replaced by actual game elements
            _spriteBatch.Draw(_pixel, new Rectangle(WINDOW_WIDTH / 2, WINDOW_HEIGHT / 2, 32, 32), Color.DarkBlue);
            
            _spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
