using System;
using Microsoft.Xna.Framework;

namespace Sprint0
{
    public class AxisXFollowState : IMobBehaviorState
    {
        public void Update(Mob mob)
        {
            var pos = mob.GetPosition();
            var target = mob.LastKnownPlayerPosition;
            float delta = target.X - pos.X;
            if (Math.Abs(delta) > 5f)
            {
                // move along X only
                mob.SetVelocity(new Vector2(
                    Math.Sign(delta) * mob.DefaultMovementSpeed * Globals.GlobalMobData.GetModifier(MobData.MobModifiers.MovementSpeed),
                    0));
            }
            else
            {
                mob.SetVelocity(Vector2.Zero);
                mob.PointCannonPlayer();
            }
        }
    }
}
