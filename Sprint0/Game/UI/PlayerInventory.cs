using System.Collections.Generic;

namespace Sprint0
{
    public class PlayerInventory : Screen
    {
        public List<Item> Inventory { get; set; }
        public int CoinCount { get; set; }

        public PlayerInventory()
        {
            Inventory = new List<Item>
            {
                new Item("null", 0),
                new Item("null", 0),
                new Item("null", 0),
                new Item("null", 0)
            };

            CoinCount = 0;
        }

        public void AddCoins(int amount)
        {
            if (amount > 0)
            {
                CoinCount += amount;
            }
        }

        public void RemoveCoins(int amount)
        {
            if (amount > 0 && amount <= CoinCount)
            {
                CoinCount -= amount;
            }
        }

        public class Item
        {
            public string ItemKey { get; set; }
            public int Count { get; set; }

            public Item(string itemKey, int count)
            {
                ItemKey = itemKey;
                Count = count;
            }

            public void AddCount(int amount)
            {
                if (amount > 0)
                {
                    Count += amount;
                }
            }

            public void RemoveCount(int amount)
            {
                if (amount > 0 && amount <= Count)
                {
                    Count -= amount;
                }
            }
        }
    }
}
