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
        }

        protected override void InitializeMob()
        {
            currentMobType = EntityKeys.MobType.SmallEnemy;
            defaultMovementSpeed = 80f;
            firingInterval = 1.4f;
            currentProjectileVariables["projectileType"] = "Default";

            var bodySprite = new AnimatedSprite(0.3f);
            bodySprite.LoadContent(content, "TDTanksAllSprites", 768, 256, 95, 113, 1);
            SetSprite(bodySprite);

            var cannonSprite = new AnimatedSprite(0.3f);
            cannonSprite.LoadContent(content, "TDTanksAllSprites", 832, 186, 28, 64, 1);
            cannon = new Cannon(content, cannonSprite, this, new Vector2(14, 10), 50f, new Vector2(12, 70),
                                0f, MathHelper.ToRadians(20), MathHelper.PiOver2, MathHelper.Pi + MathHelper.PiOver2);

            velocity = new Vector2(defaultMovementSpeed, 0f);
            bodyRotation = 0f;
        }

        protected override void UpdateMobBehavior()
        {
            FollowPlayer(aggressionLevel); 
            PointCannonPlayer();
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
