using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;


namespace Sprint0
{
    public class SmallEnemy : Mob
    {
        private float phaseTimer;
        private int phase; // 0: Right, 1: Down, 2: Left, 3: Up
        private readonly float phaseDuration = 1.5f;

        public SmallEnemy(ContentManager content) : base(content) 
        { 
            TrackTrailsEnabled = true; 
            spriteWidth = 95;
            spriteHeight = 113;
            aggressionLevel = "Aggressive";
            MobXP = 10;
        }

        protected override void InitializeMob()
        {
            currentMobType = EntityKeys.MobType.SmallEnemy;
            defaultMovementSpeed = 80f;
            firingInterval = 1.4f;
            currentProjectileVariables["projectileType"] = "Default";
            velocity = new Vector2(defaultMovementSpeed, 0f);
            bodyRotation = 0f;
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
