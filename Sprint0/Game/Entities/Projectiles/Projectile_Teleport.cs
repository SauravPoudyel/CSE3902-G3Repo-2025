using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Sprint0
{
    public class TeleportProjectile : Projectile
    {
        private const float TeleportDelay = 2.5f; 
        private float teleportTimer;

        public TeleportProjectile(ContentManager content, string entityKey) : base(content, entityKey)
        {
            // This projectile is stationary.
            baseSpeed = 80f;

            // Load the teleporter sprite as provided.
            AddSprite("Teleporter", new AnimatedSprite(0.15f));
            sprites["Teleporter"].LoadContent(content, "PickupItemSpritesheet2", 0, 680, 40, 40, 10);
            SetSprite("Teleporter");

            // Prevent auto-destruction due to distance traveled.
            maxDistance = 256f;
            teleportTimer = 0f;
        }

        public override void Update()
        {
            sprite.Update();
            teleportTimer += Globals.FRAMETIME;
            position += velocity * Globals.FRAMETIME; 

            // When the delay is reached, trigger the teleport.
            if (teleportTimer >= TeleportDelay)
            {
                TriggerTeleport();
            }
        }

        private void TriggerTeleport()
        {
            // Assumes the Player class has a static Instance property.
            Player player = Player.Instance;
            if (player != null)
            {
                Vector2 originalPosition = player.GetPosition();

                var effectParams = new Dictionary<string, object>
                {
                    { "spawnPosition", originalPosition },
                    { "effectType", "teleportOut" }
                };
                commandQueue.Enqueue(new CommandRequest("SpawnEffect", effectParams));

                var effectParams2 = new Dictionary<string, object>
                {
                    { "spawnPosition", position },
                    { "effectType", "teleportIn" }
                };
                commandQueue.Enqueue(new CommandRequest("SpawnEffect", effectParams2));
                player.SetPosition(this.position);
            }

            // Enqueue the command to destroy this projectile.
            var destroyParams = new Dictionary<string, object>
            {
                { "destroyEntity", entityKey }
            };
            commandQueue.Enqueue(new CommandRequest("DestroyEntity", destroyParams));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, position, SpriteEffects.None, 0f, null, Color.White);
        }
    }
}
