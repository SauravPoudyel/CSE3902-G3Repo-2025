using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0
{
    class XPHUD : IHUD
    {
        private Texture2D blue;
        private Texture2D gray;
        private PlayerData playerData;
        private int maxXP;
        private int currentXP;
        public XPHUD(GraphicsDevice graphicsDevice)
        {
            this.playerData = Globals.PlayerData;
            gray = new Texture2D(graphicsDevice, 1, 1);
            gray.SetData(new Color[] { Color.Gray });
            blue = new Texture2D(graphicsDevice, 1, 1);
            blue.SetData(new Color[] { Color.Blue });
        }
        public void Update()
        {
            this.currentXP = playerData.GetInt("XP");
            this.maxXP = playerData.GetInt("MaxXP");
            if (currentXP >= maxXP)
            {
                playerData.SetInt("XP", currentXP-maxXP);
                playerData.SetInt("MaxXP", (int)(maxXP * 1.3));
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int xpBarLength = Globals.SCREENWIDTH/3;
            double ratio = (double)currentXP / maxXP;
            int yLocation = Globals.SCREENHEIGHT -140;
            int xLocation = Globals.SCREENWIDTH / 3;
            spriteBatch.Draw(gray, new Rectangle(xLocation, yLocation, xpBarLength, 25), Color.White * 0.5f);
            spriteBatch.Draw(blue, new Rectangle(xLocation, yLocation, (int)(xpBarLength*ratio), 25), Color.White);
        }
    }
}
