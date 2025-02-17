using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Sprint0
{
    public class Player : Entity
    {
        private Cannon cannon;
        private ContentManager content;
        private Dictionary<string, object> currentProjectileVariables;

        public Player(ContentManager content)
        {
            this.content = content;
            AddSprite("Idle", new AnimatedSprite(0.3f));
            sprites["Idle"].LoadContent(content, "LinkSpritesheet", 5, 2, 66, 64, 1);
            AddSprite("Up", new AnimatedSprite(0.3f));
            sprites["Up"].LoadContent(content, "LinkSpritesheet", 280, 2, 62, 64, 2);
            AddSprite("Down", new AnimatedSprite(0.3f));
            sprites["Down"].LoadContent(content, "LinkSpritesheet", 5, 2, 66, 64, 2);
            AddSprite("Left", new AnimatedSprite(0.3f));
            sprites["Left"].LoadContent(content, "LinkSpritesheet", 140, 2, 62, 66, 2);
            AddSprite("Right", new AnimatedSprite(0.3f));
            sprites["Right"].LoadContent(content, "LinkSpritesheet", 140, 2, 62, 66, 2);
            ISprite cannonSprite = new StaticSprite();
            cannonSprite.LoadContent(content, "TDTanksAllSprites", 832, 186, 28, 64, 1);
            // The cannon is attached to the player at (14,10); its tip (for spawning projectiles) is offset (0,44) from the owner.
            cannon = new Cannon(cannonSprite, this, new Vector2(14, 10), new Vector2(0, 44));
            SetSprite("Idle");
            currentProjectileVariables = new Dictionary<string, object>
            {
                { "projectileSpeedModifer", 1f },
                { "projectileSpawnOffset", new Vector2(0, 44) },
                { "projectileSpread", MathHelper.ToRadians(10) },
                { "projectileCounter", 0 },
                { "projectileType", "Default" }
            };
        }

        public override void Update(GameTime gameTime)
        {
            if (velocity.LengthSquared() > 10f)
            {
                sprite.Update(gameTime);
            }
            position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            SpriteEffects effects = SpriteEffects.None;
            if (sprite == sprites["Left"])
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            sprite.Draw(spriteBatch, position, effects, 0f);
            cannon.Draw(spriteBatch);
        }

        public void SetProjectileType(string newType)
        {
            if (newType == "Default" || newType == "Sniper" || newType == "Rocket" ||
                newType == "Shotgun" || newType == "Bomb")
                currentProjectileVariables["projectileType"] = newType;
            else
                System.Console.WriteLine("Invalid projectile type: " + newType);
        }

        public void FireProjectile()
        {
            Vector2 cannonTip = cannon.GetTipPosition();
            var parameters = new Dictionary<string, object>
            {
                { "projectileType", currentProjectileVariables["projectileType"] },
                { "spawnPosition", cannonTip },
                { "cannonRotation", cannon.Rotation },
                { "shooterVelocity", velocity }
            };
            if ((string)currentProjectileVariables["projectileType"] == "Shotgun")
            {
                parameters["spreadAngle"] = MathHelper.ToRadians(10);
                parameters["numberOfProjectiles"] = 3;
            }
            commandQueue.Enqueue(new CommandRequest("CreateProjectile", parameters));
            HandleRecoil();
        }

        private void HandleRecoil()
        {
            string projType = (string)currentProjectileVariables["projectileType"];
            float recoilStrength = 20f;
            if (projType.Equals("Sniper"))
                recoilStrength = 30f;
            else if (projType.Equals("Rocket"))
                recoilStrength = 80f;
            Vector2 recoilDir = Vector2.Transform(new Vector2(0, 1), Matrix.CreateRotationZ(cannon.Rotation));
            velocity -= recoilDir * recoilStrength;
        }

        public void SetCannonRotation(float rotation)
        {
            cannon.Rotation = rotation;
        }
    }
}
