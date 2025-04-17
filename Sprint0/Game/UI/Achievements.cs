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
        public string Text { get; set;}
        public Texture2D Icon { get; set; }
        public bool Completed {get ; set;}

        public Achievement(string name, string text, Texture2D icon, bool completed)
        {
            Name = name;
            Text = text;
            Icon = icon;
            Completed = completed;
        }
    }
    public class Achievements : IScreen
    {
        private List<Achievement> achievements;
        private List<Button> buttons;
        private Rectangle windowRectangle;
        private Texture2D backgroundTexture;
        private Texture2D boxTexture;
        public bool BlocksInput => true;
        SpriteFont font = Globals.FONT;

        public Achievements(Game1 game)
        {
            ContentManager content = game.Content;
            GraphicsDevice graphicsDevice = game.GraphicsDevice;
            achievements = new List<Achievement>();
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
            boxTexture = new Texture2D(graphicsDevice, 1, 1);
            boxTexture.SetData(new Color[] { Color.Gray});

            achievements.Add(new Achievement("HeadHunter", "Kill 20 Small Enemys", content.Load<Texture2D>("UI/checkmark"), false));

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
            Vector2 iconPos;
            Rectangle iconDestinationRect;
            Vector2 textPos;
            string itemText;
            Vector2 textSize;
            foreach (Button button in buttons)
            {
                button.Draw(spriteBatch);
            }

            for (int i = 0; i < 16; i ++)
            {
                Achievement item = achievements[i];
                iconPos = new Vector2(windowRectangle.X + 20 + 500, windowRectangle.Y + 50 + i * 60 - 350);
                iconDestinationRect = new Rectangle((int)iconPos.X, (int)iconPos.Y, 40, 40);
                textPos = new Vector2(iconPos.X + 50, iconPos.Y + 10);
                itemText = item.Text;
                textSize = font.MeasureString(itemText);
                // spriteBatch.Draw(item.Icon, iconDestinationRect, item.IconRect, Color.White);
                spriteBatch.DrawString(font, itemText, textPos, Color.White);
            }
        }

        /* public bool isAchievementCompleted() 
        {

        } */

    }
}
