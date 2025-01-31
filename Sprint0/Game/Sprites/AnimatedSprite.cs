using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Sprint0
{
    public class AnimatedSprite : ISprite
    {
        private Texture2D spriteSheet;
        private List<Rectangle> frames;
        private int currentFrame;
        private float frameTime;
        private float timer;

        public AnimatedSprite(float frameTime)
        {
            this.frameTime = frameTime;
            frames = new List<Rectangle>();
            currentFrame = 0;
            timer = 0f;
        }

        public void LoadContent(ContentManager content, string assetName, int startX, int startY, int frameWidth, int frameHeight, int frameCount)
        {
            spriteSheet = content.Load<Texture2D>(assetName);
            frames = SpriteManager.ExtractFrames(startX, startY, frameWidth, frameHeight, frameCount);
        }

        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer > frameTime)
            {
                currentFrame = (currentFrame + 1) % frames.Count;
                timer = 0f;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(spriteSheet, position, frames[currentFrame], Color.White);
        }
    }
}
