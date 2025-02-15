using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint0
{
    public class BombProjectile : Projectile
    {
        private float explosionTimer;
        private const float ExplosionDelay = 2f;

        public BombProjectile(ContentManager content, string entityKey) : base(content, entityKey)
        {
            AddSprite("Bomb", new StaticSprite());
            sprites["Bomb"].LoadContent(content, "TDTanksAllSprites", 0, 1090, 40, 32, 1);

            SetSprite("Bomb");

            baseSpeed = 0f;
            //Set the maxDistance to a really large number so it will not explode due to distance traveled
            maxDistance = 256f;
            explosionTimer = 0f;
        }

        public override void Update(GameTime gameTime)
        {
            // special behavior (acceleration, explosion timer) here
            base.Update(gameTime);

            //Detonate the bomb after the time controlled by explosionTimer
            explosionTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (explosionTimer >= ExplosionDelay)
            {
                Dictionary<string, object> destroyParams = new Dictionary<string, object>()
                {
                    { "destroyEntity", GetEntityKey() }
                };
                commandQueue.Enqueue(new CommandRequest("DestroyEntity", destroyParams));
            }
        }
    }
}
