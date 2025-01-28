using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Sprint0
{
    public interface IController
    {
        void Update(Game1 game);
    }

    public class KeyboardController : IController
    {
        private Dictionary<Keys, string> keyMappings;

        public KeyboardController()
        {
            keyMappings = new Dictionary<Keys, string>
            {
                {Keys.Escape, "Quit"},
                {Keys.D1, "Static"},
                {Keys.D2, "Animated"},
                {Keys.D3, "Moving"},
                {Keys.D4, "MovingAnimated"}
            };
        }

        public void Update(Game1 game)
        {
            KeyboardState state = Keyboard.GetState();
            foreach (var key in keyMappings.Keys)
            {
                if (state.IsKeyDown(key))
                {
                    game.GameManager.ExecuteCommand(keyMappings[key]);
                }
            }
        }
    }

    public class MouseController : IController
    {
        public void Update(Game1 game)
        {
            MouseState state = Mouse.GetState();

            if (state.LeftButton == ButtonState.Pressed)
            {
                if (state.X < 400 && state.Y < 300) game.GameManager.ExecuteCommand("Static");
                else if (state.X >= 400 && state.Y < 300) game.GameManager.ExecuteCommand("Animated");
                else if (state.X < 400 && state.Y >= 300) game.GameManager.ExecuteCommand("Moving");
                else if (state.X >= 400 && state.Y >= 300) game.GameManager.ExecuteCommand("MovingAnimated");
            }
            else if (state.RightButton == ButtonState.Pressed)
            {
                game.GameManager.ExecuteCommand("Quit");
            }
        }
    }
}
