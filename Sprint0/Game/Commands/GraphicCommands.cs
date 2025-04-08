using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Sprint0
{
    public static class GraphicCommands
    {
        public class DisplayStaticGameCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("gameManager", out object value) && value is GameManager gameManager)
                {
                    var staticSprite = new StaticSprite();
                    staticSprite.LoadContent(gameManager.GetContent(), "LinkSpritesheet", 140, 2, 62, 62, 1);
                    gameManager.GetEntity("player").SetSprite(staticSprite);
                }
            }
        }

        public class DisplayAnimatedGameCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("gameManager", out object value) && value is GameManager gameManager)
                {
                    var animatedSprite = new AnimatedSprite(0.4f);
                    animatedSprite.LoadContent(gameManager.GetContent(), "LinkSpritesheet", 140, 2, 62, 62, 2);
                    gameManager.GetEntity("player").SetSprite(animatedSprite);
                }
            }
        }

        public class SetSpriteCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("gameManager", out object value) && value is GameManager gameManager &&
                    parameters.TryGetValue("sprite", out object value) && value is ISprite sprite)
                {
                    gameManager.GetEntity("player").SetSprite(sprite);
                }
            }
        }

        public class UpdateCannonCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("player", out object value) && value is Player player &&
                    parameters.TryGetValue("rotation", out object value) && value is float rotation)
                {
                    player.SetCannonRotation(rotation);
                }
            }
        }

        public class CycleBlockPrevCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("blocks", out object value) && value is Blocks blocks)
                {
                    blocks.CycleBlockPrev();
                }
            }
        }

        public class CycleBlockNextCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("blocks", out object value) && value is Blocks blocks)
                {
                    blocks.CycleBlockNext();
                }
            }
        }

        public class CycleItemPrevCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                // Implement the logic to cycle to the previous item
                if (parameters.TryGetValue("pickupItem", out object value) && value is PickupItem pickupItem)
                {
                    pickupItem.CycleItemPrev();
                }
            }
        }

        public class CycleItemNextCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("pickupItem", out object value) && value is PickupItem pickupItem)
                {
                    pickupItem.CycleItemNext();
                }
            }
        }

        public class CycleEnemyNextCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("gameManager", out object value) && value is GameManager gameManager)
                {
                    MobFactory.CycleNextMob(gameManager);
                }
            }
        }

        public class CycleEnemyPrevCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("gameManager", out object value) && value is GameManager gameManager)
                {
                    MobFactory.CyclePreviousMob(gameManager);
                }
            }
        }

        public class SpawnEffectCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                // Using TryGetValue to safely extract the parameters
                if (!parameters.TryGetValue("gameManager", out var gmObj) ||
                    !(gmObj is GameManager gameManager))
                {
                    return;
                }

                if (!parameters.TryGetValue("spawnPosition", out var posObj) ||
                    !(posObj is Vector2 spawnPosition))
                {
                    return;
                }

                if (!parameters.TryGetValue("effectType", out var typeObj) ||
                    !(typeObj is EntityKeys.EffectType effectType))
                {
                    return;
                }

                string effectKey = "effect_" + Guid.NewGuid().ToString();
                Effect effect = new Effect(gameManager.GetContent(), spawnPosition, effectKey, effectType);
                gameManager.GetEntities().Add(effectKey, effect);

                // Play the sound
                if (effectType == EntityKeys.EffectType.Explosion)
                {
                    AudioManager.PlaySound(AudioManager.SoundKey.Explosion);
                }

                // Mine range detection logic
                if (parameters.TryGetValue("explosionSource", out var sourceObj) &&
                    sourceObj as string == "Mine" &&
                    parameters.TryGetValue("explosionRadius", out var radiusObj) &&
                    radiusObj is float radius)
                {
                    DetectDestructibleBlocks(spawnPosition, radius, gameManager);
                }
            }

            private void DetectDestructibleBlocks(Vector2 center, float radius, GameManager gm)
            {
                float radiusSquared = radius * radius;

                foreach (var entity in gm.GetEntities().Values)
                {
                    if (entity is PushableDestructibleBlock block &&
                        !block.IsDestroyed &&
                        IsInExplosionRange(block.GetPosition(), center, radiusSquared))
                    {
                        block.Destroy("Mine");
                    }

                    if (entity is FlammableBlock block1 &&
                        !block1.IsDestroyed &&
                        IsInExplosionRange(block1.GetPosition(), center, radiusSquared))
                    {
                        block1.Destroy("Mine");
                        block1.Ignite();
                    }
                }
            }

            private bool IsInExplosionRange(Vector2 blockPos, Vector2 explosionCenter, float radiusSquared)
            {
                Vector2 offset = blockPos - explosionCenter;
                return offset.LengthSquared() <= radiusSquared;
            }
        }
    }
}