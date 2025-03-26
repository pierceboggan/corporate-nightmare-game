using Microsoft.Xna.Framework;

namespace CorporateNightmare.Entities
{
    /// <summary>
    /// Represents a coffee cup collectible that increases the snake's speed when collected.
    /// </summary>
    public class CoffeeCollectible : Collectible
    {
        // Speed boost amount when collected
        private readonly float _speedBoost;
        
        /// <summary>
        /// Gets the speed boost value
        /// </summary>
        public float SpeedBoost => _speedBoost;
        
        /// <summary>
        /// Creates a new coffee cup collectible
        /// </summary>
        /// <param name="position">The position of the coffee cup</param>
        /// <param name="size">The size in pixels</param>
        /// <param name="pointValue">Points awarded when collected</param>
        /// <param name="speedBoost">Amount to increase snake speed by</param>
        public CoffeeCollectible(Vector2 position, int size, int pointValue, float speedBoost) 
            : base(position, size, pointValue)
        {
            _speedBoost = speedBoost;
            _color = new Color(139, 69, 19); // Brown color for coffee
        }
    }
}