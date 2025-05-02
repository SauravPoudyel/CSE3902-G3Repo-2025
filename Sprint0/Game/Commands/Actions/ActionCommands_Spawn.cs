using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;

namespace Sprint0
{
    // commands to handle spawning entities and projectiles actions
    public static partial class ActionCommands
    {
        public class CreateProjectileCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("gameManager", out object gameManagerObj) && gameManagerObj is GameManager gameManager &&
                    parameters.TryGetValue("projectileType", out object typeObj) && typeObj is string projectileType &&
                    parameters.TryGetValue("spawnPosition", out object posObj) && posObj is Vector2 spawnPosition &&
                    parameters.TryGetValue("cannonRotation", out object rotObj) && rotObj is float cannonRotation &&
                    parameters.TryGetValue("owner", out object ownerObj) && ownerObj is Character owner)
                {
                    ActionCommandsLogic_Spawn.HandleCreateProjectile(
                        parameters, owner, projectileType, spawnPosition, cannonRotation, gameManager
                    );
                }
            }
        }

        public class CreateEntityCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("gameManager", out object gameManagerObj) && gameManagerObj is GameManager gameManager &&
                    parameters.TryGetValue("create", out object entityObj) && entityObj is Entity entity &&
                    parameters.TryGetValue("entityName", out object nameObj) && nameObj is string entityName &&
                    parameters.TryGetValue("position", out object posObj) && posObj is Vector2 position &&
                    parameters.TryGetValue("velocity", out object velObj) && velObj is Vector2 velocity)
                {
                    ActionCommandsLogic_Spawn.HandleCreateEntity(
                        parameters, gameManager, entityName, entity, position, velocity
                    );
                }
            }
        }

        public class DestroyEntityCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("gameManager", out object gameManagerObj) && gameManagerObj is GameManager gameManager &&
                    parameters.TryGetValue("destroyEntity", out object keyObj) && keyObj is string entityName)
                {
                    gameManager.GetEntities().Remove(entityName);
                }
            }
        }

        public class AutoDestroyCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters["targetKey"] is string key &&
                    parameters["gameManager"] is GameManager gameManager)
                {
                    gameManager.RemoveEntity(key);
                }
            }
        }
    }
}
