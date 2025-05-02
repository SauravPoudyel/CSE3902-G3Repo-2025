using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;

namespace Sprint0
{
    public class HoveringTank : Mob
    {
        public HoveringTank(ContentManager content) : base(content)
        {
            TrackTrailsEnabled = false;
            spriteWidth = 86;
            spriteHeight = 92;
            aggressionLevel = "Neutral";
            MobXP = 20;
        }

        protected override void InitializeMob()
        {
            currentMobType = EntityKeys.MobType.HoveringTank; 
            defaultMovementSpeed = 80f;
            firingInterval = 1.4f;

            bodyRotation = 0f;
            velocity = Vector2.Zero;
        }

        protected override void ChangeMobType(EntityKeys.MobType type)
        {
            currentMobType = type;
            InitializeMob();
        }

        protected override void ResetMobPosition()
        {
            position = new Vector2(Globals.SCREENWIDTH / 2 + 100, 400);
        }
    }
}
