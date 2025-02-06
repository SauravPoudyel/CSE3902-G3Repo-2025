using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Sprint0
{
    public interface IController
    {
        void Update(Game1 game);
    }

    public class KeyboardController : IController
    {
        private Dictionary<Keys, string> keyMappings;
        private KeyboardState previousKeyboardState;

        public KeyboardController()
        {
            previousKeyboardState = new KeyboardState();
            keyMappings = new Dictionary<Keys, string>
            {
                {Keys.Escape, "Quit"},
                {Keys.R, "Reset"},

                {Keys.D1, "UseItem1"},
                {Keys.D2, "UseItem2"},

                {Keys.W, "Move"},
                {Keys.A, "Move"},
                {Keys.S, "Move"},
                {Keys.D, "Move"},
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
            KeyboardState currentKeyboardState = Keyboard.GetState();
            Vector2 playerVelocity = Vector2.Zero;
            bool playerMoving = false;

            if (currentKeyboardState.IsKeyDown(Keys.W) || currentKeyboardState.IsKeyDown(Keys.Up)) playerVelocity.Y -= 40;
            if (currentKeyboardState.IsKeyDown(Keys.S) || currentKeyboardState.IsKeyDown(Keys.Down)) playerVelocity.Y += 40;
            if (currentKeyboardState.IsKeyDown(Keys.A) || currentKeyboardState.IsKeyDown(Keys.Left)) playerVelocity.X -= 40;
            if (currentKeyboardState.IsKeyDown(Keys.D) || currentKeyboardState.IsKeyDown(Keys.Right)) playerVelocity.X += 40;

            playerMoving = (playerVelocity != Vector2.Zero);

            if (playerMoving)
            {
                game.GameManager.ExecuteCommand("Move", new Dictionary<string, object>{{"player", game.GameManager.GetEntity("player") }, {"velocity", playerVelocity}});
            }
            else 
            {
                game.GameManager.ExecuteCommand("StopeMove", new Dictionary<string, object>{{ "player", game.GameManager.GetEntity("player")}});
            }
    
            foreach (var key in keyMappings.Keys)
            {
                if (currentKeyboardState.IsKeyDown(key) && !previousKeyboardState.IsKeyDown(key))
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>
                    {
                        { "gameManager", game.GameManager },
                        { "content", game.GameManager.GetContent() },
                        { "player", game.GameManager.GetEntity("player") }, // Player is always entity(0)
                        { "playerVelocity", playerVelocity },
                        { "pickupItem", game.GameManager.GetEntity("pickupItem") },
                        { "blocks", game.GameManager.GetEntity("blocks") }
                    };
                    game.GameManager.ExecuteCommand(keyMappings[key], parameters);
                }
            }
            previousKeyboardState = currentKeyboardState;
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
                    { "player", game.GameManager.GetEntity("player") }
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
                    { "player", game.GameManager.GetEntity("player") }
                };

                game.GameManager.ExecuteCommand("Quit", parameters);
                game.GameManager.ExecuteCommand("Static", parameters);
            }
        }
    }
}