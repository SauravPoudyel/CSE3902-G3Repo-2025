using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Sprint0
{
    public interface ISprite
    {
        void LoadContent(ContentManager content, string assetName, int startX, int startY, int frameWidth, int frameHeight, int frameCount);
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects effects = SpriteEffects.None);
    }

    public class StaticSprite : ISprite
    {
        private Texture2D spriteSheet;
        private Rectangle frame;

        public void LoadContent(ContentManager content, string assetName, int startX, int startY, int frameWidth, int frameHeight, int frameCount)
        {
            spriteSheet = content.Load<Texture2D>(assetName);
            frame = new Rectangle(startX, startY, frameWidth, frameHeight);
        }

        public void Update(GameTime gameTime) { }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects effects = SpriteEffects.None)
        {
            spriteBatch.Draw(spriteSheet, position, frame, Color.White, 0f, Vector2.Zero, 1f, effects, 0f);
        }
    }

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

        public void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects effects = SpriteEffects.None)
        {
            spriteBatch.Draw(spriteSheet, position, frames[currentFrame], Color.White, 0f, Vector2.Zero, 1f, effects, 0f);
        }
    }

    public class TextSprite : ISprite
    {
        private string text;
        private SpriteFont font;
        private Vector2 position;
        private Color color;

        public TextSprite(Vector2 position, string text, Color color)
        {
            this.position = position;
            this.text = text;
            this.color = color;
        }

        public void LoadContent(ContentManager content, string assetName, int startX, int startY, int frameWidth, int frameHeight, int frameCount)
        {
            font = content.Load<SpriteFont>(assetName);
        }

        public void SetText(string text)
        {
            this.text = text;
        }

        public void Update(GameTime gameTime) {}

        public void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects effects = SpriteEffects.None)
        {
            if (font != null && !string.IsNullOrEmpty(text))
            {
                spriteBatch.DrawString(font, text, position, color);
            }
        }
    }
}
