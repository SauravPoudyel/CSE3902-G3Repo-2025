using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Threading;

namespace Sprint0
{
    public class Mob : Entity
    {
        private static int projectileCounter = 0;
        private ContentManager content;
        private float timer;

        public Mob(ContentManager content) 
        {
            this.content = content;
            this.velocity = new Vector2(50, 0);

            AddSprite("Idle", new AnimatedSprite(0.3f));
            sprites["Idle"].LoadContent(content, "2DTanksSprites", 5, 2, 188, 141, 1);

            AddSprite("Up", new AnimatedSprite(0.3f));
            sprites["Up"].LoadContent(content, "2DTanksSprites", 280, 2, 62, 64, 2);

            AddSprite("Down", new AnimatedSprite(0.3f));
            sprites["Down"].LoadContent(content, "2DTanksSprites", 5, 2, 66, 64, 2);

            AddSprite("Left", new AnimatedSprite(0.3f));
            sprites["Left"].LoadContent(content, "2DTanksSprites", 140, 2, 62, 66, 2); 

            AddSprite("Right", new AnimatedSprite(0.3f));
            sprites["Right"].LoadContent(content, "2DTanksSprites", 140, 2, 62, 66, 2);    

            SetSprite(sprites["Idle"]);
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
        public override void Draw(SpriteBatch spriteBatch){
            SpriteEffects effects = SpriteEffects.None;

            // Flip the sprite if it is facing left -- since sprite sheet does not have left sprites
            if (sprite == sprites["Left"])
            {
                effects = SpriteEffects.FlipHorizontally;
            }

            sprite.Draw(spriteBatch, position, effects);
        }
    }
}
