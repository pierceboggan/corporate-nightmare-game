using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CorporateNightmare.GameComponents;
using CorporateNightmare.Entities;

namespace CorporateNightmare
{
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
        private const float INITIAL_SNAKE_SPEED = 15.0f; // Increased from 8.0f
        
        // Core graphics components
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        // Core game components
        private GameState _gameState;
        private readonly InputManager _inputManager;
        private readonly ScoreManager _scoreManager;
        private readonly SoundManager _soundManager;
        
        // Game entities
        private Snake _snake;
        private readonly Rectangle _gameAreaBounds;
        
        // Temporary variables for MVP - will be replaced by proper components later
        private Texture2D _pixel;
        private SpriteFont _font;
        
        // Death animation
        private const float DEATH_ANIMATION_DELAY = 0.5f; // Half second delay
        private float _deathTimer;
        private bool _deathAnimationComplete;

        // Collectible settings
        private const int COLLECTIBLE_SIZE = 15;
        private const float COLLECTIBLE_SPAWN_INTERVAL = 3.0f;
        private readonly CollectibleManager _collectibleManager;

        // Obstacle settings
        private const float OBSTACLE_SPAWN_INTERVAL = 5.0f;
        private const float POWERUP_SPAWN_INTERVAL = 15.0f;
        private readonly ObstacleManager _obstacleManager;

        // UI components
        private readonly StatusManager _statusManager;
        private const int STATUS_DISPLAY_PADDING = 10;

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

            // Initialize game components
            _inputManager = new InputManager();
            _scoreManager = new ScoreManager();
            _soundManager = new SoundManager();
            
            // Define the game area bounds
            _gameAreaBounds = new Rectangle(
                GAME_AREA_PADDING, 
                GAME_AREA_PADDING, 
                WINDOW_WIDTH - (GAME_AREA_PADDING * 2), 
                WINDOW_HEIGHT - (GAME_AREA_PADDING * 2)
            );

            // Initialize the collectible manager
            _collectibleManager = new CollectibleManager(
                _gameAreaBounds,
                COLLECTIBLE_SPAWN_INTERVAL,
                COLLECTIBLE_SIZE
            );
            
            // Initialize the obstacle manager
            _obstacleManager = new ObstacleManager(
                _gameAreaBounds,
                OBSTACLE_SPAWN_INTERVAL,
                POWERUP_SPAWN_INTERVAL
            );
            
            // Initialize status manager in top-left corner with padding
            _statusManager = new StatusManager(new Vector2(STATUS_DISPLAY_PADDING, STATUS_DISPLAY_PADDING));
        }

        /// <summary>
        /// Initialize the game state and variables before the game starts.
        /// </summary>
        protected override void Initialize()
        {
            // Initialize game components
            _gameState = new GameState(this);
            
            // Initialize the snake
            InitializeSnake();
            
            _deathTimer = 0;
            _deathAnimationComplete = false;
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
            
            // Load the font
            _font = Content.Load<SpriteFont>("GameFont");
            
            // Initialize fonts for all entities
            CoffeeCollectible.SetFont(_font);
            OfficeSupplyCollectible.SetFont(_font);
            LTReviewObstacle.SetFont(_font);
            MeetingObstacle.SetFont(_font);
            OKRObstacle.SetFont(_font);
            WorkFromHomePowerUp.SetFont(_font);
            TeamCollaborationPowerUp.SetFont(_font);
            CorporateRetreatPowerUp.SetFont(_font);

            // Initialize game state and managers
            _gameState = new GameState(this);

            // Initialize snake and managers with correct parameters
            _snake = new Snake(
                new Vector2(WINDOW_WIDTH / 2, WINDOW_HEIGHT / 2),
                SEGMENT_SIZE,
                INITIAL_SNAKE_LENGTH,
                INITIAL_SNAKE_SPEED,
                _gameAreaBounds
            );
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
                
                // Check if the snake has died
                if (!_snake.IsAlive && !_deathAnimationComplete)
                {
                    _deathTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    
                    // Wait for animation delay before transitioning to game over
                    if (_deathTimer >= DEATH_ANIMATION_DELAY)
                    {
                        _deathAnimationComplete = true;
                        _gameState.ChangeState(GameState.State.GameOver);
                        
                        // Play appropriate sound based on collision type
                        switch (_snake.LastCollisionType)
                        {
                            case Snake.CollisionType.Self:
                                _soundManager.PlaySound("SelfCollision"); // Will be implemented when sound effects are added
                                break;
                            case Snake.CollisionType.Boundary:
                                _soundManager.PlaySound("WallCollision"); // Will be implemented when sound effects are added
                                break;
                        }
                    }
                }
                
                // Update obstacles with the snake's position for tracking
                _obstacleManager.Update(gameTime, _snake.HeadPosition);
                
                // Check for obstacle collisions if snake is alive
                if (_snake.IsAlive)
                {
                    // Handle obstacle collisions
                    var hitObstacle = _obstacleManager.CheckObstacleCollision(_snake.HeadBounds);
                    if (hitObstacle != null)
                    {
                        string penaltyName = hitObstacle switch
                        {
                            LTReviewObstacle => "Leadership Review",
                            MeetingObstacle => "Endless Meeting",
                            OKRObstacle => "OKR Assessment",
                            _ => "Corporate Obstacle"
                        };
                        
                        // Apply obstacle penalties
                        _snake.ReduceSpeed(hitObstacle.SpeedPenalty);
                        _scoreManager.AddScore(-hitObstacle.PointPenalty);
                        _soundManager.PlaySound("ObstacleHit");
                        
                        // Show penalty status effect (5 second duration)
                        _statusManager.AddEffect(penaltyName, 5.0f, Color.Red);
                        
                        // Flash the snake head to indicate damage
                        _snake.GetSegment(0).SetColor(Color.Red);
                    }
                    
                    // Handle power-up collection
                    var collectedPowerUp = _obstacleManager.CheckPowerUpCollision(_snake.HeadBounds);
                    if (collectedPowerUp != null)
                    {
                        _soundManager.PlaySound("PowerUp");
                        
                        // Apply and show appropriate power-up effect
                        if (collectedPowerUp is WorkFromHomePowerUp wfh)
                        {
                            _statusManager.AddEffect("Work From Home", wfh.RemainingDuration, Color.Green);
                        }
                        else if (collectedPowerUp is TeamCollaborationPowerUp collab)
                        {
                            _statusManager.AddEffect("Team Collaboration", collab.RemainingDuration, Color.DeepSkyBlue);
                        }
                        else if (collectedPowerUp is CorporateRetreatPowerUp retreat)
                        {
                            _statusManager.AddEffect("Corporate Retreat", retreat.RemainingDuration, Color.MediumPurple);
                        }
                        
                        // Flash the snake head to indicate power-up
                        _snake.GetSegment(0).SetColor(Color.Yellow);
                    }
                }
                
                // Update collectibles and check for collection
                _collectibleManager.Update(gameTime);
                var collected = _collectibleManager.CheckCollisions(_snake.HeadBounds);
                
                if (collected != null)
                {
                    _scoreManager.AddScore(collected.PointValue);
                    
                    // Handle different collectible types
                    if (collected is CoffeeCollectible coffee)
                    {
                        _snake.IncreaseSpeed(coffee.SpeedBoost);
                        _soundManager.PlaySound("DrinkCoffee"); // Will be implemented when sound effects are added
                    }
                    else if (collected is OfficeSupplyCollectible supply)
                    {
                        HandleCollectible(supply);
                    }
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
            
            // Draw game elements when playing or game over
            if (_gameState.IsPlaying || _gameState.IsGameOver)
            {
                DrawGameArea();
                _snake.Draw(_spriteBatch, _pixel);
                _collectibleManager.Draw(_spriteBatch, _pixel);
                _obstacleManager.Draw(_spriteBatch, _pixel);
                
                // Draw status effects when playing
                if (_gameState.IsPlaying && _font != null)
                {
                    _statusManager.Draw(_spriteBatch, _font);
                }
            }
            
            // Draw game state-specific elements
            _gameState.Draw(_spriteBatch, gameTime);
            
            // Draw the current score
            if (_font != null)
            {
                _scoreManager.DrawScore(_spriteBatch, _font, 
                    new Vector2(WINDOW_WIDTH - 150, STATUS_DISPLAY_PADDING), 
                    Color.White);
            }
            
            _spriteBatch.End();
            
            base.Draw(gameTime);
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
            _collectibleManager?.Clear();
            _obstacleManager?.Clear();
            _statusManager?.Clear();
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

        private void HandleCollectible(Collectible collectible)
        {
            if (collectible is OfficeSupplyCollectible)
            {
                for (int i = 0; i < 3; i++)
                {
                    _snake.Grow();
                }
            }
        }
    }
}
