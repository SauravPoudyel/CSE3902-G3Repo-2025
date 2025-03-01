using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0
{
    class CoinHUD : IHUD
    {
        private Texture2D icon;
        private PlayerData playerData;
        private List<Rectangle> sourceRectangle;
        private int maxCoin;
        private int currentCoin;
        private double timer = 0;

        public CoinHUD(Texture2D texture)
        {
            this.icon = texture;
            playerData = Globals.PlayerData;
            maxCoin = playerData.PermanentCoins;
            currentCoin = playerData.TemporaryCoins;

            // Initialize source rectangles for coin icon
            sourceRectangle = new List<Rectangle>();
            sourceRectangle.Add(new Rectangle(0, 640, 40, 40));
            sourceRectangle.Add(new Rectangle(40, 640, 40, 40));
            sourceRectangle.Add(new Rectangle(80, 640, 40, 40));
            sourceRectangle.Add(new Rectangle(120, 640, 40, 40));
            sourceRectangle.Add(new Rectangle(160, 640, 40, 40));
            sourceRectangle.Add(new Rectangle(200, 640, 40, 40));
            sourceRectangle.Add(new Rectangle(240, 640, 40, 40));
            sourceRectangle.Add(new Rectangle(280, 640, 40, 40));
            sourceRectangle.Add(new Rectangle(320, 640, 40, 40));
            sourceRectangle.Add(new Rectangle(360, 640, 40, 40));
        }
        public void Update()
        {
            timer += Globals.FRAMETIME*5;
            currentCoin = playerData.TemporaryCoins;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(icon, new Rectangle(10, 50, 50, 50), sourceRectangle[(int)timer % sourceRectangle.Count], Color.White);
            spriteBatch.DrawString(Globals.FONT, "X " + currentCoin, new Vector2(70, 65), Color.Gold);
        }
    }
}
