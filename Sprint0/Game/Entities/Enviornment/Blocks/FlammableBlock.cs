using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Sprint0
{
    public class FlammableBlock : BaseBlock, IObtuse, IFlammable, IDestructible, IRigid
    {
        public bool IsDestroyed { get; private set; }
        public bool IsIgnited { get; private set; }

        public FlammableBlock(ContentManager content, EntityKeys.BlockType blockType, float frameTime = 0.3f)
        {
            LoadBlockContent(content, blockType);
        }

        public override void LoadBlockContent(ContentManager content, EntityKeys.BlockType blockType)
        {
            animatedSprite = new AnimatedSprite(frameTime);

            var (x, y, width, height, texture, scale) = GetSpriteCoords(blockType);
            Scale = scale; // this gets aplied in draw
            animatedSprite.LoadContent(content, texture, x, y, width, height, 1);

            spriteWidth = width;
            spriteHeight = height;
            UpdateBounds();
        }

        private (int x, int y, int width, int height, string texture, float scale) GetSpriteCoords(EntityKeys.BlockType blockType)
        {
            return blockType switch
            {
                EntityKeys.BlockType.Barrel => (485, 1523, 80, 99, "2DTanksSprites", 0.7f),
                EntityKeys.BlockType.RedBarrel => (485, 1622, 80, 99, "2DTanksSprites", 0.7f),
                EntityKeys.BlockType.Oil => (524, 1024, 100, 100, "TDTanksAllSprites", 0.7f),
                EntityKeys.BlockType.SmallBarrel => (1016, 510, 40, 56, "TDTanksAllSprites", 1f),
                _ => throw new System.ArgumentException($"Invalid BlockSpriteKey: {blockType}")
            };
        }

        public void Destroy()
        {
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

        public void Ignite()
        {
            if (IsIgnited || string.IsNullOrEmpty(EntityKey)) return;
            IsIgnited = true;

            var effectParams = new Dictionary<string, object>
            {
                { "spawnPosition", position },
                { "effectType", EntityKeys.EffectType.Explosion },
            };
            commandQueue.Enqueue(new CommandRequest("SpawnEffect", effectParams));

            var effectParams2 = new Dictionary<string, object>
            {
                { "spawnPosition", position },
                { "effectType", "fire" },
            };
            commandQueue.Enqueue(new CommandRequest("SpawnEffect", effectParams2));

            commandQueue.Enqueue(new CommandRequest("DestroyEntity", new Dictionary<string, object>
            {
                { "destroyEntity", EntityKey }
            }));
        }
    }
}
