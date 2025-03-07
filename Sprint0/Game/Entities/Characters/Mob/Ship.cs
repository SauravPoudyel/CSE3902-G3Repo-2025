using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Sprint0
{
    public class Ship : Mob
    {
        private bool vertical;

        // Constructor: Pass in the ContentManager and a flag for vertical orientation.
        public Ship(ContentManager content, bool isVertical) : base(content)
        {
            vertical = isVertical;
            InitializeMob();
        }

        protected override void InitializeMob()
        {
            currentMobType = EntityKeys.MobType.ShipVertical;
            defaultMovementSpeed = 120f;
            firingInterval = 1.5f;
            currentProjectileVariables["projectileType"] = "Default";

            AnimatedSprite shipSprite = new AnimatedSprite(0.3f);
            if (vertical)
            {
                shipSprite.LoadContent(content, "TDTanksAllSprites", 1135, 841, 70, 120, 1);
                bodyRotation = MathHelper.PiOver2;
            }
            else
            {
                shipSprite.LoadContent(content, "TDTanksAllSprites", 1135, 841, 70, 120, 1);
                bodyRotation = 0f;
            }
            SetSprite(shipSprite);

            Vector2 cannonOffset = vertical ? new Vector2(25, 60) : new Vector2(60, 25);
            cannon = new Cannon(content, Globals.NULLSPRITE_A, this, cannonOffset, 30f, new Vector2(0, 0),
                                0f, 0f, 0f, 0f);

            ResetMobPosition();
        }

        protected override void UpdateMobBehavior()
        {
            // Use the new axis-specific following behavior.
            if (vertical)
            {
                FollowPlayer("FollowY-Axis");
            }
            else
            {
                FollowPlayer("FollowX-Axis");
            }
        }

        protected override void ChangeMobType(EntityKeys.MobType type)
        {
            currentMobType = type;
            InitializeMob();
        }

        protected override void ResetMobPosition()
        {
            if (vertical)
            {
                // Start off-screen at the top center.
                position = new Vector2(Globals.SCREENWIDTH / 2, -50);
            }
            else
            {
                // Start off-screen at the left center.
                position = new Vector2(-50, Globals.SCREENHEIGHT / 2);
            }
        }


        public override void FireProjectile()
        {
            Vector2 spawnPos = GetPosition();
            var p = new Dictionary<string, object>
            {
                { "projectileType", currentProjectileVariables["projectileType"] },
                { "spawnPosition", spawnPos },
                { "cannonRotation", cannon.Rotation },
                { "speedModifier", -200f }
            };
            commandQueue.Enqueue(new CommandRequest("CreateProjectile", p));
        }
    }
}
