using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CorporateNightmare.Entities
{
    public class OKRObstacle : Obstacle
    {
        private const string ASCII_REPRESENTATION = "OKR";
        private Vector2 _targetPosition;
        private const float TRACKING_SPEED = 100f;
        
        public OKRObstacle(Vector2 position, int size) : base(position, size)
        {
            _color = Color.Red;
            _targetPosition = position;
        }

        public override void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            if (_font == null) return;
            spriteBatch.DrawString(_font, ASCII_REPRESENTATION, Position, _color);
        }

        public void UpdateTarget(Vector2 target)
        {
            _targetPosition = target;
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsActive) return;

            Vector2 direction = _targetPosition - Position;
            if (direction != Vector2.Zero)
            {
                direction.Normalize();
                Position += direction * TRACKING_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;
                Bounds = new Rectangle((int)Position.X, (int)Position.Y, Size, Size);
            }
        }

        private static SpriteFont _font;

        public static void SetFont(SpriteFont font)
        {
            _font = font;
        }
    }
}