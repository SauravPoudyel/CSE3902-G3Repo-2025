using Microsoft.Xna.Framework;
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
                if (parameters.ContainsKey("actor") && parameters["actor"] is Player player &&
                    ((parameters.ContainsKey("target") && (parameters["target"] is Blocks)) ||
                     (parameters.ContainsKey("target") && (parameters["target"] is Mob))))
                {
                    float currentRotation = player.bodyRotation;
                    Vector2 backwardDir = new Vector2(-(float)System.Math.Sin(player.bodyRotation),
                                                      (float)System.Math.Cos(player.bodyRotation));
                    backwardDir.Normalize();

                    float bounceOffset = BounceSpeed * Globals.FRAMETIME;

                    player.SetPosition(player.GetPosition() + backwardDir * bounceOffset);
                    player.SetVelocity(new Vector2(0, BounceSpeed));
                    player.bodyRotation = currentRotation;
                }
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
                if (parameters.ContainsKey("actor") && parameters["actor"] is Projectile projectile && parameters.ContainsKey("target") 
                    && parameters["target"] is Mob mob) 
                    {
                        //Explode mob
                        mob.SetSprite("NULLSPRITE");
                    }
            }
        }
    
    }
}
