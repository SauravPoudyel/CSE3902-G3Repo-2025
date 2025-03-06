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
                    // Bounce the player backwards.
                    Vector2 backwardDir = new Vector2(-(float)Math.Sin(player.bodyRotation),
                                                    (float)Math.Cos(player.bodyRotation));
                    backwardDir.Normalize();
                    float bounceOffset = 70f * Globals.FRAMETIME;
                    player.SetPosition(player.GetPosition() + backwardDir * bounceOffset);
                    player.SetVelocity(Vector2.Zero);

                    // For a mob target (if not a Plane), resolve collision.
                    if (parameters.ContainsKey("target") && parameters["target"] is Mob mobTargetInner && !(mobTargetInner is Plane))
                    {
                        ResolveCollision(mobTargetInner, player);
                        mobTargetInner.SetVelocity(Vector2.Zero);
                    }
                }

                // For collisions between Mob and RigidBlock, Blocks, or Mob.
                if (parameters.ContainsKey("actor") && parameters["actor"] is Mob mobActor && !(mobActor is Plane) &&
                    ((parameters.ContainsKey("target") && 
                    (parameters["target"] is RigidBlock || parameters["target"] is Blocks)) ||
                    (parameters.ContainsKey("target") && 
                    (parameters["target"] is Mob mobTarget && !(mobTarget is Plane)))))
                {
                    System.Console.WriteLine("Mob collision with block, rigidblock, or mob");
                    ResolveCollision(mobActor, (Entity)parameters["target"]);
                    mobActor.SetVelocity(Vector2.Zero);

                    if (parameters.ContainsKey("target") && parameters["target"] is Mob mobOther && !(mobOther is Plane))
                    {
                        ResolveCollision(mobOther, mobActor);
                        mobOther.SetVelocity(Vector2.Zero);
                    }
                }

                // For collisions between PushableBlock and RigidBlock or Mob.
                if (parameters.ContainsKey("actor") && parameters["actor"] is PushableBlock block1 &&
                    ((parameters.ContainsKey("target") && (parameters["target"] is RigidBlock)) ||
                    (parameters.ContainsKey("target") && (parameters["target"] is Mob))))
                {
                    ResolveCollision(block1, (Entity)parameters["target"]);
                    block1.SetVelocity(Vector2.Zero);

                    if (parameters.ContainsKey("target") && parameters["target"] is Mob mob3 && !(mob3 is Plane || mob3 is HoveringTank))
                    {
                        ResolveCollision(mob3, block1);
                        mob3.SetVelocity(Vector2.Zero);
                    }
                }
            }

            private void ResolveCollision(Entity movingEntity, Entity otherEntity)
            {
                Rectangle boundsA = movingEntity.GetBounds();
                Rectangle boundsB = otherEntity.GetBounds();
                Rectangle intersect = Rectangle.Intersect(boundsA, boundsB);

                if (intersect.IsEmpty)
                    return;

                // Determine minimal translation along X or Y.
                Vector2 displacement = Vector2.Zero;
                if (intersect.Width < intersect.Height)
                {
                    // Push along X axis.
                    if (boundsA.Center.X < boundsB.Center.X)
                        displacement = new Vector2(-intersect.Width, 0);
                    else
                        displacement = new Vector2(intersect.Width, 0);
                }
                else
                {
                    // Push along Y axis.
                    if (boundsA.Center.Y < boundsB.Center.Y)
                        displacement = new Vector2(0, -intersect.Height);
                    else
                        displacement = new Vector2(0, intersect.Height);
                }
                movingEntity.SetPosition(movingEntity.GetPosition() + displacement);
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
                    parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager)
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
                    }
                    AudioManager.PlaySound(AudioManager.SoundKey.PowerUp);
                }
            }
        }

        public class CollisionProjectileDestroyCommand : ICommand 
        {
            public void Execute(Dictionary<string, object> parameters) 
            {
                if (parameters.ContainsKey("actor") && parameters["actor"] is Projectile projectile 
                    && parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager 
                    && (parameters.ContainsKey("target") && parameters["target"] is Mob|| 
                        parameters.ContainsKey("target") && parameters["target"] is RigidBlock ||
                        parameters.ContainsKey("target") && parameters["target"] is Player)) 
                    {
                        if(parameters["target"] is Mob mob) {
                            if (projectile.Owner is Player player2) {

                                if(mob is ShieldTank shieldedTank) { // if mob is shielded, calculate if damage goes through
                                    shieldedTank.ShieldedDamage(projectile.damage, player2.GetPosition());
                                } else {
                                    mob.ChangeHealth(-projectile.damage);
                                    projectile.OnDeath(); 
                                }
                            }
                        }
                        if(parameters["target"] is Player player) {
                            if (projectile.Owner != player) {
                                player.ChangeHealth(-projectile.damage);
                                projectile.OnDeath(); 
                            }
                        }

                        if(parameters["target"] is RigidBlock block) {
                            projectile.OnDeath(); 
                        }

                    }
            }
        }
        public class CollisionProjectileReflectCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("actor") && parameters["actor"] is Projectile projectile &&
                    parameters.ContainsKey("target") && (parameters["target"] is RigidBlock || parameters["target"] is Blocks))
                {
                    // If the projectile was recently reflected, skip processing.
                    if (projectile.ReflectCooldown > 0)
                    {
                        return;
                    }

                    // Check if the projectile supports ricocheting.
                    if (projectile is IRicochet ricochetProj)
                    {
                        Entity target = (Entity)parameters["target"];

                        // First, compute the penetration normal using the overlapping bounds.
                        Vector2 normal = GetPenetrationNormal(projectile, target);
                        if (normal == Vector2.Zero)
                        {
                            // If that fails, fallback to center difference.
                            Vector2 diff = projectile.GetPosition() - target.GetPosition();
                            normal = (diff != Vector2.Zero) ? Vector2.Normalize(diff) : new Vector2(0, -1);
                        }

                        // Reflect the projectile's velocity.
                        Vector2 incident = projectile.GetVelocity();
                        Vector2 reflected = RayTracer.ReflectVector(incident, normal);
                        projectile.SetVelocity(reflected);

                        Rectangle targetBounds = target.GetBounds();
                        Rectangle projBounds = projectile.GetBounds();
                        Vector2 newPos = projectile.GetPosition();
                        float margin = 10f; // extra clearance

                        if (normal == new Vector2(0, -1))
                        {
                            newPos.Y = targetBounds.Top - projBounds.Height - margin;
                        }
                        else if (normal == new Vector2(0, 1))
                        {
                            newPos.Y = targetBounds.Bottom + margin;
                        }
                        else if (normal == new Vector2(-1, 0))
                        {
                            newPos.X = targetBounds.Left - projBounds.Width - margin;
                        }
                        else if (normal == new Vector2(1, 0))
                        {
                            newPos.X = targetBounds.Right + margin;
                        }
                        projectile.SetPosition(newPos);

                        ricochetProj.RicochetCount--;

                        projectile.ReflectCooldown = 0.2f;

                        if (ricochetProj.RicochetCount <= 0)
                        {
                            projectile.OnDeath();
                        }
                    }
                    else
                    {
                        projectile.OnDeath();
                    }
                }
            }

            // Compute a penetration-based normal from the overlapping bounds.
            private Vector2 GetPenetrationNormal(Projectile projectile, Entity target)
            {
                Rectangle projBounds = projectile.GetBounds();
                Rectangle targetBounds = target.GetBounds();
                Rectangle intersection = Rectangle.Intersect(projBounds, targetBounds);

                if (intersection.IsEmpty)
                    return Vector2.Zero;

                // Compute centers.
                Vector2 projCenter = new Vector2(projBounds.Center.X, projBounds.Center.Y);
                Vector2 targetCenter = new Vector2(targetBounds.Center.X, targetBounds.Center.Y);
                Vector2 diff = projCenter - targetCenter;

                // Determine penetration depths.
                float penX = intersection.Width;
                float penY = intersection.Height;

                // Choose the axis with minimal penetration.
                if (penX < penY)
                {
                    return new Vector2(diff.X < 0 ? -1 : 1, 0);
                }
                else if (penY < penX)
                {
                    return new Vector2(0, diff.Y < 0 ? -1 : 1);
                }
                else
                {
                    return Vector2.Zero;
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

        public class CollisionHurtCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("actor") && parameters["actor"] is Player player &&
                    parameters.ContainsKey("target") && parameters["target"] is Effect effect)
                {
                    if(effect.effectType.Equals("explosion") && !effect.didDamage) {
                        player.ChangeHealth(-50); 
                        effect.didDamage = true; 
                    }
                }
            }
        }
    }
}
