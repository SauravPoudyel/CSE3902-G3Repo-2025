using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Sprint0
{
    public class SwarmingTank : Mob
    {
        private float explosionRadius = 200f; // Explodes when within this range of the player.
        private bool inExplosionState = false;

        public SwarmingTank(ContentManager content) : base(content)
        {
            TrackTrailsEnabled = true;
            spriteWidth = 53;
            spriteHeight = 56;
            aggressionLevel = "Aggressive";
            MobXP = 20;
        }

        protected override void InitializeMob()
        {
            currentMobType = EntityKeys.MobType.SwarmingTank;
            defaultMovementSpeed = 120f; // Runs much faster towards the player.
            firingInterval = 0f; // Doesn't shoot.
            inExplosionState = false;

            var bodySprite = new AnimatedSprite(0.3f);
            bodySprite.LoadContent(content, "TDTanksAllSprites", 1126, 275, 53, 56, 2);
            SetSprite(bodySprite);

            cannon = new Cannon(content, Globals.NULLSPRITE_A, this, new Vector2(14, 10), 30f, new Vector2(0, 0),
                    0f, 0f, 0f, 0f);
        }

        protected override void UpdateMobBehavior()
        {
            if (inExplosionState) return;

            FollowPlayer(aggressionLevel); 

            float distanceToPlayer = Vector2.Distance(GetPosition(), lastKnownPlayerPosition);
            if (distanceToPlayer < explosionRadius)
            {
                inExplosionState = true;
                Explode();
            }
        }

        private void Explode()
        {
            base.OnDeath(); 
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
