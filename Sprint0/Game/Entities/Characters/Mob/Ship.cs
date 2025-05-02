using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0
{
    public class Ship : Mob
    {
        private bool vertical;
        private float fixedY; // For horizontal ships, this stores the constant Y coordinate.

        public Ship(ContentManager content, bool isVertical) : base(content)
        {
            TrackTrailsEnabled = false;
            spriteWidth = 94;
            spriteHeight = 140;
            vertical = isVertical;
            health = 200;
            MobXP = 50;
            InitializeMob(); // this needs to be called here after the base constructor to pass isVertical
        }

        protected override void InitializeMob()
        {
            // Set the mob type based on orientation.
            currentMobType = vertical ? EntityKeys.MobType.ShipVertical : EntityKeys.MobType.ShipHorizontal;
            defaultMovementSpeed = 80f;
            firingInterval = 2f;
            bodyRotation = vertical ? 0f : -MathHelper.PiOver2;

            ResetMobPosition();
        }

        protected override void ChangeMobType(EntityKeys.MobType type)
        {
            currentMobType = type;
            InitializeMob();
        }

        protected override void ResetMobPosition()
        {
            if (vertical)
                position = new Vector2(Globals.SCREENWIDTH / 2, -50);
            else
            {
                position = new Vector2(-50, Globals.SCREENHEIGHT / 2);
                fixedY = position.Y; // Lock the Y coordinate for horizontal movement.
            }
        }

        public override void FireProjectile()
        {
            Vector2 spawnPos = GetPosition();
            var parameters = new Dictionary<string, object>
            {
                { "projectileType", currentProjectileVariables["projectileType"] },
                { "spawnPosition", spawnPos },
                { "cannonRotation", cannon.Rotation },
                { "speedModifier", -200f }, 
                { "owner", this }
            };
            commandQueue.Enqueue(new CommandRequest("CreateProjectile", parameters));

            AudioManager.PlaySound(AudioManager.SoundKey.Shoot);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (var trail in trackTrailList)
                trail.Draw(spriteBatch);
            sprite?.Draw(spriteBatch, position, SpriteEffects.None, bodyRotation, null, changeIndicator, 1.5f);
            cannon?.Draw(spriteBatch);
        }
    }
}
