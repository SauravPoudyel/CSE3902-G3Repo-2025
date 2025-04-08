using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using static Sprint0.EntityKeys;

namespace Sprint0
{
    public class CollisionCommands
    {
        public class CollisionStopCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("actor", out object actorObj1) && actorObj1 is Player player &&
                    parameters.TryGetValue("target", out object targetObj1) && targetObj1 is Entity target)
                {
                    if (target is IRigid || (target is Mob mob && !(mob is Plane || mob is HoveringTank)))
                    {
                        Vector2 bounceDir = CollisionHandler.CalculateBounceDirection(player, target, player.bodyRotation);
                        float bounceOffset = 70f * Globals.FRAMETIME;
                        player.SetPosition(player.GetPosition() + bounceDir * bounceOffset);
                        player.SetVelocity(Vector2.Zero);

                        if (target is Mob mobTarget && !(mobTarget is Plane || mobTarget is HoveringTank))
                        {
                            CollisionHandler.ResolveCollision(mobTarget, player);
                            mobTarget.SetVelocity(Vector2.Zero);
                        }
                    }
                }
                else if (parameters.TryGetValue("actor", out object actorObj2) && actorObj2 is Mob mobActor &&
                         parameters.TryGetValue("target", out object targetObj2) && targetObj2 is Entity target2)
                {
                    if (mobActor is Plane)
                        return;

                    if (target2 is IRigid || target2 is Blocks ||
                        (target2 is Mob mobTarget && !(mobTarget is Plane || mobTarget is HoveringTank)))
                    {
                        CollisionHandler.ResolveCollision(mobActor, target2);
                        mobActor.SetVelocity(Vector2.Zero);
                        if (target2 is Mob mobOther && !(mobOther is Plane))
                        {
                            CollisionHandler.ResolveCollision(mobOther, mobActor);
                            mobOther.SetVelocity(Vector2.Zero);
                        }
                    }
                }
                else if (parameters.TryGetValue("actor", out object actorObj3) && actorObj3 is PushableBlock pushableBlock &&
                         parameters.TryGetValue("target", out object targetObj3) && targetObj3 is Entity target3)
                {
                    if (target3 is IRigid || target3 is Mob)
                    {
                        CollisionHandler.ResolveCollision(pushableBlock, target3);
                        pushableBlock.SetVelocity(Vector2.Zero);
                        if (target3 is Mob mob && !(mob is Plane || mob is HoveringTank))
                        {
                            CollisionHandler.ResolveCollision(mob, pushableBlock);
                            mob.SetVelocity(Vector2.Zero);
                        }
                    }
                }
            }
        }

        public class CollisionProjectileDestroyCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("actor", out object actorObj) && actorObj is Projectile projectile &&
                    parameters.TryGetValue("gameManager", out object gmObj) && gmObj is GameManager gameManager &&
                    parameters.TryGetValue("target", out object targetObj))
                {
                    CollisionHandler.HandleProjectileDestroy(projectile, (Entity)targetObj);
                }
            }
        }

        public class CollisionProjectileReflectCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("actor", out object actorObj) && actorObj is Projectile projectile &&
                    parameters.TryGetValue("target", out object targetObj) && targetObj is Entity target)
                {
                    CollisionHandler.HandleProjectileReflect(projectile, target);
                }
            }
        }

        public class CollisionPickUpCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("actor", out object actorObj) && actorObj is Player player &&
                    parameters.TryGetValue("gameManager", out object gmObj) && gmObj is GameManager gameManager &&
                    parameters.TryGetValue("target", out object targetObj))
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
                        {
                            player.activeFireEffects.Add(effect);
                        }
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
                if (parameters.TryGetValue("actor", out object actorObj) && actorObj is Player player &&
                    parameters.TryGetValue("target", out object targetObj) && targetObj is IPushable pushable)
                {
                    CollisionHandler.HandlePush(player, pushable);
                }
            }
        }

        public class DestroyFlammableCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("target", out object targetObj) && targetObj is IFlammable flammableBlock)
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
                if (parameters.TryGetValue("target", out object targetObj) && targetObj is IDestructible destructibleBlock &&
                    parameters.TryGetValue("actor", out object actorObj) && actorObj is MineProjectile)
                {
                    destructibleBlock.Destroy();
                }
            }
        }
    }
}
