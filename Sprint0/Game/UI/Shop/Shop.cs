using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sprint0
{

    public class Shop : IScreen
    {
        Game1 game;
        private List<ShopItem> shopItems;
        private List<ShopItem> filteredItems;
        private List<string> categories;
        private string selectedCategory;
        private int selectedIndex;
        private SpriteFont font;
        private Texture2D background;
        private Rectangle windowRectangle;
        private MouseState previousMouseState;
        public bool BlocksInput => true; 

        public Shop(Game1 game)
        {
            this.game = game;
            shopItems = new List<ShopItem>();
            filteredItems = new List<ShopItem>();
            categories = new List<string> { "Ammo", "permanent" };
            selectedCategory = "Ammo";
            selectedIndex = -1;
            previousMouseState = Mouse.GetState();            

            int width = Globals.SCREENWIDTH/4;
            int height = Globals.SCREENHEIGHT/2;
            int x = (Globals.SCREENWIDTH - width) / 2;
            int y = (Globals.SCREENHEIGHT - height) / 2;
            windowRectangle = new Rectangle(x, y, width, height);
        }
        public void LoadContent()
        {
            ContentManager content = game.Content;
            GraphicsDevice graphicsDevice = game.GraphicsDevice;
            font = Globals.FONT;
            background = new Texture2D(graphicsDevice, 1, 1);
            background.SetData(new Color[] { Color.LightGray});

            // Load item icons.
            Texture2D sheet = content.Load<Texture2D>("PickupItemSpritesheet2");
            Rectangle sniperRect = new Rectangle(0, 160, 40, 40);
            Rectangle rocketRect = new Rectangle(0, 200, 40, 40);
            Rectangle shotgunRect = new Rectangle(0, 120, 40, 40);
            Rectangle teleporterRect = new Rectangle(0, 680, 40, 40);
            Rectangle mineRect = new Rectangle(0, 280, 40, 40);
            Rectangle fireRate = new Rectangle(0, 360, 40, 40);
            Rectangle speed = new Rectangle(0, 840, 40, 40);

            // Add shop items.
            shopItems.Add(new ShopItem("Sniper Rifle", 100, sheet, sniperRect, "Ammo", 3));
            shopItems.Add(new ShopItem("Rocket Launcher", 150, sheet, rocketRect, "Ammo", 1));
            shopItems.Add(new ShopItem("Shotgun", 75, sheet, shotgunRect, "Ammo", 5));
            shopItems.Add(new ShopItem("Teleporter", 200, sheet, teleporterRect, "Ammo", 1));
            shopItems.Add(new ShopItem("Mine", 50, sheet, mineRect, "Ammo", 1));
            shopItems.Add(new ShopItem("Fire Rate", 1000, sheet, fireRate, "permanent", 1));
            shopItems.Add(new ShopItem("Speed", 1000, sheet, speed, "permanent", 1));

            shopItems.Sort((a, b) => a.Name.CompareTo(b.Name));


            FilterItems();
        }

        public void Update()
        {
            
            
            MouseState currentMouseState = Mouse.GetState();

            Vector2 categoryPos = new Vector2(windowRectangle.X + 20, windowRectangle.Y + 10);

            foreach (string cat in categories)
            {
                Vector2 catSize = font.MeasureString(cat);
                Rectangle catRect = new Rectangle((int)categoryPos.X, (int)categoryPos.Y, (int)catSize.X + 10, (int)catSize.Y + 10);
                if (catRect.Contains(currentMouseState.Position) &&
                    currentMouseState.LeftButton == ButtonState.Pressed &&
                    previousMouseState.LeftButton == ButtonState.Released)
                {
                    selectedCategory = cat;
                    FilterItems();
                }
                categoryPos.X += catSize.X + 20;
            }

            int itemsStartY = windowRectangle.Y + 50;
            for (int i = 0; i < filteredItems.Count; i++)
            {
                Vector2 itemPos = new Vector2(windowRectangle.X + 20, itemsStartY + i * 60);

                Rectangle iconDetinationRect = new Rectangle((int)itemPos.X, (int)itemPos.Y, 40, 40);
                // Text rectangle.
                Vector2 textPos = new Vector2(itemPos.X + 50, itemPos.Y + 10);
                string itemText = $"{filteredItems[i].Name} - ${filteredItems[i].Price}";
                Vector2 textSize = font.MeasureString(itemText);
                Rectangle textRect = new Rectangle((int)textPos.X, (int)textPos.Y, (int)textSize.X, (int)textSize.Y);
                // Combined hit area.
                Rectangle combinedRect = Rectangle.Union(iconDetinationRect, textRect);

                if (combinedRect.Contains(currentMouseState.Position))
                {
                    selectedIndex = i;
                    if (currentMouseState.LeftButton == ButtonState.Pressed &&
                        previousMouseState.LeftButton == ButtonState.Released)
                    {
                        PurchaseItem(filteredItems[i]);
                    }
                }
            }

            previousMouseState = currentMouseState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, windowRectangle, Color.White);

            Vector2 categoryPos = new Vector2(windowRectangle.X + 20, windowRectangle.Y + 10);
            foreach (string cat in categories)
            {
                Color catColor = (cat == selectedCategory) ? Color.Yellow : Color.White;
                Vector2 catSize = font.MeasureString(cat);
                Rectangle catRect = new Rectangle((int)categoryPos.X, (int)categoryPos.Y, (int)catSize.X + 10, (int)catSize.Y + 10);

                spriteBatch.Draw(background, catRect, Color.Gray * 0.5f);
                spriteBatch.DrawString(font, cat, new Vector2(categoryPos.X + 5, categoryPos.Y + 5), catColor);
                categoryPos.X += catSize.X + 20;
            }

            int itemsStartY = windowRectangle.Y + 50;
            for (int i = 0; i < filteredItems.Count; i++)
            {
                ShopItem item = filteredItems[i];
                Vector2 itemPos = new Vector2(windowRectangle.X + 20, itemsStartY + i * 60);

                // Draw the item icon.
                Rectangle iconDetinationRect = new Rectangle((int)itemPos.X, (int)itemPos.Y, 40, 40);
                spriteBatch.Draw(item.Icon, iconDetinationRect, item.IconRect, Color.White);

                // Draw the item text.
                Vector2 textPos = new Vector2(itemPos.X + 50, itemPos.Y + 10);
                string itemText = $"{item.Name} - ${item.Price}";
                Color textColor = (i == selectedIndex) ? Color.Yellow : Color.White;
                spriteBatch.DrawString(font, itemText, textPos, textColor);
            }


        }

        private void FilterItems()
        {
            if (selectedCategory == "All")
                filteredItems = new List<ShopItem>(shopItems);
            else
                filteredItems = shopItems.Where(item => item.Category == selectedCategory).ToList();

            selectedIndex = (filteredItems.Count > 0) ? 0 : -1;
        }

        private void PurchaseItem(ShopItem item)
        {
            switch (item.Name)
            {
                case "Sniper Rifle" when Globals.PlayerData.GetInt("Coins") >= item.Price:
                    Globals.PlayerData.SetInt("Coins", Globals.PlayerData.GetInt("Coins") - item.Price);
                    Globals.PlayerData.SetInt("AmmoSniper", Globals.PlayerData.GetInt("AmmoSniper") + item.Amount);
                    break;
                case "Rocket Launcher" when Globals.PlayerData.GetInt("Coins") >= item.Price:
                    Globals.PlayerData.SetInt("Coins", Globals.PlayerData.GetInt("Coins") - item.Price);
                    Globals.PlayerData.SetInt("AmmoRocket", Globals.PlayerData.GetInt("AmmoRocket") + item.Amount);
                    break;
                case "Shotgun" when Globals.PlayerData.GetInt("Coins") >= item.Price:
                    Globals.PlayerData.SetInt("Coins", Globals.PlayerData.GetInt("Coins") - item.Price);
                    Globals.PlayerData.SetInt("AmmoShotgun", Globals.PlayerData.GetInt("AmmoShotgun") + item.Amount);
                    break;
                case "Teleporter" when Globals.PlayerData.GetInt("Coins") >= item.Price:
                    Globals.PlayerData.SetInt("Coins", Globals.PlayerData.GetInt("Coins") - item.Price);
                    Globals.PlayerData.SetInt("AmmoTeleporter", Globals.PlayerData.GetInt("AmmoTeleporter") + item.Amount);
                    break;
                case "Mine" when Globals.PlayerData.GetInt("Coins") >= item.Price:
                    Globals.PlayerData.SetInt("Coins", Globals.PlayerData.GetInt("Coins") - item.Price);
                    Globals.PlayerData.SetInt("AmmoMine", Globals.PlayerData.GetInt("AmmoMine") + item.Amount);
                    break;
                case "Fire Rate" when Globals.PlayerData.GetInt("Coins") >= item.Price:
                    Globals.PlayerData.SetInt("Coins", Globals.PlayerData.GetInt("Coins") - item.Price);
                    Globals.PlayerData.SetInt("FireRateModifier", Globals.PlayerData.GetInt("FireRateModifier") + item.Amount);
                    break;
                case "Speed" when Globals.PlayerData.GetInt("Coins") >= item.Price:
                    Globals.PlayerData.SetInt("Coins", Globals.PlayerData.GetInt("Coins") - item.Price);
                    Globals.PlayerData.SetInt("SpeedModifier", Globals.PlayerData.GetInt("SpeedModifier") + item.Amount);
                    break;
                default:
                    break;
            }
                
        }
    }
}
