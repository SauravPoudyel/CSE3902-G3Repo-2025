using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0
{
    class KillCountHUD : IHUD
    {
        private Texture2D icon;
        private Rectangle sourceRectangle;
        private int currentLevelEnemies;
        private int currentKillCount;

        public KillCountHUD(GameManager gameManager, Texture2D texture)
        {
            this.icon = texture;
            currentLevelEnemies = 0;
            currentKillCount = 0;
            sourceRectangle = new Rectangle(0, 0, 512, 512);
        }
        public void Update()
        {

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(icon, new Rectangle(1920 - 90, 1080-110, 80, 80), sourceRectangle, Color.White);
            spriteBatch.DrawString(Globals.FONT, currentKillCount + "/" + currentLevelEnemies, new Vector2(1920 - 80, 1080-140), Color.Black, 0f, new Vector2(0,0), 1.5f, SpriteEffects.None, 0f);

        }
    }
}
