using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;

namespace Sprint0
{
    // commands to handle player actions
    public static partial class ActionCommands
    {
        public class PlayerActionCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("player", out object playerObj) && playerObj is Player player &&
                    parameters.TryGetValue("gameManager", out object gameManagerObj) && gameManagerObj is GameManager gameManager &&
                    parameters.TryGetValue("actionType", out object actionTypeObj) && actionTypeObj is string actionType)
                {
                    ActionCommandsLogic_PlayerAction.HandlePlayerAction(player, gameManager, actionType);
                }
            }
        }

        public class DamageCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("gameManager", out object gameManagerObj) && gameManagerObj is GameManager gameManager)
                {
                    if (gameManager.GetEntity("player") is Character player)
                    {
                        player.ChangeHealth(-20);
                    }
                }
            }
        }

        public class PlayerInteractCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("gameManager", out object gameManagerObj) && gameManagerObj is GameManager gameManager &&
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
                if (parameters.TryGetValue("gameManager", out object gameManagerObj) && gameManagerObj is GameManager gameManager &&
                    parameters.TryGetValue("player", out object playerObj) && playerObj is Player player)
                {
                    player.IsImmortal = !player.IsImmortal;
                }
            }
        }
    }
}
