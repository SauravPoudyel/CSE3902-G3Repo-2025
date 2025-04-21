using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Sprint0
{
    public class Plane : Mob
    {
        private float orbitAngle;
        private Vector2 orbitCenter;
        private float orbitRadius;
        private float orbitSpeedFactor = 2f;

        public Plane(ContentManager content) : base(content) 
        {
            TrackTrailsEnabled = false; 
            spriteWidth = 100;
            spriteHeight = 113;
            health = 100;
            MobXP = 50;
        }

        protected override void InitializeMob()
        {
            currentMobType = EntityKeys.MobType.Plane;
            firingInterval = 2f;
            currentProjectileVariables["projectileType"] = "Default";
            orbitCenter = new Vector2(Globals.SCREENWIDTH / 2, Globals.SCREENHEIGHT / 2);
            orbitRadius = 150f;
            orbitAngle = 0f;
            
            var planeBodySprite = new Sprite(0.3f);
            planeBodySprite.LoadContent(content, "TDTowerDefenseSprites", 2183, 1411, 135, 134, 1);
            SetSprite(planeBodySprite);

            cannon = new Cannon(content, Globals.NULLSPRITE_A, this, new Vector2(14, 10), 30f, new Vector2(0, 0),
                    0f, MathHelper.ToRadians(20), 0f, 0f);
            velocity = Vector2.Zero;
            bodyRotation = 0f;
        }

        protected override void UpdateMobBehavior()
        {
            float elap = Globals.FRAMETIME;
            // Instead of instantly setting orbitCenter to lastKnownPlayerPosition,
            // gradually move orbitCenter toward it.
            if (lastKnownPlayerPosition != Vector2.Zero)
            {
                float accelerationFactor = 0.5f; // Adjust this value for faster or slower acceleration
                orbitCenter = Vector2.Lerp(orbitCenter, lastKnownPlayerPosition, accelerationFactor * elap);
            }
            orbitAngle += MathHelper.ToRadians(25) * elap * orbitSpeedFactor;
            orbitAngle %= MathHelper.TwoPi;
            // Set the plane's position based on the current orbit center.
            position = orbitCenter + new Vector2((float)Math.Cos(orbitAngle), (float)Math.Sin(orbitAngle)) * orbitRadius;
            PointCannonPlayer();
        }

        public override void FireProjectile()
        {
            Vector2 spawnPos = GetPosition();
            var parameters = new Dictionary<string, object>
            {
                { "projectileType", currentProjectileVariables["projectileType"] },
                { "spawnPosition", spawnPos },
                { "cannonRotation", cannon.Rotation},
                { "speedModifier", -200f },
                { "owner", this }
            };
            commandQueue.Enqueue(new CommandRequest("CreateProjectile", parameters));
            AudioManager.PlaySound(AudioManager.SoundKey.Shoot);
        }

        protected override void ChangeMobType(EntityKeys.MobType type)
        {
            currentMobType = type;
            InitializeMob();
        }

        protected override void ResetMobPosition()
        {
            orbitCenter = new Vector2(Globals.SCREENWIDTH / 2, Globals.SCREENHEIGHT / 2);
            orbitAngle = 0f;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            float planeRot = orbitAngle + MathHelper.PiOver2;
            if (sprite != null)
                sprite.Draw(spriteBatch, position, SpriteEffects.None, planeRot);
        }
    }
}
