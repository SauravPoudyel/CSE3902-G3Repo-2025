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
        public bool Completed {get ; set;}

        public Achievement(string name, string text, bool completed)
        {
            Name = name;
            Text = text;
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

            achievements.Add(new Achievement("HeadHunter", "Kill 20 Small Enemys", false));
            achievements.Add(new Achievement("Juggernaut", "Kill 10 Boss Tanks", false));
            achievements.Add(new Achievement("Aerial Defender", "Kill 20 Planes", false));
            achievements.Add(new Achievement("Speedster", "Get a speed modifier of 4X", false));
            achievements.Add(new Achievement("XP Farmer", "Get an XP level of 5,000", false));
            achievements.Add(new Achievement("The Tank", "Have at least 500 health", false));
            achievements.Add(new Achievement("Bread Collector", "Get at least 5,000 coins", false));
            achievements.Add(new Achievement("Traveler", "Travel a distance of 10,000", false));
            achievements.Add(new Achievement("The Ninja", "Kill 10 Stealth Tanks", false));
            achievements.Add(new Achievement("Anti Pacifist", "Kill 10 Healer Tanks", false));
            achievements.Add(new Achievement("Loaded", "Have at least 100 ammo", false));

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
            Vector2 namePos;
            Texture2D markTexture = xMark;
            foreach (Button button in buttons)
            {
                button.Draw(spriteBatch);
            }

            for (int i = 0; i < 10; i ++)
            {
                Achievement item = achievements[i];
                if (i > 5) 
                {
                    iconPos = new Vector2(windowRectangle.X + 20 + 500, windowRectangle.Y + i * 90 + 10 - 540);
                    iconDestinationRect = new Rectangle((int)iconPos.X, (int)iconPos.Y, 40, 40);
                    boxDestinationRect = new Rectangle((int)iconPos.X - 5, (int)iconPos.Y - 5, 306, 80);
                    namePos = new Vector2(iconPos.X + 50, iconPos.Y + 10);
                    textPos = new Vector2(namePos.X, namePos.Y + 30);
                } 
                else 
                {
                    iconPos = new Vector2(windowRectangle.X + 20, windowRectangle.Y + i * 90 + 10);
                    iconDestinationRect = new Rectangle((int)iconPos.X, (int)iconPos.Y, 40, 40);
                    boxDestinationRect = new Rectangle((int)iconPos.X - 5, (int)iconPos.Y - 5, 306, 80);
                    namePos = new Vector2(iconPos.X + 50, iconPos.Y + 10);
                    textPos = new Vector2(namePos.X, namePos.Y + 30);
                }
                spriteBatch.Draw(boxTexture, boxDestinationRect, Color.Gray);
                if (IsAchievementCompleted(item)) {
                    item.Completed = true;
                    markTexture = checkmark;
                }
                spriteBatch.Draw(markTexture, iconDestinationRect, Color.WhiteSmoke);
                if (!item.Completed) 
                {
                    spriteBatch.DrawString(font, item.Name, namePos, Color.Red);
                } 
                else 
                {
                    spriteBatch.DrawString(font, item.Name, namePos, Color.Green);
                }
                spriteBatch.DrawString(font, item.Text, textPos, Color.White);
                markTexture = xMark;
            }
        }

        private bool IsAchievementCompleted(Achievement currentAch) 
        {
            switch(currentAch.Name)
            {
                case "HeadHunter":
                    return (Globals.PlayerData.GetInt("SmallEnemyKilled") >= 20) ? true : false;
                case "Juggernaut":
                    return (Globals.PlayerData.GetInt("BossTankKilled") >= 10) ? true : false;
                case "Aeiral Defender":
                    return (Globals.PlayerData.GetInt("PlaneKilled") >= 10) ? true : false;
                case "Speedster":
                    return (Globals.PlayerData.GetInt("SpeedModifier") >= 4) ? true : false;
                case "XP Farmer":
                    return (Globals.PlayerData.GetInt("XP") >= 5000) ? true : false;
                case "The Tank":
                    return (Globals.PlayerData.GetInt("Health") >= 500) ? true : false;
                case "Bread Collector":
                    return (Globals.PlayerData.GetInt("Coins") >= 5000) ? true : false;
                case "Traveler":
                    return (Globals.PlayerData.GetInt("DistanceTraveled") >= 10000) ? true : false;
                case "The Ninja":
                    return (Globals.PlayerData.GetInt("StealthTankKilled") >= 10) ? true : false;
                case "Anti Pacifist":
                    return (Globals.PlayerData.GetInt("HealerTankKilled") >= 10) ? true : false;
                case "Loaded":
                    return (Globals.PlayerData.GetInt("AmmoDefault") >= 100) ? true : false;
                default:
                    return false;
            }
        }

    }
}
