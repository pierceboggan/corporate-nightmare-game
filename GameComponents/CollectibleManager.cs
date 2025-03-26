using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CorporateNightmare.Entities;

namespace CorporateNightmare.GameComponents
{
    public class CollectibleManager
    {
        private readonly List<Collectible> _collectibles;
        private readonly Random _random;
        private readonly Rectangle _spawnBounds;
        private float _spawnTimer;
        private readonly float _spawnInterval;
        private readonly int _collectibleSize;
        
        // Game balance settings
        private const int COFFEE_POINTS = 10;
        private const float COFFEE_SPEED_BOOST = 0.5f;
        private const int STAPLER_POINTS = 30;
        private const int PAPERCLIP_POINTS = 10;
        private const int PUSHPIN_POINTS = 20;
        private const int RUBBERBAND_POINTS = 10;
        
        // Spawn probabilities (must sum to 100)
        private const int COFFEE_SPAWN_CHANCE = 40;      // 40% chance
        private const int STAPLER_SPAWN_CHANCE = 10;     // 10% chance
        private const int PAPERCLIP_SPAWN_CHANCE = 20;   // 20% chance
        private const int PUSHPIN_SPAWN_CHANCE = 15;     // 15% chance
        private const int RUBBERBAND_SPAWN_CHANCE = 15;  // 15% chance
        
        private static SpriteFont _font;

        public CollectibleManager(Rectangle spawnBounds, float spawnInterval, int collectibleSize)
        {
            _collectibles = new List<Collectible>();
            _random = new Random();
            _spawnBounds = spawnBounds;
            _spawnInterval = spawnInterval;
            _collectibleSize = collectibleSize;
            _spawnTimer = 0;
        }

        public static void SetFont(SpriteFont font)
        {
            _font = font;
            CoffeeCollectible.SetFont(font);
            OfficeSupplyCollectible.SetFont(font);
        }

        public void Update(GameTime gameTime)
        {
            _spawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if (_spawnTimer >= _spawnInterval)
            {
                SpawnCollectible();
                _spawnTimer = 0;
            }
            
            foreach (var collectible in _collectibles)
            {
                collectible.Update(gameTime);
            }
            
            _collectibles.RemoveAll(c => c.IsCollected);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D defaultTexture)
        {
            foreach (var collectible in _collectibles)
            {
                collectible.Draw(spriteBatch, defaultTexture);
            }
        }

        public Collectible CheckCollisions(Rectangle snakeBounds)
        {
            foreach (var collectible in _collectibles)
            {
                if (collectible.Intersects(snakeBounds))
                {
                    collectible.Collect();
                    return collectible;
                }
            }
            return null;
        }

        private void SpawnCollectible()
        {
            // Calculate random position ensuring the collectible is fully within bounds
            int x = _random.Next(
                _spawnBounds.X + _collectibleSize, 
                _spawnBounds.Right - _collectibleSize
            );
            int y = _random.Next(
                _spawnBounds.Y + _collectibleSize, 
                _spawnBounds.Bottom - _collectibleSize
            );
            
            Vector2 position = new Vector2(x, y);
            
            int spawnRoll = _random.Next(100);
            Collectible collectible;
            
            if (spawnRoll < COFFEE_SPAWN_CHANCE)
            {
                collectible = new CoffeeCollectible(
                    position, 
                    _collectibleSize, 
                    COFFEE_POINTS,
                    COFFEE_SPEED_BOOST
                );
            }
            else if (spawnRoll < COFFEE_SPAWN_CHANCE + STAPLER_SPAWN_CHANCE)
            {
                collectible = new OfficeSupplyCollectible(
                    position,
                    _collectibleSize,
                    STAPLER_POINTS,
                    OfficeSupplyCollectible.OfficeSupplyType.Stapler
                );
            }
            else if (spawnRoll < COFFEE_SPAWN_CHANCE + STAPLER_SPAWN_CHANCE + PAPERCLIP_SPAWN_CHANCE)
            {
                collectible = new OfficeSupplyCollectible(
                    position,
                    _collectibleSize,
                    PAPERCLIP_POINTS,
                    OfficeSupplyCollectible.OfficeSupplyType.Paperclip
                );
            }
            else if (spawnRoll < COFFEE_SPAWN_CHANCE + STAPLER_SPAWN_CHANCE + PAPERCLIP_SPAWN_CHANCE + PUSHPIN_SPAWN_CHANCE)
            {
                collectible = new OfficeSupplyCollectible(
                    position,
                    _collectibleSize,
                    PUSHPIN_POINTS,
                    OfficeSupplyCollectible.OfficeSupplyType.PushPin
                );
            }
            else
            {
                collectible = new OfficeSupplyCollectible(
                    position,
                    _collectibleSize,
                    RUBBERBAND_POINTS,
                    OfficeSupplyCollectible.OfficeSupplyType.RubberBand
                );
            }
            
            if (!CheckOverlap(collectible))
            {
                _collectibles.Add(collectible);
            }
        }

        private bool CheckOverlap(Collectible newCollectible)
        {
            foreach (var existingCollectible in _collectibles)
            {
                if (newCollectible.Bounds.Intersects(existingCollectible.Bounds))
                {
                    return true;
                }
            }
            return false;
        }

        public void Clear()
        {
            _collectibles.Clear();
            _spawnTimer = 0;
        }
    }
}