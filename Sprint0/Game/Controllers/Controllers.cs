using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Numerics;

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
                {Keys.R, "Reset"},

                {Keys.D1, "UseItem1"},
                {Keys.D2, "UseItem2"},

                {Keys.W, "MoveUp"},
                {Keys.A, "MoveLeft"},
                {Keys.S, "MoveDown"},
                {Keys.D, "MoveRight"},
                {Keys.Up, "MoveUp"},
                {Keys.Left, "MoveLeft"},
                {Keys.Down, "MoveDown"},
                {Keys.Right, "MoveRight"},

                {Keys.Z, "Attack"},
                {Keys.E, "Damage"},

                {Keys.T, "CycleBlockPrev"},
                {Keys.Y, "CycleBlockNext"},
                {Keys.U, "CycleItemPrev"},
                {Keys.I, "CycleItemNext"},
                {Keys.O, "CycleEnemyPrev"},
                {Keys.P, "CycleEnemyNext"}
            };
        }

        public void Update(Game1 game)
        {
            KeyboardState state = Keyboard.GetState();

            foreach (var key in keyMappings.Keys)
            {
                if (state.IsKeyDown(key))
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>
                    {
                        { "gameManager", game.GameManager },
                        { "content", game.GameManager.GetContent() },
                        { "player", game.GameManager.GetEntity(0) } // Player is always entity(0)
                    };
                    game.GameManager.ExecuteCommand(keyMappings[key], parameters);

                    if (!(key == Keys.W || key == Keys.A || key == Keys.S || key == Keys.D ||
                        key == Keys.Up || key == Keys.Left || key == Keys.Down || key == Keys.Right))
                    {
                        System.Console.WriteLine("Stopping player");
                        game.GameManager.ExecuteCommand("StopMoveCommand", parameters);
                    }
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