using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Sprint0
{
    public class CollisionCommands
    {
        public class PlayerBlockCollisionCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("player") && parameters["player"] is Player player &&
                    parameters.ContainsKey("block") && parameters["block"] is Blocks block)
                {
                    Vector2 velocity = player.GetVelocity(); 
                    velocity.Normalize(); 
                    player.SetPosition(player.GetPreviousPosition() - velocity); 
                }
            }
        }
    }
}
