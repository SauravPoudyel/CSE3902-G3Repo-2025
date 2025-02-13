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

        private static int projectileCounter = 0;
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
        private const float CannonLowerBound = MathHelper.PiOver2;
        private const float CannonUpperBound = MathHelper.Pi + MathHelper.PiOver2;

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

            AddSprite("ExplodingTank", new AnimatedSprite(0.3f));
            sprites["ExplodingTank"].LoadContent(content, "TDTanksAllSprites", 768, 256, 95, 113, 1);  


            AddSprite("Explosion1", new AnimatedSprite(0.3f));
            sprites["Explosion1"].LoadContent(content, "TDTanksAllSprites", 765, 508, 114, 112, 1);
            
            AddSprite("Explosion2", new AnimatedSprite(0.3f));
            sprites["Explosion2"].LoadContent(content, "TDTanksAllSprites", 641, 383, 125, 125, 1);

            AddSprite("Explosion3", new AnimatedSprite(0.3f));
            sprites["Explosion3"].LoadContent(content, "TDTanksAllSprites", 641, 256, 125, 126, 1);

            SetSprite(sprites["ExplodingTank"]);
            SetSpriteSecondary(sprites["Cannon"]);
        }

        private void SetEnemyType(MobType mobType)
        {
            this.mobType = mobType;
            this.SetSprite(this.mobType.ToString());
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
            else
            {
                UpdateSmallEnemy(gameTime);
            } else if (mobType == MobType.ExplodingTank) 
            {
                System.Console.WriteLine("Exploding tank"); 
                UpdateExplodingTank(gameTime);
            }
            

            position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            cannonRotation += cannonAngularVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (cannonRotation > CannonUpperBound)
            {
                cannonRotation = CannonUpperBound;
                cannonAngularVelocity = -System.Math.Abs(cannonAngularVelocity);
            }
            else if (cannonRotation < CannonLowerBound)
            {
                cannonRotation = CannonLowerBound;
                cannonAngularVelocity = System.Math.Abs(cannonAngularVelocity);
            }

            shootTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (shootTimer > 4)
            {
                SpawnProjectile();
                shootTimer = 0f;
            }

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
            {
                velocity = new Vector2(-80, 0);
            }
            else if (position.X < 600)
            {
                velocity = new Vector2(80, 0);
            }
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

        public void SpawnProjectile()
        {
            Vector2 cannonTipOffset = new Vector2(0, 48); // so the bullets spwan from the cannon
            Vector2 cannonTip = position + Vector2.Transform(cannonTipOffset, Matrix.CreateRotationZ(cannonRotation));

            /* 
             * The cannon has a base direction and two additional directions to create a spread effect. the cannon **points downward** so by default, we use (0, 1)
             * The left and right directions are slightly rotated from the base direction.
             * Matrix.CreateRotationZ(angle)` rotates a vector counterclockwise by the specified angle in radians, 
             * got reference from: https://community.monogame.net/t/rotating-a-sprite-and-getting-the-new-point-and-rotation/20058
             * If anybody feels a burning desire to work on collision and stuff, this is good to look at for the future
             */
             
            Vector2 baseDirection = Vector2.Transform(new Vector2(0, 1), Matrix.CreateRotationZ(cannonRotation));
            float spreadAngle = MathHelper.ToRadians(10);
            Vector2 leftDirection = Vector2.Transform(baseDirection, Matrix.CreateRotationZ(-spreadAngle));
            Vector2 rightDirection = Vector2.Transform(baseDirection, Matrix.CreateRotationZ(spreadAngle));

            Vector2[] projectileDirections = {leftDirection, baseDirection, rightDirection};

            for (int i = 0; i < 3; i++)
            {
                string projectileKey = "MobProjectile_" + projectileCounter++;
                Projectile projectile = new Projectile(content, projectileKey);

                Dictionary<string, object> projectileParams = new Dictionary<string, object>
                {
                    { "create", projectile},
                    { "entityName", projectile.GetEntityKey()},
                    { "position", cannonTip },
                    { "velocity", (projectileDirections[i] * projectile.GetBaseSpeed()) + velocity}
                };

                commandQueue.Enqueue(new CommandRequest("CreateEntity", projectileParams));
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
