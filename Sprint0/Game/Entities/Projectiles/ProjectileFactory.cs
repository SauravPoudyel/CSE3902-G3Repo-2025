using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Sprint0
{
    public static class ProjectileFactory
    {
        private static ContentManager content;
        private static int projectileCounter = 0;
        private static List<ProjectileData> projectileDataList = new List<ProjectileData>();

        public class ProjectileData
        {
            public Projectile ProjectileVar { get; }
            public string EntityName { get; }
            public Vector2 SpawnPosition { get; }
            public Vector2 Velocity { get; }

            public ProjectileData(Projectile projectile, string entityName, Vector2 spawnPosition, Vector2 velocity)
            {
                ProjectileVar = projectile;
                EntityName = entityName;
                SpawnPosition = spawnPosition;
                Velocity = velocity;
            }
        }

        public static void Initialize(ContentManager cm)
        {
            content = cm;
        }

        public static void CalculateProjectiles(string projectileType, Vector2 spawnPosition, float cannonRotation, float spreadAngle = 0f, int numberOfProjectiles = 1, float speedModifer = 0)
        {
            projectileDataList.Clear();
            float startAngle = cannonRotation - ((numberOfProjectiles - 1) * spreadAngle / 2f);
            for (int i = 0; i < numberOfProjectiles; i++)
            {
                string entityName = projectileType + "_" + projectileCounter++;
                Projectile projectile = InstantiateProjectile(projectileType, entityName);
                float currentAngle = startAngle + i * spreadAngle;
 
                // Use (0,1) as the base vector so that when currentAngle is 0, the projectile moves in the same direction as the tip offset (which is (0,30) normalized).
                Vector2 direction = Vector2.Transform(new Vector2(0, 1), Matrix.CreateRotationZ(currentAngle));
                float speed = projectile.GetBaseSpeed();
                Vector2 velocity = direction * (speed - speedModifer);
                projectileDataList.Add(new ProjectileData(projectile, entityName, spawnPosition, velocity));
            }
        }

        public static void SpawnProjectiles(GameManager gameManager)
        {
            foreach (var data in projectileDataList)
            {
                var parameters = new Dictionary<string, object>
                {
                    { "gameManager", gameManager },
                    { "create", data.ProjectileVar },
                    { "entityName", data.EntityName },
                    { "position", data.SpawnPosition },
                    { "velocity", data.Velocity }
                };
                gameManager.eventManager.ExecuteCommand("CreateEntity", parameters);
            }
            projectileDataList.Clear();
        }

        private static Projectile InstantiateProjectile(string projectileType, string entityName)
        {
            if (projectileType == "Sniper")
                return new SniperProjectile(content, entityName);
            else if (projectileType == "Rocket")
                return new RocketProjectile(content, entityName);
            else if (projectileType == "Bomb")
                return new BombProjectile(content, entityName);
            else if (projectileType == "Teleporter")
                return new TeleportProjectile(content, entityName);
            else
                return new Projectile(content, entityName);
        }
    }
}
