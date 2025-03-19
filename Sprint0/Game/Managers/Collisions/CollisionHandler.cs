using System;
using Microsoft.Xna.Framework;

namespace Sprint0
{
    public static class CollisionHandler
    {
        private const float BounceSpeed = 70f;
        private const float ReflectMargin = 10f;

        public static Vector2 CalculateBounceDirection(Entity actor, Entity target, float fallbackRotation)
        {
            Vector2 diff = actor.GetPosition() - target.GetPosition();
            if (diff != Vector2.Zero)
                return Vector2.Normalize(diff);
            return new Vector2((float)Math.Sin(fallbackRotation), -(float)Math.Cos(fallbackRotation));
        }

        public static void ResolveCollision(Entity movingEntity, Entity otherEntity)
        {
            Rectangle boundsA = movingEntity.GetBounds();
            Rectangle boundsB = otherEntity.GetBounds();
            Rectangle intersect = Rectangle.Intersect(boundsA, boundsB);
            if (intersect.IsEmpty)
                return;

            Vector2 displacement = (intersect.Width < intersect.Height)
                ? new Vector2(boundsA.Center.X < boundsB.Center.X ? -intersect.Width : intersect.Width, 0)
                : new Vector2(0, boundsA.Center.Y < boundsB.Center.Y ? -intersect.Height : intersect.Height);
            movingEntity.SetPosition(movingEntity.GetPosition() + displacement);
        }

        public static void HandlePickup(Player player, object target, GameManager gameManager)
        {
            if (target is PickupItem pickupItem)
            {
                PowerUpFactory.ApplyPickupEffect(player, pickupItem.GetItemType());
                gameManager.RemoveEntity(pickupItem.EntityKey);
            }
            else if (target is Item item)
            {
                PowerUpFactory.ApplyPickupEffect(player, item.GetItemType());
                gameManager.RemoveEntity(item.EntityKey);
            }
            AudioManager.PlaySound(AudioManager.SoundKey.PowerUp);
        }

        public static void HandlePush(Player player, IPushable pushable)
        {
            // Calculate push direction based on the player's rotation.
            Vector2 direction = new Vector2(-(float)Math.Sin(player.bodyRotation), (float)Math.Cos(player.bodyRotation));
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

        public static void HandleProjectileReflect(Projectile projectile, Entity target)
        {
            if (projectile.ReflectCooldown > 0)
                return;

            if (projectile is IRicochet ricochetProj)
            {
                Vector2 normal = GetPenetrationNormal(projectile, target);
                if (normal == Vector2.Zero)
                {
                    Vector2 diff = projectile.GetPosition() - target.GetPosition();
                    normal = (diff != Vector2.Zero) ? Vector2.Normalize(diff) : new Vector2(0, -1);
                }

                Vector2 reflected = RayTracer.ReflectVector(projectile.GetVelocity(), normal);
                projectile.SetVelocity(reflected);

                Vector2 newPos = projectile.GetPosition();
                if (normal == new Vector2(0, -1))
                    newPos.Y -= ReflectMargin;
                else if (normal == new Vector2(0, 1))
                    newPos.Y += ReflectMargin;
                else if (normal == new Vector2(-1, 0))
                    newPos.X -= ReflectMargin;
                else if (normal == new Vector2(1, 0))
                    newPos.X += ReflectMargin;
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

        public static Vector2 GetPenetrationNormal(Projectile projectile, Entity target)
        {
            Rectangle projBounds = projectile.GetBounds();
            Rectangle targetBounds = target.GetBounds();
            Rectangle intersection = Rectangle.Intersect(projBounds, targetBounds);
            if (intersection.IsEmpty)
                return Vector2.Zero;

            Vector2 projCenter = new Vector2(projBounds.Center.X, projBounds.Center.Y);
            Vector2 targetCenter = new Vector2(targetBounds.Center.X, targetBounds.Center.Y);
            Vector2 diff = projCenter - targetCenter;
            return (intersection.Width < intersection.Height)
                ? new Vector2(diff.X < 0 ? -1 : 1, 0)
                : new Vector2(0, diff.Y < 0 ? -1 : 1);
        }

        public static void HandleProjectileDestroy(Projectile projectile, Entity target)
        {
            if (target is Mob mob)
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
            else if (target is Player player && projectile.Owner != player)
            {
                player.ChangeHealth(-projectile.damage);
                projectile.OnDeath();
            }
            else if (target is IRigid)
            {
                projectile.OnDeath();
            }
        }
    }
}
