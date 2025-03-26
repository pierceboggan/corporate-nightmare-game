using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CorporateNightmare.Entities;

namespace CorporateNightmare.GameComponents
{
    /// <summary>
    /// Manages the spawning, updating, and collection of collectible items in the game.
    /// Handles random placement and ensures collectibles don't overlap.
    /// </summary>
    public class CollectibleManager
    {
        // Collection of active collectibles
        private readonly List<Collectible> _collectibles;
        
        // Spawn settings
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
        
        /// <summary>
        /// Creates a new CollectibleManager
        /// </summary>
        /// <param name="spawnBounds">Rectangle defining where collectibles can spawn</param>
        /// <param name="spawnInterval">Time between spawns in seconds</param>
        /// <param name="collectibleSize">Size of collectibles in pixels</param>
        public CollectibleManager(Rectangle spawnBounds, float spawnInterval, int collectibleSize)
        {
            _collectibles = new List<Collectible>();
            _random = new Random();
            _spawnBounds = spawnBounds;
            _spawnInterval = spawnInterval;
            _collectibleSize = collectibleSize;
            _spawnTimer = 0;
        }
        
        /// <summary>
        /// Updates all collectibles and handles spawning new ones
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        public void Update(GameTime gameTime)
        {
            // Update spawn timer
            _spawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            // Check if it's time to spawn a new collectible
            if (_spawnTimer >= _spawnInterval)
            {
                SpawnCollectible();
                _spawnTimer = 0;
            }
            
            // Update existing collectibles
            foreach (var collectible in _collectibles)
            {
                collectible.Update(gameTime);
            }
            
            // Remove collected items
            _collectibles.RemoveAll(c => c.IsCollected);
        }
        
        /// <summary>
        /// Draws all active collectibles
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to use for drawing</param>
        /// <param name="texture">Texture to use for rendering</param>
        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            foreach (var collectible in _collectibles)
            {
                collectible.Draw(spriteBatch, texture);
            }
        }
        
        /// <summary>
        /// Checks for collision between the snake and any collectibles
        /// </summary>
        /// <param name="snakeBounds">The bounds of the snake's head</param>
        /// <returns>The collected item if there was a collision, null otherwise</returns>
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
        
        /// <summary>
        /// Spawns a new collectible at a random position
        /// </summary>
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
            
            // Determine which type of collectible to spawn based on probability
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
            
            // Add to active collectibles if it doesn't overlap with existing ones
            if (!CheckOverlap(collectible))
            {
                _collectibles.Add(collectible);
            }
        }
        
        /// <summary>
        /// Checks if a collectible overlaps with any existing collectibles
        /// </summary>
        /// <param name="newCollectible">The collectible to check</param>
        /// <returns>True if there is an overlap, false otherwise</returns>
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
        
        /// <summary>
        /// Clears all active collectibles
        /// </summary>
        public void Clear()
        {
            _collectibles.Clear();
            _spawnTimer = 0;
        }
    }
}