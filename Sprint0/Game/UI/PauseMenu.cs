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

            buttons = new List<Button>();
            overlayColor = new Color(0, 0, 0, 180);

            SpriteFont font = Globals.FONT;
            Texture2D buttonTexture = new Texture2D(graphicsDevice, 1, 1);
            buttonTexture.SetData(new Color[] { Color.Gray });

            Dictionary<string, object> gameParams = new Dictionary<string, object>
            {
                { "gameManager", game.GameManager },
                { "game", game }
            };
            Button restartButton = new textButton(buttonTexture,
                new Rectangle(Globals.SCREENWIDTH / 2 - 75, Globals.SCREENHEIGHT / 2 - 140, 150, 40),
                "Restart", new GameCommands.resetLevelCommand(), gameParams);
            Button menuButton = new textButton(buttonTexture,
                new Rectangle(Globals.SCREENWIDTH / 2 - 75, Globals.SCREENHEIGHT / 2 - 20, 150, 40),
                "Main Menu", new GameCommands.ResetCommand(), gameParams);
            Button quitButton = new textButton(buttonTexture,
                new Rectangle(Globals.SCREENWIDTH / 2 - 75, Globals.SCREENHEIGHT / 2 + 100, 150, 40),
                "Quit", new GameCommands.QuitCommand(), gameParams);

            Dictionary<string, object> screenParams = new Dictionary<string, object>
            {
                { "gameManager", game.GameManager },
                { "screen", this }
            };
            // Note: The Start button uses "StartGameCommand" to remove the start menu.
            Button resumeButton = new textButton(buttonTexture,
                new Rectangle(Globals.SCREENWIDTH / 2 - 75, Globals.SCREENHEIGHT / 2 - 80, 150, 40),
                "Resume", new GameCommands.StartGameCommand(), screenParams);

            Button increaseLevelButton = new textButton(buttonTexture,
                new Rectangle(Globals.SCREENWIDTH / 2 - 75 + 160, Globals.SCREENHEIGHT / 2 + 40, 40, 40),
                "+", new GameCommands.IncreaseLevelCommand(), screenParams);
            Button decreaseLevelButton = new textButton(buttonTexture,
                new Rectangle(Globals.SCREENWIDTH / 2 - 75 - 50, Globals.SCREENHEIGHT / 2 + 40, 40, 40),
                "-", new GameCommands.DecreaseLevelCommand(), screenParams);

            Button levelButton = new textButton(buttonTexture,
                new Rectangle(Globals.SCREENWIDTH / 2 - 75, Globals.SCREENHEIGHT / 2 + 40, 150, 40),
                "Level: " + game.GameManager.LevelNumber.ToString(), null, screenParams);

            buttons.Add(restartButton);
            buttons.Add(menuButton);
            buttons.Add(quitButton);
            buttons.Add(resumeButton);
            buttons.Add(levelButton);
            buttons.Add(decreaseLevelButton);
            buttons.Add(increaseLevelButton);
        }

        public void Update()
        {
            foreach (Button button in buttons)
            {
                button.Update();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Button button in buttons)
            {
                button.Draw(spriteBatch);
            }
        }
    }
}
