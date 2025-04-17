using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;


namespace Sprint0
{
     public class RocketProjectile : Projectile
    {   
        public RocketProjectile(ContentManager content, string entityKey, Character owner) : base(content, entityKey, owner)
        {
            AddSprite("Rocket", new Sprite());
            sprites["Rocket"].LoadContent(content, "TDTanksAllSprites", 120, 1040, 20, 20, 1); 

            SetSprite("Rocket");

            baseSpeed = 200f; 
            maxDistance = 250f; 
            colors = new Color[] { Color.Green, Color.DarkGreen, Color.Lime };
            damage = 100;
        }
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
                OnDeath(); 
                
                var destroyParams = new Dictionary<string, object>
                {
                    {"destroyEntity", EntityKey}
                };
                commandQueue.Enqueue(new CommandRequest("DestroyEntity", destroyParams));
            }
            bounds = new Rectangle((int)position.X, (int)position.Y, 20, 20);
        }

        public override void OnDeath()
        {
            var effectParams = new Dictionary<string, object>
            {
                { "spawnPosition", position },
                { "effectType", EntityKeys.EffectType.Explosion }
            };
            commandQueue.Enqueue(new CommandRequest("SpawnEffect", effectParams));
            var destroyParams = new Dictionary<string, object>
            {
                {"destroyEntity", EntityKey}
            };
            commandQueue.Enqueue(new CommandRequest("DestroyEntity", destroyParams));
        }
    }
}
