using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Sprint0
{
    public class CollisionCommands
    {
        public class CollisionStopCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("actor") && parameters["actor"] is Player player &&
                    parameters.ContainsKey("target") && parameters["target"] is Entity target)
                {
                    // Only process if target is IRigid or a Mob.
                    if (target is IRigid || target is Mob)
                    {
                        Vector2 bounceDir = CollisionHandler.CalculateBounceDirection(player, target, player.bodyRotation);
                        float bounceOffset = 70f * Globals.FRAMETIME;
                        player.SetPosition(player.GetPosition() + bounceDir * bounceOffset);
                        player.SetVelocity(Vector2.Zero);

                        if (target is Mob mob && !(mob is Plane || mob is HoveringTank))
                        {
                            CollisionHandler.ResolveCollision(mob, player);
                            mob.SetVelocity(Vector2.Zero);
                        }
                    }
                }
                else if (parameters.ContainsKey("actor") && parameters["actor"] is Mob mobActor &&
                         parameters.ContainsKey("target") && parameters["target"] is Entity target2)
                {
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
                else if (parameters.ContainsKey("actor") && parameters["actor"] is PushableBlock pushableBlock &&
                         parameters.ContainsKey("target") && parameters["target"] is Entity target3)
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
                if (parameters.ContainsKey("actor") && parameters["actor"] is Projectile projectile &&
                    parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager &&
                    parameters.ContainsKey("target"))
                {
                    CollisionHandler.HandleProjectileDestroy(projectile, (Entity)parameters["target"]);
                }
            }
        }

        public class CollisionProjectileReflectCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("actor") && parameters["actor"] is Projectile projectile &&
                    parameters.ContainsKey("target") && parameters["target"] is Entity target)
                {
                    CollisionHandler.HandleProjectileReflect(projectile, target);
                }
            }
        }

        public class CollisionPickUpCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("actor") && parameters["actor"] is Player player &&
                    parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager &&
                    parameters.ContainsKey("target"))
                {
                    // Since collision handler is static, passing gameManager as a parameter still keeps our singelton design concept
                    CollisionHandler.HandlePickup(player, parameters["target"], gameManager);
                }
            }
        }

        public class CollisionHurtCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("actor") && parameters["actor"] is Player player &&
                    parameters.ContainsKey("target") && parameters["target"] is Effect effect &&
                    effect.effectType == EntityKeys.EffectType.Explosion && !effect.didDamage)
                {
                    player.ChangeHealth(-50);
                    effect.didDamage = true;
                }
            }
        }

        public class CollisionPushCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("actor") && parameters["actor"] is Player player &&
                    parameters.ContainsKey("target") && parameters["target"] is IPushable pushable)
                {
                    CollisionHandler.HandlePush(player, pushable);
                }
            }
        }

        public class DestroyFlammableCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("target") && parameters["target"] is IFlammable flammableBlock)
                {
                    flammableBlock.Destroy();
                }
            }
        }
    }
}
