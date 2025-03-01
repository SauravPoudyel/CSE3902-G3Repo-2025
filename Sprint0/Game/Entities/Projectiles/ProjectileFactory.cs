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
            public Projectile ProjectileVar { get; private set; }
            public string EntityName { get; private set; }
            public Vector2 SpawnPosition { get; private set; }
            public Vector2 Velocity { get; private set; }

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

        public static void CalculateProjectiles(Character owner, string projectileType, Vector2 spawnPosition, float cannonRotation, float spreadAngle, int numberOfProjectiles, float speedModifer)
        {
            projectileDataList.Clear();
            float startAngle = cannonRotation - ((numberOfProjectiles - 1) * spreadAngle / 2f);
            int i;
            for (i = 0; i < numberOfProjectiles; i++)
            {
                string entityName = projectileType + "_" + projectileCounter.ToString();
                Projectile projectile = InstantiateProjectile(projectileType, entityName, owner);
                float currentAngle = startAngle + i * spreadAngle;
                Vector2 direction = Vector2.Transform(new Vector2(0, 1), Matrix.CreateRotationZ(currentAngle));
                float speed = projectile.GetBaseSpeed();
                Vector2 velocity = direction * (speed - speedModifer);
                projectileDataList.Add(new ProjectileData(projectile, entityName, spawnPosition, velocity));
                projectileCounter++; 
            }
        }

        public static void SpawnProjectiles(GameManager gameManager, Character owner)
        {
            int i;
            for (i = 0; i < projectileDataList.Count; i++)
            {
                ProjectileData data = projectileDataList[i];
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("gameManager", gameManager);
                parameters.Add("create", data.ProjectileVar);
                parameters.Add("entityName", data.EntityName);
                parameters.Add("position", data.SpawnPosition);
                parameters.Add("velocity", data.Velocity);
                parameters.Add("owner", owner);
                gameManager.eventManager.ExecuteCommand("CreateEntity", parameters);
            }
            projectileDataList.Clear();
        }

        private static Projectile InstantiateProjectile(string projectileType, string entityName, Character owner)
        {
            if (projectileType == "Sniper")
            {
                return new SniperProjectile(content, entityName, owner);
            }
            else if (projectileType == "Rocket")
            {
                return new RocketProjectile(content, entityName, owner);
            }
            else if (projectileType == "Bomb")
            {
                return new BombProjectile(content, entityName, owner);
            }
            else if (projectileType == "Teleporter")
            {
                return new TeleportProjectile(content, entityName, owner);
            }
            else
            {
                return new Projectile(content, entityName, owner);
            }
        }
    }
}
