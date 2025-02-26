using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0
{
    public class FlammableBlock : BaseBlock, IObtuse, IFlammable, IDestructible
    {
        public bool IsDestroyed { get; private set; }

        public FlammableBlock(ContentManager content, BlockSpriteKey spriteKey, float frameTime = 0.3f)
        {
            LoadBlockContent(content, spriteKey);
            SetFrameTime(frameTime);
        }

        public override void LoadBlockContent(ContentManager content, BlockSpriteKey spriteKey)
        {
            animatedSprite = new AnimatedSprite(frameTime);

            switch (spriteKey)
            {
                case BlockSpriteKey.OilBarrel_Red:
                    animatedSprite.LoadContent(content, "2DTanksSprites", 485, 1523, 80, 99, 1);
                    bounds = new Rectangle((int)position.X, (int)position.Y, 80, 99);
                    break;

                case BlockSpriteKey.OilBarrel_Black:
                    animatedSprite.LoadContent(content, "2DTanksSprites", 485, 1622, 80, 99, 1);
                    bounds = new Rectangle((int)position.X, (int)position.Y, 100, 100);
                    break;

                case BlockSpriteKey.Oil:
                    animatedSprite.LoadContent(content, "TDTanksAllSprites", 524, 1024, 100, 100, 1);
                    bounds = new Rectangle((int)position.X, (int)position.Y, 100, 100);
                    break;

                default:
                    throw new System.ArgumentException($"Invalid BlockSpriteKey: {spriteKey}");
            }
        }

        public void Destroy()
        {
            IsDestroyed = true;
        }

        public void Ignite()
        {
            // Ignite logic (spawn fire, etc.)
        }
    }
}
