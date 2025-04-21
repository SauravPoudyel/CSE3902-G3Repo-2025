using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Sprint0
{
    public class CliffBlock : BaseBlock, IRigid, ICliff, IObtuse
    {
        public CliffBlock(ContentManager content, EntityKeys.BlockType blockType, float frameTime = 0.3f)
        {
            LoadBlockContent(content, blockType);
            SetFrameTime(frameTime);
        }

        public override void LoadBlockContent(ContentManager content, EntityKeys.BlockType blockType)
        {
            animatedSprite = new Sprite(frameTime);
            var (x, y, width, height, scale) = GetSpriteCoords(blockType);
            Scale = scale;

            animatedSprite.LoadContent(content, "TDTanksAllSprites", x, y, width, height, 1);
            spriteWidth = width;
            spriteHeight = height;
            UpdateBounds();
        }

        private (int x, int y, int width, int height, float scale) GetSpriteCoords(EntityKeys.BlockType blockType)
        {
            return blockType switch
            {
                // Change the texture for the final version, the texture coordinates now are just for visual test.
                EntityKeys.BlockType.CliffHorizontalLeft => (1981, 499, 16, 128, 1f),
                EntityKeys.BlockType.CliffHorizontalRight => (1981, 499, 16, 128, 1f),
                EntityKeys.BlockType.CliffVerticalTop => (1981, 499, 128, 16, 1f),
                EntityKeys.BlockType.CliffVerticalBottom => (1981, 499, 128, 70, 1f),
                _ => throw new System.ArgumentException($"Invalid BlockType: {blockType}")
            };
        }
    }
}