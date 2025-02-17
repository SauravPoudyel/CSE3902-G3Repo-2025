using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Sprint0
{
    public class Plane : Mob
    {
        private float flightAngle;
        private Vector2 flightCenter;
        private float flightRadius;
        private float flightSpeedModifier = 2f;

        public Plane(ContentManager content) : base(content)
        {
        }

        protected override void InitializeMob()
        {
            mobType = MobType.Plane;
            shootInterval = 2f;
            currentProjectileVariables["projectileType"] = "Default";
            flightCenter = new Vector2(Globals.SCREENWIDTH / 2, Globals.SCREENHEIGHT / 2);
            flightRadius = 150f;
            flightAngle = 0f;
            ISprite planeSprite = new AnimatedSprite(0.3f);
            planeSprite.LoadContent(content, "TDTowerDefenseSprites", 2183, 1411, 135, 135, 1);
            SetSprite(planeSprite);
            cannon = new Cannon(Globals.NULLSPRITE, this, new Vector2(0, 0), 30f, 0f, 0f, 0f, 0f);
        }

        protected override void UpdateMobBehavior(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            flightAngle += MathHelper.ToRadians(45) * elapsed * flightSpeedModifier;
            if (flightAngle > MathHelper.TwoPi)
                flightAngle -= MathHelper.TwoPi;
            position = flightCenter + new Vector2((float)Math.Cos(flightAngle), (float)Math.Sin(flightAngle)) * flightRadius;
        }

        public override void FireProjectile()
        {
            Vector2 spawnPos = GetPosition();
            var parameters = new Dictionary<string, object>
            {
                { "projectileType", currentProjectileVariables["projectileType"] },
                { "spawnPosition", spawnPos },
                { "cannonRotation", 0f },
                { "speedModifier", -200f }
            };
            commandQueue.Enqueue(new CommandRequest("CreateProjectile", parameters));
        }

        protected override void SetEnemyType(MobType type)
        {
            InitializeMob();
        }

        protected override void ResetPosition()
        {
            flightCenter = new Vector2(Globals.SCREENWIDTH / 2, Globals.SCREENHEIGHT / 2);
            flightAngle = 0f;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw the plane sprite rotated so it faces the tangent direction.
            // For a sprite facing right by default, the tangent (for counterclockwise orbit) is flightAngle + Pi/2.
            if (sprite != null)
                sprite.Draw(spriteBatch, position, SpriteEffects.None, flightAngle + MathHelper.PiOver2);
        }
    }
}
