using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Sprint0
{
    public class TrackTrail
    {
        private Vector2 position;
        private float rotation;
        private ISprite sprite;
        private float lifeTime;
        private float maxLifeTime;
        private float alpha;
        public bool IsExpired => lifeTime >= maxLifeTime;

        public TrackTrail(Vector2 pos, float rot, ISprite sprite)
        {
            this.position = pos;
            this.rotation = rot;
            this.sprite = sprite;
            lifeTime = 0f;
            maxLifeTime = 5f;
            alpha = 1f;
        }

        public void Update(float deltaTime)
        {
            lifeTime += deltaTime;
            alpha = 1f - (lifeTime / maxLifeTime);
            if (alpha < 0f)
                alpha = 0f;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (alpha > 0f && sprite != null)
                sprite.Draw(spriteBatch, position, SpriteEffects.None, rotation, null, Color.White * alpha);
        }

        public static void UpdateTrackTrails(List<TrackTrail> trails, float deltaTime, Vector2 position, float rotation, ISprite sprite, ref float trackTrailSpawnTimer, float spawnInterval)
        {
            trackTrailSpawnTimer += deltaTime;

            if (trackTrailSpawnTimer >= spawnInterval)
            {
                trackTrailSpawnTimer = 0f;
                if (trails.Count == 0 || trails[^1].position != position) // Prevents excessive stacking at the same position
                {
                    trails.Add(new TrackTrail(position, rotation, sprite));
                }
            }

            for (int i = trails.Count - 1; i >= 0; i--)
            {
                trails[i].Update(deltaTime);
                if (trails[i].IsExpired)
                    trails.RemoveAt(i);
            }
        }

        public static void DrawTrackTrails(List<TrackTrail> trails, SpriteBatch spriteBatch)
        {
            foreach (var trail in trails)
                trail.Draw(spriteBatch);
        }
    }
}
