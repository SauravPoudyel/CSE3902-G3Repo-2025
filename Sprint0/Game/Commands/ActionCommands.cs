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
                if (parameters.ContainsKey("entity") && parameters["entity"] is IEntity entity)
                {
                    // Implement the logic for the entity to attack
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

    }
}
