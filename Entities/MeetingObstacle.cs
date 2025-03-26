using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CorporateNightmare.Entities
{
    public class MeetingObstacle : Obstacle
    {
        private const string ASCII_REPRESENTATION = "MTG";
        
        public MeetingObstacle(Vector2 position, int size) : base(position, size)
        {
            _color = Color.Orange; // Orange for meetings
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