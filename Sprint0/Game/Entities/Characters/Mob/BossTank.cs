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
        }

        protected override void InitializeMob()
        {
            currentMobType = EntityKeys.MobType.BossTank;
            defaultMovementSpeed = 40f;
            firingInterval = 4f;
            currentProjectileVariables["projectileType"] = "Shotgun";

            var bodySprite = new AnimatedSprite(0.3f);
            bodySprite.LoadContent(content, "TDTanksAllSprites", 641, 661, 123, 144, 1);
            SetSprite(bodySprite);

            var cannonSprite = new AnimatedSprite(0.3f);
            cannonSprite.LoadContent(content, "TDTanksAllSprites", 832, 186, 28, 64, 1);
            cannon = new Cannon(content, cannonSprite, this, new Vector2(14, 10), 50f, new Vector2(12, 70),
                                0f, MathHelper.ToRadians(20), MathHelper.PiOver2, MathHelper.Pi + MathHelper.PiOver2);

            position = new Vector2(Globals.SCREENWIDTH / 2, 200);
            bodyRotation = 0f;
            movementDirection = new Vector2(0f, -1f); // Initially moving UP
            velocity = movementDirection * defaultMovementSpeed;
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
            position = new Vector2(Globals.SCREENWIDTH / 2, 200);
        }
    }

}
