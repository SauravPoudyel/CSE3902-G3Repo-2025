
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0
{
    public class ShopItem
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public Texture2D Icon { get; set; }
        public Rectangle IconRect { get; set; }
        public string Category { get; set; }
        public int Amount { get; set; }

        public ShopItem(string name, int price, Texture2D icon, Rectangle rectangle, string category, int amount)
        {
            Name = name;
            Price = price;
            Icon = icon;
            IconRect = rectangle;
            Category = category;
            Amount = amount;
        }
    }
}
