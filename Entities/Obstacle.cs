using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CorporateNightmare.Entities
{
    public abstract class Obstacle
    {
        protected Vector2 Position;
        protected Color _color;
        protected readonly int Size;
        public Rectangle Bounds { get; protected set; }
        
        public float SpeedPenalty { get; protected set; } = 0.2f;
        public int PointPenalty { get; protected set; } = 50;
        public bool IsActive { get; protected set; } = true;

        protected Obstacle(Vector2 position, int size)
        {
            Position = position;
            Size = size;
            Bounds = new Rectangle((int)position.X, (int)position.Y, size, size);
            _color = Color.White;
        }

        public abstract void Draw(SpriteBatch spriteBatch, Texture2D texture);
        
        public virtual void Update(GameTime gameTime) { }

        public virtual bool Intersects(Rectangle rectangle)
        {
            return IsActive && Bounds.Intersects(rectangle);
        }

        public virtual void Deactivate()
        {
            IsActive = false;
        }

        public Rectangle GetBounds() => Bounds;
    }
}