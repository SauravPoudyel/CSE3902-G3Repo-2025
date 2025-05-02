using System;
using Microsoft.Xna.Framework;

namespace Sprint0
{
    public class AxisYFollowState : IMobBehaviorState
    {
        public void Update(Mob mob)
        {
            Vector2 position = mob.GetPosition();
            Vector2 playerPosition = mob.GetLastKnownPlayerPosition();
            float deltaY = playerPosition.Y - position.Y;

            if (Math.Abs(deltaY) > 5f)
            {
                float speed = mob.GetDefaultMovementSpeed()
                            * Globals.GlobalMobData.GetModifier(MobData.MobModifiers.MovementSpeed);
                mob.SetVelocity(new Vector2(0f, Math.Sign(deltaY) * speed));
            }
            else
            {
                mob.SetVelocity(Vector2.Zero);
                mob.PointCannonPlayer();
            }
        }
    }
}
