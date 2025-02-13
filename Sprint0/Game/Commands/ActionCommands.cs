using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Sprint0
{
    public static class ActionCommands
    {
        public class AttackCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("player") && parameters["player"] is Player player &&
                parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager)
                {
                    // this command tells the player to create a projectile which then eventually calls create entity command
                    // it's all a bit tedious but it's the only way to get the player to create a projectile while storing it's own projectiles 
                    player.SetProjectileType("Default");
                    player.CreateProjectile();
                }
            }
        }
    
        public class UseItemCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("player") && parameters["player"] is Player player &&
                parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager &&
                parameters.ContainsKey("itemType") && parameters["itemType"] is int itemType)
                {
                    // this command tells the player to create a projectile which then eventually calls create entity command
                    // it's all a bit tedious but it's the only way to get the player to create a projectile while storing it's own projectiles 
                    if(itemType == 1)
                    {
                        player.SetProjectileType("Sniper");
                        player.CreateProjectile();
                    }
                    else if(itemType == 2)
                    {
                        player.SetProjectileType("Rocket");
                        player.CreateProjectile();
                    }
                    else if(itemType == 3)
                    {
                        player.SetProjectileType("Shotgun");
                        player.CreateProjectile();
                    }
                    else if(itemType == 4)
                    {
                        // logic to use a bomb maybe? it's an item so I don't know how to do that in player
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
