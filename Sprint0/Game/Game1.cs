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
        private Texture2D blackTexture;

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

            blackTexture = new Texture2D(GraphicsDevice, 1, 1);
            blackTexture.SetData(new[] { Color.White });

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
            
            ScreenFader.Update(gameTime);
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {   
            GameManager.Draw(_spriteBatch);

            MouseState mouseState = Mouse.GetState();
            Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);

            // Draw cursor and screenfader without the shader
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            
            _spriteBatch.Draw(cursorTexture, mousePosition, null, Color.White, 0f,
                new Vector2(cursorTexture.Width / 2, cursorTexture.Height / 2), 0.1f, SpriteEffects.None, 1f);
            _spriteBatch.Draw(blackTexture, new Rectangle(0, 0, Globals.SCREENWIDTH, Globals.SCREENHEIGHT),
                Color.Black * ScreenFader.FadeAlpha);
                
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
