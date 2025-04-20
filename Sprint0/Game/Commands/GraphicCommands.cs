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
                if (parameters.TryGetValue("gameManager", out object gmObj) && gmObj is GameManager gameManager)
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
                if (parameters.TryGetValue("gameManager", out object gmObj) && gmObj is GameManager gameManager)
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
                if (parameters.TryGetValue("gameManager", out object gmObj) && gmObj is GameManager gameManager &&
                    parameters.TryGetValue("sprite", out object spriteObj) && spriteObj is ISprite sprite)
                {
                    gameManager.GetEntity("player").SetSprite(sprite);
                }
            }
        }

        public class UpdateCannonCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("player", out object playerObj) && playerObj is Player player &&
                    parameters.TryGetValue("rotation", out object rotObj) && rotObj is float rotation)
                {
                    player.SetCannonRotation(rotation);
                }
            }
        }

        public class CycleBlockPrevCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("blocks", out object blockObj) && blockObj is Blocks blocks)
                {
                    blocks.CycleBlockPrev();
                }
            }
        }

        public class CycleBlockNextCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("blocks", out object blockObj) && blockObj is Blocks blocks)
                {
                    blocks.CycleBlockNext();
                }
            }
        }

        public class CycleItemPrevCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("pickupItem", out object itemObj) && itemObj is PickupItem pickupItem)
                {
                    pickupItem.CycleItemPrev();
                }
            }
        }

        public class CycleItemNextCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("pickupItem", out object itemObj) && itemObj is PickupItem pickupItem)
                {
                    pickupItem.CycleItemNext();
                }
            }
        }

        public class CycleEnemyNextCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("gameManager", out object gmObj) && gmObj is GameManager gameManager)
                {
                    MobFactory.CycleNextMob(gameManager);
                }
            }
        }

        public class CycleEnemyPrevCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("gameManager", out object gmObj) && gmObj is GameManager gameManager)
                {
                    MobFactory.CyclePreviousMob(gameManager);
                }
            }
        }

 // GraphicCommands.cs
        public class SpawnEffectCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (!parameters.TryGetValue("gameManager", out var gmObj) || !(gmObj is GameManager gameManager))
                    return;
                if (!parameters.TryGetValue("spawnPosition", out var posObj) || !(posObj is Vector2 spawnPosition))
                    return;
                if (!parameters.TryGetValue("effectType", out var typeObj) || !(typeObj is EntityKeys.EffectType effectType))
                    return;

                string effectKey;
                if (parameters.TryGetValue("customKey", out var customObj) && customObj is string ck)
                    effectKey = ck;
                else
                    effectKey = "effect_" + Guid.NewGuid();

                Effect effect = new Effect(gameManager.GetContent(), spawnPosition, effectKey, effectType);
                gameManager.GetEntities().Add(effectKey, effect);

                if (parameters.TryGetValue("followTarget", out var followObj) && followObj is Entity target)
                {
                    Vector2 offset = Vector2.Zero;
                    if (parameters.TryGetValue("offset", out var offsetObj) && offsetObj is Vector2 o)
                        offset = o;
                    effect.AttachTo(target, offset);
                }

                if (parameters.TryGetValue("damagesPlayer", out var damages) && damages is bool dp && dp == false)
                    effect.DisableDamage();

                if (parameters.TryGetValue("rotation", out var rotation) && rotation is float rotate)
                    effect.SetRotation(rotate);
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
