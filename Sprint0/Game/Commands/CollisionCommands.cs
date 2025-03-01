using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Sprint0
{
    public class CollisionCommands
    {
        private const float BounceSpeed = 70f;

        private static void ResetEntity(Entity entity)
        {
            // Resets the entity to its previous position with zero velocity.
            entity.SetPosition(entity.GetPreviousPosition());
            entity.SetVelocity(Vector2.Zero);
        }

        public class CollisionStopCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                // Handle collisions where the actor is a Player.
                if (parameters.ContainsKey("actor") && parameters["actor"] is Player player &&
                    parameters.ContainsKey("target") &&
                    (parameters["target"] is Blocks || parameters["target"] is RigidBlock ||
                     parameters["target"] is Mob))
                {
                    // Bounce the player backwards based on current rotation.
                    Vector2 backwardDir = new Vector2(-(float)Math.Sin(player.bodyRotation), (float)Math.Cos(player.bodyRotation));
                    backwardDir.Normalize();
                    float bounceOffset = BounceSpeed * Globals.FRAMETIME;
                    player.SetPosition(player.GetPosition() + backwardDir * bounceOffset);
                    player.SetVelocity(new Vector2(0, BounceSpeed));

                    // If target is a Mob, reset it.
                    if (parameters["target"] is Mob targetMob)
                    {
                        ResetEntity(targetMob);
                    }
                }
                // Handle collisions where the actor is a Mob.
                else if (parameters.ContainsKey("actor") && parameters["actor"] is Mob mob &&
                         parameters.ContainsKey("target") &&
                         (parameters["target"] is RigidBlock || parameters["target"] is Mob))
                {
                    ResetEntity(mob);
                    if (parameters["target"] is Mob targetMob)
                    {
                        ResetEntity(targetMob);
                    }
                }
                // Handle collisions where the actor is a PushableBlock.
                else if (parameters.ContainsKey("actor") && parameters["actor"] is PushableBlock pushBlock &&
                         parameters.ContainsKey("target") &&
                         (parameters["target"] is RigidBlock || parameters["target"] is Mob || parameters["target"] is PushableBlock))
                {
                    ResetEntity(pushBlock);
                    if (parameters["target"] is Mob targetMob)
                    {
                        ResetEntity(targetMob);
                    }
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
                    Vector2 direction = CalculatePushDirection(player);
                    pushable.Push(direction);

                    if (parameters["target"] is PushableBlock pushableBlock)
                    {
                        if (pushableBlock.GetVelocity() == Vector2.Zero)
                        {
                            // Bounce the player back if the pushable block is stationary.
                            Vector2 backwardDir = new Vector2(-(float)Math.Sin(player.bodyRotation), (float)Math.Cos(player.bodyRotation));
                            backwardDir.Normalize();
                            float bounceOffset = BounceSpeed * Globals.FRAMETIME;
                            player.SetPosition(player.GetPosition() + backwardDir * bounceOffset);
                            player.SetVelocity(new Vector2(0, BounceSpeed));
                        }
                    }
                }
            }

            private Vector2 CalculatePushDirection(Player player)
            {
                return new Vector2((float)Math.Sin(player.bodyRotation), -(float)Math.Cos(player.bodyRotation));
            }
        }

        public class CollisionPickUpCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("actor") && parameters["actor"] is Player player &&
                    parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager &&
                    ((parameters.ContainsKey("target") && (parameters["target"] is PickupItem) || 
                    (parameters.ContainsKey("target") && parameters["target"] is Item))))
                {
                    if (parameters["target"] is PickupItem pickupItem)
                    {
                        PowerUpFactory.ApplyPickupEffect(player, pickupItem.GetItemType());
                        gameManager.RemoveEntity(pickupItem.EntityKey);
                    }
                    else if (parameters["target"] is Item item)
                    {
                        PowerUpFactory.ApplyPickupEffect(player, item.GetItemType());
                        gameManager.RemoveEntity(item.EntityKey);
                        System.Diagnostics.Debug.WriteLine("Picked up item: " + item.GetItemType());
                    }
                }
            }
        }

        public class CollisionProjectileDestroy : ICommand 
        {
            public void Execute(Dictionary<string, object> parameters) 
            {
                if (parameters.ContainsKey("actor") && parameters["actor"] is Projectile projectile 
                    && parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager 
                    && (parameters.ContainsKey("target") && (parameters["target"] is Mob || parameters["target"] is RigidBlock))) 
                {
                    if (parameters["target"] is Mob mob)
                    {
                        if (projectile.Owner != mob)
                        {
                            mob.health -= projectile.damage;
                            projectile.OnDeath(); 
                        }
                    }
                    if (parameters["target"] is RigidBlock block)
                    {
                        projectile.OnDeath(); 
                    }
                }
            }
        }

        public class DestroyFlammableCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("target") && parameters["target"] is FlammableBlock block)
                {
                    block.Destroy();
                }
            }
        }
<<<<<<< HEAD

        public class CollisionHurtCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("actor") && parameters["actor"] is Player player &&
                    parameters.ContainsKey("target") && parameters["target"] is Effect effect)
                {
                    if(effect.effectType.Equals("explosion") && !effect.didDamage) {
                        player.Damage(50); 
                        effect.didDamage = true; 
                    }
                }
            }
        }
=======
>>>>>>> origin/test
    }
}
