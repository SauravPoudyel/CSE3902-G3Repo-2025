using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace Sprint0
{
    public class ProjectileFactory : Entity
    {
        private ContentManager content;
        private int projectileCounter = 0;
        private List<ProjectileData> projectileDataList;

        public class ProjectileData
        {
            public Projectile projectileVar { get; }
            public string entityName { get; }
            public Vector2 spawnPosition { get; }
            public Vector2 velocity { get; }

            public ProjectileData(Projectile projectile, string entityName, Vector2 spawnPosition, Vector2 velocity)
            {
                projectileVar = projectile;
                this.entityName = entityName;
                this.spawnPosition = spawnPosition;
                this.velocity = velocity;
            }
        }

        public ProjectileFactory(ContentManager content)
        {
            this.content = content;
            projectileDataList = new List<ProjectileData>();
            hasSprite = false; 
        }

        public void CalculateProjectiles(string projectileType, Vector2 spawnPosition, float cannonRotation, Vector2 shooterVelocity, float spreadAngle = 0f, int numberOfProjectiles = 1)
        {
            projectileDataList.Clear();

            float startAngle = cannonRotation - ((numberOfProjectiles - 1) * spreadAngle / 2f);
            for (int i = 0; i < numberOfProjectiles; i++)
            {
                string entityName = projectileType + "_" + projectileCounter++;
                Projectile projectile = InstantiateProjectile(projectileType, entityName);

                float currentAngle = startAngle + i * spreadAngle;
                Vector2 direction = Vector2.Transform(Vector2.UnitY, Matrix.CreateRotationZ(currentAngle));
                float speed = projectile.GetBaseSpeed(); 
                Vector2 velocity = direction * speed + shooterVelocity;

                projectileDataList.Add(new ProjectileData(projectile, entityName, spawnPosition, velocity));
            }
        }

        public void SpawnProjectiles()
        {
            foreach (var projectile in projectileDataList)
            {
                var parameters = new Dictionary<string, object>
                {
                    { "create", projectile.projectileVar },
                    { "entityName", projectile.entityName },
                    { "position", projectile.spawnPosition },
                    { "velocity", projectile.velocity }
                };

                commandQueue.Enqueue(new CommandRequest("CreateEntity", parameters));
            }
            projectileDataList.Clear();
        }

        private Projectile InstantiateProjectile(string projectileType, string entityName)
        {
            if (projectileType == "Sniper")
                return new SniperProjectile(content, entityName);
            else if (projectileType == "Rocket")
                return new RocketProjectile(content, entityName);
            else
                return new Projectile(content, entityName);
        }
    }
}
