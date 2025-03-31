using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0
{
    class KillCountHUD : IHUD
    {
        private GameManager gameManager;
        private Texture2D icon;
        private Texture2D arrow;
        private Rectangle iconSourceRectangle;
        private List<Rectangle> arrowSourceRectangle;
        private int currentLevelEnemies;
        private int currentKillCount;
        private bool levelComplete;
        private double timer = 0;
        public KillCountHUD(GameManager gameManager, Texture2D iconTexture, Texture2D arrowTexture)
        {
            this.gameManager = gameManager;
            icon = iconTexture;
            arrow = arrowTexture;
            currentLevelEnemies = 0;
            currentKillCount = 0;
            levelComplete = false;
            iconSourceRectangle = new Rectangle(0, 0, 512, 512);
            arrowSourceRectangle = new List<Rectangle>();
            arrowSourceRectangle.Add(new Rectangle(0, 558, 180, 96));
            arrowSourceRectangle.Add(new Rectangle(0, 654, 180, 96));
        }
        public void Update()
        {
            levelComplete = gameManager.LevelManager.ActiveLevel.Complete;
            timer += Globals.FRAMETIME*5;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            // spriteBatch.Draw(icon, new Rectangle(1830, 970, 80, 80), iconSourceRectangle, Color.White);
            // spriteBatch.DrawString(Globals.FONT, currentKillCount + "/" + currentLevelEnemies, new Vector2(1920 - 80, 1080-140), Color.Black, 0f, new Vector2(0,0), 1.5f, SpriteEffects.None, 0f);
            if(levelComplete) {
                spriteBatch.DrawString(Globals.FONT, "LEVEL COMPLETE!", new Vector2(1920 - 200, 1080-140), Color.Red, 0f, new Vector2(0,0), 1f, SpriteEffects.None, 0f);
                spriteBatch.Draw(arrow, new Rectangle(1730, 970, 180, 96), arrowSourceRectangle[(int)timer%2], Color.White);
            }
        }
    }
}
