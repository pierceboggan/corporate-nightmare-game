// filepath: /Users/pierce/Documents/GitHub/corporate-nightmare-game/GameComponents/ScoreManager.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CorporateNightmare.GameComponents
{
    /// <summary>
    /// Manages the player's score during gameplay and handles high score tracking.
    /// This component provides methods for increasing scores, tracking high scores,
    /// and persisting scores between game sessions.
    /// </summary>
    public class ScoreManager
    {
        // Current game score
        private int _currentScore;
        
        // High scores list
        private List<HighScore> _highScores;
        
        // Constants
        private const int MAX_HIGH_SCORES = 10;
        private const string HIGH_SCORES_FILE = "highscores.txt";
        
        // Properties
        public int CurrentScore => _currentScore;
        public IReadOnlyList<HighScore> HighScores => _highScores.AsReadOnly();

        /// <summary>
        /// Initializes a new instance of the ScoreManager class.
        /// </summary>
        public ScoreManager()
        {
            _currentScore = 0;
            _highScores = new List<HighScore>();
            LoadHighScores();
        }

        /// <summary>
        /// Adds points to the current score.
        /// </summary>
        /// <param name="points">The number of points to add</param>
        public void AddScore(int points)
        {
            _currentScore += points;
        }

        /// <summary>
        /// Resets the current score to zero.
        /// </summary>
        public void ResetScore()
        {
            _currentScore = 0;
        }

        /// <summary>
        /// Checks if the current score qualifies for the high scores list.
        /// </summary>
        /// <returns>True if the score qualifies, false otherwise</returns>
        public bool IsHighScore()
        {
            return _highScores.Count < MAX_HIGH_SCORES || _currentScore > _highScores.Min(s => s.Score);
        }

        /// <summary>
        /// Adds the current score to the high scores list if it qualifies.
        /// </summary>
        /// <param name="playerName">The name of the player</param>
        public void AddHighScore(string playerName)
        {
            // Create a new high score entry
            var newScore = new HighScore(playerName, _currentScore, DateTime.Now);
            
            // Add to the list
            _highScores.Add(newScore);
            
            // Sort in descending order
            _highScores = _highScores.OrderByDescending(s => s.Score).ToList();
            
            // Trim the list if it's too long
            if (_highScores.Count > MAX_HIGH_SCORES)
            {
                _highScores = _highScores.Take(MAX_HIGH_SCORES).ToList();
            }
            
            // Save the updated high scores
            SaveHighScores();
        }

        /// <summary>
        /// Draws the current score on the screen.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to use for drawing</param>
        /// <param name="font">SpriteFont to use for rendering text</param>
        /// <param name="position">Position to draw the score</param>
        /// <param name="color">Color to draw the score</param>
        public void DrawScore(SpriteBatch spriteBatch, SpriteFont font, Vector2 position, Color color)
        {
            spriteBatch.DrawString(font, $"Score: {_currentScore}", position, color);
        }

        /// <summary>
        /// Draws the high scores list on the screen.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to use for drawing</param>
        /// <param name="font">SpriteFont to use for rendering text</param>
        /// <param name="position">Position to start drawing the list</param>
        /// <param name="color">Color to draw the text</param>
        /// <param name="lineSpacing">Spacing between lines</param>
        public void DrawHighScores(SpriteBatch spriteBatch, SpriteFont font, Vector2 position, Color color, float lineSpacing = 30)
        {
            spriteBatch.DrawString(font, "High Scores:", position, color);
            
            for (int i = 0; i < _highScores.Count; i++)
            {
                var score = _highScores[i];
                var scoreText = $"{i + 1}. {score.PlayerName}: {score.Score} ({score.Date.ToString("yyyy-MM-dd")})";
                var scorePos = new Vector2(position.X, position.Y + (i + 1) * lineSpacing);
                
                spriteBatch.DrawString(font, scoreText, scorePos, color);
            }
        }

        /// <summary>
        /// Loads high scores from a file.
        /// </summary>
        private void LoadHighScores()
        {
            _highScores.Clear();
            
            try
            {
                if (File.Exists(HIGH_SCORES_FILE))
                {
                    var lines = File.ReadAllLines(HIGH_SCORES_FILE);
                    
                    foreach (var line in lines)
                    {
                        var parts = line.Split('|');
                        
                        if (parts.Length == 3 && 
                            !string.IsNullOrWhiteSpace(parts[0]) && 
                            int.TryParse(parts[1], out int score) && 
                            DateTime.TryParse(parts[2], out DateTime date))
                        {
                            _highScores.Add(new HighScore(parts[0], score, date));
                        }
                    }
                    
                    // Sort in descending order
                    _highScores = _highScores.OrderByDescending(s => s.Score).ToList();
                }
            }
            catch (Exception ex)
            {
                // Handle file loading errors gracefully - just start with an empty list
                System.Diagnostics.Debug.WriteLine($"Error loading high scores: {ex.Message}");
                _highScores.Clear();
            }
        }

        /// <summary>
        /// Saves high scores to a file.
        /// </summary>
        private void SaveHighScores()
        {
            try
            {
                var lines = new List<string>();
                
                foreach (var score in _highScores)
                {
                    lines.Add($"{score.PlayerName}|{score.Score}|{score.Date:yyyy-MM-dd HH:mm:ss}");
                }
                
                File.WriteAllLines(HIGH_SCORES_FILE, lines);
            }
            catch (Exception ex)
            {
                // Handle file saving errors gracefully
                System.Diagnostics.Debug.WriteLine($"Error saving high scores: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Represents a high score entry with player name, score, and date.
    /// </summary>
    public class HighScore
    {
        public string PlayerName { get; }
        public int Score { get; }
        public DateTime Date { get; }

        public HighScore(string playerName, int score, DateTime date)
        {
            PlayerName = playerName;
            Score = score;
            Date = date;
        }
    }
}