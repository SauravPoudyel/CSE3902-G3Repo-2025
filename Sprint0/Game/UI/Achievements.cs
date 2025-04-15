using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using static Sprint0.EntityKeys;

namespace Sprint0
{
    public class Achievement
    {
        public string Name { get; set; }
        public Texture2D Icon { get; set; }
        public Rectangle IconRect { get; set; }
        public int Amount {get ; set;}

        public Achievement(string name, Texture2D icon, Rectangle rectangle, int amount)
        {
            Name = name;
            Icon = icon;
            IconRect = rectangle;
            Amount = amount;
        }
    }
    public class Achievements : IScreen
    {
        private List<Achievements> achievements;
        private List<Button> buttons;
        private Rectangle windowRectangle;
        private Texture2D backgroundTexture;
        public bool BlocksInput => true;
        SpriteFont font = Globals.FONT;

        public Achievements(Game1 game)
        {
            ContentManager content = game.Content;
            GraphicsDevice graphicsDevice = game.GraphicsDevice;
            achievements = new List<Achievements>();
            buttons = new List<Button>();
            //Set size of the window
            int width = Globals.SCREENWIDTH/2;
            int height = Globals.SCREENHEIGHT/2;
            int x = (Globals.SCREENWIDTH - width) / 2;
            int y = (Globals.SCREENHEIGHT - height) / 2;
            windowRectangle = new Rectangle(x, y, width, height);

            //Set the background of the window
            backgroundTexture = new Texture2D(graphicsDevice, 1, 1);
            backgroundTexture.SetData(new Color[] { Color.LightGray});

            //Add close screen buttons
            Texture2D buttonTexture = game.Content.Load<Texture2D>("UI/ShopExit");
            Vector2 exitButtonPos = new Vector2(x - buttonTexture.Width + width, y);
            buttons.Add(new StatsExitButton(buttonTexture, exitButtonPos, 
                new Dictionary<string, object> { { "gameManager", game.GameManager } }));

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
            spriteBatch.Draw(backgroundTexture, windowRectangle, Color.White);
            foreach (Button button in buttons)
            {
                button.Draw(spriteBatch);
            }
        }

    }
}
