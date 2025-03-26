using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Threading.Tasks;

namespace Sprint0
{
    public class ShieldTank : Mob
    {
        private float shieldAngleThreshold = MathHelper.ToRadians(60);
        public float shieldDurability = 400f;

        public ShieldTank(ContentManager content) : base(content)
        {
            TrackTrailsEnabled = true; 
            spriteWidth = 94;
            spriteHeight = 97;
            aggressionLevel= "Neutral";
            health = 200;
            MobXP = 20;
        }

        protected override void InitializeMob()
        {
            currentMobType = EntityKeys.MobType.ShieldTank;
            defaultMovementSpeed = 30f;
            firingInterval = 0f;
            currentProjectileVariables["projectileType"] = "Default";

            var bodySprite = new AnimatedSprite(0.3f);
            bodySprite.LoadContent(content, "TDTanksAllSprites", 768, 0, 94, 97, 1);
            SetSprite(bodySprite);

            cannon = new Cannon(content, Globals.NULLSPRITE_A, this, new Vector2(14, 10), 30f, new Vector2(0, 0),
                    0f, 0f, 0f, 0f);
        }

        protected override void UpdateMobBehavior()
        {
            FollowPlayer(aggressionLevel);
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

        public override void ChangeHealth(int change)
        {
            if (change < 0)
            {
                ShieldedDamage(Math.Abs(change), Vector2.Zero);
            }
            else if (change > 0) 
            {
                isHealed = true;
                changeIndicator = Color.Green;
                Task.Delay(250).ContinueWith(_ =>
                {
                    isHealed = false;
                    changeIndicator = null;
                });
            }
        }

        // Use this method when the damage source position is known.
        public void ShieldedDamage(int damage, Vector2 attackerPosition)
        {
            if (attackerPosition == Vector2.Zero)
            {
                base.ChangeHealth(-damage); // Ensure damage is correctly subtracted
                return;
            }

            Vector2 directionToAttacker = Vector2.Normalize(attackerPosition - GetPosition());
            Vector2 mobForward = new Vector2((float)Math.Sin(bodyRotation), -(float)Math.Cos(bodyRotation));
            float angle = (float)Math.Acos(Vector2.Dot(directionToAttacker, mobForward));

            if (angle < shieldAngleThreshold)
            {
                shieldDurability -= damage;
                if (shieldDurability <= 0)
                {
                    base.ChangeHealth(-damage); // Correctly apply the damage
                }
            }
            else
            {
                base.ChangeHealth(-damage); // Correctly apply the damage
            }
        }
    }
}
