using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Numerics;

namespace Sprint0
{
public class MouseController : IController
    {
        public void Update(Game1 game)
        {
            MouseState state = Mouse.GetState();

            if (state.LeftButton == ButtonState.Pressed)
            {
                var parameters = new Dictionary<string, object>
                {
                    { "gameManager", game.GameManager },
                    { "content", game.GameManager.GetContent() },
                    { "player", game.GameManager.GetEntity(0) }
                };

                if (state.X < 400 && state.Y < 300) game.GameManager.ExecuteCommand("Static", parameters);
                else if (state.X >= 400 && state.Y < 300) game.GameManager.ExecuteCommand("Animated", parameters);
                else if (state.X < 400 && state.Y >= 300) game.GameManager.ExecuteCommand("Moving", parameters);
                else if (state.X >= 400 && state.Y >= 300) game.GameManager.ExecuteCommand("MovingAnimated", parameters);
            }
            else if (state.RightButton == ButtonState.Pressed)
            {
                var parameters = new Dictionary<string, object>
                {
                    { "gameManager", game.GameManager },
                    { "content", game.GameManager.GetContent() },
                    { "player", game.GameManager.GetEntity(0) }
                };

                game.GameManager.ExecuteCommand("Quit", parameters);
            }
        }
    }
}