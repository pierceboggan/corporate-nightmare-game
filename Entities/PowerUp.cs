using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CorporateNightmare.Entities
{
    public abstract class PowerUp
    {
        protected Vector2 Position;
        protected Color _color;
        protected readonly int Size;
        public Rectangle Bounds { get; protected set; }
        protected float Duration;
        protected float RemainingTime;
        public bool IsActive { get; protected set; } = true;
        public float RemainingDuration => RemainingTime;

        protected PowerUp(Vector2 position, int size, float duration)
        {
            Position = position;
            Size = size;
            Duration = duration;
            RemainingTime = duration;
            Bounds = new Rectangle((int)position.X, (int)position.Y, size, size);
            _color = Color.White;
            IsActive = true;
        }

        public abstract void Draw(SpriteBatch spriteBatch, Texture2D texture);

        public virtual void Update(GameTime gameTime)
        {
            RemainingTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (RemainingTime <= 0)
            {
                IsActive = false;
            }
        }

        public virtual bool Intersects(Rectangle rectangle)
        {
            return IsActive && Bounds.Intersects(rectangle);
        }

        public virtual void Collect()
        {
            IsActive = false;
        }
    }
}