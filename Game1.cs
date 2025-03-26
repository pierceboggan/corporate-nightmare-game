using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CorporateNightmare.GameComponents;

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
        
        // Core game components
        private GameState _gameState;
        private InputManager _inputManager;
        private ScoreManager _scoreManager;
        private SoundManager _soundManager;
        
        // Temporary variables for MVP - will be replaced by proper components later
        private Texture2D _pixel;
        private SpriteFont _font;
        
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
            // Initialize game components
            _inputManager = new InputManager();
            _scoreManager = new ScoreManager();
            _soundManager = new SoundManager();
            _gameState = new GameState(this);
            
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
            
            // Load fonts and other assets when they are available
            // _font = Content.Load<SpriteFont>("Fonts/GameFont");
            
            // Load sound effects and music when they are available
            // _soundManager.LoadSoundEffect(this, "Sounds/Collect");
            // _soundManager.LoadSong(this, "Music/GameTheme");
            
            // Future content loading will be added here
        }

        /// <summary>
        /// Update game logic such as player movement, collisions, and game state.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Update input state
            _inputManager.Update();
            
            // Check if player wants to exit the game
            if (_inputManager.IsKeyPressed(Keys.Escape))
            {
                Exit();
            }

            // Toggle game state with space bar (for testing)
            if (_inputManager.IsKeyPressed(Keys.Space))
            {
                if (_gameState.CurrentState == GameState.State.Playing)
                {
                    _gameState.ChangeState(GameState.State.Paused);
                }
                else if (_gameState.CurrentState == GameState.State.Paused)
                {
                    _gameState.ChangeState(GameState.State.Playing);
                }
                else if (_gameState.CurrentState == GameState.State.MainMenu)
                {
                    _gameState.ChangeState(GameState.State.Playing);
                }
                else if (_gameState.CurrentState == GameState.State.GameOver)
                {
                    _scoreManager.ResetScore();
                    _gameState.ChangeState(GameState.State.MainMenu);
                }
            }
            
            // Update the game state
            _gameState.Update(gameTime);
            
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
            
            // Draw game state-specific elements
            _gameState.Draw(_spriteBatch, gameTime);
            
            // Draw the current score in the top-right corner
            // When font is available
            // _scoreManager.DrawScore(_spriteBatch, _font, new Vector2(WINDOW_WIDTH - 150, 10), Color.Black);
            
            _spriteBatch.End();
            
            base.Draw(gameTime);
        }
        
        /// <summary>
        /// Clean up resources when the game is closing
        /// </summary>
        protected override void UnloadContent()
        {
            // Dispose of any textures and managed resources
            _pixel?.Dispose();
            _soundManager?.Unload();
            
            base.UnloadContent();
        }
    }
}
