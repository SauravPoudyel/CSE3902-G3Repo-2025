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
                parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager &&
                parameters.ContainsKey("content") && parameters["content"] is ContentManager content)
                {
                    // this command tells the player to create a projectile which then eventually calls create entity command
                    // it's all a bit tedious but it's the only way to get the player to create a projectile while storing it's own projectiles 
                    player.CreateProjectile(content);
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

    }
}
