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
                    if (gameManager.GetEntity("player").GetSprite() is AnimatedSprite player)
                    {
                        player.Damage();
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
                    parameters.ContainsKey("cannonRotation") && parameters["cannonRotation"] is float cannonRotation &&
                    parameters.ContainsKey("shooterVelocity") && parameters["shooterVelocity"] is Vector2 shooterVelocity)
                {
                    var projectileFactory = gameManager.GetEntity("projectileFactory") as ProjectileFactory;

                    if (projectileFactory == null)
                    {
                        System.Console.WriteLine("Error: ProjectileFactory not found or invalid type.");
                        return;
                    }
                    int numberOfProjectiles = 1;
                    float spreadAngle = 0f;

                    // Additional parameters if neccesary 
                    if (parameters.ContainsKey("numberOfProjectiles") && parameters["numberOfProjectiles"] is int num)
                        numberOfProjectiles = num;

                    if (parameters.ContainsKey("spreadAngle") && parameters["spreadAngle"] is float angle)
                        spreadAngle = angle;

                    projectileFactory.CalculateProjectiles(projectileType, spawnPosition, cannonRotation, shooterVelocity, spreadAngle, numberOfProjectiles);
                    projectileFactory.SpawnProjectiles();
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

    }
}
