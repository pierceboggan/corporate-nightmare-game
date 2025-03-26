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
        
        public bool IsCollected => _isCollected;
        public Rectangle Bounds => _bounds;
        public int PointValue => _pointValue;
        public Vector2 Position => _position;
        
        protected Collectible(Vector2 position, int size, int pointValue)
        {
            _position = position;
            _size = size;
            _pointValue = pointValue;
            _isCollected = false;
            _color = Color.White;
            
            // Create the bounds rectangle
            _bounds = new Rectangle(
                (int)position.X,
                (int)position.Y,
                size,
                size
            );
        }
        
        public virtual void Update(GameTime gameTime)
        {
            // Base implementation does nothing
        }
        
        public abstract void Draw(SpriteBatch spriteBatch, Texture2D texture);
        
        public virtual void Collect()
        {
            _isCollected = true;
        }
        
        public bool Intersects(Rectangle rectangle)
        {
            return !_isCollected && _bounds.Intersects(rectangle);
        }
    }
}