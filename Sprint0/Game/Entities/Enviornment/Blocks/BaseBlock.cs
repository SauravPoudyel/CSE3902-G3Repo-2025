using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0
{
    public enum BlockSpriteKey
    {
        Tree,
        Box,
        OilBarrel_Red,
        OilBarrel_Black,
        BarbedFence,
        Oil,
    }

    public abstract class BaseBlock : Entity
    {
        protected AnimatedSprite animatedSprite;
        protected float frameTime = 0.3f; // Default animation frame time.

        public void SetFrameTime(float newFrameTime)
        {
            frameTime = newFrameTime;
            if (animatedSprite != null)
                animatedSprite.SetFrameTime(frameTime);
        }

        public abstract void LoadBlockContent(ContentManager content, BlockSpriteKey spriteKey);
        
        public override void Update()
        {
            animatedSprite?.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            animatedSprite?.Draw(spriteBatch, position, SpriteEffects.None, 0f);
        }
    }

}
