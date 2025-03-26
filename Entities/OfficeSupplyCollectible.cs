using Microsoft.Xna.Framework;

namespace CorporateNightmare.Entities
{
    /// <summary>
    /// Represents an office supply collectible that provides different effects
    /// based on the type of supply (stapler, paperclip, etc.)
    /// </summary>
    public class OfficeSupplyCollectible : Collectible
    {
        private readonly OfficeSupplyType _supplyType;
        private readonly int _growthAmount;
        
        /// <summary>
        /// Gets the type of office supply
        /// </summary>
        public OfficeSupplyType SupplyType => _supplyType;
        
        /// <summary>
        /// Gets the amount of growth this supply provides
        /// </summary>
        public int GrowthAmount => _growthAmount;
        
        /// <summary>
        /// Different types of office supplies and their colors
        /// </summary>
        public enum OfficeSupplyType
        {
            Stapler,    // Red - makes snake grow by 3
            Paperclip,  // Silver - makes snake grow by 1
            PushPin,    // Yellow - makes snake grow by 2
            RubberBand  // Green - makes snake grow by 1
        }
        
        /// <summary>
        /// Creates a new office supply collectible
        /// </summary>
        /// <param name="position">The position of the supply</param>
        /// <param name="size">The size in pixels</param>
        /// <param name="pointValue">Points awarded when collected</param>
        /// <param name="supplyType">Type of office supply</param>
        public OfficeSupplyCollectible(Vector2 position, int size, int pointValue, OfficeSupplyType supplyType) 
            : base(position, size, pointValue)
        {
            _supplyType = supplyType;
            
            // Set color and growth amount based on supply type
            switch (supplyType)
            {
                case OfficeSupplyType.Stapler:
                    _color = new Color(200, 0, 0); // Red
                    _growthAmount = 3;
                    break;
                case OfficeSupplyType.Paperclip:
                    _color = new Color(192, 192, 192); // Silver
                    _growthAmount = 1;
                    break;
                case OfficeSupplyType.PushPin:
                    _color = new Color(255, 215, 0); // Yellow
                    _growthAmount = 2;
                    break;
                case OfficeSupplyType.RubberBand:
                    _color = new Color(0, 128, 0); // Green
                    _growthAmount = 1;
                    break;
            }
        }
    }
}