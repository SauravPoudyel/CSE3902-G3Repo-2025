using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using static Sprint0.EntityKeys;

namespace Sprint0
{
    public class StatsItem
    {
        public string Name { get; set; }
        public Texture2D Icon { get; set; }
        public Rectangle IconRect { get; set; }
        public int Amount {get ; set;}

        public StatsItem(string name, Texture2D icon, Rectangle rectangle, int amount)
        {
            Name = name;
            Icon = icon;
            IconRect = rectangle;
            Amount = amount;
        }
    }
    public class StatsScreen : IScreen
    {
        private List<StatsItem> statsItems;
        private List<Button> statsButtons;
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
            statsItems = new List<StatsItem>();
            statsButtons = new List<Button>();
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
            statsItems.Add(new StatsItem("BossTank",spriteSheet1, new Rectangle(641, 661, 123, 144), Globals.PlayerData.GetInt("BossTankKilled")));
            statsItems.Add(new StatsItem("HealerTank", spriteSheet1, new Rectangle(1126, 334, 76, 72), Globals.PlayerData.GetInt("HealerTankKilled")));
            statsItems.Add(new StatsItem("HoveringTank", spriteSheet1, new Rectangle(1135, 180, 86, 92), Globals.PlayerData.GetInt("HoveringTankKilled")));
            statsItems.Add(new StatsItem("Plane", spriteSheet2, new Rectangle(2183, 1411, 135, 134), Globals.PlayerData.GetInt("PlaneKilled")));
            statsItems.Add(new StatsItem("ShieldTank", spriteSheet1, new Rectangle(768, 0, 94, 97), Globals.PlayerData.GetInt("ShieldTankKilled")));
            statsItems.Add(new StatsItem("ShipVertical", spriteSheet1, new Rectangle(1135, 840, 68, 116), Globals.PlayerData.GetInt("ShipKilled")));
            statsItems.Add(new StatsItem("ShipHorizontal", spriteSheet1, new Rectangle(1135, 840, 68, 116), Globals.PlayerData.GetInt("ShipKilled")));
            statsItems.Add(new StatsItem("SmallEnemy", spriteSheet1, new Rectangle(768, 256, 95, 113), Globals.PlayerData.GetInt("SmallEnemyKilled")));
            statsItems.Add(new StatsItem("StealthTank", spriteSheet1, new Rectangle( 876, 783, 84, 80), Globals.PlayerData.GetInt("StealthTankKilled")));
            statsItems.Add(new StatsItem("SwarmingTank", spriteSheet1, new Rectangle( 1126, 275, 53, 56), Globals.PlayerData.GetInt("SwarmingTankKilled")));
            statsItems.Add(new StatsItem("Turret", spriteSheet2, new Rectangle(2444, 908, 104, 104), Globals.PlayerData.GetInt("TurretKilled")));

            //Add close screen buttons
            Texture2D buttonTexture = game.Content.Load<Texture2D>("UI/ShopExit");
            Vector2 exitButtonPos = new Vector2(x - buttonTexture.Width + width, y);
            statsButtons.Add(new StatsExitButton(buttonTexture, exitButtonPos, 
                new Dictionary<string, object> { { "gameManager", game.GameManager } }));

        }


        //These two methods follow the shop.cs format very similarly 
        public void Update()
        {   
            foreach (Button button in statsButtons)
            {
                button.Update();
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
            spriteBatch.Draw(backgroundTexture, windowRectangle, Color.White);
            //loop for each mob to draw
            int secondColumn = 0;
            Vector2 iconPos;
            Rectangle iconDestinationRect;
            Vector2 textPos;
            string itemText;
            Vector2 textSize;
            Rectangle textRect;
            foreach (Button button in statsButtons)
            {
                button.Draw(spriteBatch);
            }
            for (int i = 0; i < 11; i++)
            {
                StatsItem item = statsItems[i];
                if (i > 5)
                {
                    iconPos = new Vector2(windowRectangle.X + 20 + 500, windowRectangle.Y + 50 + i * 60 - 350);
                    iconDestinationRect = new Rectangle((int)iconPos.X, (int)iconPos.Y, 40, 40);
                    textPos = new Vector2(iconPos.X + 50, iconPos.Y + 10);
                    itemText = item.Name + " Killed " + Globals.PlayerData.GetInt(item.Name.ToString() + "Killed");
                    textSize = font.MeasureString(itemText);
                    textRect = new Rectangle((int)textPos.X + 500, (int)textPos.Y, (int)textSize.X, (int)textSize.Y);
                    spriteBatch.Draw(item.Icon, iconDestinationRect, item.IconRect, Color.White);
                    spriteBatch.DrawString(font, itemText, textPos, Color.White);
                } else 
                {
                    iconPos = new Vector2(windowRectangle.X + 20, windowRectangle.Y + 50 + i * 60);
                    iconDestinationRect = new Rectangle((int)iconPos.X, (int)iconPos.Y, 40, 40);
                    textPos = new Vector2(iconPos.X + 50 + secondColumn, iconPos.Y + 10);
                    itemText = item.Name + " Killed " + Globals.PlayerData.GetInt(item.Name.ToString() + "Killed");
                    textSize = font.MeasureString(itemText);
                    textRect = new Rectangle((int)textPos.X, (int)textPos.Y, (int)textSize.X, (int)textSize.Y);
                    spriteBatch.Draw(item.Icon, iconDestinationRect, item.IconRect, Color.White);
                    spriteBatch.DrawString(font, itemText, textPos, Color.White);
                }
            }
        }
    }
}