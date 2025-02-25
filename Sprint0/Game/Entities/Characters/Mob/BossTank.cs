using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Sprint0
{
    public class BossTank : Mob
    {
        private enum BossState { Moving, Turning }
        private BossState state;
        private float targetRotation;
        private Vector2 movementDirection;

        public BossTank(ContentManager content) : base(content) { }

        protected override void InitializeMob()
        {
            currentMobType = MobType.BossTank;
            defaultMovementSpeed = 40f;
            firingInterval = 4f;
            currentProjectileVariables["projectileType"] = "Shotgun";

            var bodySprite = new AnimatedSprite(0.3f);
            bodySprite.LoadContent(content, "TDTanksAllSprites", 641, 661, 123, 144, 1);
            SetSprite(bodySprite);

            var cannonSprite = new AnimatedSprite(0.3f);
            cannonSprite.LoadContent(content, "TDTanksAllSprites", 832, 186, 28, 64, 1);
            cannon = new Cannon(cannonSprite, this, new Vector2(14, 10), 30f,
                                0f, MathHelper.ToRadians(20), MathHelper.PiOver2, MathHelper.Pi + MathHelper.PiOver2);

            position = new Vector2(Globals.SCREENWIDTH / 2, 200);
            bodyRotation = 0f;
            movementDirection = new Vector2(0f, -1f); // Start moving UP
            velocity = movementDirection * defaultMovementSpeed;
            state = BossState.Moving;
        }

        protected override void UpdateMobBehavior()
        {
            float elapsed = Globals.FRAMETIME;

            switch (state)
            {
                case BossState.Moving:
                    // If reaching an edge, stop moving and rotate
                    if (position.Y <= 100 || position.Y >= 500)
                    {
                        velocity = Vector2.Zero;
                        state = BossState.Turning;
                        targetRotation = bodyRotation + MathHelper.Pi; // Rotate 180 degrees
                    }
                    break;

                case BossState.Turning:
                    TurnToTarget(elapsed, targetRotation);
                    break;
            }
        }

        private void TurnToTarget(float elapsed, float targetAngle)
        {
            float angleDiff = MathHelper.WrapAngle(targetAngle - bodyRotation);
            float turnSpeed = MathHelper.ToRadians(90) * elapsed;

            if (Math.Abs(angleDiff) > turnSpeed)
                angleDiff = Math.Sign(angleDiff) * turnSpeed;

            bodyRotation += angleDiff;

            if (Math.Abs(MathHelper.WrapAngle(targetAngle - bodyRotation)) < 0.05f)
            {
                bodyRotation = targetAngle;
                state = BossState.Moving;

                // Switch movement direction after turning
                movementDirection *= -1;
                velocity = movementDirection * defaultMovementSpeed;
            }
        }

        protected override void ChangeMobType(MobType type)
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
