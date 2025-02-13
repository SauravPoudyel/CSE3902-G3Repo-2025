using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Sprint0
{
    public class Projectile : Entity
    {
        private float colorChangeTimer;
        private readonly Color[] colors = { Color.Red, Color.Yellow, Color.Purple, Color.Orange };
        private int colorIndex;
        private Vector2 startPosition;
        private bool startPositionSet; 
        private string entityKey;
        protected float maxDistance;
        protected float baseSpeed;

        public Projectile(ContentManager content, string entityKey)
        {
            AddSprite("Default", new StaticSprite());
            sprites["Default"].LoadContent(content, "TDTanksAllSprites", 0, 1028, 34, 32, 1); 
            SetSprite("Default");

            baseSpeed = 250f;
            colorIndex = 0;
            colorChangeTimer = 100f;
            maxDistance = 600f;  
            
            this.entityKey = entityKey;  // Store the entity key when created
        }

        public float GetBaseSpeed()
        {
            return baseSpeed;
        }

        public string GetEntityKey()
        {
            return entityKey;
        }

        public override void SetPosition(Vector2 newPosition)
        {
            base.SetPosition(newPosition); // call parent set positon 

            /* When we initialize a entity, it's position is (0,0), a trash variable, so
            only grab start posiiton when it has a valid position */
            if (!startPositionSet)
            {
                startPosition = newPosition;
                startPositionSet = true;
            }
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
                Dictionary<string, object> destroyParams = new Dictionary<string, object>(){ {"destroyEntity", entityKey} };
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
