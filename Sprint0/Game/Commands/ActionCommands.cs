using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.VisualBasic;
using System.Linq;

namespace Sprint0
{
    public static class ActionCommands
    {

        public class PlayerActionCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("player", out object value) && value is Player player &&
                    parameters.TryGetValue("gameManager", out object value) && value is GameManager gameManager &&
                    parameters.TryGetValue("actionType", out object value) && value is string actionType)
                {
                    ActionCommands_Logic.HandlePlayerAction(player, gameManager, actionType);
                }
            }
        }

        public class DamageCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("gameManager", out object value) && value is GameManager gameManager)
                {
                    // Implement the logic for the entity to take damage
                    if (gameManager.GetEntity("player") is Character player)
                    {
                        player.ChangeHealth(-20);
                    }
                }
            }
        }
        
    public class CreateProjectileCommand : ICommand
    {
        public void Execute(Dictionary<string, object> parameters)
        {
            if (parameters.TryGetValue("gameManager", out object value) && value is GameManager gameManager &&
                parameters.TryGetValue("projectileType", out object value) && value is string projectileType &&
                parameters.TryGetValue("spawnPosition", out object value) && value is Vector2 spawnPosition &&
                parameters.TryGetValue("cannonRotation", out object value) && value is float cannonRotation &&
                parameters.TryGetValue("owner", out object value) && value is Character owner)
            {
                ActionCommands_Logic.HandleCreateProjectileCommand(parameters, owner, projectileType, spawnPosition, cannonRotation, gameManager);
            }
        }
    }

        public class CreateEntityCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("gameManager", out object value) && value is GameManager gameManager &&
                    parameters.TryGetValue("create", out object value) && value is Entity entity &&
                    parameters.TryGetValue("entityName", out object value) && value is string entityName &&
                    parameters.TryGetValue("position", out object value) && value is Vector2 position &&
                    parameters.TryGetValue("velocity", out object value) && value is Vector2 velocity)
                {
                    ActionCommands_Logic.HandleCreateEntityCommand(parameters, gameManager, entityName, entity, position, velocity);
                }
            }
        }

        public class DestroyEntityCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("gameManager", out object value) && value is GameManager gameManager &&
                    parameters.TryGetValue("destroyEntity", out object value) && value is string entityName)
                {
                    gameManager.GetEntities().Remove(entityName);
                }
            }
        }

        public class RequestPlayerPositionCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("gameManager", out object value) && value is GameManager gameManager &&
                    parameters.TryGetValue("mob", out object value) && value is Mob mob)
                {
                    ActionCommands_Logic.HandleRequestPlayerPositionCommand(gameManager, mob);
                }
            }
        }

        public class AutoDestroyCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters["targetKey"] is string key &&
                    parameters["gameManager"] is GameManager gm)
                {
                    gm.RemoveEntity(key);
                }
            }
        }

        public class HealRadiusCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("gameManager", out object value) && value is GameManager gameManager &&
                    parameters.TryGetValue("healOrigin", out object value) && value is Vector2 origin &&
                    parameters.TryGetValue("healRadius", out object value) && value is float healRadius &&
                    parameters.TryGetValue("healAmount", out object value) && value is int healAmount)
                {
                    ActionCommands_Logic.HandleHealRadiusCommand(gameManager, origin, healRadius, healAmount);
                }
            }
        }

        public class PlayerInteractCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("gameManager", out object value) && value is GameManager gameManager &&
                    parameters.TryGetValue("player", out object value) && value is Player player)
                {
                    foreach(var InteractableBlock in gameManager.GetEntities().Values.OfType<InteractableBlock>())
                    {
                        System.Console.WriteLine("Checking interactable block: " + InteractableBlock.GetType().Name);
                        InteractableBlock.IsInRange(player.GetPosition());
                        if (InteractableBlock.CanInteract)
                        {
                            InteractableBlock.Interact();
                            break;
                        }
                    }
                }
            }
        }

        public class TogglePlayerImmortalityCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("gameManager", out object value) && value is GameManager gameManager &&
                    parameters.TryGetValue("player", out object value) && value is Player player)
                {
                    player.IsImmortal = !player.IsImmortal;
                }
            }
        }
    }
}
