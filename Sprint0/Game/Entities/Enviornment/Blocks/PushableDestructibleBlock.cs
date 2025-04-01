using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint0
{
    public class PushableDestructibleBlock : BaseBlock, IObtuse, IPushable, IDestructible
    {
        private Vector2 velocity;
        private const float Friction = 0.9f;
        private const float PushForce = 256f;
        public bool IsDestroyed { get; private set; }

        public PushableDestructibleBlock(ContentManager content, EntityKeys.BlockType blockType, float frameTime = 0.3f)
        {
            animatedSprite = new AnimatedSprite(frameTime);
            LoadBlockContent(content, blockType);
            SetFrameTime(frameTime);
        }

        public override void LoadBlockContent(ContentManager content, EntityKeys.BlockType blockType)
        {
            if (animatedSprite == null)
                animatedSprite = new AnimatedSprite(frameTime);

            var (x, y, width, height, texture, scale) = GetSpriteCoords(blockType);
            Scale = scale;
            animatedSprite.LoadContent(content, texture, x, y, width, height, 1);

            spriteHeight = height;
            spriteWidth = width;
            UpdateBounds();
        }

        private (int x, int y, int width, int height, string texture, float scale) GetSpriteCoords(EntityKeys.BlockType blockType)
        {
            return blockType switch
            {
                EntityKeys.BlockType.SmallBarrel => (1016, 510, 40, 56, "TDTanksAllSprites", 1f),
                _ => throw new System.ArgumentException($"Invalid BlockSpriteKey: {blockType}")
            };
        }

        public void Destroy()
        {
            Destroy(null);
        }

        public void Destroy(string source)
        {
            if (source != "Mine" && source != null) return;

            if (IsDestroyed || string.IsNullOrEmpty(EntityKey)) return;
            IsDestroyed = true;

            var effectParams = new Dictionary<string, object>
            {
                { "spawnPosition", position },
                { "effectType", EntityKeys.EffectType.Explosion },
            };
            commandQueue.Enqueue(new CommandRequest("SpawnEffect", effectParams));

            commandQueue.Enqueue(new CommandRequest("DestroyEntity", new Dictionary<string, object>
            {
                { "destroyEntity", EntityKey }
            }));
        }

        public void Push(Vector2 direction)
        {
            if(IsDestroyed || direction == Vector2.Zero) return;

            direction.Normalize();
            velocity = direction * PushForce;
        }

        public override void Update()
        {
            if (IsDestroyed) return;

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
