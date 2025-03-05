using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Sprint0
{
    public interface ISprite
    {
        void LoadContent(ContentManager content, string assetName, int startX, int startY, int frameWidth, int frameHeight, int frameCount);
        void Update();
        void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects effects = SpriteEffects.None, float rotation = 0f, Vector2? pivot = null, Color? color = null);
    }

    public class StaticSprite : ISprite
    {
        public Texture2D spriteSheet {get; set;}
        private Rectangle frame;

        public void LoadContent(ContentManager content, string assetName, int startX, int startY, int frameWidth, int frameHeight, int frameCount)
        {
            spriteSheet = content.Load<Texture2D>(assetName);
            frame = new Rectangle(startX, startY, frameWidth, frameHeight);
        }

        public void Update() { }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects effects = SpriteEffects.None, float rotation = 0f, Vector2? pivot = null, Color? color = null)
        {
            // Use the center of the frame as the default origin.
            Vector2 origin = pivot ?? new Vector2(frame.Width / 2f, frame.Height / 2f);
            
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

        public void Draw(SpriteBatch spriteBatch, Vector2 position, float scale = 1f, SpriteEffects effects = SpriteEffects.None, float rotation = 0f, Vector2? pivot = null, Color? color = null)
        {
            // Use the center of the frame as the default origin.
            Vector2 origin = pivot ?? new Vector2(frame.Width / 2f, frame.Height / 2f);
            
            spriteBatch.Draw(
                spriteSheet,
                position,
                frame,
                color ?? Color.White,
                rotation, 
                origin, 
                scale,
                effects,
                0f
            );
        }

        }


    public class AnimatedSprite : ISprite
    {
        public Texture2D spriteSheet {get; set;}
        private List<Rectangle> frames;
        private int currentFrame;
        private float frameTime;
        private float timer;
        private bool isDamaged;
        private FrameOrientation orientation;

        public enum FrameOrientation
        {
            Horizontal,
            Vertical
        }

        public AnimatedSprite(float frameTime, FrameOrientation orientation = FrameOrientation.Horizontal)
        {
            this.frameTime = frameTime;
            this.orientation = orientation;
            frames = new List<Rectangle>();
            currentFrame = 0;
            timer = 0f;
            isDamaged = false;
        }

        public void LoadContent(ContentManager content, string assetName, int startX, int startY, int frameWidth, int frameHeight, int frameCount)
        {
            spriteSheet = content.Load<Texture2D>(assetName);
            frames = ExtractFrames(startX, startY, frameWidth, frameHeight, frameCount);
        }

        private List<Rectangle> ExtractFrames(int startX, int startY, int frameWidth, int frameHeight, int frameCount)
        {
            List<Rectangle> frameList = new List<Rectangle>();

            for (int i = 0; i < frameCount; i++)
            {
                int x = orientation == FrameOrientation.Horizontal
                    ? startX + (i * frameWidth)
                    : startX;

                int y = orientation == FrameOrientation.Vertical
                    ? startY + (i * frameHeight)
                    : startY;

                frameList.Add(new Rectangle(x, y, frameWidth, frameHeight));
            }

            return frameList;
        }
        public void AddFrame(int startX, int startY, int frameWidth, int frameHeight)
        {
            frames.Add(new Rectangle(startX, startY, frameWidth, frameHeight)); 
        }

        public void ResetAnimation()
        {
            currentFrame = 0;
            timer = 0f;
        }

        public void Damage()
        {
            isDamaged = true;
        }

        public void SetFrameTime(float frameTime)
        {
            this.frameTime = frameTime;
        }

        public void Update()
        {
            if (frames.Count == 0) return;

            timer += Globals.FRAMETIME;
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
            Vector2 origin = pivot ?? new Vector2(frames[currentFrame].Width / 2, frames[currentFrame].Height / 2);

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

        public void Update() { }

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

