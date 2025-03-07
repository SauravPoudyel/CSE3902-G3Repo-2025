using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0
{
    public class PushableBlock : BaseBlock, IObtuse, IPushable
    {
        private Vector2 velocity;
        private const float Friction = 0.9f;
        private const float PushForce = 256f;

        public PushableBlock(ContentManager content, EntityKeys.BlockType blockType, float frameTime = 0.3f)
        {
            animatedSprite = new AnimatedSprite(frameTime);
            LoadBlockContent(content, blockType);
            SetFrameTime(frameTime);
        }

        public override void LoadBlockContent(ContentManager content, EntityKeys.BlockType blockType)
        {
            if (animatedSprite == null)
                animatedSprite = new AnimatedSprite(frameTime);

            var (x, y, width, height) = GetSpriteCoords(blockType);
            animatedSprite.LoadContent(content, "TDTanksAllSprites", x, y, width, height, 1);
            spriteHeight = height;
            spriteWidth = width;
            UpdateBounds(); 
        }

        private (int x, int y, int width, int height) GetSpriteCoords(EntityKeys.BlockType blockType)
        {
            return blockType switch
            {
                EntityKeys.BlockType.Box => (960, 753, 56, 56),
                EntityKeys.BlockType.SmallBarrel => (1016, 510, 40, 56),
                _ => throw new System.ArgumentException($"Invalid BlockSpriteKey: {blockType}")
            };
        }

        public void Push(Vector2 direction)
        {
            direction.Normalize();
            velocity = direction * PushForce;
        }

        public override void Update()
        {
            base.Update();
            ApplyMovement();
        }

        private void ApplyMovement()
        {
            position += velocity * Globals.FRAMETIME;
            velocity *= Friction;

            if (velocity.Length() < 0.5f)
                velocity = Vector2.Zero;

            bounds.Location = position.ToPoint();
        }
    }
}
