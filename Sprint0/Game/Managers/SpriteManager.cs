using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Sprint0
{
    public class SpriteManager
    {
        private ISprite currentSprite;

        public void SetSprite(ISprite sprite)
        {
            currentSprite = sprite;
        }

        public void Update(GameTime gameTime)
        {
            if (currentSprite != null)
            {
                currentSprite.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (currentSprite != null)
            {
                currentSprite.Draw(spriteBatch, Vector2.Zero);
            }
        }
        public static List<Rectangle> ExtractFrames(int startX, int startY, int frameWidth, int frameHeight, int frameCount)
        {
            var frames = new List<Rectangle>();
            for (int i = 0; i < frameCount; i++)
            {
                frames.Add(new Rectangle(startX + i * frameWidth, startY, frameWidth, frameHeight));
            }
            return frames;
        }
    }
}