using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CorporateNightmare.Entities
{
    public class TeamCollaborationPowerUp : PowerUp
    {
        private const string ASCII_REPRESENTATION = "TEAM";
        public float ClearRadius { get; private set; } = 100f;
        
        public TeamCollaborationPowerUp(Vector2 position, int size, float duration) 
            : base(position, size, duration)
        {
            _color = Color.Blue; // Blue for team collaboration
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