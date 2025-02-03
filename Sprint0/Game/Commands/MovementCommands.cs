using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Sprint0
{
    public class MovementCommands
    {
        public class StopMoveCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("player") && parameters["player"] is Entity entity)
                {

                    entity.SetVelocity(new Vector2(0,0));
                }
            }
        }

        public class MoveCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("player") && parameters["player"] is Entity entity && 
                    parameters.ContainsKey("velocity") && parameters["velocity"] is Vector2 velocity) 
                {
                    entity.SetVelocity(velocity);
                    entity.SetSprite(GetDirectionFromVelocity(velocity));
                }
                
            }
            private string GetDirectionFromVelocity(Vector2 velocity)
            {
                if (velocity.Y < 0) return "Up";
                if (velocity.Y > 0) return "Down";
                if (velocity.X < 0) return "Left";
                if (velocity.X > 0) return "Right";
                return "Idle";
            }
        }
    }

}