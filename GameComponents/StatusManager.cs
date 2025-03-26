using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CorporateNightmare.GameComponents
{
    /// <summary>
    /// Manages and displays the current status effects (power-ups, penalties)
    /// affecting the player during gameplay.
    /// </summary>
    public class StatusManager
    {
        private class StatusEffect
        {
            public string Name { get; }
            public float RemainingDuration { get; set; }
            public Color Color { get; }
            
            public StatusEffect(string name, float duration, Color color)
            {
                Name = name;
                RemainingDuration = duration;
                Color = color;
            }
        }
        
        private readonly List<StatusEffect> _activeEffects;
        private readonly Vector2 _displayPosition;
        private const float STATUS_LINE_HEIGHT = 25f;
        
        /// <summary>
        /// Creates a new StatusManager
        /// </summary>
        /// <param name="displayPosition">Screen position to display status effects</param>
        public StatusManager(Vector2 displayPosition)
        {
            _activeEffects = new List<StatusEffect>();
            _displayPosition = displayPosition;
        }
        
        /// <summary>
        /// Updates all active status effects and removes expired ones
        /// </summary>
        /// <param name="gameTime">Current game time</param>
        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            // Update durations and remove expired effects
            for (int i = _activeEffects.Count - 1; i >= 0; i--)
            {
                _activeEffects[i].RemainingDuration -= deltaTime;
                if (_activeEffects[i].RemainingDuration <= 0)
                {
                    _activeEffects.RemoveAt(i);
                }
            }
        }
        
        /// <summary>
        /// Draws all active status effects
        /// </summary>
        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            if (_activeEffects.Count == 0) return;
            
            // Draw status effect header
            spriteBatch.DrawString(font, "Active Effects:", _displayPosition, Color.White);
            
            // Draw each active effect
            for (int i = 0; i < _activeEffects.Count; i++)
            {
                var effect = _activeEffects[i];
                var position = new Vector2(
                    _displayPosition.X,
                    _displayPosition.Y + ((i + 1) * STATUS_LINE_HEIGHT)
                );
                
                string timeLeft = effect.RemainingDuration.ToString("F1");
                string status = $"{effect.Name} ({timeLeft}s)";
                
                spriteBatch.DrawString(font, status, position, effect.Color);
            }
        }
        
        /// <summary>
        /// Adds or refreshes a status effect
        /// </summary>
        /// <param name="name">Name of the effect</param>
        /// <param name="duration">Duration in seconds</param>
        /// <param name="color">Color to display the effect in</param>
        public void AddEffect(string name, float duration, Color color)
        {
            // Remove existing effect of the same name if it exists
            _activeEffects.RemoveAll(e => e.Name == name);
            
            // Add the new effect
            _activeEffects.Add(new StatusEffect(name, duration, color));
        }
        
        /// <summary>
        /// Removes all active effects
        /// </summary>
        public void Clear()
        {
            _activeEffects.Clear();
        }
        
        /// <summary>
        /// Checks if an effect is currently active
        /// </summary>
        /// <param name="name">Name of the effect to check</param>
        /// <returns>True if the effect is active, false otherwise</returns>
        public bool HasEffect(string name)
        {
            return _activeEffects.Exists(e => e.Name == name);
        }
        
        /// <summary>
        /// Gets the remaining duration of an effect
        /// </summary>
        /// <param name="name">Name of the effect</param>
        /// <returns>Remaining duration in seconds, or 0 if not active</returns>
        public float GetRemainingDuration(string name)
        {
            var effect = _activeEffects.Find(e => e.Name == name);
            return effect?.RemainingDuration ?? 0;
        }
    }
}