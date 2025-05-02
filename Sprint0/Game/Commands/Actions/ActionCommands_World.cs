using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;

namespace Sprint0
{
    // commands to handle world related actions
    public static partial class ActionCommands
    {
        public class RequestPlayerPositionCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("gameManager", out object gameManagerObj) && gameManagerObj is GameManager gameManager &&
                    parameters.TryGetValue("mob", out object mobObj) && mobObj is Mob mob)
                {
                    ActionCommandsLogic_World.HandleRequestPlayerPositionCommand(gameManager, mob);
                }
            }
        }

        public class HealRadiusCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("gameManager", out object gameManagerObj) && gameManagerObj is GameManager gameManager &&
                    parameters.TryGetValue("healOrigin", out object originObj) && originObj is Vector2 origin &&
                    parameters.TryGetValue("healRadius", out object radiusObj) && radiusObj is float healRadius &&
                    parameters.TryGetValue("healAmount", out object amountObj) && amountObj is int healAmount)
                {
                    ActionCommandsLogic_World.HandleHealRadiusCommand(gameManager, origin, healRadius, healAmount);
                }
            }
        }
    }
}
