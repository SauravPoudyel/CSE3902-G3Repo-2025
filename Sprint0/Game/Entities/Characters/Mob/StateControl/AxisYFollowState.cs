using Microsoft.Xna.Framework;

namespace Sprint0
{
    public class AxisYFollowState : IMobBehaviorState
    {
        public void Update(Mob mob)
        {
            var pos = mob.GetPosition();
            var target = mob.LastKnownPlayerPosition;
            float delta = target.Y - pos.Y;
            if (Math.Abs(delta) > 5f)
            {
                mob.SetVelocity(new Vector2(
                    0,
                    Math.Sign(delta) * mob.DefaultMovementSpeed * Globals.GlobalMobData.GetModifier(MobData.MobModifiers.MovementSpeed)));
            }
            else
            {
                mob.SetVelocity(Vector2.Zero);
                mob.PointCannonPlayer();
            }
        }
    }
}
