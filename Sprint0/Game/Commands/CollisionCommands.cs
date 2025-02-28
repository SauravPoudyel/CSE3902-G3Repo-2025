using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Sprint0
{
    public class CollisionCommands
    {
        private const float BounceSpeed = 70f;
        public class CollisionStopCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                // For collisions between Player and RigidBlock or Mob.
                if (parameters.ContainsKey("actor") && parameters["actor"] is Player player &&
                    ((parameters.ContainsKey("target") && (parameters["target"] is RigidBlock)) ||
                    (parameters.ContainsKey("target") && (parameters["target"] is Mob))))
                {
                    // Compute the backward (bounce) direction based on the player's current rotation.
                    Vector2 backwardDir = new Vector2(-(float)System.Math.Sin(player.bodyRotation),
                                                      (float)System.Math.Cos(player.bodyRotation));
                                                      
                    backwardDir.Normalize();

                    float bounceOffset = BounceSpeed * Globals.FRAMETIME;
                    player.SetPosition(player.GetPosition() + backwardDir * bounceOffset);

                    player.SetVelocity(new Vector2(0, BounceSpeed));

                    if (parameters.ContainsKey("target") && (parameters["target"] is Mob mob))
                    {
                        mob.SetPosition(mob.GetPreviousPosition());
                        mob.SetVelocity(Vector2.Zero);
                    }
                }

                // For collisions between Mob and RigidBlock or Mob.
                if (parameters.ContainsKey("actor") && parameters["actor"] is Mob mob2 &&
                    ((parameters.ContainsKey("target") && (parameters["target"] is RigidBlock)) ||
                    (parameters.ContainsKey("target") && (parameters["target"] is Mob))))
                {
                    mob2.SetPosition(mob2.GetPreviousPosition());
                    mob2.SetVelocity(Vector2.Zero);

                    if (parameters.ContainsKey("target") && (parameters["target"] is Mob mob3))
                    {
                        mob3.SetPosition(mob3.GetPreviousPosition());
                        mob3.SetVelocity(Vector2.Zero);
                    }
                }

                // For collisions between PushableBlock and RigidBlock or Mob.
                if (parameters.ContainsKey("actor") && parameters["actor"] is PushableBlock block1 &&
                    ((parameters.ContainsKey("target") && (parameters["target"] is RigidBlock)) ||
                    (parameters.ContainsKey("target") && (parameters["target"] is Mob))))
                {
                    block1.SetPosition(block1.GetPreviousPosition());
                    block1.SetVelocity(Vector2.Zero);

                    if (parameters.ContainsKey("target") && (parameters["target"] is Mob mob3))
                    {
                        mob3.SetPosition(mob3.GetPreviousPosition());
                        mob3.SetVelocity(Vector2.Zero);
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
                    
                    if(parameters["target"] is PushableBlock pushableBlock) {
                        if(pushableBlock.GetVelocity() == Vector2.Zero) {
                        // Compute the backward (bounce) direction based on the player's current rotation.
                        Vector2 backwardDir = new Vector2(-(float)System.Math.Sin(player.bodyRotation),
                                                        (float)System.Math.Cos(player.bodyRotation));
                                                        
                        backwardDir.Normalize();

                        float bounceOffset = BounceSpeed * Globals.FRAMETIME;
                        player.SetPosition(player.GetPosition() + backwardDir * bounceOffset);

                        player.SetVelocity(new Vector2(0, BounceSpeed));
                    }
                    } else {
                        return; 
                    }

                }
            }

            private Vector2 CalculatePushDirection(Player player)
            {
                return new Vector2(
                    (float)Math.Sin(player.bodyRotation),
                    -(float)Math.Cos(player.bodyRotation)
                );
            }
        }

        public class CollisionPickUpCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("actor") && parameters["actor"] is Player player &&
                    parameters.ContainsKey("target") && parameters["target"] is PickupItem pickupItem)
                {
                   PowerUpFactory.ApplyPickupEffect(player, pickupItem.GetItemType());

                    if (parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager)
                    {
                        gameManager.RemoveEntity("pickupItem");
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
                    && (parameters.ContainsKey("target") && parameters["target"] is Mob|| 
                    parameters.ContainsKey("target") && parameters["target"] is RigidBlock)) 
                    {
                        if(parameters["target"] is Mob mob) {
                            if (projectile.Owner != mob) {
                                mob.health -= projectile.damage;
                                projectile.OnDeath(); 
                            }
                        }

                        if(parameters["target"] is RigidBlock block) {
                            projectile.OnDeath(); 
                        }

                    }
            }
        }

        public class DestroyFlammableCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters["target"] is FlammableBlock block)
                {
                    block.Destroy();
                    block.Ignite();
                }
            }
        }

    }
}
