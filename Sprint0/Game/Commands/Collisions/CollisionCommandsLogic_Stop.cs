using Microsoft.Xna.Framework;

namespace Sprint0
{
    public static class CollisionCommandsLogic_Stop
    {
        private static bool IsGroundBasedMob(Entity entity) =>
            entity is Mob mob && !(mob is Plane || mob is HoveringTank);

        private static bool IsRigidOrGroundBasedMob(Entity entity) =>
            entity is IRigid || IsGroundBasedMob(entity);

        public static void HandlePlayerCollision(Player player, Entity target)
        {
            if (!IsRigidOrGroundBasedMob(target))
                return;

            var bounceDirection = CollisionHandler.CalculateBounceDirection(player, target, player.bodyRotation);
            player.SetPosition(player.GetPosition() + bounceDirection * 70f * Globals.FRAMETIME);
            player.SetVelocity(Vector2.Zero);

            if (target is Mob mobTarget && !(mobTarget is Plane || mobTarget is HoveringTank))
            {
                CollisionHandler.ResolveCollision(mobTarget, player);
                mobTarget.SetVelocity(Vector2.Zero);
            }
        }

        public static void HandleMobCollision(Mob mobActor, Entity target)
        {
            if (mobActor is Plane || (!IsRigidOrGroundBasedMob(target) && !(target is BaseBlock)))
                return;

            CollisionHandler.ResolveCollision(mobActor, target);
            mobActor.SetVelocity(Vector2.Zero);

            if (target is Mob mobOther && !(mobOther is Plane))
            {
                CollisionHandler.ResolveCollision(mobOther, mobActor);
                mobOther.SetVelocity(Vector2.Zero);
            }
        }

        public static void HandleBlockCollision(PushableBlock block, Entity target)
        {
            if (!(target is IRigid) && !(target is Mob))
                return;

            CollisionHandler.ResolveCollision(block, target);
            block.SetVelocity(Vector2.Zero);

            if (target is Mob mob && !(mob is Plane || mob is HoveringTank))
            {
                CollisionHandler.ResolveCollision(mob, block);
                mob.SetVelocity(Vector2.Zero);
            }
        }
    }
}
