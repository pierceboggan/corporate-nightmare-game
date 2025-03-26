using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CorporateNightmare.Entities
{
    /// <summary>
    /// Base class for all collectible items in the game.
    /// Provides common functionality for position, collision detection,
    /// and basic rendering.
    /// </summary>
    public abstract class Collectible
    {
        // Position and bounds
        protected Vector2 _position;
        protected readonly Rectangle _bounds;
        protected readonly int _size;
        
        // Visual properties
        protected Color _color;
        protected bool _isCollected;
        
        // Points awarded when collected
        protected readonly int _pointValue;
        
        /// <summary>
        /// Gets whether the collectible has been collected
        /// </summary>
        public bool IsCollected => _isCollected;
        
        /// <summary>
        /// Gets the bounds of the collectible for collision detection
        /// </summary>
        public Rectangle Bounds => _bounds;
        
        /// <summary>
        /// Gets the point value of the collectible
        /// </summary>
        public int PointValue => _pointValue;
        
        /// <summary>
        /// Creates a new collectible at the specified position
        /// </summary>
        /// <param name="position">The position of the collectible</param>
        /// <param name="size">The size of the collectible in pixels</param>
        /// <param name="pointValue">Points awarded when collected</param>
        protected Collectible(Vector2 position, int size, int pointValue)
        {
            _position = position;
            _size = size;
            _pointValue = pointValue;
            _isCollected = false;
            
            // Create the bounds rectangle
            _bounds = new Rectangle(
                (int)position.X,
                (int)position.Y,
                size,
                size
            );
        }
        
        /// <summary>
        /// Updates the collectible state
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        public virtual void Update(GameTime gameTime)
        {
            // Base implementation does nothing
        }
        
        /// <summary>
        /// Draws the collectible
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to use for drawing</param>
        /// <param name="texture">Texture to use for rendering</param>
        public virtual void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            if (!_isCollected)
            {
                spriteBatch.Draw(texture, _bounds, _color);
            }
        }
        
        /// <summary>
        /// Marks the collectible as collected
        /// </summary>
        public virtual void Collect()
        {
            _isCollected = true;
        }
        
        /// <summary>
        /// Checks if the collectible intersects with a given rectangle
        /// </summary>
        /// <param name="rectangle">Rectangle to check intersection with</param>
        /// <returns>True if intersecting, false otherwise</returns>
        public bool Intersects(Rectangle rectangle)
        {
            return !_isCollected && _bounds.Intersects(rectangle);
        }
    }
}