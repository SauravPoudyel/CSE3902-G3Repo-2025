using System;
using System.Collections.Generic;
using System.Linq;
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
        private int levelEnemies;
        private int currentKillCount;
        private bool levelComplete;
        private double timer = 0;
        public KillCountHUD(GameManager gameManager, Texture2D iconTexture, Texture2D arrowTexture)
        {
            this.gameManager = gameManager;
            icon = iconTexture;
            arrow = arrowTexture;
            levelEnemies = gameManager.LevelManager.ActiveLevel.InitialEnemies;
            currentKillCount = 0;
            levelComplete = false;
            iconSourceRectangle = new Rectangle(0, 0, 512, 512);
            arrowSourceRectangle = new List<Rectangle>();
            arrowSourceRectangle.Add(new Rectangle(0, 558, 180, 96));
            arrowSourceRectangle.Add(new Rectangle(0, 654, 180, 96));
        }
        public void Update()
        {
            currentKillCount = levelEnemies - gameManager.LevelManager.ActiveLevel.GetEnemies().Count();
            levelComplete = gameManager.LevelManager.ActiveLevel.Complete;
            timer += Globals.FRAMETIME*5;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(icon, new Rectangle(20, 120, 80, 80), iconSourceRectangle, Color.White * 0.5f);
            if(!levelComplete) {
                spriteBatch.DrawString(Globals.FONT, currentKillCount + "/" + levelEnemies, new Vector2(40, 100), Color.Black, 0f, new Vector2(0,0), 1.5f, SpriteEffects.None, 0f);
            } else {
                spriteBatch.DrawString(Globals.FONT, currentKillCount + "/" + levelEnemies, new Vector2(40, 100), Color.Gold, 0f, new Vector2(0,0), 1.5f, SpriteEffects.None, 0f);
                Level.Direction nextLevelDirection = gameManager.LevelManager.ActiveLevel.NextLevelDirection();
                switch(nextLevelDirection) {
                    case Level.Direction.Top:
                    spriteBatch.Draw(arrow, new Rectangle(Globals.SCREENWIDTH/2, 0, 180, 96), arrowSourceRectangle[(int)timer%2], Color.White*.35f, (float)(Math.PI * 0.5), new Vector2(0,0), SpriteEffects.FlipHorizontally, 1f);
                    break;
                    case Level.Direction.Bottom:
                    spriteBatch.Draw(arrow, new Rectangle(Globals.SCREENWIDTH/2, Globals.SCREENHEIGHT-180, 180, 96), arrowSourceRectangle[(int)timer%2], Color.White*.35f, (float)(Math.PI * 0.5), new Vector2(0,0), SpriteEffects.None, 1f);
                    break;
                    case Level.Direction.Left:
                    spriteBatch.Draw(arrow, new Rectangle(0, Globals.SCREENHEIGHT/2, 180, 96), arrowSourceRectangle[(int)timer%2], Color.White*.35f, 0f, new Vector2(0,0), SpriteEffects.FlipHorizontally, 1f);
                    break;
                    case Level.Direction.Right:
                    spriteBatch.Draw(arrow, new Rectangle(Globals.SCREENWIDTH - 180, Globals.SCREENHEIGHT/2, 180, 96), arrowSourceRectangle[(int)timer%2], Color.White*.35f);
                    break;
                }
            }
        }
    }
}
