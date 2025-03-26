using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CorporateNightmare.Entities
{
    public class CoffeeCollectible : Collectible
    {
        private const string ASCII_REPRESENTATION = "☕";
        
        public CoffeeCollectible(Vector2 position, int size, int pointValue, float speedBoost) 
            : base(position, size, pointValue)
        {
            _speedBoost = speedBoost;
            _color = new Color(139, 69, 19); // Brown color for coffee
        }

        public override void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            if (_font == null) return;
            spriteBatch.DrawString(_font, ASCII_REPRESENTATION, base.Position, _color);
        }

        private readonly float _speedBoost;
        private static SpriteFont _font;

        public static void SetFont(SpriteFont font)
        {
            _font = font;
        }

        public float SpeedBoost => _speedBoost;
    }
}