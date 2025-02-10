using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Sprint0
{
    public class Projectile : Entity
    {
        
        public Projectile(ContentManager content) 
        {
            //need to rotate
            AddSprite("Idle", new AnimatedSprite(0.3f));
            sprites["Idle"].LoadContent(content, "2DTanksSprites", 0, 750, 177, 53, 1);
  

            SetSprite(sprites["Idle"]);
        }
        public override void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
            position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        public override void Draw(SpriteBatch spriteBatch){
            SpriteEffects effects = SpriteEffects.None;

            // Flip the sprite if it is facing left -- since sprite sheet does not have left sprites
            //Changed to correct the new projectile(Payton)
            if (sprite == sprites["Idle"])
            {
                effects = SpriteEffects.FlipHorizontally;
            }

            sprite.Draw(spriteBatch, position, effects);
        }

    }
}
