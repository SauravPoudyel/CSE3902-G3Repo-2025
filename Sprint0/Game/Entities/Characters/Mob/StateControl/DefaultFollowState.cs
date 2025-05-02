using System;
using Microsoft.Xna.Framework;

namespace Sprint0
{
    public class DefaultFollowState : IMobBehaviorState
    {
        public void Update(Mob mob)
        {
            Vector2 toPlayer = mob.GetLastKnownPlayerPosition() - mob.GetPosition();
            if (toPlayer != Vector2.Zero) toPlayer.Normalize();

            float frameTime = Globals.FRAMETIME;
            float turnRate = MathHelper.ToRadians(90f) * frameTime;
            float desiredAngle = (float)Math.Atan2(toPlayer.Y, toPlayer.X) - MathHelper.PiOver2;
            float angleDiff = MathHelper.WrapAngle(desiredAngle - mob.GetBodyRotation());
            if (Math.Abs(angleDiff) > turnRate)
                angleDiff = Math.Sign(angleDiff) * turnRate;
            mob.SetBodyRotation(mob.GetBodyRotation() + angleDiff);

            float multiplier = 1f;
            string aggression = mob.GetAggressionLevel();
            if (aggression == "Passive")
                multiplier = -1f;
            else if (aggression == "Neutral")
            {
                mob.StartNeutralCycle();
                multiplier = mob.GetNeutralToggle() ? 1f : -1f;
            }

            float speed = mob.GetDefaultMovementSpeed()
                        * multiplier
                        * Globals.GlobalMobData.GetModifier(MobData.MobModifiers.MovementSpeed);
            mob.SetVelocity(new Vector2(0f, speed));
        }
    }
}
