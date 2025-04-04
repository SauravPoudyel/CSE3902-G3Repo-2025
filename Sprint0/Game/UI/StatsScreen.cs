using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using static Sprint0.EntityKeys;

namespace Sprint0
{
    public class StatsScreen : IScreen
    {
        private Dictionary<string, Rectangle> mobList = new Dictionary<string, Rectangle>();
        private Rectangle windowRectangle;
        private Texture2D backgroundTexture;
        private Texture2D spriteSheet1;
        private Texture2D spriteSheet2;
        public bool BlocksInput => true;
        SpriteFont font = Globals.FONT;
        public StatsScreen(Game1 game) 
        {
            ContentManager content = game.Content;
            GraphicsDevice graphicsDevice = game.GraphicsDevice;
            //Set size of the window
            int width = Globals.SCREENWIDTH/2;
            int height = Globals.SCREENHEIGHT/2;
            int x = (Globals.SCREENWIDTH - width) / 2;
            int y = (Globals.SCREENHEIGHT - height) / 2;
            windowRectangle = new Rectangle(x, y, width, height);

            //Set the background of the window
            backgroundTexture = new Texture2D(graphicsDevice, 1, 1);
            backgroundTexture.SetData(new Color[] { Color.LightGray});
        

            //Add Text and Images of rectangles
            spriteSheet1 = content.Load<Texture2D>("TDTanksAllSprites");
            spriteSheet2 = content.Load<Texture2D>("TDTowerDefenseSprites");
            mobList.Add("BossTank", new Rectangle(641, 661, 123, 144));
            mobList.Add("HealerTank", new Rectangle(1126, 334, 76, 72));
            mobList.Add("HoveringTank", new Rectangle(1135, 180, 86, 92));
            mobList.Add("Plane", new Rectangle(2183, 1411, 135, 134));
            mobList.Add("ShieldTank", new Rectangle(768, 0, 94, 97));
            mobList.Add("ShipVertical", new Rectangle(1135, 840, 68, 116));
            mobList.Add("ShipHorizontal", new Rectangle(1135, 840, 68, 116));
            mobList.Add("SmallEnemy", new Rectangle(768, 256, 95, 113));
            mobList.Add("StealthTank", new Rectangle( 876, 783, 84, 80));
            mobList.Add("SwarmingTank", new Rectangle( 1126, 275, 53, 56));
            mobList.Add("Turret", new Rectangle(2444, 908, 104, 104));

            //Add close screen buttons


        }


        //These two methods follow the shop.cs format very similarly 
        public void Update()
        {   
            //Might not need this since we aren't clicking anything (as of now)
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
            spriteBatch.Draw(backgroundTexture, windowRectangle, Color.White);
            //loop for each mob to draw
            int i = 0;
            int secondColumn = 0;
            foreach (MobType mob in EntityKeys.MobType.GetValues(typeof (MobType)))
            {
                Vector2 iconPos;
                Rectangle iconDestinationRect;
                Vector2 textPos;
                string itemText;
                Vector2 textSize;
                Rectangle textRect;
                if (i > 5)
                {
                    iconPos = new Vector2(windowRectangle.X + 20, windowRectangle.Y + 50 + i * 60);
                    iconDestinationRect = new Rectangle((int)iconPos.X + 500, (int)iconPos.Y - 350, 40, 40);
                    textPos = new Vector2(iconPos.X + 50 + 500, iconPos.Y + 10 + 300);
                    itemText = mob + " Killed " + Globals.PlayerData.GetInt(mob.ToString() + "killed");
                    textSize = font.MeasureString(itemText);
                    textRect = new Rectangle((int)textPos.X + 500, (int)textPos.Y, (int)textSize.X, (int)textSize.Y);
                } else 
                {
                    iconPos = new Vector2(windowRectangle.X + 20, windowRectangle.Y + 50 + i * 60);
                    iconDestinationRect = new Rectangle((int)iconPos.X, (int)iconPos.Y, 40, 40);
                    textPos = new Vector2(iconPos.X + 50 + secondColumn, iconPos.Y + 10);
                    itemText = mob + " Killed " + Globals.PlayerData.GetInt(mob.ToString() + "killed");
                    textSize = font.MeasureString(itemText);
                    textRect = new Rectangle((int)textPos.X, (int)textPos.Y, (int)textSize.X, (int)textSize.Y);
                }
                
                if ((mob == MobType.Plane) || (mob == MobType.Turret)) 
                {
                    spriteBatch.Draw(spriteSheet2, iconDestinationRect, mobList[mob.ToString()], Color.White);
                } else 
                {
                    spriteBatch.Draw(spriteSheet1, iconDestinationRect, mobList[mob.ToString()], Color.White);
                }
                spriteBatch.DrawString(font, itemText, textPos, Color.White);

                i++;
            }
        }
    }
}