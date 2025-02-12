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

        // Store projectile variables in a dictionary for flexibility (powerups, ammo types, etc.)
        private Dictionary<string, object> currentProjectileVariables;

        public Player(ContentManager content)
        {
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

            // Initialize projectile variables in a dictionary
            currentProjectileVariables = new Dictionary<string, object>
            {
                { "projectileSpeed", 300f },
                { "projectileOffset", new Vector2(0, 48) }, // Offset from player's center (cannon tip)
                { "projectileSpread", MathHelper.ToRadians(10) },
                { "projectileCounter", 0 }
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
            Vector2 cannonPivot = new Vector2(10, 10);
            spriteSecondary.Draw(spriteBatch, playerCenter, effects, cannonRotation, cannonPivot);
        }

        public void CreateProjectile(ContentManager content)
        {
            float projectileSpeed = (float)currentProjectileVariables["projectileSpeed"];
            Vector2 projectileOffset = (Vector2)currentProjectileVariables["projectileOffset"];
            float projectileSpread = (float)currentProjectileVariables["projectileSpread"];
            int projectileCounter = (int)currentProjectileVariables["projectileCounter"];

            // Calculate the cannon tip position using the stored offset rotated by cannonRotation
            Vector2 cannonTip = position + Vector2.Transform(projectileOffset, Matrix.CreateRotationZ(cannonRotation));

            // Compute the projectile's velocity vector (direction * speed)
            Vector2 baseDirection = Vector2.Transform(new Vector2(0, 1), Matrix.CreateRotationZ(cannonRotation)); // (0,1) because cannon sprite faces down normally
            Vector2 projectileVelocity = baseDirection * projectileSpeed + velocity; // adds player's movement velocity

            
            Dictionary<string, object> projectileCreationParams = new Dictionary<string, object>
            {
                { "create", new Projectile(content) },
                { "entityName", "PlayerProjectile_" + projectileCounter },
                { "position", cannonTip },
                { "velocity", projectileVelocity}
            };

            commandQueue.Enqueue(new CommandRequest("CreateEntity", projectileCreationParams));

            // Update the projectile counter in the dictionary
            projectileCounter++;
            currentProjectileVariables["projectileCounter"] = projectileCounter;
        }

        // Getter methods for external use (if needed)
        public Vector2 GetPosition()
        {
            return position;
        }

        public Vector2 GetVelocity()
        {
            return velocity;
        }

        public float GetCannonRotation()
        {
            return cannonRotation;
        }
    }
}
