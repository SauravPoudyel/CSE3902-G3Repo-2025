using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sprint0
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Color Background;
        public GameManager GameManager;
        private IController keyboardController;
        private IController mouseController;
        private Texture2D cursorTexture;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
            Background = new Color(116, 116, 200);
            _graphics.PreferredBackBufferWidth = Globals.SCREENWIDTH;
            _graphics.PreferredBackBufferHeight = Globals.SCREENHEIGHT;
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            GameManager = new GameManager(this);

            keyboardController = new KeyboardController();
            mouseController = new MouseController();
            cursorTexture = Content.Load<Texture2D>("crosshairs_red");

            base.Initialize();
        }

        protected override void LoadContent()
        {
            GameManager.LoadContent(Content);
        }

        public void ResetGame()
        {
            GameManager = new GameManager(this);
            GameManager.LoadContent(Content);
        }
        
        protected override void Update(GameTime gameTime)
        {
            keyboardController.Update(this);
            mouseController.Update(this);
            GameManager.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {   
            MouseState mouseState = Mouse.GetState();
            Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);

        
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

            GameManager.Draw(_spriteBatch);
            _spriteBatch.Draw(cursorTexture, mousePosition, null, Color.White, 0f, new Vector2(cursorTexture.Width / 2, cursorTexture.Height / 2), 0.1f, SpriteEffects.None, 1f);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
