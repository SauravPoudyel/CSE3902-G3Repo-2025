using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Data;

namespace Sprint0
{
    public class Projectile : Entity
    {
        protected float colorChangeTimer;
        protected int colorIndex;
        protected Vector2 startPosition;
        private bool startPositionSet; 
        public string entityKey;
        protected Color[] colors;
        protected float maxDistance;
        protected float baseSpeed;
        public int damage = 20; 
        public IEntity Owner { get; private set; }
        public bool canReflect;

        public Projectile(ContentManager content, string entityKey, Character owner)
        {
            AddSprite("Default", new StaticSprite());
            sprites["Default"].LoadContent(content, "TDTanksAllSprites", 120, 1040, 20, 20, 1); 
            SetSprite("Default");

            baseSpeed = 250f;
            colorIndex = 0;
            colorChangeTimer = 200f;
            maxDistance = 600f;  
            colors = new Color[] { Color.Red, Color.Orange };
            Owner = owner;
            
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

        public virtual void OnDeath(){}

        public override void Update()
        {
            sprite.Update();
            position += velocity * Globals.FRAMETIME; 

            colorChangeTimer += Globals.FRAMETIME; 
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
            bounds = new Rectangle((int)position.X, (int)position.Y, 20, 20);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            SpriteEffects effects = SpriteEffects.None;
            Color flashColor = colors[colorIndex];
            sprite.Draw(spriteBatch, position, effects, 0f, null, flashColor);
        }
    }
}
