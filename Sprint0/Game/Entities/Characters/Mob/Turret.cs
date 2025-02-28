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
            TrackTrailsEnabled = true; 
            spriteWidth = 104;
            spriteHeight = 104;
        }

        protected override void InitializeMob()
        {
            currentMobType = MobType.Turret;
            defaultMovementSpeed = 0f;
            firingInterval = 2.5f; // Shoots every 2.5 seconds
            turretRotationSpeed = MathHelper.ToRadians(40);
            currentProjectileVariables["projectileType"] = "Rocket";

            var turretBaseSprite = new AnimatedSprite(0.3f);
            turretBaseSprite.LoadContent(content, "TDTowerDefenseSprites", 2444, 908, 104, 104, 1);
            SetSprite(turretBaseSprite);

            var turretCannonSprite = new AnimatedSprite(0.3f);
            turretCannonSprite.LoadContent(content, "TDTowerDefenseSprites", 2455, 1290, 85, 110, 1);
            
            cannon = new Cannon(content, turretCannonSprite, this, new Vector2(42, 34), -70f, new Vector2(42, -80),
                                0f, MathHelper.ToRadians(30), MathHelper.PiOver2, MathHelper.Pi + MathHelper.PiOver2);
            cannon.CannonEffects = SpriteEffects.FlipVertically;
            cannon.HasFiringEffect = false; 
            velocity = Vector2.Zero;
        }


        protected override void UpdateMobBehavior()
        {
            velocity = Vector2.Zero;

            if (lastKnownPlayerPosition != Vector2.Zero)
            {
                Vector2 directionToPlayer = lastKnownPlayerPosition - position;
                float targetRotation = (float)Math.Atan2(directionToPlayer.Y, directionToPlayer.X) - MathHelper.PiOver2;
                float angleDifference = MathHelper.WrapAngle(targetRotation - cannon.Rotation);
                float maxTurnAmount = turretRotationSpeed * Globals.FRAMETIME;

                if (Math.Abs(angleDifference) > maxTurnAmount)
                    angleDifference = Math.Sign(angleDifference) * maxTurnAmount;

                cannon.Rotation += angleDifference;
            }
        }

        protected override void ChangeMobType(MobType type)
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
