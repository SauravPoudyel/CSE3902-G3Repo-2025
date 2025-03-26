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
        private Dictionary<string, object> parameters;
        public bool BlocksInput => true;
        private Game1 game;

        public StartMenu(Game1 game)
        {
            this.game = game;
            this.content = game.Content;
            buttons = new List<Button>();
            overlayColor = new Color(0, 0, 0, 180);
            backgroundTexture = content.Load<Texture2D>("UI/StartMenu");
            parameters = new Dictionary<string, object>
            {
                { "gameManager", game.GameManager },
                { "game", game }
            };

            Texture2D backgroundButton = content.Load<Texture2D>("UI/ButtonBackground1");
            int xCenter = (Globals.SCREENWIDTH - backgroundButton.Width) / 2;
            // Assume these buttons properly execute their commands.
            buttons.Add(new StartButton(content.Load<Texture2D>("UI/PlayButton"), backgroundButton, new Vector2(xCenter - 200, 500), parameters));
            buttons.Add(new RestartButton(content.Load<Texture2D>("UI/RestartButton"), backgroundButton, new Vector2(xCenter, 500), parameters));
            buttons.Add(new ExitGameButton(content.Load<Texture2D>("UI/ExitGameButton"), backgroundButton, new Vector2(xCenter + 200, 500), parameters));
        }

        public void Update()
        {
            // Update buttons.
            foreach (Button button in buttons)
            {
                button.Update();
            }

            // Fallback: if the player presses Enter, execute the StartGameCommand.
            KeyboardState kbState = Keyboard.GetState();
            if (kbState.IsKeyDown(Keys.Enter))
            {
                System.Console.WriteLine("[StartMenu] Enter pressed, executing StartGameCommand");
                new GameCommands.StartGameCommand().Execute(parameters);
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
    }
}
