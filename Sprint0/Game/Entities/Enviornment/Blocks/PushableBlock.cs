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

        public void Push(Vector2 direction)
        {
            direction.Normalize();
            velocity = direction * PushForce;
        }

        public override void Update()
        {
            ApplyMovement();
            base.Update();
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
