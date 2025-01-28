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
        void Draw(SpriteBatch spriteBatch);
    }


    public static class SpriteManager
    {
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

    public class StaticSprite : ISprite
    {
        private Texture2D spriteSheet;
        private Rectangle frame;
        private Vector2 position;

        public StaticSprite(Vector2 position)
        {
            this.position = position;
        }

        public void LoadContent(ContentManager content, string assetName, int startX, int startY, int frameWidth, int frameHeight, int frameCount)
        {
            spriteSheet = content.Load<Texture2D>(assetName);
            frame = new Rectangle(startX, startY, frameWidth, frameHeight);
        }

        public void Update(GameTime gameTime) {}

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteSheet, position, frame, Color.White);
        }
    }

    public class MovingSprite : ISprite
    {
        private Texture2D spriteSheet;
        private Rectangle frame;
        private Vector2 position;
        private float speed;

        public MovingSprite(Vector2 position, float speed)
        {
            this.position = position;
            this.speed = speed;
        }

        public void LoadContent(ContentManager content, string assetName, int startX, int startY, int frameWidth, int frameHeight, int frameCount)
        {
            spriteSheet = content.Load<Texture2D>(assetName);
            frame = new Rectangle(startX, startY, frameWidth, frameHeight);
        }

        public void Update(GameTime gameTime)
        {
            position.X += speed;
            if (position.X >= Globals.SCREENWIDTH)
            {
                position.X = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteSheet, position, frame, Color.White);
        }
    }

    public class AnimatedSprite : ISprite
    {
        private Texture2D spriteSheet;
        private List<Rectangle> frames;
        private int currentFrame;
        private float frameTime;
        private float timer;
        private Vector2 position;

        public AnimatedSprite(Vector2 position, float frameTime)
        {
            this.position = position;
            this.frameTime = frameTime;
            frames = new List<Rectangle>();
            currentFrame = 0;
        }

        public void LoadContent(ContentManager content, string assetName, int startX, int startY, int frameWidth, int frameHeight, int frameCount)
        {
            spriteSheet = content.Load<Texture2D>(assetName);
            frames = SpriteManager.ExtractFrames(startX, startY, frameWidth, frameHeight, frameCount);
        }

        public void Update(GameTime gameTime)
        {
            // Cycle frames 
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer >= frameTime)
            {
                timer -= frameTime;
                currentFrame++;
                if (currentFrame >= frames.Count)
                {
                    currentFrame = 0;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteSheet, position, frames[currentFrame], Color.White);
        }
    }

    public class MovingAnimatedSprite : ISprite
    {
        private Texture2D spriteSheet;
        private List<Rectangle> frames;
        private int currentFrame;
        private float frameTime;
        private float timer;
        private Vector2 position;
        private float speed;

        public MovingAnimatedSprite(Vector2 position, float speed, float frameTime)
        {
            this.position = position;
            this.speed = speed;
            this.frameTime = frameTime;
            frames = new List<Rectangle>();
            currentFrame = 0;
        }

        public void LoadContent(ContentManager content, string assetName, int startX, int startY, int frameWidth, int frameHeight, int frameCount)
        {
            spriteSheet = content.Load<Texture2D>(assetName);
            frames = SpriteManager.ExtractFrames(startX, startY, frameWidth, frameHeight, frameCount);
        }

        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer >= frameTime)
            {
                // Cycle frames 
                timer -= frameTime;
                currentFrame++;
                if (currentFrame >= frames.Count)
                {
                    currentFrame = 0;
                }
            }
            position.X += speed;
            if (position.X >= Globals.SCREENWIDTH)
            {
                position.X = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteSheet, position, frames[currentFrame], Color.White);
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

        public void LoadContent(ContentManager content, string assetName)
        {
            font = content.Load<SpriteFont>(assetName);
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

        public void Draw(SpriteBatch spriteBatch)
        {
            if (font != null && !string.IsNullOrEmpty(text))
            {
                spriteBatch.DrawString(font, text, position, color);
            }
        }
    }

}
