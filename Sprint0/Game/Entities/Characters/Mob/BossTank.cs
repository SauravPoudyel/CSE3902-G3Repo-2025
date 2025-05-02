using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Sprint0
{
    public class BossTank : Mob
    {
        private enum BossState { Moving, Turning }
        private Vector2 movementDirection;

        public BossTank(ContentManager content) : base(content) 
        { 
            TrackTrailsEnabled = true; 
            spriteWidth = 123;
            spriteHeight = 144;
            aggressionLevel = "Neutral";
            MobXP = 100;
        }

        protected override void InitializeMob()
        {
            currentMobType = EntityKeys.MobType.BossTank;
            defaultMovementSpeed = 40f;
            firingInterval = 4f;
            currentProjectileVariables["projectileType"] = "Shotgun";

            position = new Vector2(Globals.SCREENWIDTH / 2, 200);
            bodyRotation = 0f;
            movementDirection = new Vector2(0f, -1f); // Initially moving UP
            velocity = movementDirection * defaultMovementSpeed;
        }

        protected override void ChangeMobType(EntityKeys.MobType type)
        {
            currentMobType = type;
            InitializeMob();
        }

        protected override void ResetMobPosition()
        {
            position = new Vector2(Globals.SCREENWIDTH / 2, 200);
        }
    }

}
