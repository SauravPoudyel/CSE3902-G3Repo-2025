using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Sprint0
{
    public class PauseMenu : IScreen
    {
        private List<Button> buttons;
        private Color overlayColor;
        private ContentManager content;
        private GraphicsDevice graphicsDevice;

        // PauseMenu should block game input.
        public bool BlocksInput => true;

        public PauseMenu(ContentManager content, GraphicsDevice graphicsDevice, Game1 game)
        {
            this.content = content;
            this.graphicsDevice = graphicsDevice;
            buttons = new List<Button>();
            overlayColor = new Color(0, 0, 0, 180);

            SpriteFont font = content.Load<SpriteFont>("Arial");
            Texture2D buttonTexture = new Texture2D(graphicsDevice, 1, 1);
            buttonTexture.SetData(new Color[] { Color.Gray });

            Dictionary<string, object> gameParams = new Dictionary<string, object>
            {
                { "gameManager", game.GameManager },
                { "game", game }
            };
            Button menuButton = new Button(buttonTexture, font,
                new Rectangle(Globals.SCREENWIDTH / 2 - 75, Globals.SCREENHEIGHT / 2 - 20, 150, 40),
                "Main Menu", new GameCommands.ResetCommand(game), gameParams);
            Button quitButton = new Button(buttonTexture, font,
                new Rectangle(Globals.SCREENWIDTH / 2 - 75, Globals.SCREENHEIGHT / 2 + 100, 150, 40),
                "Quit", new GameCommands.QuitCommand(game), gameParams);

            Dictionary<string, object> screenParams = new Dictionary<string, object>
            {
                { "gameManager", game.GameManager },
                { "screen", this }
            };
            // Note: The Start button uses "StartGameCommand" to remove the start menu.
            Button resumeButton = new Button(buttonTexture, font,
                new Rectangle(Globals.SCREENWIDTH / 2 - 75, Globals.SCREENHEIGHT / 2 - 80, 150, 40),
                "Resume", new GameCommands.StartGameCommand(), screenParams);

            Button increaseLevelButton = new Button(buttonTexture, font,
                new Rectangle(Globals.SCREENWIDTH / 2 - 75 + 160, Globals.SCREENHEIGHT / 2 + 40, 40, 40),
                "+", new GameCommands.IncreaseLevelCommand(), screenParams);
            Button decreaseLevelButton = new Button(buttonTexture, font,
                new Rectangle(Globals.SCREENWIDTH / 2 - 75 - 50, Globals.SCREENHEIGHT / 2 + 40, 40, 40),
                "-", new GameCommands.DecreaseLevelCommand(), screenParams);

            Button levelButton = new Button(buttonTexture, font,
                new Rectangle(Globals.SCREENWIDTH / 2 - 75, Globals.SCREENHEIGHT / 2 + 40, 150, 40),
                "Level: " + game.GameManager.LevelNumber.ToString(), null, screenParams);

            buttons.Add(menuButton);
            buttons.Add(quitButton);
            buttons.Add(resumeButton);
            buttons.Add(levelButton);
            buttons.Add(decreaseLevelButton);
            buttons.Add(increaseLevelButton);
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
