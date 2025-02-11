using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Threading;

namespace Sprint0
{
    public class Mob : Entity
    {
        enum MobType
        {
            BossTank,
            SmallEnemy,
        }

        private static int projectileCounter = 0;
        private ContentManager content;
        private float movementTimer;
        private float shootTimer;
        private ISprite spriteSecondary;
        private MobType mobType;
        private int mobPhase;
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

            SetSprite(sprites["BossTank"]);
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
            }
            else
            {
                SetEnemyType(MobType.BossTank);
            }
        }

        public void CycleEnemyPrev()
        {
            if (mobType == MobType.BossTank)
            {
                SetEnemyType(MobType.SmallEnemy);
            }
            else
            {
                SetEnemyType(MobType.BossTank);
            }
        }

        public override void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);

            if (mobType == MobType.BossTank)
            {
                UpdateBossTank();
            }
            else
            {
                UpdateSmallEnemy(gameTime);
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

        public void SpawnProjectile()
        {
            float speed = 280f;

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

            Vector2[] projectileVelocities =
            {
                leftDirection * speed + velocity,
                baseDirection * speed + velocity,
                rightDirection * speed + velocity
            };

            for (int i = 0; i < 3; i++)
            {
                Dictionary<string, object> projectileParams = new Dictionary<string, object>
                {
                    { "create", new Projectile(content) },
                    { "entityName", "MobProjectile_" + projectileCounter++ },
                    { "position", cannonTip },
                    { "velocity", projectileVelocities[i] }
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
