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
            TrackTrailsEnabled = true; 
            spriteWidth = 100;
            spriteHeight = 113;
        }

        protected override void InitializeMob()
        {
            currentMobType = MobType.Plane;
            firingInterval = 2f;
            currentProjectileVariables["projectileType"] = "Default";
            orbitCenter = new Vector2(Globals.SCREENWIDTH / 2, Globals.SCREENHEIGHT / 2);
            orbitRadius = 150f;
            orbitAngle = 0f;
            var planeBodySprite = new AnimatedSprite(0.3f);
            planeBodySprite.LoadContent(content, "TDTowerDefenseSprites", 2183, 1411, 135, 135, 1);
            SetSprite(planeBodySprite);
            cannon = new Cannon(content, Globals.NULLSPRITE_A, this, new Vector2(14, 10), 30f, new Vector2(0, 0),
                    0f, 0f, 0f, 0f);
            velocity = Vector2.Zero;
            bodyRotation = 0f;
        }

        protected override void UpdateMobBehavior()
        {
            float elap = Globals.FRAMETIME;
            orbitAngle += MathHelper.ToRadians(25) * elap * orbitSpeedFactor;
            orbitAngle %= MathHelper.TwoPi;
            position = orbitCenter + new Vector2((float)Math.Cos(orbitAngle), (float)Math.Sin(orbitAngle)) * orbitRadius;
            if (lastKnownPlayerPosition != Vector2.Zero)
            {
                Vector2 dir = lastKnownPlayerPosition - position;
                float desired = (float)Math.Atan2(dir.Y, dir.X) - MathHelper.PiOver2;
                float diff = MathHelper.WrapAngle(desired - bodyRotation);
                float turnRate = MathHelper.ToRadians(50) * elap;
                if (Math.Abs(diff) > turnRate)
                    diff = Math.Sign(diff) * turnRate;
                bodyRotation += diff;
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

        protected override void ChangeMobType(MobType type)
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
