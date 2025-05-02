using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Sprint0
{
    public static class ActionCommandsLogic_Spawn
    {
        public static void HandleCreateEntity(
            Dictionary<string, object> parameters,
            GameManager gameManager,
            string entityName,
            Entity entity,
            Vector2 position,
            Vector2 velocity)
        {
            gameManager.GetEntities().Add(entityName, entity);
            Entity newEntity = gameManager.GetEntities()[entityName];
            newEntity.SetPosition(position);
            newEntity.SetVelocity(velocity);

            if (parameters.TryGetValue("owner", out object ownerObj) && ownerObj is Character owner)
            {
                newEntity.Owner = owner;
            }
        }

        public static void HandleCreateProjectile(
            Dictionary<string, object> parameters,
            Character owner,
            string projectileType,
            Vector2 spawnPosition,
            float cannonRotation,
            GameManager gameManager)
        {
            int numberOfProjectiles = 1;
            if (parameters.TryGetValue("numberOfProjectiles", out object numberObj) && numberObj is int num)
            {
                numberOfProjectiles = num;
            }

            float spreadAngle = 0f;
            if (parameters.TryGetValue("spreadAngle", out object spreadObj) && spreadObj is float angle)
            {
                spreadAngle = angle;
            }

            float speedModifier = 0f;
            if (parameters.TryGetValue("speedModifier", out object speedObj) && speedObj is float speed)
            {
                speedModifier = speed;
            }

            ProjectileFactory.CalculateProjectiles(
                owner,
                projectileType,
                spawnPosition,
                cannonRotation,
                spreadAngle,
                numberOfProjectiles,
                speedModifier
            );
            ProjectileFactory.SpawnProjectiles(gameManager, owner);
        }
    }
}
