using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Sprint0
{
    public static class ActionCommands
    {

        public class PlayerActionCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("player") && parameters["player"] is Player player &&
                parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager &&
                parameters.ContainsKey("actionType") && parameters["actionType"] is string actionType)
                {
                    // this command tells the player to create a projectile which then eventually calls create entity command
                    // it's all a bit tedious but it's the only way to get the player to create a projectile while storing it's own projectiles 
                    switch (actionType)
                    {
                        case "fire":
                            player.SetProjectileType("Default");
                            player.FireProjectile();
                            break;

                        case "item1":
                            player.SetProjectileType("Sniper");
                            player.FireProjectile();
                            break;

                        case "item2":
                            player.SetProjectileType("Rocket");
                            player.FireProjectile();
                            break;

                        case "item3":
                            player.SetProjectileType("Shotgun");
                            player.FireProjectile();
                            break;
                        case "item4":
                            player.SetProjectileType("Bomb");
                            player.FireProjectile();
                            break;

                        default:
                            System.Console.WriteLine("Error: invalid aactionType.");
                            break;
                    }
                }
            }
        }
    
        public class DamageCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager)
                {
                    // Implement the logic for the entity to take damage
                    if (gameManager.GetEntity("player") is Character player)
                    {
                        player.Damage(100f);
                    }
                }
            }
        }
        
    public class CreateProjectileCommand : ICommand
    {
        public void Execute(Dictionary<string, object> parameters)
        {
            if (parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager &&
                parameters.ContainsKey("projectileType") && parameters["projectileType"] is string projectileType &&
                parameters.ContainsKey("spawnPosition") && parameters["spawnPosition"] is Vector2 spawnPosition &&
                parameters.ContainsKey("cannonRotation") && parameters["cannonRotation"] is float cannonRotation)
            {
                int numberOfProjectiles = 1;
                float spreadAngle = 0f;
                float speedModifer = 0f;
                if (parameters.ContainsKey("numberOfProjectiles") && parameters["numberOfProjectiles"] is int num)
                    numberOfProjectiles = num;
                if (parameters.ContainsKey("spreadAngle") && parameters["spreadAngle"] is float angle)
                    spreadAngle = angle;
                if (parameters.ContainsKey("speedModifier") && parameters["speedModifier"] is float speedMod)
                    speedModifer = speedMod;
                ProjectileFactory.CalculateProjectiles(projectileType, spawnPosition, cannonRotation, spreadAngle, numberOfProjectiles, speedModifer);
                ProjectileFactory.SpawnProjectiles(gameManager);
            }
        }
    }

        public class CreateEntityCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager &&
                    parameters.ContainsKey("create") && parameters["create"] is Projectile entity &&
                    parameters.ContainsKey("entityName") && parameters["entityName"] is string entityName &&
                    parameters.ContainsKey("position") && parameters["position"] is Vector2 position &&
                    parameters.ContainsKey("velocity") && parameters["velocity"] is Vector2 velocity)
                {
                    gameManager.GetEntities().Add(entityName, entity);
                    gameManager.GetEntities()[entityName].SetPosition(position);
                    gameManager.GetEntities()[entityName].SetVelocity(velocity);
                }
            }
        }

        public class DestroyEntityCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager &&
                    parameters.ContainsKey("destroyEntity") && parameters["destroyEntity"] is string entityName)
                {
                    gameManager.GetEntities().Remove(entityName);
                }
            }
        }

        public class RequestPlayerPositionCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager &&
                    parameters.ContainsKey("mob") && parameters["mob"] is Mob mob)
                {
                    if (gameManager.GetEntity("player") is Player player)
                    {
                        Vector2 playerPos = player.GetPosition();
                        Rectangle playerBounds = player.GetBounds();

                        List<Entity> blocks = new List<Entity>();
                        foreach (var entity in gameManager.GetEntities().Values)
                        {
                            if (entity is Blocks)
                                blocks.Add(entity);
                        }

                        // Use the new RayTracer function to determine full exposure
                        bool fullyVisible = RayTracer.IsPlayerFullyExposed(mob.GetPosition(), playerBounds, blocks);

                        if (fullyVisible)
                        {
                            mob.UpdateKnownPlayerPosition(playerPos);
                        }
                    }
                }
            }
        }
    }
}
