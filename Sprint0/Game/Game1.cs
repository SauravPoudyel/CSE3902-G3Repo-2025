using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

/**
 * Author: Saurav Poudyel
 * Date: 1/22/2025 
 * Class: CSE 3902
*/

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
            Background = new Color(116, 116, 116);
            _graphics.PreferredBackBufferWidth = Globals.SCREENWIDTH;
            _graphics.PreferredBackBufferHeight = Globals.SCREENHEIGHT;
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            GameManager = new GameManager(this);
            keyboardController = new KeyboardController();
            mouseController = new MouseController();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            GameManager.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            keyboardController.Update(this);
            mouseController.Update(this);
            GameManager.Update(gameTime);
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
