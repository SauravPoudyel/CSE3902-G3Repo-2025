using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace Sprint0
{
    public class MineProjectile : Projectile
    {
        private float explosionTimer;
        private const float ExplosionDelay = 2f;
        private const float FrameTime = 0.25f;

        public MineProjectile(ContentManager content, string entityKey, Character owner) : base(content, entityKey, owner)
        {
            AddSprite("Mine", new AnimatedSprite(FrameTime, AnimatedSprite.FrameOrientation.Vertical));
            sprites["Mine"].LoadContent(content, "TDTanksAllSprites", 1014, 936, 48, 48, 2);
            SetSprite("Mine");

            baseSpeed = 0f;
            //Set the maxDistance to a really large number so it will not explode due to distance traveled
            maxDistance = 256f;
            explosionTimer = 0f;
        }

        public override void Update()
        {
            sprite.Update();

            //Detonate the bomb after the time controlled by explosionTimer
            explosionTimer += Globals.FRAMETIME; 
            if (explosionTimer >= ExplosionDelay)
            {
                OnDeath(); 
            }
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
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            SpriteEffects effects = SpriteEffects.None;
            sprite.Draw(spriteBatch, position, effects, 0f, null, Color.White);
        }
    }
}
