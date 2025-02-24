using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


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

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
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
            GameManager.eventManager.ExecuteCommand("ShowStartMenu", new Dictionary<string, object>
            {
                { "gameManager", GameManager },
                { "content", Content },
                { "game", this }
            });
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
            GraphicsDevice.Clear(Background);
            _spriteBatch.Begin();
            GameManager.Draw(_spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
