using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Sprint0
{
    public class Player: Entity
    {
        
        public Player(ContentManager content) 
        {
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
            
            //The sprite location is a guess as of now
            AddSprite("Attack", new AnimatedSprite(0.3f));
            sprites["Attack"].LoadContent(content, "LinkSpritesheet", 5, 8, 62, 64, 2);

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
            if (sprite == sprites["Left"])
            {
                effects = SpriteEffects.FlipHorizontally;
            }

            sprite.Draw(spriteBatch, position, effects);
        }

    }
}
