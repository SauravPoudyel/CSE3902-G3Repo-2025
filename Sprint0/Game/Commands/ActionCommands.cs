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
                if (parameters.TryGetValue("player", out object playerObj) && playerObj is Player player &&
                    parameters.TryGetValue("gameManager", out object gmObj) && gmObj is GameManager gameManager &&
                    parameters.TryGetValue("actionType", out object actionTypeObj) && actionTypeObj is string actionType)
                {
                    ActionCommands_Logic.HandlePlayerAction(player, gameManager, actionType);
                }
            }
        }

        public class DamageCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("gameManager", out object gmObj) && gmObj is GameManager gameManager)
                {
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
                if (parameters.TryGetValue("gameManager", out object gmObj) && gmObj is GameManager gameManager &&
                    parameters.TryGetValue("projectileType", out object typeObj) && typeObj is string projectileType &&
                    parameters.TryGetValue("spawnPosition", out object posObj) && posObj is Vector2 spawnPosition &&
                    parameters.TryGetValue("cannonRotation", out object rotObj) && rotObj is float cannonRotation &&
                    parameters.TryGetValue("owner", out object ownerObj) && ownerObj is Character owner)
                {
                    ActionCommands_Logic.HandleCreateProjectileCommand(parameters, owner, projectileType, spawnPosition, cannonRotation, gameManager);
                }
            }
        }

        public class CreateEntityCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("gameManager", out object gmObj) && gmObj is GameManager gameManager &&
                    parameters.TryGetValue("create", out object entityObj) && entityObj is Entity entity &&
                    parameters.TryGetValue("entityName", out object nameObj) && nameObj is string entityName &&
                    parameters.TryGetValue("position", out object posObj) && posObj is Vector2 position &&
                    parameters.TryGetValue("velocity", out object velObj) && velObj is Vector2 velocity)
                {
                    ActionCommands_Logic.HandleCreateEntityCommand(parameters, gameManager, entityName, entity, position, velocity);
                }
            }
        }

        public class DestroyEntityCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("gameManager", out object gmObj) && gmObj is GameManager gameManager &&
                    parameters.TryGetValue("destroyEntity", out object keyObj) && keyObj is string entityName)
                {
                    gameManager.GetEntities().Remove(entityName);
                }
            }
        }

        public class RequestPlayerPositionCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("gameManager", out object gmObj) && gmObj is GameManager gameManager &&
                    parameters.TryGetValue("mob", out object mobObj) && mobObj is Mob mob)
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
                if (parameters.TryGetValue("gameManager", out object gmObj) && gmObj is GameManager gameManager &&
                    parameters.TryGetValue("healOrigin", out object originObj) && originObj is Vector2 origin &&
                    parameters.TryGetValue("healRadius", out object radiusObj) && radiusObj is float healRadius &&
                    parameters.TryGetValue("healAmount", out object amountObj) && amountObj is int healAmount)
                {
                    ActionCommands_Logic.HandleHealRadiusCommand(gameManager, origin, healRadius, healAmount);
                }
            }
        }

        public class PlayerInteractCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("gameManager", out object gmObj) && gmObj is GameManager gameManager &&
                    parameters.TryGetValue("player", out object playerObj) && playerObj is Player player)
                {
                    foreach (var interactableBlock in gameManager.GetEntities().Values.OfType<InteractableBlock>())
                    {
                        System.Console.WriteLine("Checking interactable block: " + interactableBlock.GetType().Name);
                        interactableBlock.IsInRange(player.GetPosition());
                        if (interactableBlock.CanInteract)
                        {
                            interactableBlock.Interact();
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
                if (parameters.TryGetValue("gameManager", out object gmObj) && gmObj is GameManager gameManager &&
                    parameters.TryGetValue("player", out object playerObj) && playerObj is Player player)
                {
                    player.IsImmortal = !player.IsImmortal;
                }
            }
        }
    }
}
