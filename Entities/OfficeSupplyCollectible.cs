using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace CorporateNightmare.Entities
{
    public class OfficeSupplyCollectible : Collectible
    {
        public enum OfficeSupplyType { Stapler, Paperclip, PushPin, RubberBand }

        private readonly OfficeSupplyType _supplyType;
        private static SpriteFont _font;
        private static readonly Dictionary<OfficeSupplyType, string> ASCII_REPRESENTATIONS = new()
        {
            { OfficeSupplyType.Stapler, "📎" },
            { OfficeSupplyType.Paperclip, "🔗" },
            { OfficeSupplyType.PushPin, "📌" },
            { OfficeSupplyType.RubberBand, "⭕" }
        };

        public OfficeSupplyCollectible(Vector2 position, int size, int pointValue, OfficeSupplyType type) 
            : base(position, size, pointValue)
        {
            _supplyType = type;
            _color = Color.Gray; // Gray for office supplies
        }

        public override void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            if (_font == null) return;
            spriteBatch.DrawString(_font, ASCII_REPRESENTATIONS[_supplyType], base.Position, _color);
        }

        public static void SetFont(SpriteFont font)
        {
            _font = font;
        }

        public OfficeSupplyType SupplyType => _supplyType;
    }
}