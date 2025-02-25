using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;

namespace Sprint0
{
    public class ExplodingTank : Mob
    {
        private float explosionTimer;
        private int explosionPhaseIndex;
        private bool inExplosionState;
        private readonly string[] explosionSpriteNames = { "Explosion1", "Explosion2", "Explosion3" };

        public ExplodingTank(ContentManager content) : base(content)
        {
            ToggleTrackTrails();
            spriteWidth = 81;
            spriteHeight = 76;
        }

        protected override void InitializeMob()
        {
            currentMobType = MobType.ExplodingTank;
            defaultMovementSpeed = 50f;
            firingInterval = 3f;
            explosionTimer = 0f;
            explosionPhaseIndex = 0;
            inExplosionState = false;
            currentProjectileVariables["projectileType"] = "Default";

            var bodySprite = new AnimatedSprite(0.3f);
            bodySprite.LoadContent(content, "TDTanksAllSprites", 952, 569, 81, 76, 1);
            SetSprite(bodySprite);

            var cannonSprite = new AnimatedSprite(0.3f);
            cannonSprite.LoadContent(content, "TDTanksAllSprites", 832, 186, 28, 64, 1);
            cannon = new Cannon(cannonSprite, this, new Vector2(14, 10), 30f,
                                0f, MathHelper.ToRadians(20), MathHelper.PiOver2, MathHelper.Pi + MathHelper.PiOver2);
        }

        protected override void UpdateMobBehavior()
        {
            float elapsed = Globals.FRAMETIME;
            if (!inExplosionState)
            {
                if (lastKnownPlayerPosition != Vector2.Zero)
                {
                    Vector2 dirToPlayer = lastKnownPlayerPosition - position;
                    if (dirToPlayer != Vector2.Zero)
                        dirToPlayer.Normalize();
                    float trackingTurnRate = MathHelper.ToRadians(90) * elapsed;
                    float desiredAngle = (float)Math.Atan2(dirToPlayer.Y, dirToPlayer.X) - MathHelper.PiOver2;
                    float angleDiff = MathHelper.WrapAngle(desiredAngle - bodyRotation);
                    if (Math.Abs(angleDiff) > trackingTurnRate)
                        angleDiff = Math.Sign(angleDiff) * trackingTurnRate;
                    bodyRotation += angleDiff;
                    velocity = new Vector2(dirToPlayer.X, -dirToPlayer.Y) * defaultMovementSpeed;
                }
                else
                {
                    velocity = Vector2.Zero;
                }
            }

            explosionTimer += elapsed;
            if (!inExplosionState && explosionTimer > 5f)
            {
                inExplosionState = true;
                explosionTimer = 0f;
                explosionPhaseIndex = 0;
                cannon.SetSprite(Globals.NULLSPRITE);
                SetSprite(LoadExplosionSprite(explosionPhaseIndex));
                velocity = Vector2.Zero;
            }

            if (inExplosionState)
            {
                if (explosionTimer > 0.5f && explosionPhaseIndex < explosionSpriteNames.Length)
                {
                    SetSprite(LoadExplosionSprite(explosionPhaseIndex));
                    explosionPhaseIndex++;
                    explosionTimer = 0f;
                }
                if (explosionPhaseIndex >= explosionSpriteNames.Length)
                {
                    inExplosionState = false;
                    InitializeMob();
                }
            }
        }

        private ISprite LoadExplosionSprite(int phase)
        {
            var expSprite = new AnimatedSprite(0.3f);
            if (phase == 0)
                expSprite.LoadContent(content, "TDTanksAllSprites", 765, 508, 113, 112, 1);
            else if (phase == 1)
                expSprite.LoadContent(content, "TDTanksAllSprites", 642, 256, 124, 126, 1);
            else
                expSprite.LoadContent(content, "TDTanksAllSprites", 641, 383, 124, 125, 1);
            return expSprite;
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
