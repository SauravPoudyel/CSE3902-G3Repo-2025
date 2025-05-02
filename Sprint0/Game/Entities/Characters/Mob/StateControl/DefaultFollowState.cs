using Microsoft.Xna.Framework;

namespace Sprint0
{
    public class DefaultFollowState : IMobBehaviorState
    {
        public void Update(Mob mob)
        {
            // compute and normalize direction
            Vector2 toPlayer = mob.LastKnownPlayerPosition - mob.GetPosition();
            if (toPlayer != Vector2.Zero)
                toPlayer.Normalize();

            // turn toward player
            float frameTime = Globals.FRAMETIME;
            float turnRate = MathHelper.ToRadians(90) * frameTime;
            float desiredAngle = (float)Math.Atan2(toPlayer.Y, toPlayer.X) - MathHelper.PiOver2;
            float angleDiff = MathHelper.WrapAngle(desiredAngle - mob.BodyRotation);
            if (Math.Abs(angleDiff) > turnRate)
                angleDiff = Math.Sign(angleDiff) * turnRate;
            mob.BodyRotation += angleDiff;

            // determine movement multiplier
            float multiplier = 1f;
            if (mob.AggressionLevel == "Passive")
            {
                multiplier = -1f;
            }
            else if (mob.AggressionLevel == "Neutral")
            {
                if (!mob.IsNeutralTaskRunning)
                    mob.StartNeutralCycle();
                multiplier = mob.NeutralToggle ? 1f : -1f;
            }

            // move along Y axis
            mob.Velocity = new Vector2(
                0,
                mob.DefaultMovementSpeed
                  * multiplier
                  * Globals.GlobalMobData.GetModifier(MobData.MobModifiers.MovementSpeed)
            );
        }
    }
}
