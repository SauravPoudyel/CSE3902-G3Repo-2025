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
        RockPile,
        Factory,
        RockPileVar1,
        RockPileVar2,
        Hosue,
        House2,
        SmallTree,
        Fence,
        DeadTree,
        Garage,
        CoconutTree,
        SmallBarrel,
    }

    public abstract class BaseBlock : Entity
    {
        protected AnimatedSprite animatedSprite;
        protected float frameTime = 0.3f;

        public void SetFrameTime(float newFrameTime)
        {
            frameTime = newFrameTime;
            if (animatedSprite != null)
            {
                animatedSprite.SetFrameTime(frameTime);
            }
        }

        public abstract void LoadBlockContent(ContentManager content, BlockSpriteKey spriteKey);

        public override void Update()
        {
            prevPosition = position;
            if (animatedSprite != null)
            {
                animatedSprite.Update();
            }
            position += velocity * Globals.FRAMETIME;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (animatedSprite != null)
            {
                animatedSprite.Draw(spriteBatch, position, SpriteEffects.None, 0f);
            }
        }
    }
}
