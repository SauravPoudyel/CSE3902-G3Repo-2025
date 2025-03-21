using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Sprint0
{
    public class LaserProjectile : Projectile
    {
        // Duration the laser remains visible (in seconds)
        private float lifeTime = 0.1f;
        // Maximum distance of the laser beam
        private float length = 400f;
        // The normalized firing direction (set once)
        private Vector2 direction;
        // The current endpoint (adjusted if a collision is detected)
        private Vector2 currentEndPoint;
        // A static texture used to draw the 1x1 pixel line
        private static Texture2D laserTexture;
        // Flag to ensure damage is applied only once
        private bool hasDamaged = false;
        // Flag to ensure we only draw once per frame.
        private bool drawnThisFrame = false;
        // Store the spawn position explicitly for the laser
        private Vector2 laserStart;

        public LaserProjectile(ContentManager content, string entityKey, Character owner)
            : base(content, entityKey, owner)
        {
            damage = 200;
            // Disable default sprite drawing.
            sprites.Clear();
            sprite = null;
            hasSprite = false;
        }

        // Override SetPosition so that the spawn position is stored reliably.
        public override void SetPosition(Vector2 newPosition)
        {
            base.SetPosition(newPosition);
            laserStart = newPosition;
        }

        // Provide a nonzero base speed so that ProjectileFactory calculates a proper velocity.
        public override float GetBaseSpeed()
        {
            return 250f;
        }

        public override void Update()
        {
            // Reset drawn flag each update.
            drawnThisFrame = false;

            // On first update, compute the firing direction from the provided velocity.
            if (direction == Vector2.Zero)
            {
                direction = (velocity != Vector2.Zero) ? Vector2.Normalize(velocity) : new Vector2(0, -1);
            }

            // Always anchor the laser at the stored spawn position.
            currentEndPoint = laserStart + direction * length;

            // Raycast to adjust the beam endpoint if it hits an entity.
            if (GameManager.Instance != null)
            {
                List<Entity> entities = new List<Entity>(GameManager.Instance.GetEntities().Values);
                Entity hitEntity;
                Vector2 hitPoint;
                if (RayTracer.Raycast(laserStart, currentEndPoint, entities, out hitEntity, out hitPoint))
                {
                    currentEndPoint = hitPoint;
                    // Apply damage only once.
                    if (hitEntity != null && !hasDamaged && hitEntity != Owner)
                    {
                        Dictionary<string, object> parameters = new Dictionary<string, object>();
                        parameters.Add("actor", this);
                        parameters.Add("target", hitEntity);
                        commandQueue.Enqueue(new CommandRequest("CollisionProjectileDestroy", parameters));
                        hasDamaged = true;
                    }
                }
            }

            // Decrement lifetime and self-destruct when time is up.
            lifeTime -= Globals.FRAMETIME;
            if (lifeTime <= 0f)
            {
                OnDeath();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Ensure we draw only once per frame.
            if (drawnThisFrame)
                return;
            drawnThisFrame = true;

            // Draw the laser as a red line from the stored spawn to the computed endpoint.
            DrawLine(spriteBatch, laserStart, currentEndPoint, Color.Red, 2);
        }

        /// <summary>
        /// Draws a line between two points using a stretched 1x1 texture.
        /// </summary>
        private void DrawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color, int thickness)
        {
            float distance = Vector2.Distance(start, end);
            float angle = (float)System.Math.Atan2(end.Y - start.Y, end.X - start.X);

            if (laserTexture == null)
            {
                laserTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                laserTexture.SetData(new[] { Color.White });
            }

            spriteBatch.Draw(laserTexture, start, null, color, angle, Vector2.Zero, new Vector2(distance, thickness), SpriteEffects.None, 0f);
        }
    }
}
