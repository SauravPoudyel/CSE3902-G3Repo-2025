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
            }
        }

        public class CollisionPushCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters["actor"] is Player player &&
                parameters["target"] is IPushable pushable)
                {
                    Vector2 direction = CalculatePushDirection(player);
                    pushable.Push(direction);
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

        public class CollisionProjectileToMobCommand : ICommand 
        {
            public void Execute(Dictionary<string, object> parameters) 
            {
                if (parameters.ContainsKey("actor") && parameters["actor"] is Projectile projectile 
                    && parameters.ContainsKey("target") && parameters["target"] is Mob mob
                    && parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager) 
                    {
                        //Explode mob
                        if (projectile.Owner != mob) {
                            mob.health -= projectile.damage;
                            gameManager.RemoveEntity(projectile.entityKey);
                        }
                    }
            }
        }

        public class CollisionProjectileToBlockCommand : ICommand 
        {
            public void Execute(Dictionary<string, object> parameters) 
            {
                if (parameters.ContainsKey("actor") && parameters["actor"] is Projectile projectile 
                    && parameters.ContainsKey("target") && parameters["target"] is Mob mob
                    && parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager) 
                    {
                        //Deleteblock 
                        //Subclass of reflect off block
                    }
            }
        }

        public class DestroyFlammableCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters["target"] is FlammableBlock block)
                    block.Destroy();
            }
        }

    }
}
