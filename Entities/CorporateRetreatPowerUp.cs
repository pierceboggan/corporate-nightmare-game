using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CorporateNightmare.Entities
{
    public class CorporateRetreatPowerUp : PowerUp
    {
        private const string ASCII_REPRESENTATION = "RTR";
        public float TimeScale { get; private set; } = 0.5f;
        
        public CorporateRetreatPowerUp(Vector2 position, int size, float duration) 
            : base(position, size, duration)
        {
            _color = Color.Purple; // Purple for retreat
        }

        public override void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            if (_font == null) return;
            spriteBatch.DrawString(_font, ASCII_REPRESENTATION, Position, _color);
        }

        private static SpriteFont _font;

        public static void SetFont(SpriteFont font)
        {
            _font = font;
        }
    }
}