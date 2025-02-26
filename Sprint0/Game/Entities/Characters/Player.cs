using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Sprint0
{
    public class Player : Character
    {
        public Dictionary<string, float> EffectTimers = new Dictionary<string, float>();
        public float speedMultiplier = 1f;
        public bool shieldActive = false;

        public Player(ContentManager content) : base(content)
        {
            bodyRotation = 0f;

            AddSprite("TankBody", new AnimatedSprite(0.3f));
            sprites["TankBody"].LoadContent(content, "TDTanksAllSprites", 795, 1052, 74, 76, 1);
            SetSprite("TankBody");

            ISprite cannonSprite = new AnimatedSprite(0.3f);
            cannonSprite.LoadContent(content, "TDTanksAllSprites", 1060, 837, 24, 60, 1);
            cannon = new Cannon(cannonSprite, this, new Vector2(12, 5), 50f,
                                  0f, MathHelper.ToRadians(20), MathHelper.PiOver2, MathHelper.Pi + MathHelper.PiOver2);

            trackTrailSprite = new StaticSprite();
            trackTrailSprite.LoadContent(content, "TDTanksAllSprites", 953, 665, 73, 88, 1);
        }

        public override void Update()
        {
            prevPosition = position;
            float baseTurnSpeed = 0.05f;
            float turnSpeedFactor = MathHelper.Clamp(Math.Abs(velocity.Y) / 50f, 0.5f, 0.6f);
            if (Math.Abs(velocity.X) > 1f || Math.Abs(velocity.Y) > 1f)
               bodyRotation += velocity.X * baseTurnSpeed * turnSpeedFactor * Globals.FRAMETIME;
            Vector2 forwardDir = new Vector2((float)Math.Sin(bodyRotation), -(float)Math.Cos(bodyRotation));
            position += forwardDir * -velocity.Y * speedMultiplier * Globals.FRAMETIME;
            CalculateBounds(74f, 76f);

            if (areTrackTrailsEnabled)
                TrackTrail.UpdateTrackTrails(trackTrailList, Globals.FRAMETIME, position, bodyRotation, trackTrailSprite, ref trackTrailSpawnTimer, trackTrailSpawnInterval);

            if (Math.Abs(velocity.Y) > 0.1f)
                sprite.Update();

            if (health <= 0 && !isDead) {
                isDead = true; 
                OnDeath();
            }

            EffectFactory.UpdateEffects(this);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            TrackTrail.DrawTrackTrails(trackTrailList, spriteBatch);
            Vector2 tankCenter = new Vector2(37, 38);
            sprite.Draw(spriteBatch, position, SpriteEffects.FlipVertically, bodyRotation, tankCenter, Color.White);
            cannon.Draw(spriteBatch);
        }
    }
}
