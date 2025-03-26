// filepath: /Users/pierce/Documents/GitHub/corporate-nightmare-game/GameComponents/InputManager.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CorporateNightmare.GameComponents
{
    /// <summary>
    /// Manages user input from keyboard and mouse.
    /// This component tracks input states and provides methods for checking key presses,
    /// key holds, mouse movements, and mouse button clicks.
    /// </summary>
    public class InputManager
    {
        // Current and previous keyboard and mouse states for detecting state changes
        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;
        private MouseState _currentMouseState;
        private MouseState _previousMouseState;

        // Mouse position information
        private Point _currentMousePosition;
        private Point _previousMousePosition;

        /// <summary>
        /// Initializes a new instance of the InputManager class.
        /// </summary>
        public InputManager()
        {
            _currentKeyboardState = Keyboard.GetState();
            _previousKeyboardState = _currentKeyboardState;
            _currentMouseState = Mouse.GetState();
            _previousMouseState = _currentMouseState;
            _currentMousePosition = new Point(_currentMouseState.X, _currentMouseState.Y);
            _previousMousePosition = _currentMousePosition;
        }

        /// <summary>
        /// Updates input states. Should be called once per frame.
        /// </summary>
        public void Update()
        {
            // Save previous state
            _previousKeyboardState = _currentKeyboardState;
            _previousMouseState = _currentMouseState;
            _previousMousePosition = _currentMousePosition;
            
            // Get current state
            _currentKeyboardState = Keyboard.GetState();
            _currentMouseState = Mouse.GetState();
            _currentMousePosition = new Point(_currentMouseState.X, _currentMouseState.Y);
        }

        /// <summary>
        /// Checks if a key was just pressed (pressed this frame but not the previous frame).
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if the key was just pressed, false otherwise</returns>
        public bool IsKeyPressed(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key) && _previousKeyboardState.IsKeyUp(key);
        }

        /// <summary>
        /// Checks if a key is currently being held down.
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if the key is currently down, false otherwise</returns>
        public bool IsKeyDown(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Checks if a key was just released (released this frame but down the previous frame).
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if the key was just released, false otherwise</returns>
        public bool IsKeyReleased(Keys key)
        {
            return _currentKeyboardState.IsKeyUp(key) && _previousKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Gets the current direction input from WASD or arrow keys.
        /// </summary>
        /// <returns>Vector2 representing the direction (normalized)</returns>
        public Vector2 GetMovementDirection()
        {
            Vector2 direction = Vector2.Zero;
            
            // Check horizontal movement
            if (IsKeyDown(Keys.Left) || IsKeyDown(Keys.A))
                direction.X -= 1;
            if (IsKeyDown(Keys.Right) || IsKeyDown(Keys.D))
                direction.X += 1;
            
            // Check vertical movement
            if (IsKeyDown(Keys.Up) || IsKeyDown(Keys.W))
                direction.Y -= 1;
            if (IsKeyDown(Keys.Down) || IsKeyDown(Keys.S))
                direction.Y += 1;
            
            // Normalize if not zero to ensure consistent movement speed in all directions
            if (direction != Vector2.Zero)
                direction.Normalize();
            
            return direction;
        }

        /// <summary>
        /// Checks if the left mouse button was just clicked.
        /// </summary>
        /// <returns>True if the left mouse button was just clicked, false otherwise</returns>
        public bool IsLeftMouseClicked()
        {
            return _currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released;
        }

        /// <summary>
        /// Checks if the right mouse button was just clicked.
        /// </summary>
        /// <returns>True if the right mouse button was just clicked, false otherwise</returns>
        public bool IsRightMouseClicked()
        {
            return _currentMouseState.RightButton == ButtonState.Pressed && _previousMouseState.RightButton == ButtonState.Released;
        }

        /// <summary>
        /// Gets the current mouse position.
        /// </summary>
        /// <returns>Point representing the current mouse position</returns>
        public Point GetMousePosition()
        {
            return _currentMousePosition;
        }

        /// <summary>
        /// Gets the mouse movement delta since the last update.
        /// </summary>
        /// <returns>Point representing the mouse movement delta</returns>
        public Point GetMouseDelta()
        {
            return new Point(
                _currentMousePosition.X - _previousMousePosition.X, 
                _currentMousePosition.Y - _previousMousePosition.Y);
        }

        /// <summary>
        /// Checks if the mouse is currently over the specified rectangle.
        /// </summary>
        /// <param name="rectangle">Rectangle to check</param>
        /// <returns>True if the mouse is over the rectangle, false otherwise</returns>
        public bool IsMouseOver(Rectangle rectangle)
        {
            return rectangle.Contains(_currentMousePosition);
        }
    }
}