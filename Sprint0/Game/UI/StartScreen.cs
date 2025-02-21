using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Sprint0
{
    public class StartMenuScreen : IScreen
    {
        private List<Button> buttons;
        private Texture2D backgroundTexture;
        private Color overlayColor;
        private bool drawn; 
        private ContentManager content;
        private GraphicsDevice graphicsDevice;

        public StartMenuScreen(ContentManager content, GraphicsDevice graphicsDevice, Game1 game)
        {
            this.content = content;
            this.graphicsDevice = graphicsDevice;
            buttons = new List<Button>();
            overlayColor = new Color(0, 0, 0, 180);
            backgroundTexture = new Texture2D(graphicsDevice, 1, 1);
            backgroundTexture.SetData(new Color[] { Color.DimGray });

            SpriteFont font = content.Load<SpriteFont>("Arial");
            Texture2D buttonTexture = new Texture2D(graphicsDevice, 1, 1);
            buttonTexture.SetData(new Color[] { Color.Gray });

            Dictionary<string, object> restartParams = new Dictionary<string, object>();
            restartParams.Add("gameManager", game.GameManager);
            restartParams.Add("game", game);
            Button restartButton = new Button(buttonTexture, font,
                new Rectangle(Globals.SCREENWIDTH / 2 - 75, Globals.SCREENHEIGHT / 2 - 80, 150, 40),
                "Restart", new GameCommands.ResetCommand(game), restartParams);

            Dictionary<string, object> quitParams = new Dictionary<string, object>();
            quitParams.Add("gameManager", game.GameManager);
            quitParams.Add("game", game);
            Button quitButton = new Button(buttonTexture, font,
                new Rectangle(Globals.SCREENWIDTH / 2 - 75, Globals.SCREENHEIGHT / 2 - 20, 150, 40),
                "Quit", new GameCommands.QuitCommand(game), quitParams);

            Dictionary<string, object> startParams = new Dictionary<string, object>();
            startParams.Add("gameManager", game.GameManager);
            startParams.Add("screen", this);
            Button startButton = new Button(buttonTexture, font,
                new Rectangle(Globals.SCREENWIDTH / 2 - 75, Globals.SCREENHEIGHT / 2 + 40, 150, 40),
                "Start", new GameCommands.StartGameCommand(), startParams);

            buttons.Add(restartButton);
            buttons.Add(quitButton);
            buttons.Add(startButton);
        }

        public void Update()
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, Globals.SCREENWIDTH, Globals.SCREENHEIGHT), overlayColor);
            drawn = true; 

            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].Draw(spriteBatch);
            }
        }

        public void HandleClick(Point clickLocation)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                if (buttons[i].ContainsPoint(clickLocation))
                {
                    buttons[i].Click();
                }
            }
        }
    }
}
