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

        public SmallEnemy(ContentManager content) : base(content) { }

        protected override void InitializeMob()
        {
            currentMobType = MobType.SmallEnemy;
            defaultMovementSpeed = 80f;
            firingInterval = 1.4f;
            phaseTimer = 0f;
            phase = 0;
            currentProjectileVariables["projectileType"] = "Default";

            var bodySprite = new AnimatedSprite(0.3f);
            bodySprite.LoadContent(content, "TDTanksAllSprites", 768, 256, 95, 113, 1);
            SetSprite(bodySprite);

            var cannonSprite = new AnimatedSprite(0.3f);
            cannonSprite.LoadContent(content, "TDTanksAllSprites", 832, 186, 28, 64, 1);
            cannon = new Cannon(cannonSprite, this, new Vector2(14, 10), 30f,
                                0f, MathHelper.ToRadians(20), MathHelper.PiOver2, MathHelper.Pi + MathHelper.PiOver2);

            // Start at a corner of a square
            position = new Vector2(Globals.SCREENWIDTH / 2, 150);
            velocity = new Vector2(defaultMovementSpeed, 0f);
            bodyRotation = 0f;
        }

        protected override void UpdateMobBehavior()
        {
            float elapsed = Globals.FRAMETIME;
            phaseTimer += elapsed;
            if (phaseTimer >= phaseDuration)
            {
                phaseTimer = 0f;
                phase = (phase + 1) % 4;
                velocity = Vector2.Zero; // wait until fully rotated
            }

            float desiredAngle = 0f;
            Vector2 desiredVelocity = Vector2.Zero;
            switch (phase)
            {
                case 0:
                    desiredAngle = 0f;
                    desiredVelocity = new Vector2(defaultMovementSpeed, 0f);
                    break;
                case 1:
                    desiredAngle = MathHelper.PiOver2;
                    desiredVelocity = new Vector2(0f, defaultMovementSpeed);
                    break;
                case 2:
                    desiredAngle = MathHelper.Pi;
                    desiredVelocity = new Vector2(-defaultMovementSpeed, 0f);
                    break;
                case 3:
                    desiredAngle = -MathHelper.PiOver2;
                    desiredVelocity = new Vector2(0f, -defaultMovementSpeed);
                    break;
            }
            float angleDiff = MathHelper.WrapAngle(desiredAngle - bodyRotation);
            float turnSpeed = MathHelper.ToRadians(90) * elapsed;
            if (Math.Abs(angleDiff) > 0.05f)
            {
                if (Math.Abs(angleDiff) > turnSpeed)
                    angleDiff = Math.Sign(angleDiff) * turnSpeed;
                bodyRotation += angleDiff;
                velocity = Vector2.Zero;
            }
            else
            {
                velocity = desiredVelocity;
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
