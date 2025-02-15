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
        private ISprite spriteSecondary;
        private float cannonRotation;
        private Vector2 playerCenter; 
        private ContentManager content;

        // Store projectile variables in a dictionary for flexibility (powerups, ammo types, etc.)
        private Dictionary<string, object> currentProjectileVariables;

        // Store iventory variables in a dictionary for flexibility (ammo amount, bombs, coins, etc.)
        private Dictionary<string, object> inventoryVariables;

        public Player(ContentManager content)
        {
            this.content = content;
            // Load Player Sprites
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

            // Load Cannon Sprite
            spriteSecondary = new StaticSprite();
            spriteSecondary.LoadContent(content, "TDTanksAllSprites", 832, 186, 28, 64, 1);

            SetSprite("Idle");

            // These will probably change with permanent upgrades
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
            // Only update animation if player is moving noticeably
            if (velocity.LengthSquared() > 10f)
            {
                sprite.Update(gameTime);
            }
            CalculateMovement(gameTime);

            // Update Cannon Rotation Based on Mouse Position
            MouseState mouseState = Mouse.GetState();
            Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);
            playerCenter = position; // Using player's center

            Vector2 direction = mousePosition - playerCenter;
            cannonRotation = (float)Math.Atan2(direction.Y, direction.X) - MathHelper.PiOver2;
        }

        private void CalculateMovement(GameTime gameTime)
        {
            position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            KeyboardState state = Keyboard.GetState();

            // If no movement keys are pressed, apply friction (simulate tank wheels slowing down)
            if (!(state.IsKeyDown(Keys.W) || state.IsKeyDown(Keys.A) || 
                  state.IsKeyDown(Keys.S) || state.IsKeyDown(Keys.D) ||
                  state.IsKeyDown(Keys.Up) || state.IsKeyDown(Keys.Left) ||
                  state.IsKeyDown(Keys.Down) || state.IsKeyDown(Keys.Right)))
            {
                velocity *= 0.982f;
                if (velocity.LengthSquared() < 0.05f)
                    velocity = Vector2.Zero;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            SpriteEffects effects = SpriteEffects.None;

            // Flip player sprite if moving left
            if (sprite == sprites["Left"])
            {
                effects = SpriteEffects.FlipHorizontally;
            }

            // Draw the player sprite
            sprite.Draw(spriteBatch, position, effects, 0f);

            // Draw the cannon using player's center and a fixed pivot (where the cannon attaches)
            Vector2 cannonPivot = new Vector2(14, 10);
            spriteSecondary.Draw(spriteBatch, playerCenter, effects, cannonRotation, cannonPivot);
        }
        public void SetProjectileType(string newType)
        {
            if (newType == "Default" || newType == "Sniper" || newType == "Rocket" || newType == "Shotgun")
                currentProjectileVariables["projectileType"] = newType;
            else
                System.Console.WriteLine("Invalid projectile type: " + newType);
        }

        public void FireProjectile()
        {
            Vector2 cannonTipOffset = (Vector2)currentProjectileVariables["projectileSpawnOffset"];
            Vector2 cannonTip = position + Vector2.Transform(cannonTipOffset, Matrix.CreateRotationZ(cannonRotation));

            var parameters = new Dictionary<string, object>
            {
                { "projectileType", currentProjectileVariables["projectileType"] },
                { "spawnPosition", cannonTip },
                { "cannonRotation", cannonRotation },
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

        private void HandleRecoil(){
            string projType = (string)currentProjectileVariables["projectileType"];

            float recoilStrength = 20f;
            if (projType.Equals("Sniper"))
                recoilStrength = 30f;
            else if (projType.Equals("Rocket"))
                recoilStrength = 80f;

            Vector2 recoilDir = Vector2.Transform(new Vector2(0, 1), Matrix.CreateRotationZ(cannonRotation));
            velocity -= recoilDir * recoilStrength;
        }

        public float GetCannonRotation()
        {
            return cannonRotation;
        }
    }
}
