using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0
{
    public class PushableBlock : BaseBlock, IObtuse, IPushable
    {
        public PushableBlock(ContentManager content, BlockSpriteKey spriteKey, float frameTime = 0.3f)
        {
            LoadBlockContent(content, spriteKey);
            SetFrameTime(frameTime);
        }

        public override void LoadBlockContent(ContentManager content, BlockSpriteKey spriteKey)
        {
            animatedSprite = new AnimatedSprite(frameTime);

            switch (spriteKey)
            {
                case BlockSpriteKey.Box:
                    animatedSprite.LoadContent(content, "TDTanksAllSprites", 960, 753, 56, 56, 1);
                    bounds = new Rectangle((int)position.X, (int)position.Y, 56, 56);
                    break;

                default:
                    throw new System.ArgumentException($"Invalid BlockSpriteKey: {spriteKey}");
            }
        }
    }
}
