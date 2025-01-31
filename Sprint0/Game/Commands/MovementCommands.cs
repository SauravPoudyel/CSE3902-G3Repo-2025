using Microsoft.Xna.Framework;
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
                    entity.SetVelocity(0, 0);
                }
            }
        }
        public class MoveUpCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("player") && parameters["player"] is Entity entity)
                {
                    entity.SetVelocity(0, -80);
                }
            }
        }

        public class MoveDownCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("player") && parameters["player"] is Entity entity)
                {
                    entity.SetVelocity(0, 80);
                }
            }
        }

        public class MoveLeftCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("player") && parameters["player"] is Entity entity)
                {
                    entity.SetVelocity(-80, 0);
                }
            }
        }

        public class MoveRightCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("player") && parameters["player"] is Entity entity)
                {
                    entity.SetVelocity(80, 0);
                }
            }
        }
    }
}