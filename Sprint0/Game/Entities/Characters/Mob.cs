using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading;


namespace Sprint0
{
    public class Mob : Entity
    {
        enum MobType
        {
            BossTank,
            SmallEnemy,
            ExplodingTank
        }

        private ContentManager content;
        private float movementTimer;
        private float shootTimer;
        private ISprite spriteSecondary;
        private MobType mobType;
        private int mobPhase; // Tracks phase of "L" movement for SmallEnemy
        Boolean isExploding;
        private int explosionPhase = 0;
        private float explodeTimer = 0f; //explodeTimer is when explosion starts
        private float explosionTimer = 0f; //explosionTimer is once the explosion starts 
        private float cannonRotation = MathHelper.ToRadians(200);
        private float cannonAngularVelocity = MathHelper.ToRadians(20);
        // Cannon oscillates between 90 (π/2) and 270 (3π/2)
        private float cannonLowerBound = MathHelper.PiOver2;
        private float cannonUpperBound = MathHelper.Pi + MathHelper.PiOver2;

        public Mob(ContentManager content)
        {
            this.content = content;
            this.velocity = new Vector2(80, 0);
            this.mobType = MobType.BossTank;

            AddSprite("BossTank", new AnimatedSprite(0.3f));
            sprites["BossTank"].LoadContent(content, "TDTanksAllSprites", 641, 661, 123, 144, 1);

            AddSprite("SmallEnemy", new AnimatedSprite(0.3f));
            sprites["SmallEnemy"].LoadContent(content, "TDTanksAllSprites", 768, 256, 95, 113, 1);

            AddSprite("Cannon", new AnimatedSprite(0.3f));
            sprites["Cannon"].LoadContent(content, "TDTanksAllSprites", 832, 186, 28, 64, 1);

            //Change this sprite to a different tank
            AddSprite("ExplodingTank", new AnimatedSprite(0.3f));
            sprites["ExplodingTank"].LoadContent(content, "TDTanksAllSprites", 952, 569, 81, 76, 1);  

            AddSprite("Explosion1", new AnimatedSprite(0.3f));
            sprites["Explosion1"].LoadContent(content, "TDTanksAllSprites", 765, 508, 113, 112, 1);
            
            AddSprite("Explosion3", new AnimatedSprite(0.3f));
            sprites["Explosion3"].LoadContent(content, "TDTanksAllSprites", 641, 383, 124, 125, 1);

            AddSprite("Explosion2", new AnimatedSprite(0.3f));
            sprites["Explosion2"].LoadContent(content, "TDTanksAllSprites", 642, 256, 124, 126, 1);

            AddSprite("NULL", new AnimatedSprite(0.3f));
            sprites["NULL"].LoadContent(content, "TDTanksAllSprites", 129, 0, 12, 12, 1);

            SetSprite(sprites["BossTank"]);
            SetSpriteSecondary(sprites["Cannon"]);
        }

        private void SetEnemyType(MobType mobType)
        {
            this.mobType = mobType;
            this.SetSprite(this.mobType.ToString());
            SetSpriteSecondary(sprites["Cannon"]);
        }

        public string GetEnemyType()
        {
            return mobType.ToString();
        }

        public void CycleEnemyNext()
        {
            if (mobType == MobType.BossTank)
            {
                SetEnemyType(MobType.SmallEnemy);
            } else if (mobType == MobType.SmallEnemy) {
                SetEnemyType(MobType.ExplodingTank);
                explosionPhase = 0;
            } else if (mobType == MobType.ExplodingTank) {
                SetEnemyType(MobType.BossTank);
            }
        }
        public void CycleEnemyPrev() {
            if(mobType == MobType.BossTank) {
                SetEnemyType(MobType.ExplodingTank);
                explosionPhase = 0;
            } else if (mobType == MobType.SmallEnemy) {
                SetEnemyType(MobType.BossTank);
            } else if (mobType == MobType.ExplodingTank) {
                SetEnemyType(MobType.SmallEnemy);
            }
        }

        public override void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);

            if (isExploding) 
            {
                HandleExplosion(gameTime);
                System.Console.WriteLine("Exploding"); 
            } 
            if (mobType == MobType.BossTank)
            {
                UpdateBossTank();
            }
            else if (mobType == MobType.SmallEnemy) 
            {
                UpdateSmallEnemy(gameTime);
            } else if (mobType == MobType.ExplodingTank) 
            {
                System.Console.WriteLine("Exploding tank"); 
                UpdateExplodingTank(gameTime);
            }

            UpdateCannon(gameTime); 

            position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        private void UpdateCannon(GameTime gameTime) 
        {
            // Set speed and bounds based on mob type **only once** when the type changes
            float newAngularVelocity = 0;
            if (mobType == MobType.BossTank)
            {
                newAngularVelocity = MathHelper.ToRadians(20); // Slower oscillation speed
                cannonLowerBound = MathHelper.PiOver2;            // 90 degrees
                cannonUpperBound = MathHelper.Pi + MathHelper.PiOver2; // 270 degrees
            }
            else if (mobType == MobType.SmallEnemy)
            {
                newAngularVelocity = MathHelper.ToRadians(80); // Faster oscillation speed
                cannonLowerBound = MathHelper.ToRadians(120);     // 120 degrees
                cannonUpperBound = MathHelper.ToRadians(240);     // 240 degrees
            }
            
            if (System.Math.Abs(cannonAngularVelocity) != newAngularVelocity)
            {
                if (cannonAngularVelocity < 0)
                    cannonAngularVelocity = -newAngularVelocity;
                else
                    cannonAngularVelocity = newAngularVelocity;
            }

            // Update cannon rotation
            cannonRotation += cannonAngularVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // oscillate
            if (cannonRotation >= cannonUpperBound)
            {
                cannonRotation = cannonUpperBound;
                cannonAngularVelocity = -System.Math.Abs(cannonAngularVelocity);
            }
            else if (cannonRotation <= cannonLowerBound)
            {
                cannonRotation = cannonLowerBound;
                cannonAngularVelocity = System.Math.Abs(cannonAngularVelocity);
            }

            // Fire projectile every 4 seconds
            shootTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (shootTimer > 4)
            {
                FireProjectile();
                shootTimer = 0f;
            }

        }

        public void FireProjectile()
        {
            Vector2 cannonTipOffset = new Vector2(0, 48); // Ensures bullets spawn from the cannon
            Vector2 cannonTip = position + Vector2.Transform(cannonTipOffset, Matrix.CreateRotationZ(cannonRotation));

            var parameters = new Dictionary<string, object>
            {
                { "projectileType", "Shotgun" },
                { "spawnPosition", cannonTip },
                { "cannonRotation", cannonRotation },
                { "shooterVelocity", velocity }
            };

            parameters["spreadAngle"] = MathHelper.ToRadians(10);
            parameters["numberOfProjectiles"] = 3;

            commandQueue.Enqueue(new CommandRequest("CreateProjectile", parameters));
        }

        public void UpdateSmallEnemy(GameTime gameTime)
        {
            float speed = 200f;
            switch (mobPhase)
            {
                case 0:
                    velocity = new Vector2(speed, 0);
                    break;
                case 1:
                    velocity = new Vector2(0, speed);
                    break;
                case 2:
                    velocity = new Vector2(-speed, 0);
                    break;
                case 3:
                    velocity = new Vector2(0, -speed);
                    break;
            }

            movementTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (movementTimer > 0.6f)
            {
                mobPhase = (mobPhase + 1) % 4;
                movementTimer = 0f;
            }
        }

        public void UpdateBossTank()
        {
            if (position.X > 700)
                velocity = new Vector2(-80, 0);
            else if (position.X < 600)
                velocity = new Vector2(80, 0);
        }

        public void UpdateExplodingTank(GameTime gameTime) {

            if (position.X > 700)
                velocity = new Vector2(-50, 0);
            else if (position.X < 600)
                velocity = new Vector2(50, 0);

            explodeTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if(explodeTimer > 2) {
                System.Console.WriteLine("Exploding 1"); 
                isExploding = true;
                explosionTimer = 0f;
                explodeTimer = 0; 
            }
            if (explosionPhase > 2) {
                isExploding = false;
            }
        }

        private void HandleExplosion(GameTime gameTime){
            explosionTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(explosionTimer > 0.5f) {
                SetSpriteSecondary(sprites["NULL"]);
                switch (explosionPhase)
            {
                case 0: SetSprite("Explosion1"); break;
                case 1: SetSprite("Explosion2"); break;
                case 2: SetSprite("Explosion3"); break;
            }
            explosionPhase++;
            explosionTimer = 0f;
            }
    
        }

        public void SetSpriteSecondary(ISprite sprite)
        {
            spriteSecondary = sprite;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            SpriteEffects effects = SpriteEffects.None;
            sprite.Draw(spriteBatch, position, effects, 0f);

            // Draw the cannon using the tank's center (position) and the given pivot (14,10).
            Vector2 cannonPivot = new Vector2(14, 10);
            spriteSecondary.Draw(spriteBatch, position, effects, cannonRotation, cannonPivot);
        }
    }
}
