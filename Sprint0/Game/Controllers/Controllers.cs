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
                {Keys.Q, "Quit"},
                {Keys.R, "Reset"},

                {Keys.D1, "UseItem"},
                {Keys.D2, "UseItem"},
                {Keys.D3, "UseItem"},
                {Keys.D4, "UseItem"},

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

            KeyboardState state = Keyboard.GetState();

            // Movement logic
            Vector2 playerVelocity = Vector2.Zero;
            bool playerMoving = false;

            if (state.IsKeyDown(Keys.W) || state.IsKeyDown(Keys.Up)) playerVelocity.Y -= 40;
            if (state.IsKeyDown(Keys.S) || state.IsKeyDown(Keys.Down)) playerVelocity.Y += 40;
            if (state.IsKeyDown(Keys.A) || state.IsKeyDown(Keys.Left)) playerVelocity.X -= 40;
            if (state.IsKeyDown(Keys.D) || state.IsKeyDown(Keys.Right)) playerVelocity.X += 40;

            playerMoving = (playerVelocity != Vector2.Zero);

            if (playerMoving)
                game.GameManager.ExecuteCommand("Move", new Dictionary<string, object>{{"player", game.GameManager.GetEntity("player") }, {"velocity", playerVelocity}});

            // Everything Else Logic
            int itemType = 0;
            if (state.IsKeyDown(Keys.D1)) itemType = 1;
            if (state.IsKeyDown(Keys.D2)) itemType = 2;
            if (state.IsKeyDown(Keys.D3)) itemType = 3;
            if (state.IsKeyDown(Keys.D4)) itemType = 4;
    
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "gameManager", game.GameManager },
                { "content", game.GameManager.GetContent() },
                { "player", game.GameManager.GetEntity("player") }, 
                { "playerVelocity", playerVelocity },
                { "pickupItem", game.GameManager.GetEntity("pickupItem") },
                { "blocks", game.GameManager.GetEntity("blocks") },
                { "mob", game.GameManager.GetEntity("mob") },
                { "itemType", itemType}
            };

            foreach (var key in keyMappings.Keys)
            {
                if (state.IsKeyDown(key) && !previousKeyboardState.IsKeyDown(key))
                {
    
                    game.GameManager.ExecuteCommand(keyMappings[key], parameters);
                
                }
            }
            previousKeyboardState = state;
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