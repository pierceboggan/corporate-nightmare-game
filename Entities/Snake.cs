using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CorporateNightmare.GameComponents;

namespace CorporateNightmare.Entities
{
    /// <summary>
    /// Main player-controlled snake entity for the Corporate Nightmare game.
    /// The snake consists of multiple segments, moves around the game area,
    /// grows when collecting items, and dies when colliding with boundaries or itself.
    /// </summary>
    public class Snake
    {
        // Movement and positioning
        private Vector2 _direction;
        private Vector2 _pendingDirection;
        private float _moveSpeed;
        private float _moveTimer;
        private float _moveInterval;
        
        // Snake properties
        private readonly List<SnakeSegment> _segments;
        private readonly int _segmentSize;
        private readonly Color _headColor;
        private readonly Color _bodyColor;
        
        // Game area boundaries
        private readonly Rectangle _gameBounds;
        
        // Snake state
        private bool _isAlive;
        private bool _hasGrown;
        private CollisionType _lastCollisionType;
        
        // Textures
        private Texture2D _headTexture;
        private Texture2D _bodyTexture;
        
        // Properties with public getters
        public bool IsAlive => _isAlive;
        public int Length => _segments.Count;
        public Vector2 HeadPosition => _segments[0].Position;
        public Rectangle HeadBounds => _segments[0].Bounds;
        public IReadOnlyList<SnakeSegment> Segments => _segments.AsReadOnly();
        public CollisionType LastCollisionType => _lastCollisionType;

        public enum CollisionType
        {
            None,
            Boundary,
            Self
        }
        
        /// <summary>
        /// Creates a new snake at the specified position.
        /// </summary>
        /// <param name="startPosition">The starting position of the snake's head</param>
        /// <param name="segmentSize">Size of each segment in pixels</param>
        /// <param name="initialLength">Initial number of segments</param>
        /// <param name="moveSpeed">Movement speed (lower is faster)</param>
        /// <param name="gameBounds">Rectangle defining the game area boundaries</param>
        public Snake(Vector2 startPosition, int segmentSize, int initialLength, float moveSpeed, Rectangle gameBounds)
        {
            _segmentSize = segmentSize;
            _moveSpeed = moveSpeed;
            _moveInterval = 1.0f / _moveSpeed;
            _moveTimer = 0;
            _gameBounds = gameBounds;
            
            // Start with a rightward direction
            _direction = new Vector2(1, 0);
            _pendingDirection = _direction;
            
            // Colors for the snake
            _headColor = new Color(0, 100, 180); // Corporate blue for head
            _bodyColor = new Color(30, 70, 140); // Darker blue for body
            
            // Initialize segments
            _segments = new List<SnakeSegment>();
            
            // Create head
            _segments.Add(new SnakeSegment(startPosition, _direction, _segmentSize, _headColor));
            
            // Create body segments behind the head
            for (int i = 1; i < initialLength; i++)
            {
                Vector2 position = new Vector2(
                    startPosition.X - (i * _segmentSize), 
                    startPosition.Y
                );
                _segments.Add(new SnakeSegment(position, _direction, _segmentSize, _bodyColor));
            }
            
            _isAlive = true;
            _hasGrown = false;
        }
        
        /// <summary>
        /// Updates the snake's position and checks for collisions.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        public void Update(GameTime gameTime)
        {
            if (!_isAlive) return;
            
            // Add elapsed time to the move timer
            _moveTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            // Check if it's time to move
            if (_moveTimer >= _moveInterval)
            {
                _moveTimer = 0; // Reset the timer
                
                // Update direction based on pending direction
                UpdateDirection();
                
                // Move the snake
                Move();
                
                // Check for collisions
                CheckCollisions();
            }
        }
        
        /// <summary>
        /// Sets the snake's textures
        /// </summary>
        public void SetTextures(Texture2D headTexture, Texture2D bodyTexture)
        {
            _headTexture = headTexture;
            _bodyTexture = bodyTexture;
        }
        
        /// <summary>
        /// Draws the snake on the screen.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to use for drawing</param>
        /// <param name="defaultTexture">Texture to use for segments</param>
        public void Draw(SpriteBatch spriteBatch, Texture2D defaultTexture)
        {
            for (int i = 0; i < _segments.Count; i++)
            {
                var segment = _segments[i];
                var texture = i == 0 ? _headTexture : _bodyTexture;
                
                // If no texture is set, fall back to colored rectangle
                if (texture == null)
                {
                    segment.Draw(spriteBatch, defaultTexture);
                }
                else
                {
                    float rotation = 0f;
                    if (i == 0) // Rotate head based on direction
                    {
                        rotation = (float)Math.Atan2(_direction.Y, _direction.X);
                    }
                    
                    spriteBatch.Draw(
                        texture,
                        segment.Bounds,
                        null,
                        segment.Color,
                        rotation,
                        new Vector2(texture.Width / 2, texture.Height / 2),
                        SpriteEffects.None,
                        0
                    );
                }
            }
        }
        
        /// <summary>
        /// Sets the direction the snake will move in.
        /// Cannot reverse direction (e.g., if moving right, cannot immediately move left).
        /// </summary>
        /// <param name="newDirection">The new direction vector</param>
        public void SetDirection(Vector2 newDirection)
        {
            // Ensure the direction is normalized
            if (newDirection != Vector2.Zero)
            {
                newDirection.Normalize();
            }
            else
            {
                // If zero vector provided, don't change direction
                return;
            }
            
            // Prevent reversing direction (can't go directly opposite current direction)
            if (newDirection == -_direction)
            {
                return;
            }
            
            // Store the requested direction - will be applied on next move
            _pendingDirection = newDirection;
        }
        
        /// <summary>
        /// Makes the snake grow by adding a new segment.
        /// </summary>
        public void Grow()
        {
            // Mark that the snake should grow on the next move
            _hasGrown = true;
        }
        
        /// <summary>
        /// Kills the snake, setting its alive status to false.
        /// </summary>
        public void Die()
        {
            _isAlive = false;
        }
        
        /// <summary>
        /// Checks if the snake's head intersects with a specified rectangle.
        /// </summary>
        /// <param name="rectangle">Rectangle to check for intersection</param>
        /// <returns>True if there is an intersection, false otherwise</returns>
        public bool HeadIntersects(Rectangle rectangle)
        {
            return HeadBounds.Intersects(rectangle);
        }
        
        /// <summary>
        /// Gets a segment at the specified index.
        /// </summary>
        /// <param name="index">Index of the segment to retrieve</param>
        /// <returns>The snake segment at the specified index</returns>
        public SnakeSegment GetSegment(int index)
        {
            if (index >= 0 && index < _segments.Count)
            {
                return _segments[index];
            }
            
            throw new ArgumentOutOfRangeException(nameof(index), "Segment index out of range");
        }
        
        /// <summary>
        /// Updates the snake's movement direction.
        /// </summary>
        private void UpdateDirection()
        {
            _direction = _pendingDirection;
        }
        
        /// <summary>
        /// Moves the snake by updating each segment's position.
        /// </summary>
        private void Move()
        {
            // Store the current head position and direction
            Vector2 prevPosition = _segments[0].Position;
            Vector2 prevDirection = _segments[0].Direction;
            
            // Move the head in the current direction
            Vector2 newHeadPosition = new Vector2(
                prevPosition.X + (_direction.X * _segmentSize),
                prevPosition.Y + (_direction.Y * _segmentSize)
            );
            
            // Update head position and direction
            _segments[0].Update(newHeadPosition, _direction);
            
            // Update body segments
            for (int i = 1; i < _segments.Count; i++)
            {
                // Save current position and direction before updating
                Vector2 currentPos = _segments[i].Position;
                Vector2 currentDir = _segments[i].Direction;
                
                // Update this segment to the previous segment's old position
                _segments[i].Update(prevPosition, prevDirection);
                
                // Save this segment's old position and direction for the next segment
                prevPosition = currentPos;
                prevDirection = currentDir;
            }
            
            // Add a new segment at the end if the snake has grown
            if (_hasGrown)
            {
                _segments.Add(new SnakeSegment(prevPosition, prevDirection, _segmentSize, _bodyColor));
                _hasGrown = false;
            }
        }
        
        /// <summary>
        /// Checks for collisions with boundaries and self.
        /// </summary>
        private void CheckCollisions()
        {
            // Check boundary collision
            if (!_gameBounds.Contains(HeadBounds))
            {
                _lastCollisionType = CollisionType.Boundary;
                Die();
                return;
            }
            
            // Check self-collision (skip the first few segments to prevent false positives)
            for (int i = 4; i < _segments.Count; i++)
            {
                if (HeadIntersects(_segments[i].Bounds))
                {
                    _lastCollisionType = CollisionType.Self;
                    // Turn the collided segment red for visual feedback
                    _segments[i].SetColor(Color.Red);
                    // Turn the head red too
                    _segments[0].SetColor(Color.Red);
                    Die();
                    return;
                }
            }
        }
        
        /// <summary>
        /// Increases the snake's movement speed.
        /// </summary>
        /// <param name="speedIncrease">Amount to increase the speed by</param>
        public void IncreaseSpeed(float speedIncrease)
        {
            _moveSpeed += speedIncrease;
            _moveInterval = 1.0f / _moveSpeed;
        }

        /// <summary>
        /// Reduces the snake's movement speed
        /// </summary>
        /// <param name="speedPenalty">Amount to reduce speed by</param>
        public void ReduceSpeed(float speedPenalty)
        {
            _moveSpeed = MathHelper.Max(_moveSpeed - speedPenalty, 1.0f);
            _moveInterval = 1.0f / _moveSpeed;
        }
    }
}