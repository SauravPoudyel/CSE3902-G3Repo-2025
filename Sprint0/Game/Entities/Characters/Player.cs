using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Sprint0
{
    public class Player : Character
    {
        public Player(ContentManager content) : base(content)
        {
            this.bounds = new Rectangle((int)position.X, (int)position.Y, 66, 66);

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
            SetSprite("Idle");

            ISprite cannonSprite = new AnimatedSprite(0.3f);
            cannonSprite.LoadContent(content, "TDTanksAllSprites", 832, 186, 28, 64, 1);
            cannon = new Cannon(cannonSprite, this, new Vector2(14, 10), 30f,
                                0f, MathHelper.ToRadians(20), MathHelper.PiOver2, MathHelper.Pi + MathHelper.PiOver2);

            currentProjectileVariables["projectileType"] = "Default";
        }

        public override void Update()
        {
            this.bounds = new Rectangle((int)position.X, (int)position.Y, 64, 64);
            if (velocity.LengthSquared() > 10f)
                sprite.Update();
            prevPosition = position; 
            position += velocity * Globals.FRAMETIME;
        }

        public Vector2 GetPreviousPosition() {
            return prevPosition; 
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            SpriteEffects effects = SpriteEffects.None;
            if (sprite == sprites["Left"])
                effects = SpriteEffects.FlipHorizontally;
            sprite.Draw(spriteBatch, position, effects, 0f);
            cannon.Draw(spriteBatch);
        }

        public override void OnCollide(Entity entityActedUpon)
        {
            var parameters = new Dictionary<string, object>{{ "player", this }};

            if(entityActedUpon is Blocks block){
                parameters.Add("block", entityActedUpon); 
                commandQueue.Enqueue(new CommandRequest("PlayerBlockCollision", parameters));
            }
        }
    }
}
