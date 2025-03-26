// filepath: /Users/pierce/Documents/GitHub/corporate-nightmare-game/GameComponents/SoundManager.cs
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace CorporateNightmare.GameComponents
{
    /// <summary>
    /// Manages sound effects and music for the game.
    /// This component provides methods for loading, playing, and controlling audio assets,
    /// including background music and sound effects with volume control and state management.
    /// </summary>
    public class SoundManager
    {
        // Dictionaries to store loaded audio assets
        private readonly Dictionary<string, SoundEffect> _soundEffects;
        private readonly Dictionary<string, Song> _songs;
        
        // Audio playback settings
        private float _soundEffectVolume;
        private float _musicVolume;
        private bool _soundEffectsEnabled;
        private bool _musicEnabled;
        
        // Currently playing song
        private string _currentSongName;
        
        // Constants
        private const float DEFAULT_VOLUME = 0.5f;
        
        // Properties
        public float SoundEffectVolume 
        {
            get => _soundEffectVolume;
            set => _soundEffectVolume = MathHelper.Clamp(value, 0f, 1f);
        }
        
        public float MusicVolume 
        {
            get => _musicVolume;
            set 
            {
                _musicVolume = MathHelper.Clamp(value, 0f, 1f);
                MediaPlayer.Volume = _musicVolume;
            }
        }
        
        public bool SoundEffectsEnabled
        {
            get => _soundEffectsEnabled;
            set => _soundEffectsEnabled = value;
        }
        
        public bool MusicEnabled
        {
            get => _musicEnabled;
            set 
            {
                _musicEnabled = value;
                if (_musicEnabled)
                {
                    if (!string.IsNullOrEmpty(_currentSongName))
                    {
                        PlaySong(_currentSongName, true);
                    }
                }
                else
                {
                    MediaPlayer.Stop();
                }
            }
        }
        
        public string CurrentSongName => _currentSongName;

        /// <summary>
        /// Initializes a new instance of the SoundManager class.
        /// </summary>
        public SoundManager()
        {
            _soundEffects = new Dictionary<string, SoundEffect>();
            _songs = new Dictionary<string, Song>();
            _soundEffectVolume = DEFAULT_VOLUME;
            _musicVolume = DEFAULT_VOLUME;
            _soundEffectsEnabled = true;
            _musicEnabled = true;
            _currentSongName = string.Empty;
            
            // Set up MediaPlayer event handlers
            MediaPlayer.Volume = _musicVolume;
            MediaPlayer.MediaStateChanged += OnMediaStateChanged;
        }

        /// <summary>
        /// Loads a sound effect from the content manager.
        /// </summary>
        /// <param name="game">Reference to the main game</param>
        /// <param name="assetName">Name of the asset to load</param>
        /// <param name="soundName">Name to use for referencing the sound effect (defaults to assetName)</param>
        public void LoadSoundEffect(Game game, string assetName, string soundName = null)
        {
            // Default the sound name to the asset name if not provided
            soundName = soundName ?? assetName;
            
            // Load the sound effect if it hasn't been loaded already
            if (!_soundEffects.ContainsKey(soundName))
            {
                try
                {
                    var soundEffect = game.Content.Load<SoundEffect>(assetName);
                    _soundEffects[soundName] = soundEffect;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to load sound effect {assetName}: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Loads a song from the content manager.
        /// </summary>
        /// <param name="game">Reference to the main game</param>
        /// <param name="assetName">Name of the asset to load</param>
        /// <param name="songName">Name to use for referencing the song (defaults to assetName)</param>
        public void LoadSong(Game game, string assetName, string songName = null)
        {
            // Default the song name to the asset name if not provided
            songName = songName ?? assetName;
            
            // Load the song if it hasn't been loaded already
            if (!_songs.ContainsKey(songName))
            {
                try
                {
                    var song = game.Content.Load<Song>(assetName);
                    _songs[songName] = song;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to load song {assetName}: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Plays a sound effect.
        /// </summary>
        /// <param name="soundName">Name of the sound effect to play</param>
        /// <param name="pitch">Pitch adjustment (-1.0f to 1.0f)</param>
        /// <param name="pan">Pan adjustment (-1.0f to 1.0f)</param>
        /// <returns>SoundEffectInstance if successful, null otherwise</returns>
        public SoundEffectInstance PlaySound(string soundName, float pitch = 0f, float pan = 0f)
        {
            if (!_soundEffectsEnabled || !_soundEffects.ContainsKey(soundName))
            {
                return null;
            }
            
            var sound = _soundEffects[soundName];
            var instance = sound.CreateInstance();
            
            instance.Volume = _soundEffectVolume;
            instance.Pitch = MathHelper.Clamp(pitch, -1f, 1f);
            instance.Pan = MathHelper.Clamp(pan, -1f, 1f);
            instance.Play();
            
            return instance;
        }

        /// <summary>
        /// Plays a song.
        /// </summary>
        /// <param name="songName">Name of the song to play</param>
        /// <param name="isRepeating">Whether the song should repeat</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool PlaySong(string songName, bool isRepeating = true)
        {
            if (!_musicEnabled || !_songs.ContainsKey(songName))
            {
                return false;
            }
            
            try
            {
                var song = _songs[songName];
                MediaPlayer.Stop();
                MediaPlayer.Volume = _musicVolume;
                MediaPlayer.IsRepeating = isRepeating;
                MediaPlayer.Play(song);
                _currentSongName = songName;
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to play song {songName}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Stops the currently playing song.
        /// </summary>
        public void StopMusic()
        {
            MediaPlayer.Stop();
        }

        /// <summary>
        /// Pauses the currently playing song.
        /// </summary>
        public void PauseMusic()
        {
            if (MediaPlayer.State == MediaState.Playing)
            {
                MediaPlayer.Pause();
            }
        }

        /// <summary>
        /// Resumes a paused song.
        /// </summary>
        public void ResumeMusic()
        {
            if (MediaPlayer.State == MediaState.Paused && _musicEnabled)
            {
                MediaPlayer.Resume();
            }
        }

        /// <summary>
        /// Event handler for MediaPlayer state changes.
        /// </summary>
        private void OnMediaStateChanged(object sender, EventArgs e)
        {
            // Handle media state changes if needed
            // For example, automatically play the next song in a playlist
        }

        /// <summary>
        /// Unloads all audio content.
        /// </summary>
        public void Unload()
        {
            // Stop all audio
            StopMusic();
            
            // Dispose all sound effects
            foreach (var sound in _soundEffects.Values)
            {
                sound.Dispose();
            }
            _soundEffects.Clear();
            
            // The Song objects will be collected by the garbage collector
            _songs.Clear();
        }
    }
}