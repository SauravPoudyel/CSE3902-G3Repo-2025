using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Sprint0
{
    public class CollisionCommands
    {
        public class CollisionStopCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("actor") && parameters["actor"] is Player player &&
                    parameters.ContainsKey("target") && parameters["target"] is Blocks block)
                {

                    player.SetPosition(player.GetPreviousPosition());
                    player.SetVelocity(Vector2.Zero);
                }

                if (parameters.ContainsKey("actor") && parameters["actor"] is Player player2 &&
                    parameters.ContainsKey("target") && parameters["target"] is Mob mob)
                {

                    player2.SetPosition(player2.GetPreviousPosition());
                    player2.SetVelocity(Vector2.Zero);
                }
            }
        }

        public class CollisionPushCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("actor") && parameters["actor"] is Player player &&
                    parameters.ContainsKey("target") && parameters["target"] is Blocks block)
                {

                    player.SetPosition(player.GetPreviousPosition());
                    player.SetVelocity(Vector2.Zero);
                }

                if (parameters.ContainsKey("actor") && parameters["actor"] is Player player2 &&
                    parameters.ContainsKey("target") && parameters["target"] is Mob mob)
                {

                    player2.SetPosition(player2.GetPreviousPosition());
                    player2.SetVelocity(Vector2.Zero);
                }
            }
        }
    
    }
}
