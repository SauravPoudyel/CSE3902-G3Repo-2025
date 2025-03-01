using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Sprint0
{
    public class FlammableBlock : BaseBlock, IObtuse, IFlammable, IDestructible
    {
        public bool IsDestroyed { get; private set; }

        public bool IsIgnited { get; private set; }
        private string _entityKey;
        public void SetEntityKey(string key) => _entityKey = key;

        public FlammableBlock(ContentManager content, BlockSpriteKey spriteKey, float frameTime = 0.3f)
        {
            LoadBlockContent(content, spriteKey);
        }

        public void Destroy()
        {
            if (IsDestroyed || string.IsNullOrEmpty(EntityKey)) return;
            IsDestroyed = true;

            var effectParams = new Dictionary<string, object>
            {
                { "spawnPosition", position },
                { "effectType", "explosion" },
            };
            commandQueue.Enqueue(new CommandRequest("SpawnEffect", effectParams));

            commandQueue.Enqueue(new CommandRequest("DestroyEntity", new Dictionary<string, object>
            {
                { "destroyEntity", EntityKey }
            }));
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

        public void Ignite()
        {
            if (IsIgnited || string.IsNullOrEmpty(_entityKey)) return;
            IsIgnited = true;

            var effectParams = new Dictionary<string, object>
            {
                { "spawnPosition", position },
                { "effectType", "fire" },
            };
            commandQueue.Enqueue(new CommandRequest("SpawnEffect", effectParams));

            commandQueue.Enqueue(new CommandRequest("DestroyEntity", new Dictionary<string, object>
            {
                { "destroyEntity", _entityKey }
            }));
        }
    }
}
