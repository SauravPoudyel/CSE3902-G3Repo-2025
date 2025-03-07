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
                if (parameters.ContainsKey("actor") && parameters["actor"] is Player player &&
                    ((parameters.ContainsKey("target") && parameters["target"] is IRigid) ||
                     (parameters.ContainsKey("target") && parameters["target"] is Mob)))
                {
                    // Get collision direction based on relative positions
                    Vector2 playerPos = player.GetPosition();
                    Vector2 targetPos = ((Entity)parameters["target"]).GetPosition();
                    Vector2 diff = playerPos - targetPos;
                    
                    // Normalize the difference vector
                    Vector2 bounceDir;
                    if (diff != Vector2.Zero)
                    {
                        diff.Normalize();
                        bounceDir = diff;
                    }
                    else
                    {
                        // Fallback if entities are at the same position
                        bounceDir = new Vector2((float)Math.Sin(player.bodyRotation), -(float)Math.Cos(player.bodyRotation));
                    }

                    float bounceOffset = BounceSpeed * Globals.FRAMETIME;
                    player.SetPosition(playerPos + bounceDir * bounceOffset);
                    player.SetVelocity(Vector2.Zero);

                    if (parameters.ContainsKey("target") && parameters["target"] is Mob mob &&
                        !(mob is Plane || mob is HoveringTank))
                    {
                        ResolveCollision(mob, player);
                        mob.SetVelocity(Vector2.Zero);
                    }
                }
                
            }

            private void ResolveCollision(Entity movingEntity, Entity otherEntity)
            {
                Rectangle boundsA = movingEntity.GetBounds();
                Rectangle boundsB = otherEntity.GetBounds();
                Rectangle intersect = Rectangle.Intersect(boundsA, boundsB);

                if (intersect.IsEmpty) return;

                Vector2 displacement = (intersect.Width < intersect.Height)
                    ? new Vector2(boundsA.Center.X < boundsB.Center.X ? -intersect.Width : intersect.Width, 0)
                    : new Vector2(0, boundsA.Center.Y < boundsB.Center.Y ? -intersect.Height : intersect.Height);

                movingEntity.SetPosition(movingEntity.GetPosition() + displacement);
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
                    if (parameters["target"] is Mob mob)
                    {
                        if (projectile.Owner is Player player)
                        {
                            if (mob is ShieldTank shieldedTank)
                                shieldedTank.ShieldedDamage(projectile.damage, player.GetPosition());
                            else
                            {
                                mob.ChangeHealth(-projectile.damage);
                                projectile.OnDeath();
                            }
                        }
                    }
                    else if (parameters["target"] is Player player && projectile.Owner != player)
                    {
                        player.ChangeHealth(-projectile.damage);
                        projectile.OnDeath();
                    }
                    else if (parameters["target"] is IRigid)
                    {
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
                    parameters.ContainsKey("target") && parameters["target"] is IRigid)
                {
                    if (projectile.ReflectCooldown > 0) return;

                    if (projectile is IRicochet ricochetProj)
                    {
                        Vector2 normal = GetPenetrationNormal(projectile, (Entity)parameters["target"]);
                        if (normal == Vector2.Zero)
                        {
                            Vector2 diff = projectile.GetPosition() - ((Entity)parameters["target"]).GetPosition();
                            normal = (diff != Vector2.Zero) ? Vector2.Normalize(diff) : new Vector2(0, -1);
                        }

                        Vector2 reflected = RayTracer.ReflectVector(projectile.GetVelocity(), normal);
                        projectile.SetVelocity(reflected);

                        Vector2 newPos = projectile.GetPosition();
                        float margin = 10f;

                        if (normal == new Vector2(0, -1)) newPos.Y -= margin;
                        else if (normal == new Vector2(0, 1)) newPos.Y += margin;
                        else if (normal == new Vector2(-1, 0)) newPos.X -= margin;
                        else if (normal == new Vector2(1, 0)) newPos.X += margin;

                        projectile.SetPosition(newPos);
                        ricochetProj.RicochetCount--;
                        projectile.ReflectCooldown = 0.2f;

                        if (ricochetProj.RicochetCount <= 0)
                            projectile.OnDeath();
                    }
                    else
                    {
                        projectile.OnDeath();
                    }
                }
            }

            private Vector2 GetPenetrationNormal(Projectile projectile, Entity target)
            {
                Rectangle projBounds = projectile.GetBounds();
                Rectangle targetBounds = target.GetBounds();
                Rectangle intersection = Rectangle.Intersect(projBounds, targetBounds);

                if (intersection.IsEmpty) return Vector2.Zero;

                Vector2 projCenter = new Vector2(projBounds.Center.X, projBounds.Center.Y);
                Vector2 targetCenter = new Vector2(targetBounds.Center.X, targetBounds.Center.Y);
                Vector2 diff = projCenter - targetCenter;

                return (intersection.Width < intersection.Height)
                    ? new Vector2(diff.X < 0 ? -1 : 1, 0)
                    : new Vector2(0, diff.Y < 0 ? -1 : 1);
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

        public class CollisionHurtCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("actor") && parameters["actor"] is Player player &&
                    parameters.ContainsKey("target") && parameters["target"] is Effect effect &&
                    effect.effectType.Equals("explosion") && !effect.didDamage)
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
                    Vector2 direction = CalculatePushDirection(player);
                    pushable.Push(direction);

                    if (pushable is Entity pushableEntity && pushableEntity.GetVelocity() == Vector2.Zero)
                    {
                        Vector2 backwardDir = new Vector2((float)Math.Sin(player.bodyRotation), -(float)Math.Cos(player.bodyRotation));
                        backwardDir.Normalize();

                        float bounceOffset = BounceSpeed * Globals.FRAMETIME;
                        player.SetPosition(player.GetPosition() + backwardDir * bounceOffset);
                        player.SetVelocity(new Vector2(0, BounceSpeed));
                    }
                }
            }

            private Vector2 CalculatePushDirection(Player player)
            {
                return new Vector2(-(float)Math.Sin(player.bodyRotation), (float)Math.Cos(player.bodyRotation));
            }
        }

        public class DestroyFlammableCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("target") && parameters["target"] is IFlammable flammableBlock)
                    flammableBlock.Destroy();
            }
        }
    }
}
