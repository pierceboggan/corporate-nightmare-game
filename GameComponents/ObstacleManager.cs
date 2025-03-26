using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CorporateNightmare.Entities;

namespace CorporateNightmare.GameComponents
{
    /// <summary>
    /// Manages all corporate obstacles and power-ups in the game.
    /// Handles spawning, updating, and collision detection.
    /// </summary>
    public class ObstacleManager
    {
        // Collections
        private readonly List<Obstacle> _obstacles;
        private readonly List<PowerUp> _powerUps;
        
        // Spawn settings
        private readonly Random _random;
        private readonly Rectangle _bounds;
        private float _obstacleSpawnTimer;
        private float _powerUpSpawnTimer;
        private readonly float _obstacleSpawnInterval;
        private readonly float _powerUpSpawnInterval;
        private readonly int _obstacleSize;
        
        // Game state
        private bool _isPlayerInvincible;
        private float _timeScale;
        
        private Dictionary<string, Texture2D> _textures;
        
        /// <summary>
        /// Gets whether the player is currently invincible
        /// </summary>
        public bool IsPlayerInvincible => _isPlayerInvincible;
        
        /// <summary>
        /// Gets the current time scale (for slowing down time)
        /// </summary>
        public float TimeScale => _timeScale;
        
        /// <summary>
        /// Creates a new ObstacleManager
        /// </summary>
        /// <param name="bounds">Rectangle defining where obstacles can spawn</param>
        /// <param name="obstacleSpawnInterval">Time between obstacle spawns in seconds</param>
        /// <param name="powerUpSpawnInterval">Time between power-up spawns in seconds</param>
        public ObstacleManager(Rectangle bounds, float obstacleSpawnInterval = 5.0f, float powerUpSpawnInterval = 15.0f)
        {
            _obstacles = new List<Obstacle>();
            _powerUps = new List<PowerUp>();
            _random = new Random();
            _bounds = bounds;
            _obstacleSpawnInterval = obstacleSpawnInterval;
            _powerUpSpawnInterval = powerUpSpawnInterval;
            _obstacleSize = 20; // Default size for obstacles
            
            _obstacleSpawnTimer = 0;
            _powerUpSpawnTimer = 0;
            _isPlayerInvincible = false;
            _timeScale = 1.0f;
        }
        
        /// <summary>
        /// Updates all obstacles and power-ups
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        /// <param name="snakePosition">Current position of the snake's head</param>
        public void Update(GameTime gameTime, Vector2 snakePosition)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds * _timeScale;
            
            _obstacleSpawnTimer += deltaTime;
            _powerUpSpawnTimer += deltaTime;
            
            if (_obstacleSpawnTimer >= _obstacleSpawnInterval)
            {
                SpawnObstacle();
                _obstacleSpawnTimer = 0;
            }
            
            if (_powerUpSpawnTimer >= _powerUpSpawnInterval)
            {
                SpawnPowerUp();
                _powerUpSpawnTimer = 0;
            }
            
            // Update obstacles
            foreach (var obstacle in _obstacles.ToArray())
            {
                if (obstacle is OKRObstacle okr)
                {
                    okr.UpdateTarget(snakePosition);
                }
                obstacle.Update(gameTime);
            }
            
            // Update power-ups
            foreach (var powerUp in _powerUps.ToArray())
            {
                powerUp.Update(gameTime);
                
                // Handle power-up expiration
                if (!powerUp.IsActive)
                {
                    if (powerUp is WorkFromHomePowerUp)
                    {
                        _isPlayerInvincible = false;
                    }
                    else if (powerUp is CorporateRetreatPowerUp)
                    {
                        _timeScale = 1.0f;
                    }
                }
            }
            
            // Clean up inactive entities
            _obstacles.RemoveAll(o => !o.IsActive);
            _powerUps.RemoveAll(p => !p.IsActive);
        }
        
        public void SetTextures(Dictionary<string, Texture2D> textures)
        {
            _textures = textures;
        }
        
        /// <summary>
        /// Draws all obstacles and power-ups
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to use for drawing</param>
        /// <param name="defaultTexture">Texture to use for rendering</param>
        public void Draw(SpriteBatch spriteBatch, Texture2D defaultTexture)
        {
            foreach (var obstacle in _obstacles)
            {
                obstacle.Draw(spriteBatch, defaultTexture);
            }
            
            foreach (var powerUp in _powerUps)
            {
                powerUp.Draw(spriteBatch, defaultTexture);
            }
        }
        
        /// <summary>
        /// Checks for collisions with obstacles
        /// </summary>
        /// <param name="bounds">Rectangle to check for collisions</param>
        /// <returns>The obstacle that was hit, or null if no collision</returns>
        public Obstacle CheckObstacleCollision(Rectangle bounds)
        {
            if (_isPlayerInvincible) return null;
            
            foreach (var obstacle in _obstacles)
            {
                if (obstacle.Intersects(bounds))
                {
                    return obstacle;
                }
            }
            return null;
        }
        
        /// <summary>
        /// Checks for collisions with power-ups
        /// </summary>
        /// <param name="bounds">Rectangle to check for collisions</param>
        /// <returns>The power-up that was collected, or null if none</returns>
        public PowerUp CheckPowerUpCollision(Rectangle bounds)
        {
            foreach (var powerUp in _powerUps)
            {
                if (powerUp.Intersects(bounds))
                {
                    powerUp.Collect();
                    
                    // Apply power-up effects
                    if (powerUp is WorkFromHomePowerUp)
                    {
                        _isPlayerInvincible = true;
                    }
                    else if (powerUp is TeamCollaborationPowerUp collaboration)
                    {
                        ClearNearbyObstacles(bounds.Center.ToVector2(), collaboration.ClearRadius);
                    }
                    else if (powerUp is CorporateRetreatPowerUp retreat)
                    {
                        _timeScale = retreat.TimeScale;
                    }
                    
                    return powerUp;
                }
            }
            return null;
        }
        
        /// <summary>
        /// Spawns a random obstacle type
        /// </summary>
        private void SpawnObstacle()
        {
            int x = _random.Next(_bounds.X + _obstacleSize, _bounds.Right - _obstacleSize);
            int y = _random.Next(_bounds.Y + _obstacleSize, _bounds.Bottom - _obstacleSize);
            Vector2 position = new Vector2(x, y);

            Obstacle obstacle;
            int type = _random.Next(3);
            
            switch (type)
            {
                case 0:
                    obstacle = new LTReviewObstacle(position, _obstacleSize);
                    break;
                case 1:
                    obstacle = new MeetingObstacle(position, _obstacleSize);
                    break;
                default:
                    obstacle = new OKRObstacle(position, _obstacleSize);
                    break;
            }

            if (!CheckOverlapWithObstacles(obstacle))
            {
                _obstacles.Add(obstacle);
            }
        }
        
        /// <summary>
        /// Spawns a random power-up type
        /// </summary>
        private void SpawnPowerUp()
        {
            int x = _random.Next(_bounds.X + _obstacleSize, _bounds.Right - _obstacleSize);
            int y = _random.Next(_bounds.Y + _obstacleSize, _bounds.Bottom - _obstacleSize);
            Vector2 position = new Vector2(x, y);

            PowerUp powerUp;
            int type = _random.Next(3);
            float duration = 5.0f; // Default duration

            switch (type)
            {
                case 0:
                    powerUp = new WorkFromHomePowerUp(position, _obstacleSize, duration);
                    break;
                case 1:
                    powerUp = new TeamCollaborationPowerUp(position, _obstacleSize, duration);
                    break;
                default:
                    powerUp = new CorporateRetreatPowerUp(position, _obstacleSize, duration);
                    break;
            }

            if (!CheckOverlapWithPowerUps(powerUp))
            {
                _powerUps.Add(powerUp);
            }
        }
        
        /// <summary>
        /// Gets a random position within spawn bounds
        /// </summary>
        private Vector2 GetRandomSpawnPosition()
        {
            return new Vector2(
                _random.Next(_bounds.Left + 50, _bounds.Right - 50),
                _random.Next(_bounds.Top + 50, _bounds.Bottom - 50)
            );
        }
        
        /// <summary>
        /// Checks if an obstacle overlaps with existing obstacles
        /// </summary>
        private bool CheckOverlapWithObstacles(Obstacle newObstacle)
        {
            foreach (var obstacle in _obstacles)
            {
                if (obstacle.Bounds.Intersects(newObstacle.Bounds))
                {
                    return true;
                }
            }
            return false;
        }
        
        /// <summary>
        /// Checks if a power-up overlaps with existing power-ups
        /// </summary>
        private bool CheckOverlapWithPowerUps(PowerUp newPowerUp)
        {
            foreach (var powerUp in _powerUps)
            {
                if (powerUp.Bounds.Intersects(newPowerUp.Bounds))
                {
                    return true;
                }
            }
            return false;
        }
        
        /// <summary>
        /// Clears obstacles within a specified radius
        /// </summary>
        private void ClearNearbyObstacles(Vector2 center, float radius)
        {
            float radiusSquared = radius * radius;
            foreach (var obstacle in _obstacles.ToArray())
            {
                Vector2 obstacleCenter = obstacle.Bounds.Center.ToVector2();
                if (Vector2.DistanceSquared(center, obstacleCenter) <= radiusSquared)
                {
                    obstacle.Deactivate();
                }
            }
        }
        
        /// <summary>
        /// Clears all obstacles and power-ups
        /// </summary>
        public void Clear()
        {
            _obstacles.Clear();
            _powerUps.Clear();
            _obstacleSpawnTimer = 0;
            _powerUpSpawnTimer = 0;
            _isPlayerInvincible = false;
            _timeScale = 1.0f;
        }
    }
}