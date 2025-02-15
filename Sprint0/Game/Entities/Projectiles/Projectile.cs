using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Sprint0
{
    public class Projectile : Entity
    {
        private float colorChangeTimer;
        private int colorIndex;
        private Vector2 startPosition;
        private bool startPositionSet; 
        protected string entityKey;
        protected Color[] colors;
        protected float maxDistance;
        protected float baseSpeed;

        public Projectile(ContentManager content, string entityKey)
        {
            AddSprite("Default", new StaticSprite());
            sprites["Default"].LoadContent(content, "TDTanksAllSprites", 0, 1028, 34, 32, 1); 
            SetSprite("Default");

            baseSpeed = 250f;
            colorIndex = 0;
            colorChangeTimer = 200f;
            maxDistance = 600f;  
            colors = new Color[] { Color.Red, Color.Yellow, Color.Purple, Color.Orange };
            
            this.entityKey = entityKey;
        }

        public override void SetPosition(Vector2 newPosition)
        {
            base.SetPosition(newPosition);
            if (!startPositionSet)
            {
                startPosition = newPosition;
                startPositionSet = true;
            }
        }

        public float GetBaseSpeed(){
            return baseSpeed; 
        }

        public override void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
            position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            colorChangeTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (colorChangeTimer > 0.1f)
            {
                colorIndex = (colorIndex + 1) % colors.Length;
                colorChangeTimer = 0f;
            }

            float distanceTraveled = Vector2.Distance(startPosition, position);
            if (distanceTraveled > maxDistance)
            {
                var destroyParams = new Dictionary<string, object>
                {
                    {"destroyEntity", entityKey}
                };
                commandQueue.Enqueue(new CommandRequest("DestroyEntity", destroyParams));
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            SpriteEffects effects = SpriteEffects.None;
            Color flashColor = colors[colorIndex];
            sprite.Draw(spriteBatch, position, effects, 0f, null, flashColor);
        }
    }
}
