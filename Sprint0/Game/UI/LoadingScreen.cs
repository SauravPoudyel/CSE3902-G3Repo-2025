using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Threading;
using static Sprint0.GameCommands;

namespace Sprint0
{
    public class LoadingScreen : IScreen
    {
        private Texture2D backgroundTexture;
        private Color overlayColor;
        private ContentManager content;
        private GraphicsDevice graphicsDevice;
        private GameManager gameManager;
        private Texture2D loadingBarTexture;
        private int loadTimeCounter;
        public bool BlocksInput => true;

        public LoadingScreen(Game1 game)
        {
            gameManager = game.GameManager;
            graphicsDevice = game.GraphicsDevice;
            this.content = game.Content;
            overlayColor = new Color(0, 0, 0, 180);
            backgroundTexture = content.Load<Texture2D>("UI/StartMenu");
            loadingBarTexture = new Texture2D(graphicsDevice, 1, 1);
            loadingBarTexture.SetData(new Color[] { Color.Blue });
            loadTimeCounter = 0;
        }

        public void Update()
        {
            loadTimeCounter++;
            if(loadTimeCounter>300) {
                StartGameCommand startGameCommand = new StartGameCommand();
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "gameManager", gameManager }
                };
                startGameCommand.Execute(parameters);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, Globals.SCREENWIDTH, Globals.SCREENHEIGHT), Color.White);
            spriteBatch.Draw(loadingBarTexture, new Rectangle(Globals.SCREENWIDTH / 2 - 300, Globals.SCREENHEIGHT / 2 - 20, 2*loadTimeCounter, 80), Color.White);
        }
    }
}
