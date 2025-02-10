using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Threading;

namespace Sprint0
{
    public class Mob : Entity
    {
        enum MobType {
            BossTank,
            SmallEnemy,

        }
        private static int projectileCounter = 0;
        private ContentManager content;
        private float movementTimer;
        private float shootTimer;
        private ISprite spriteSecondary;
        private MobType mobType;
        private int mobPhase; // Tracks phase of "L" movement for SmallEnemy

        public Mob(ContentManager content) 
        {
            this.content = content;
            this.velocity = new Vector2(50, 0);
            this.mobType = MobType.BossTank;
            //Original boss tank params (641, 510, 123, 150)
            
            AddSprite("BossTank", new AnimatedSprite(0.3f));
            sprites["BossTank"].LoadContent(content, "TDTanksAllSprites", 641, 661, 123, 144, 1);

            AddSprite("SmallEnemy", new AnimatedSprite(0.3f));
            sprites["SmallEnemy"].LoadContent(content, "TDTanksAllSprites", 768, 256, 95, 113, 1);

            AddSprite("Cannon", new AnimatedSprite(0.3f));
            sprites["Cannon"].LoadContent(content, "TDTanksAllSprites", 832, 186, 28, 64, 1);  

            SetSprite(sprites["BossTank"]);
            SetSpriteSecondary(sprites["Cannon"]);
        }
        private void SetEnemyType(MobType mobType) {
            this.mobType = mobType;
            this.SetSprite(this.mobType.ToString());
        }

        public string GetEnemyType() {
            return mobType.ToString();
        }
        public void CycleEnemyNext() {
            if(mobType == MobType.BossTank) {
                SetEnemyType(MobType.SmallEnemy);
            } else if (mobType == MobType.SmallEnemy) {
                SetEnemyType(MobType.BossTank);
            }        
        }
        public void CycleEnemyPrev() {
            if(mobType == MobType.BossTank) {
                SetEnemyType(MobType.SmallEnemy);
            } else if (mobType == MobType.SmallEnemy) {
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
            else if (mobType == MobType.SmallEnemy)
            {
                UpdateSmallEnemy(gameTime);
            }

            position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            // Update separate shooting timer
            shootTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (shootTimer > 4)
            {
                SpawnProjectile();
                shootTimer = 0f;
            }
        }

        public void UpdateSmallEnemy(GameTime gameTime)
        {
            float movementTime = 2.0f; // time before switching direction
            float speed = 50f;

            switch (mobPhase)
            {
                case 0: velocity = new Vector2(speed, 0); break; // move Right
                case 1: velocity = new Vector2(0, speed); break; // move Down
                case 2: velocity = new Vector2(-speed, 0); break; // move Left
                case 3: velocity = new Vector2(0, -speed); break; // move Up
            }

            movementTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (movementTimer > movementTime)
            {
                mobPhase = (mobPhase + 1) % 4; // cycle through movement phases
                movementTimer = 0f;
            }
        }

        public void UpdateBossTank()
        {
            if (position.X > 700)
                velocity = new Vector2(-50, 0);
            else if (position.X < 600)
                velocity = new Vector2(50, 0);
        }

        public void SpawnProjectile()
        {
            float speed = 60f;
            Vector2 offset = new Vector2(velocity.X, velocity.Y + 70); // offset pos spawn per tank velocity
            Vector2[] velocities = { new Vector2(-speed, speed), new Vector2(0, speed), new Vector2(speed, speed) };

            for (int i = 0; i < 3; i++)
            {
                var projectileParams = new Dictionary<string, object>
                {
                    { "create", new Projectile(content) },
                    { "entityName", "MobProjectile_" + projectileCounter++ },
                    { "position", position + offset },
                    { "velocity", velocities[i] }
                };

                commandQueue.Enqueue(new CommandRequest("CreateEntity", projectileParams));
            }
        }
        public void SetSpriteSecondary(ISprite sprite)
        {
            this.spriteSecondary = sprite;
        }
        public override void Draw(SpriteBatch spriteBatch){
            SpriteEffects effects = SpriteEffects.None;
            //This is to adjust the cannon's position on the tank
            Vector2 cannonPos = new Vector2(0, 0);
            if(mobType == MobType.BossTank) 
            {
                cannonPos.X = 47;
                cannonPos.Y = 23;
            } else if(mobType == MobType.SmallEnemy) 
            {
                cannonPos.X = 34;
                cannonPos.Y = 32;
            }

            sprite.Draw(spriteBatch, position, effects);
            spriteSecondary.Draw(spriteBatch, position + cannonPos, effects);
        }
    }
}
