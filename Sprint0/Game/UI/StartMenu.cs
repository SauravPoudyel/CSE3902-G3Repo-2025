using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Sprint0
{
    public class StartMenu : IScreen
    {
        private List<Button> buttons;
        private Texture2D backgroundTexture;
        private Color overlayColor;
        private ContentManager content;
        private GraphicsDevice graphicsDevice;

        // StartMenu should block game input.
        public bool BlocksInput => true;

        public StartMenu(ContentManager content, GraphicsDevice graphicsDevice, Game1 game)
        {
            this.content = content;
            this.graphicsDevice = graphicsDevice;
            buttons = new List<Button>();
            overlayColor = new Color(0, 0, 0, 180);
            backgroundTexture = content.Load<Texture2D>("tank_menu");

            SpriteFont font = content.Load<SpriteFont>("Arial");
            Texture2D buttonTexture = new Texture2D(graphicsDevice, 1, 1);
            buttonTexture.SetData(new Color[] { Color.Gray });

            Dictionary<string, object> restartParams = new Dictionary<string, object>
            {
                { "gameManager", game.GameManager },
                { "game", game }
            };
            Button restartButton = new Button(buttonTexture, font,
                new Rectangle(Globals.SCREENWIDTH / 2 - 75, Globals.SCREENHEIGHT / 2 - 20, 150, 40),
                "Restart", new GameCommands.ResetCommand(game), restartParams);

            Dictionary<string, object> quitParams = new Dictionary<string, object>
            {
                { "gameManager", game.GameManager },
                { "game", game }
            };
            Button quitButton = new Button(buttonTexture, font,
                new Rectangle(Globals.SCREENWIDTH / 2 - 75, Globals.SCREENHEIGHT / 2 + 40, 150, 40),
                "Quit", new GameCommands.QuitCommand(game), quitParams);

            Dictionary<string, object> startParams = new Dictionary<string, object>
            {
                { "gameManager", game.GameManager },
                { "screen", this }
            };
            // Note: The Start button uses "StartGameCommand" to remove the start menu.
            Button startButton = new Button(buttonTexture, font,
                new Rectangle(Globals.SCREENWIDTH / 2 - 75, Globals.SCREENHEIGHT / 2 - 80, 150, 40),
                "Start", new GameCommands.StartGameCommand(), startParams);

            buttons.Add(restartButton);
            buttons.Add(quitButton);
            buttons.Add(startButton);
        }

        public void Update()
        {
            MouseState mouseState = Mouse.GetState();
            foreach (Button button in buttons)
            {
                button.Update(mouseState);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, Globals.SCREENWIDTH, Globals.SCREENHEIGHT), Color.White);
            foreach (Button button in buttons)
            {
                button.Draw(spriteBatch);
            }
        }

        public void HandleClick(Point clickLocation)
        {
            foreach (Button button in buttons)
            {
                if (button.ContainsPoint(clickLocation))
                {
                    button.Click();
                }
            }
        }
    }
}
