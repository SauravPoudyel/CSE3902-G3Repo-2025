using System;
using Microsoft.Xna.Framework;

namespace Sprint0
{
    public class AxisXFollowState : IMobBehaviorState
    {
        public void Update(Mob mob)
        {
            Vector2 position = mob.GetPosition();
            Vector2 playerPosition = mob.GetLastKnownPlayerPosition();
            float deltaX = playerPosition.X - position.X;

            if (Math.Abs(deltaX) > 5f)
            {
                float speed = mob.GetDefaultMovementSpeed()
                            * Globals.GlobalMobData.GetModifier(MobData.MobModifiers.MovementSpeed);
                mob.SetVelocity(new Vector2(Math.Sign(deltaX) * speed, 0f));
            }
            else
            {
                mob.SetVelocity(Vector2.Zero);
                mob.PointCannonPlayer();
            }
        }
    }
}
