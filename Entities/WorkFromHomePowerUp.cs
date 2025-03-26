using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CorporateNightmare.Entities
{
    public class WorkFromHomePowerUp : PowerUp
    {
        private const string ASCII_REPRESENTATION = "🏠";
        
        public WorkFromHomePowerUp(Vector2 position, int size, float duration) 
            : base(position, size, duration)
        {
            _color = Color.Green; // Green for WFH
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