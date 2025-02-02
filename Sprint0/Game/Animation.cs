using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint0
{
    public class Animation
    {
        public Texture2D Texture { get; } // 添加Texture属性
        public Rectangle[] Frames { get; }
        public float FrameDuration { get; }
        public bool IsLooping { get; set; } = true;
        public SpriteEffects DefaultEffects { get; set; } // 新增默认翻转效果
        public Vector2 FrameOffset { get; set; }

        public Animation(Texture2D texture, int startX, int startY,
                    int frameWidth, int frameHeight,
                    int frameCount, float frameDuration,
                    SpriteEffects effects = SpriteEffects.None, Vector2 frameOffset = default)
        {
            Texture = texture; // 保存texture引用
            FrameDuration = frameDuration;
            DefaultEffects = effects;
            Frames = new Rectangle[frameCount];
            FrameOffset = frameOffset;

            for (int i = 0; i < frameCount; i++)
            {
                Frames[i] = new Rectangle(
                    startX + i * frameWidth,
                    startY,
                    frameWidth,
                    frameHeight
                );
            }
        }
    }

    public class AnimationManager
    {
        private Dictionary<string, Animation> animations = new();
        public Animation currentAnimation;
        private float timer;
        public int currentFrame;

        public void AddAnimation(string name, Animation animation)
        {
            animations[name] = animation;
        }

        public void Play(string name)
        {
            if (currentAnimation == animations[name]) return;

            currentAnimation = animations[name];
            currentFrame = 0;
            timer = 0;
        }

        public void Update(GameTime gameTime)
        {
            if (currentAnimation == null) return;

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer >= currentAnimation.FrameDuration)
            {
                currentFrame++;
                if (currentFrame >= currentAnimation.Frames.Length)
                {
                    if (currentAnimation.IsLooping)
                        currentFrame = 0;
                    else
                        currentFrame = currentAnimation.Frames.Length - 1;
                }
                timer = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position,
                Color color, float rotation, Vector2 origin,
                float scale, SpriteEffects effects = SpriteEffects.None)
        {
            if (currentAnimation == null || currentAnimation.Texture == null) return;

            var frame = currentAnimation.Frames[currentFrame];
            var combinedEffects = currentAnimation.DefaultEffects | effects;
            position += currentAnimation.FrameOffset;

            spriteBatch.Draw(
                texture: currentAnimation.Texture,
                position: position,
                sourceRectangle: frame,
                color: color,
                rotation: rotation,
                origin: origin,
                scale: scale,
                effects: combinedEffects, // 合并默认效果和临时效果
                layerDepth: 0f
            );
        }
    }
}
