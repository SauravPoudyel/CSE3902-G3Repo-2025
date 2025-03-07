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
        Dictionary<string, object> paramters;
        // StartMenu should block game input.
        public bool BlocksInput => true;

        public StartMenu(Game1 game)
        {
            this.content = game.Content;
            buttons = new List<Button>();
            overlayColor = new Color(0, 0, 0, 180);
            backgroundTexture = content.Load<Texture2D>("UI/StartMenu");
            paramters = new Dictionary<string, object>
            {
                { "gameManager", game.GameManager },
                { "game", game}
            };
            Texture2D backgroundButton = content.Load<Texture2D>("UI/ButtonBackground1");
            int xCenter = (Globals.SCREENWIDTH - backgroundButton.Width) / 2;
            buttons.Add(new StartButton(content.Load<Texture2D>("UI/PlayButton"), backgroundButton, new(xCenter-200,500), paramters));
            buttons.Add(new RestartButton(content.Load<Texture2D>("UI/RestartButton"), backgroundButton, new(xCenter,500), paramters));
            buttons.Add(new ExitGameButton(content.Load<Texture2D>("UI/ExitGameButton"), backgroundButton, new(xCenter+200, 500), paramters));

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
            spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, Globals.SCREENWIDTH, Globals.SCREENHEIGHT), Color.White);
            foreach (Button button in buttons)
            {
                button.Draw(spriteBatch);
            }
        }
    }
}
