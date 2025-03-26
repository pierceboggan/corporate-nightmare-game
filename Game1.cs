using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CorporateNightmare.GameComponents;
using CorporateNightmare.Entities;

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
        
        // Game area constants
        private const int GAME_AREA_PADDING = 40;
        private const int SEGMENT_SIZE = 20;
        private const int INITIAL_SNAKE_LENGTH = 5;
        private const float INITIAL_SNAKE_SPEED = 8.0f;
        
        // Core graphics components
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        // Core game components
        private GameState _gameState;
        private InputManager _inputManager;
        private ScoreManager _scoreManager;
        private SoundManager _soundManager;
        
        // Game entities
        private Snake _snake;
        private Rectangle _gameAreaBounds;
        
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
            
            // Define the game area bounds
            _gameAreaBounds = new Rectangle(
                GAME_AREA_PADDING, 
                GAME_AREA_PADDING, 
                WINDOW_WIDTH - (GAME_AREA_PADDING * 2), 
                WINDOW_HEIGHT - (GAME_AREA_PADDING * 2)
            );
            
            // Initialize the snake
            InitializeSnake();
            
            base.Initialize();
        }

        /// <summary>
        /// Load all game content such as textures, fonts, and sounds.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // Create a white pixel texture for rendering simple shapes
            _pixel = new Texture2D(GraphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });
            
            // Load fonts and other assets when they are available
            // _font = Content.Load<SpriteFont>("Fonts/GameFont");
            
            // Load sound effects and music when they are available
            // _soundManager.LoadSoundEffect(this, "Sounds/Collect");
            // _soundManager.LoadSong(this, "Music/GameTheme");
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

            // Process game state changes
            UpdateGameState();
            
            // Process gameplay if we're in the playing state
            if (_gameState.IsPlaying)
            {
                // Get movement direction from the player's input
                Vector2 movementDirection = _inputManager.GetMovementDirection();
                
                // Update snake direction if the player is providing input
                if (movementDirection != Vector2.Zero)
                {
                    _snake.SetDirection(movementDirection);
                }
                
                // Update the snake's position and check for collisions
                _snake.Update(gameTime);
                
                // Check if the snake has died from boundary collision
                if (!_snake.IsAlive)
                {
                    _gameState.ChangeState(GameState.State.GameOver);
                    _soundManager.PlaySound("Death"); // Will be implemented when sound effects are added
                }

                // FOR TESTING: Press G to grow the snake
                if (_inputManager.IsKeyPressed(Keys.G))
                {
                    _snake.Grow();
                    _scoreManager.AddScore(10);
                }
            }
            
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
            
            // Draw the game area border
            DrawGameArea();
            
            // Draw the snake if we're playing or game over
            if (_gameState.IsPlaying || _gameState.IsGameOver)
            {
                _snake.Draw(_spriteBatch, _pixel);
            }
            
            // Draw game state-specific elements
            _gameState.Draw(_spriteBatch, gameTime);
            
            // Draw the current score in the top-right corner (when font is available)
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
        
        /// <summary>
        /// Initializes the player's snake with default settings.
        /// </summary>
        private void InitializeSnake()
        {
            // Calculate a good starting position (centered horizontally, upper third vertically)
            Vector2 startPosition = new Vector2(
                _gameAreaBounds.Center.X, 
                _gameAreaBounds.Y + (_gameAreaBounds.Height / 3)
            );
            
            // Create the snake
            _snake = new Snake(
                startPosition, 
                SEGMENT_SIZE, 
                INITIAL_SNAKE_LENGTH,
                INITIAL_SNAKE_SPEED,
                _gameAreaBounds
            );
        }
        
        /// <summary>
        /// Updates the game state based on player input.
        /// </summary>
        private void UpdateGameState()
        {
            // Toggle game state with space bar (for testing)
            if (_inputManager.IsKeyPressed(Keys.Space))
            {
                switch (_gameState.CurrentState)
                {
                    case GameState.State.Playing:
                        _gameState.ChangeState(GameState.State.Paused);
                        break;
                    case GameState.State.Paused:
                        _gameState.ChangeState(GameState.State.Playing);
                        break;
                    case GameState.State.MainMenu:
                        _gameState.ChangeState(GameState.State.Playing);
                        break;
                    case GameState.State.GameOver:
                        _scoreManager.ResetScore();
                        InitializeSnake(); // Reset the snake for a new game
                        _gameState.ChangeState(GameState.State.MainMenu);
                        break;
                }
            }
        }
        
        /// <summary>
        /// Draws the game area border.
        /// </summary>
        private void DrawGameArea()
        {
            // Draw border - top, right, bottom, left
            int borderThickness = 2;
            Color borderColor = Color.DarkGray;
            
            // Top border
            _spriteBatch.Draw(_pixel, 
                new Rectangle(_gameAreaBounds.X, _gameAreaBounds.Y, _gameAreaBounds.Width, borderThickness), 
                borderColor);
            
            // Right border
            _spriteBatch.Draw(_pixel, 
                new Rectangle(_gameAreaBounds.Right - borderThickness, _gameAreaBounds.Y, borderThickness, _gameAreaBounds.Height), 
                borderColor);
            
            // Bottom border
            _spriteBatch.Draw(_pixel, 
                new Rectangle(_gameAreaBounds.X, _gameAreaBounds.Bottom - borderThickness, _gameAreaBounds.Width, borderThickness), 
                borderColor);
            
            // Left border
            _spriteBatch.Draw(_pixel, 
                new Rectangle(_gameAreaBounds.X, _gameAreaBounds.Y, borderThickness, _gameAreaBounds.Height), 
                borderColor);
        }
    }
}
