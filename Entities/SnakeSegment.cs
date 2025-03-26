using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CorporateNightmare.Entities
{
    /// <summary>
    /// Represents a single segment of the snake in the Corporate Nightmare game.
    /// Each segment has a position, direction, and visual representation.
    /// The segments together form the complete snake entity.
    /// </summary>
    public class SnakeSegment
    {
        // Position and movement properties
        private Vector2 _position;
        private Vector2 _direction;
        private Vector2 _previousPosition;
        private Vector2 _previousDirection;
        
        // Visual properties
        private readonly int _size;
        private Color _color;
        
        // Properties with public getters
        public Vector2 Position => _position;
        public Vector2 Direction => _direction;
        public Vector2 PreviousPosition => _previousPosition;
        public Vector2 PreviousDirection => _previousDirection;
        public Rectangle Bounds => new Rectangle(
            (int)_position.X, 
            (int)_position.Y, 
            _size, 
            _size
        );
        public int Size => _size;
        public Color Color => _color;

        /// <summary>
        /// Creates a new snake segment at the specified position.
        /// </summary>
        /// <param name="position">The initial position of the segment</param>
        /// <param name="direction">The initial direction of the segment</param>
        /// <param name="size">The size of the segment in pixels</param>
        /// <param name="color">The color of the segment</param>
        public SnakeSegment(Vector2 position, Vector2 direction, int size, Color color)
        {
            _position = position;
            _direction = direction;
            _previousPosition = position;
            _previousDirection = direction;
            _size = size;
            _color = color;
        }
        
        /// <summary>
        /// Updates the segment's position based on its direction and speed.
        /// </summary>
        /// <param name="newPosition">The new position for this segment</param>
        /// <param name="newDirection">The new direction for this segment</param>
        public void Update(Vector2 newPosition, Vector2 newDirection)
        {
            // Store previous state for tracking
            _previousPosition = _position;
            _previousDirection = _direction;
            
            // Update to new state
            _position = newPosition;
            _direction = newDirection;
        }
        
        /// <summary>
        /// Draws the snake segment on screen.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to use for drawing</param>
        /// <param name="texture">Texture to use for the segment</param>
        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            // Draw the segment at its current position
            spriteBatch.Draw(
                texture,
                Bounds,
                _color
            );
        }
        
        /// <summary>
        /// Checks if this segment intersects with the specified rectangle.
        /// </summary>
        /// <param name="rectangle">Rectangle to check for intersection</param>
        /// <returns>True if there is an intersection, false otherwise</returns>
        public bool Intersects(Rectangle rectangle)
        {
            return Bounds.Intersects(rectangle);
        }

        /// <summary>
        /// Changes the color of the segment.
        /// </summary>
        /// <param name="newColor">The new color to set</param>
        public void SetColor(Color newColor)
        {
            _color = newColor;
        }
    }
}