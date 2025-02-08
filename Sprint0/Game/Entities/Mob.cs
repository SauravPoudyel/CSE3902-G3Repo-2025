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
        private float timer;
        private ISprite spriteSecondary;
        private MobType mobType;

        public Mob(ContentManager content) 
        {
            this.content = content;
            this.velocity = new Vector2(50, 0);
            //Original boss tank params (641, 510, 123, 150)
            
            AddSprite("BossTankBase", new AnimatedSprite(0.3f));
            sprites["BossTankBase"].LoadContent(content, "TDTanksAllSprites", 641, 661, 123, 144, 1);

            AddSprite("SmallEnemyTank", new AnimatedSprite(0.3f));
            sprites["SmallEnemyTank"].LoadContent(content, "TDTanksAllSprites", 768, 256, 95, 113, 1);

            AddSprite("Cannon", new AnimatedSprite(0.3f));
            sprites["Cannon"].LoadContent(content, "TDTanksAllSprites", 832, 186, 28, 64, 1);  

            SetSprite(sprites["BossTankBase"]);
            SetSpriteSecondary(sprites["Cannon"]);
        }
        private void SetItemType(MobType mobType) {
            this.mobType = mobType;
            this.SetSprite(this.mobType.ToString());
        }

        public string GetItemType() {
            return mobType.ToString();
        }
        public void CycleItemNext() {
            if(mobType == MobType.BossTank) {
                SetItemType(MobType.SmallEnemy);
            } else if (mobType == MobType.SmallEnemy) {
                SetItemType(MobType.BossTank);
            }        
        }
        public void CycleItemPrev() {
            if(mobType == MobType.BossTank) {
                SetItemType(MobType.SmallEnemy);
            } else if (mobType == MobType.BossTank) {
                SetItemType(MobType.SmallEnemy);
            }            
        }
        public override void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);

            position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if (position.X > 700)
            {
                velocity = new Vector2(-50, 0);
            } else if( position.X < 600)
            {
                velocity = new Vector2(50, 0);
            }

            /* Every 4 seconds create 3 projectiles */
            if(timer > 4) {
                for(int i = 1; i <= 3; i++){
                    Dictionary<string, object> parameters = new Dictionary<string, object>
                    {
                        { "create", new Projectile(content) },
                        { "entityName", "MobProjectile_" + projectileCounter++}, // THIS IS A TEMPRORARY FIX SO THAT EACH PROJECTILE HAS A UNIQUE NAME
                        { "position", new Vector2(position.X, position.Y + -100) },
                        { "velocity", new Vector2(-20, 0) }
                    };
                    commandQueue.Enqueue(new CommandRequest("CreateEntity", parameters));
                }
                timer = 0f;
            }
        }
        public void SetSpriteSecondary(ISprite sprite)
        {
            this.spriteSecondary = sprite;
        }
        public override void Draw(SpriteBatch spriteBatch){
            SpriteEffects effects = SpriteEffects.None;
            //This is to adjust the cannon's position on the tank
            Vector2 cannonPos;
            cannonPos.X = 47;
            cannonPos.Y = 23;

            sprite.Draw(spriteBatch, position, effects);
            spriteSecondary.Draw(spriteBatch, position + cannonPos, effects);
        }
    }
}
