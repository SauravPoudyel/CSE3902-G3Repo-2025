using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Sprint0
{
    public class PlayerInventory : IScreen
    {
        public List<PlayerInventory.Item> Inventory;
        public int CoinCount;

        public PlayerInventory()
        {
            Inventory = new List<PlayerInventory.Item>();
            Inventory.Add(new PlayerInventory.Item("null", 0));
            Inventory.Add(new PlayerInventory.Item("null", 0));
            Inventory.Add(new PlayerInventory.Item("null", 0));
            Inventory.Add(new PlayerInventory.Item("null", 0));
            CoinCount = 0;
        }

        public void AddCoins(int amount)
        {
            if (amount > 0)
            {
                CoinCount = CoinCount + amount;
            }
        }

        public void RemoveCoins(int amount)
        {
            if (amount > 0 && amount <= CoinCount)
            {
                CoinCount = CoinCount - amount;
            }
        }

        public void Update(GameTime gameTime)
        {
            // Update inventory animations if needed.
        }

        public void HandleClick(Point clickLocation) 
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D rect = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            rect.SetData(new Color[] { Color.White });
            Color bgColor = new Color(0, 0, 0, 128);
            Rectangle inventoryRect = new Rectangle(10, 10, 200, 100);
            spriteBatch.Draw(rect, inventoryRect, bgColor);
            // Draw coin count and items here.
        }

        public class Item
        {
            public string ItemKey;
            public int Count;

            public Item(string itemKey, int count)
            {
                ItemKey = itemKey;
                Count = count;
            }

            public void AddCount(int amount)
            {
                if (amount > 0)
                {
                    Count = Count + amount;
                }
            }

            public void RemoveCount(int amount)
            {
                if (amount > 0 && amount <= Count)
                {
                    Count = Count - amount;
                }
            }
        }
    }
}
