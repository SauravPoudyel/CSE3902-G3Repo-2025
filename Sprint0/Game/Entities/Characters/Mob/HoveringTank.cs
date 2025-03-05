using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;

namespace Sprint0
{
    public class HoveringTank : Mob
    {
        public HoveringTank(ContentManager content) : base(content)
        {
            TrackTrailsEnabled = false;
            spriteWidth = 86;
            spriteHeight = 92;
            aggressionLevel = "Neutral";
        }

        protected override void InitializeMob()
        {
            currentMobType = MobType.HoveringTank; 
            defaultMovementSpeed = 80f;
            firingInterval = 1.4f;

            var bodySprite = new AnimatedSprite(0.3f);
            bodySprite.LoadContent(content, "TDTanksAllSprites", 1135, 180, 86, 92, 1);
            SetSprite(bodySprite);

            var cannonSprite = new AnimatedSprite(0.3f);
            cannonSprite.LoadContent(content, "TDTanksAllSprites", 832, 186, 28, 64, 1);
            cannon = new Cannon(content, cannonSprite, this, new Vector2(14, 10), 50f, new Vector2(12, 70),
                                0f, MathHelper.ToRadians(20), MathHelper.PiOver2, MathHelper.Pi + MathHelper.PiOver2);

            bodyRotation = 0f;
        }

        protected override void UpdateMobBehavior()
        {
            FollowPlayer(aggressionLevel);
            PointCannonPlayer();
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
