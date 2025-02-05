using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace Sprint0
{
    public static class ActionCommands
    {
        public class AttackCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("entity") && parameters["entity"] is IEntity entity && 
                parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager)
                {
                    // Implement the logic for the entity to attack
                    gameManager.GetEntity("player").SetSprite("attack");
                }
            }
        }
    
        public class UseItemCommand : ICommand
        {
            private int itemNumber;

            public UseItemCommand(int itemNumber)
            {
                this.itemNumber = itemNumber;
            }

            public void Execute(Dictionary<string, object> parameters)
            {
                // Implement the logic for the entity to use item
            }
        }
    
        public class DamageCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("entity") && parameters["entity"] is IEntity entity)
                {
                    // Implement the logic for the entity to take damage
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
                    parameters.ContainsKey("position") && parameters["position"] is Microsoft.Xna.Framework.Vector2 position &&
                    parameters.ContainsKey("velocity") && parameters["velocity"] is Microsoft.Xna.Framework.Vector2 velocity)
                {
                    gameManager.GetEntities().Add(entityName, entity);
                    gameManager.GetEntities()[entityName].SetPosition(position);
                    gameManager.GetEntities()[entityName].SetVelocity(velocity);
                }
            }
        }

    }
}
