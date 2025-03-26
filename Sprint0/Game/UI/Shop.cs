using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sprint0
{
    public class ShopItem
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public Texture2D Icon { get; set; }
        public string Category { get; set; }
        public int Order { get; set; }

        public ShopItem(string name, int price, Texture2D icon, string category, int order)
        {
            Name = name;
            Price = price;
            Icon = icon;
            Category = category;
            Order = order;
        }
    }

    class ShopUI
    {
        private List<ShopItem> items;
        private int selectedIndex;
        private SpriteFont font;
        private Texture2D background;
        private Rectangle windowRectangle;
        private GraphicsDevice graphicsDevice;
        private KeyboardState previousKeyboardState;
        private MouseState previousMouseState;

        public bool IsOpen { get; set; }
        public ShopUI(GraphicsDevice graphicsDevice, SpriteFont font, Texture2D background)
        {
            this.graphicsDevice = graphicsDevice;
            this.font = font;
            this.background = background;
            items = new List<ShopItem>();
            selectedIndex = 0;
            IsOpen = false;  // Initially closed.
            LoadShopItems();
            previousKeyboardState = Keyboard.GetState();
            previousMouseState = Mouse.GetState();

            int width = 400;
            int height = 300;
            int x = (graphicsDevice.Viewport.Width - width) / 2;
            int y = (graphicsDevice.Viewport.Height - height) / 2;
            windowRectangle = new Rectangle(x, y, width, height);
        }
        private void LoadShopItems()
        {
            // Add shop items here.
        }

        public void Update(GameTime gameTime)
        {
            if (!IsOpen)
                return;

            MouseState currentMouseState = Mouse.GetState();
            KeyboardState currentKeyboardState = Keyboard.GetState();

            // (Optional) Allow keyboard navigation as well.
            if (currentKeyboardState.IsKeyDown(Keys.Down) && previousKeyboardState.IsKeyUp(Keys.Down))
            {
                selectedIndex++;
                if (selectedIndex >= items.Count)
                    selectedIndex = 0;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Up) && previousKeyboardState.IsKeyUp(Keys.Up))
            {
                selectedIndex--;
                if (selectedIndex < 0)
                    selectedIndex = items.Count - 1;
            }

            // Check for mouse hover and clicks on each item.
            for (int i = 0; i < items.Count; i++)
            {
                Vector2 position = new Vector2(windowRectangle.X + 20, windowRectangle.Y + 20 + i * 40);
                string text = $"{items[i].Name} - ${items[i].Price}";
                Vector2 textSize = font.MeasureString(text);
                Rectangle itemRectangle = new Rectangle((int)position.X, (int)position.Y, (int)textSize.X, (int)textSize.Y);

                // If the mouse is hovering over this item, set it as selected.
                if (itemRectangle.Contains(currentMouseState.Position))
                {
                    selectedIndex = i;

                    // If left mouse button clicked, purchase the item.
                    if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                    {
                        PurchaseItem(items[i]);
                    }
                }
            }

            previousKeyboardState = currentKeyboardState;
            previousMouseState = currentMouseState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!IsOpen)
                return;

            spriteBatch.Begin();

            // Draw the pop-up background inside the defined window rectangle.
            spriteBatch.Draw(background, windowRectangle, Color.White);

            // Draw each shop item relative to the pop-up window.
            for (int i = 0; i < items.Count; i++)
            {
                // Highlight the item if it is selected (via keyboard or mouse hover).
                Color itemColor = (i == selectedIndex) ? Color.Yellow : Color.White;
                Vector2 position = new Vector2(windowRectangle.X + 20, windowRectangle.Y + 20 + i * 40);
                string text = $"{items[i].Name} - ${items[i].Price}";
                spriteBatch.DrawString(font, text, position, itemColor);
            }

            spriteBatch.End();
        }

        private void PurchaseItem(ShopItem item)
        {
           
        }
    }
}
