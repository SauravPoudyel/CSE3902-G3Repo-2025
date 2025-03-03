using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0
{
    public class RigidBlock : BaseBlock, IRigid, IObtuse
    {
        public RigidBlock(ContentManager content, BlockSpriteKey spriteKey, float frameTime = 0.3f)
        {
            LoadBlockContent(content, spriteKey);
            SetFrameTime(frameTime);
        }

        public override void LoadBlockContent(ContentManager content, BlockSpriteKey spriteKey)
        {
            animatedSprite = new AnimatedSprite(frameTime);

            switch (spriteKey)
            {
                case BlockSpriteKey.Tree:
                    animatedSprite.LoadContent(content, "TDTanksAllSprites", 128, 0, 128, 128, 1);
                    bounds = new Rectangle((int)position.X, (int)position.Y, 115, 115);
                    break;

                case BlockSpriteKey.BarbedFence:
                    animatedSprite.LoadContent(content, "TDTanksAllSprites", 958, 1048, 56, 56, 1);
                    bounds = new Rectangle((int)position.X, (int)position.Y, 52, 52);
                    break;

                default:
                    throw new System.ArgumentException($"Invalid BlockSpriteKey: {spriteKey}");
            }
        }
    }
}
