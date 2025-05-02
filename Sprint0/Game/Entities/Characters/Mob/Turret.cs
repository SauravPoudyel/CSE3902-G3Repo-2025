using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0
{
    public class Turret : Mob
    {
        private float turretRotationSpeed;

        public Turret(ContentManager content) : base(content) {
            TrackTrailsEnabled = false; 
            spriteWidth = 104;
            spriteHeight = 104;
            MobXP = 15;
        }

        protected override void InitializeMob()
        {
            currentMobType = EntityKeys.MobType.Turret;
            defaultMovementSpeed = 0f;
            firingInterval = 2.5f; // Shoots every 2.5 seconds
            turretRotationSpeed = MathHelper.ToRadians(40);
            currentProjectileVariables["projectileType"] = "Rocket";
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
