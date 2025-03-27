using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0
{
    // Wraps a Shop instance (which is not an IScreen) so that ScreenManager can update/draw it.
    public class ShopScreenAdapter : IScreen
    {
        private Shop shop;
        public bool BlocksInput => true;

        public ShopScreenAdapter(Shop shop)
        {
            this.shop = shop;
        }

        public void Update()
        {
            shop.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            shop.Draw(spriteBatch);
        }
    }
}
