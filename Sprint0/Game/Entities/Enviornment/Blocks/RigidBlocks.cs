using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0
{
    public class RigidBlock : BaseBlock, IRigid, IObtuse
    {
        public RigidBlock(ContentManager content, EntityKeys.BlockType blockType, float frameTime = 0.3f)
        {
            LoadBlockContent(content, blockType);
            SetFrameTime(frameTime);
        }

        public override void LoadBlockContent(ContentManager content, EntityKeys.BlockType blockType)
        {
            animatedSprite = new AnimatedSprite(frameTime);
            var (x, y, width, height, scale) = GetSpriteCoords(blockType);
            Scale = scale; // this gets aplied in draw
            animatedSprite.LoadContent(content, "TDTanksAllSprites", x, y, width, height, 1);
            spriteWidth = width;
            spriteHeight = height;
            UpdateBounds();
        }

        private (int x, int y, int width, int height, float scale) GetSpriteCoords(EntityKeys.BlockType blockType)
        {
            return blockType switch
            {
                EntityKeys.BlockType.Tree => (128, 0, 128, 128, 1f),
                EntityKeys.BlockType.BarbedFence => (958, 1048, 56, 56, 1f),
                EntityKeys.BlockType.RockPile => (950, 1593, 76, 70, 1f),
                EntityKeys.BlockType.RockPileVar1 => (950, 1523, 76, 70, 1f),
                EntityKeys.BlockType.RockPileVar2 => (950, 1453, 76, 70, 1f),
                EntityKeys.BlockType.Factory => (766, 1768, 104, 96, 1.5f),
                EntityKeys.BlockType.House => (862, 1431, 88, 96, 1.5f),
                EntityKeys.BlockType.House2 => (871, 1128, 88, 120, 1.5f),
                EntityKeys.BlockType.SmallTree => (954, 362, 72, 72, 1f),
                EntityKeys.BlockType.Fence => (2159, 1095, 128, 48, 1f),
                EntityKeys.BlockType.DeadTree => (128, 512, 128, 128, 1f),
                EntityKeys.BlockType.Garage => (869, 1864, 88, 120, 1.5f),
                EntityKeys.BlockType.CoconutTree => (774, 2748, 120, 120, 1f),
                EntityKeys.BlockType.Boarder => (2872, 0, 120, 120, 1f),
                _ => throw new System.ArgumentException($"Invalid BlockSpriteKey: {blockType}")
            };
        }
    }
}
