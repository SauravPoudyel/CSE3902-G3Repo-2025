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
                if (parameters.ContainsKey("player") && parameters["player"] is Player player &&
                    parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager &&
                    parameters.ContainsKey("actionType") && parameters["actionType"] is string actionType)
                {
                    ActionCommands_Logic.HandlePlayerAction(player, gameManager, actionType);
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
                        player.ChangeHealth(-20);
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
                parameters.ContainsKey("owner") && parameters["owner"] is Character owner)
            {
                ActionCommands_Logic.HandleCreateProjectileCommand(parameters, owner, projectileType, spawnPosition, cannonRotation, gameManager);
            }
        }
    }

        public class CreateEntityCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager &&
                    parameters.ContainsKey("create") && parameters["create"] is Entity entity &&
                    parameters.ContainsKey("entityName") && parameters["entityName"] is string entityName &&
                    parameters.ContainsKey("position") && parameters["position"] is Vector2 position &&
                    parameters.ContainsKey("velocity") && parameters["velocity"] is Vector2 velocity)
                {
                    ActionCommands_Logic.HandleCreateEntityCommand(parameters, gameManager, entityName, entity, position, velocity);
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
                if (parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager &&
                    parameters.ContainsKey("healOrigin") && parameters["healOrigin"] is Vector2 origin &&
                    parameters.ContainsKey("healRadius") && parameters["healRadius"] is float healRadius &&
                    parameters.ContainsKey("healAmount") && parameters["healAmount"] is int healAmount)
                {
                    ActionCommands_Logic.HandleHealRadiusCommand(gameManager, origin, healRadius, healAmount);
                }
            }
        }

        public class PlayerInteractCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager &&
                    parameters.ContainsKey("player") && parameters["player"] is Player player)
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
                if (parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager &&
                    parameters.ContainsKey("player") && parameters["player"] is Player player)
                {
                    player.IsImmortal = !player.IsImmortal;
                }
            }
        }
    }
}
