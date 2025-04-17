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
        private Texture2D checkmark;
        private Texture2D xMark;
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
            checkmark = content.Load<Texture2D>("UI/checkmark");
            xMark = content.Load<Texture2D>("UI/xMark");

            achievements.Add(new Achievement("HeadHunter", "Kill 20 Small Enemys", content.Load<Texture2D>("UI/checkmark"), false));

            //Add close screen buttons
            Texture2D buttonTexture = game.Content.Load<Texture2D>("UI/ShopExit");
            Vector2 exitButtonPos = new Vector2(x - buttonTexture.Width + width, y);
            buttons.Add(new AchievementsExitButton(buttonTexture, exitButtonPos, 
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
            Rectangle boxDestinationRect;
            Vector2 textPos;
            Texture2D markTexture = xMark;
            foreach (Button button in buttons)
            {
                button.Draw(spriteBatch);
            }

            for (int i = 0; i < 1; i ++)
            {
                Achievement item = achievements[i];
                iconPos = new Vector2(windowRectangle.X + 20, windowRectangle.Y + i * 60 + 10);
                iconDestinationRect = new Rectangle((int)iconPos.X, (int)iconPos.Y, 40, 40);
                //3 columns, 10 pixel inbetween
                boxDestinationRect = new Rectangle((int)iconPos.X - 5, (int)iconPos.Y - 5, 306, 50);
                textPos = new Vector2(iconPos.X + 50, iconPos.Y + 10);
                spriteBatch.Draw(boxTexture, boxDestinationRect, Color.Gray);
                if (IsAchievementCompleted(item)) {
                    item.Completed = true;
                    markTexture = checkmark;
                }
                spriteBatch.Draw(markTexture, iconDestinationRect, Color.WhiteSmoke);
                spriteBatch.DrawString(font, item.Text, textPos, Color.White);
            }
        }

        private bool IsAchievementCompleted(Achievement currentAch) 
        {
            switch(currentAch.Name)
            {
                case "HeadHunter":
                    if(Globals.PlayerData.GetInt("SmallEnemyKilled") >= 20) 
                    {
                        return true;
                    } 
                    else {
                        return false;
                    }
                default:
                    return false;
            }
        }

    }
}
