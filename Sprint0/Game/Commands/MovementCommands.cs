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

        public class ApplyFrictionCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("player") && parameters["player"] is Player player)
                {
                    // Assuming the Entity base class provides a GetVelocity() method.
                    Vector2 currentVelocity = player.GetVelocity();
                    Vector2 newVelocity = currentVelocity * 0.982f;
                    if (newVelocity.LengthSquared() < 0.05f)
                    {
                        newVelocity = Vector2.Zero;
                    }
                    player.SetVelocity(newVelocity);
                }
            }
        }

 
        public class MoveCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("player") && parameters["player"] is Entity entity &&
                    parameters.ContainsKey("velocity") && parameters["velocity"] is Vector2 inputVelocity)
                {
                    float forwardAcceleration = 50f;
                    float turnAcceleration = 12f; 
                    float turnDamping = 0.9f; 

                    Vector2 currentVelocity = entity.GetVelocity();
                    Vector2 newVelocity = currentVelocity;

                    float newVelocityY = currentVelocity.Y + inputVelocity.Y * forwardAcceleration * Globals.FRAMETIME;

                    float newVelocityX = currentVelocity.X + inputVelocity.X * turnAcceleration * Globals.FRAMETIME;

                    newVelocityX *= turnDamping;

                    // Clamp forward speed (Y-axis)
                    float maxSpeed = 150f;
                    newVelocityY = MathHelper.Clamp(newVelocityY, -maxSpeed, maxSpeed);

                    newVelocity = new Vector2(newVelocityX, newVelocityY);
                    entity.SetVelocity(newVelocity);
                }
            }
        }

    }

}