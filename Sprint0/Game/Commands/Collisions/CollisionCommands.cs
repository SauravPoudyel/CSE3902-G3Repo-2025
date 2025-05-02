using System;
using System.Collections.Generic;
using static Sprint0.EntityKeys;

namespace Sprint0
{
    public class CollisionCommands
    {
        public class CollisionStopCommand : ICommand
        {
            private static readonly Dictionary<Type, Action<object, Entity>> CollisionLogicMap =
                new Dictionary<Type, Action<object, Entity>>
                {
                    { typeof(Player), (actor, target) => CollisionCommandsLogic_Stop.HandlePlayerCollision((Player)actor, target) },
                    { typeof(Mob), (actor, target) => CollisionCommandsLogic_Stop.HandleMobCollision((Mob)actor, target) },
                    { typeof(PushableBlock), (actor, target) => CollisionCommandsLogic_Stop.HandleBlockCollision((PushableBlock)actor, target)
                    },
                };

            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("actor", out var actorObj) &&
                    parameters.TryGetValue("target", out var targetObj) && targetObj is Entity target)
                {
                    var actorType = actorObj.GetType();
                    if (CollisionLogicMap.TryGetValue(actorType, out var handler))
                        handler(actorObj, target);
                }
            }
        }

        public class CollisionProjectileDestroyCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("actor", out var actorObj) && actorObj is Projectile projectile &&
                    parameters.TryGetValue("gameManager", out var gmObj) && gmObj is GameManager gameManager &&
                    parameters.TryGetValue("target", out var targetObj) && targetObj is Entity target)
                {
                    CollisionHandler.HandleProjectileDestroy(projectile, target);
                }
            }
        }

        public class CollisionProjectileReflectCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("actor", out var actorObj) && actorObj is Projectile projectile &&
                    parameters.TryGetValue("target", out var targetObj) && targetObj is Entity target)
                {
                    CollisionHandler.HandleProjectileReflect(projectile, target);
                }
            }
        }

        public class CollisionPickUpCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("actor", out var actorObj) && actorObj is Player player &&
                    parameters.TryGetValue("gameManager", out var gmObj) && gmObj is GameManager gameManager &&
                    parameters.TryGetValue("target", out var targetObj))
                {
                    CollisionHandler.HandlePickup(player, targetObj, gameManager);
                }
            }
        }

        public class CollisionHurtCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters["target"] is Effect effect &&
                    parameters["actor"] is Player player)
                {
                    if (effect.effectType == EffectType.Explosion && !effect.didDamage)
                    {
                        player.ChangeHealth(-50);
                        effect.didDamage = true;
                    }
                    else if (effect.effectType == EffectType.Fire)
                    {
                        if (!player.activeFireEffects.Contains(effect))
                            player.activeFireEffects.Add(effect);
                    }
                }
            }
        }

        public class FireCollisionExitCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters["target"] is Player player &&
                    parameters["actor"] is Effect effect &&
                    effect.effectType == EffectType.Fire)
                {
                    player.activeFireEffects.Remove(effect);
                }
            }
        }

        public class CollisionPushCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("actor", out var actorObj) && actorObj is Player player &&
                    parameters.TryGetValue("target", out var targetObj) && targetObj is IPushable pushable)
                {
                    CollisionHandler.HandlePush(player, pushable);
                }
            }
        }

        public class DestroyFlammableCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("target", out var targetObj) && targetObj is IFlammable flammableBlock)
                {
                    flammableBlock.Destroy();
                    flammableBlock.Ignite();
                }
            }
        }

        public class DestroyDestructibleCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("target", out var targetObj) && targetObj is IDestructible destructibleBlock &&
                    parameters.TryGetValue("actor", out var actorObj) && actorObj is MineProjectile)
                {
                    destructibleBlock.Destroy();
                }
            }
        }
    }
}
