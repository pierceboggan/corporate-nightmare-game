using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MyGame
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private readonly Rectangle _playerRectangle;
        private Vector2 _playerPosition;
        private readonly float _playerSpeed = 5f;
        private Texture2D _pixel;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _playerRectangle = new Rectangle(0, 0, 32, 32);
        }

        protected override void Initialize()
        {
            _playerPosition = new Vector2(100, 100);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // Create the pixel texture
            _pixel = new Texture2D(GraphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Left))
                _playerPosition.X -= _playerSpeed;
            if (keyboardState.IsKeyDown(Keys.Right))
                _playerPosition.X += _playerSpeed;
            if (keyboardState.IsKeyDown(Keys.Up))
                _playerPosition.Y -= _playerSpeed;
            if (keyboardState.IsKeyDown(Keys.Down))
                _playerPosition.Y += _playerSpeed;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            // Draw player as a white rectangle
            _spriteBatch.Draw(_pixel, new Rectangle((int)_playerPosition.X, 
                (int)_playerPosition.Y, 32, 32), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
