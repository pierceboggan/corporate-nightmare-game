// filepath: /Users/pierce/Documents/GitHub/corporate-nightmare-game/GameComponents/GameState.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CorporateNightmare.GameComponents
{
    /// <summary>
    /// Manages the different states of the game (menu, playing, game over).
    /// This component handles state transitions and state-specific update/draw logic.
    /// </summary>
    public class GameState
    {
        /// <summary>
        /// Represents the possible states the game can be in.
        /// </summary>
        public enum State
        {
            MainMenu,
            Playing,
            Paused,
            GameOver
        }

        // Current game state
        private State _currentState;

        // Reference to the game
        private readonly Game1 _game;

        // Game state properties
        public State CurrentState => _currentState;
        public bool IsPlaying => _currentState == State.Playing;
        public bool IsPaused => _currentState == State.Paused;
        public bool IsGameOver => _currentState == State.GameOver;
        public bool IsMainMenu => _currentState == State.MainMenu;

        /// <summary>
        /// Creates a new instance of the GameState manager.
        /// </summary>
        /// <param name="game">Reference to the main game object</param>
        public GameState(Game1 game)
        {
            _game = game;
            _currentState = State.MainMenu; // Start in the main menu
        }

        /// <summary>
        /// Changes the current game state and performs any necessary actions for the transition.
        /// </summary>
        /// <param name="newState">The new state to transition to</param>
        public void ChangeState(State newState)
        {
            // Handle exiting the current state
            switch (_currentState)
            {
                case State.Playing:
                    // Cleanup when leaving playing state if needed
                    break;
                case State.MainMenu:
                    // Cleanup when leaving menu if needed
                    break;
                case State.Paused:
                    // Cleanup when unpausing if needed
                    break;
                case State.GameOver:
                    // Cleanup when leaving game over if needed
                    break;
            }

            // Set the new state
            _currentState = newState;

            // Handle entering the new state
            switch (_currentState)
            {
                case State.Playing:
                    // Initialize gameplay
                    break;
                case State.MainMenu:
                    // Initialize menu
                    break;
                case State.Paused:
                    // Setup pause screen
                    break;
                case State.GameOver:
                    // Show game over screen
                    break;
            }
        }

        /// <summary>
        /// Updates game state logic based on the current state.
        /// </summary>
        /// <param name="gameTime">Snapshot of timing values</param>
        public void Update(GameTime gameTime)
        {
            // Apply state-specific update logic
            switch (_currentState)
            {
                case State.Playing:
                    UpdatePlaying(gameTime);
                    break;
                case State.MainMenu:
                    UpdateMainMenu(gameTime);
                    break;
                case State.Paused:
                    UpdatePaused(gameTime);
                    break;
                case State.GameOver:
                    UpdateGameOver(gameTime);
                    break;
            }
        }

        /// <summary>
        /// Draws elements based on the current game state.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to use for drawing</param>
        /// <param name="gameTime">Snapshot of timing values</param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // Apply state-specific drawing logic
            switch (_currentState)
            {
                case State.Playing:
                    DrawPlaying(spriteBatch, gameTime);
                    break;
                case State.MainMenu:
                    DrawMainMenu(spriteBatch, gameTime);
                    break;
                case State.Paused:
                    DrawPaused(spriteBatch, gameTime);
                    break;
                case State.GameOver:
                    DrawGameOver(spriteBatch, gameTime);
                    break;
            }
        }

        // Private update methods for each state
        private void UpdatePlaying(GameTime gameTime)
        {
            // Game play update logic will be added here
        }

        private void UpdateMainMenu(GameTime gameTime)
        {
            // Main menu update logic will be added here
        }

        private void UpdatePaused(GameTime gameTime)
        {
            // Pause screen update logic will be added here
        }

        private void UpdateGameOver(GameTime gameTime)
        {
            // Game over screen update logic will be added here
        }

        // Private draw methods for each state
        private void DrawPlaying(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // Game play drawing logic will be added here
        }

        private void DrawMainMenu(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // Main menu drawing logic will be added here
        }

        private void DrawPaused(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // Pause screen drawing logic will be added here
        }

        private void DrawGameOver(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // Game over screen drawing logic will be added here
        }
    }
}