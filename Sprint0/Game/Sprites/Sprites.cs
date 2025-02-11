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
        void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects effects = SpriteEffects.None, float rotation = 0f, Vector2? pivot = null, Color? color = null);
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

        public void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects effects = SpriteEffects.None, float rotation = 0f, Vector2? pivot = null, Color? color = null)
        {
            Vector2 origin = pivot ?? new Vector2(14, 50); // Default to center if pivot is not provided

            spriteBatch.Draw(
                spriteSheet,
                position,
                frame,
                color ?? Color.White,
                rotation, 
                origin, 
                1f,
                effects,
                0f
            );
        }
    }

    public class AnimatedSprite : ISprite
    {
        private Texture2D spriteSheet;
        private List<Rectangle> frames;
        private int currentFrame;
        private float frameTime;
        private float timer;
        private bool isDamaged;

        public AnimatedSprite(float frameTime)
        {
            this.frameTime = frameTime;
            frames = new List<Rectangle>();
            currentFrame = 0;
            timer = 0f;
            isDamaged = false;
        }

        public void LoadContent(ContentManager content, string assetName, int startX, int startY, int frameWidth, int frameHeight, int frameCount)
        {
            spriteSheet = content.Load<Texture2D>(assetName);
            frames = SpriteManager.ExtractFrames(startX, startY, frameWidth, frameHeight, frameCount);
        }

        public void Damage()
        {
            isDamaged = true;
        }

        public void Update(GameTime gameTime)
        {
            if (frames.Count == 0) return;

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer > frameTime)
            {
                currentFrame = (currentFrame + 1) % frames.Count;
                isDamaged = false;
                timer = 0f;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects effects = SpriteEffects.None, float rotation = 0f, Vector2? pivot = null, Color? color = null)
        {
            if (frames.Count == 0) return;

            Color drawColor = isDamaged ? Color.Red : color ?? Color.White;
            Vector2 origin = pivot ?? new Vector2(frames[currentFrame].Width / 2, frames[currentFrame].Height / 2); // Default to center

            spriteBatch.Draw(
                spriteSheet,
                position,
                frames[currentFrame],
                drawColor,
                rotation, 
                origin, 
                1f,
                effects,
                0f
            );
        }
    }

    public class TextSprite : ISprite
    {
        private string text;
        private SpriteFont font;
        private Color color;
        private float scale;

        public TextSprite(string text, Color color, float scale = 1f)
        {
            this.text = text;
            this.color = color;
            this.scale = scale;
        }

        public void LoadContent(ContentManager content, string assetName, int startX, int startY, int frameWidth, int frameHeight, int frameCount)
        {
            font = content.Load<SpriteFont>(assetName);
        }

        public void SetText(string text)
        {
            this.text = text;
        }

        public void Update(GameTime gameTime) { }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects effects = SpriteEffects.None, float rotation = 0f, Vector2? pivot = null, Color? color = null)
        {
            if (font != null && !string.IsNullOrEmpty(text))
            {
                Vector2 origin = pivot ?? font.MeasureString(text) / 2; // Default to center of text

                spriteBatch.DrawString(
                    font,
                    text,
                    position,
                    color ?? this.color,
                    rotation,
                    origin,
                    scale,
                    effects,
                    0f
                );
            }
        }
    }
}
