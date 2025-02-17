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
            previousKeyboardState = Keyboard.GetState();
            keyMappings = new Dictionary<Keys, string>
            {
                { Keys.Q, "Quit" },
                { Keys.R, "Reset" },
                { Keys.D1, "PlayerAction" },
                { Keys.D2, "PlayerAction" },
                { Keys.D3, "PlayerAction" },
                { Keys.D4, "PlayerAction" },
                { Keys.W, "Move" },
                { Keys.A, "Move" },
                { Keys.S, "Move" },
                { Keys.D, "Move" },
                { Keys.Z, "PlayerAction" },
                { Keys.E, "Damage" },
                { Keys.T, "CycleBlockPrev" },
                { Keys.Y, "CycleBlockNext" },
                { Keys.U, "CycleItemPrev" },
                { Keys.I, "CycleItemNext" },
                { Keys.O, "CycleEnemyPrev" },
                { Keys.P, "CycleEnemyNext" }
            };
        }

        public void Update(Game1 game)
        {
            KeyboardState state = Keyboard.GetState();
            Vector2 playerVelocity = Vector2.Zero;
            bool playerMoving = false;

            // Process movement keys.
            if (state.IsKeyDown(Keys.W) || state.IsKeyDown(Keys.Up))
                playerVelocity.Y -= 40;
            if (state.IsKeyDown(Keys.S) || state.IsKeyDown(Keys.Down))
                playerVelocity.Y += 40;
            if (state.IsKeyDown(Keys.A) || state.IsKeyDown(Keys.Left))
                playerVelocity.X -= 40;
            if (state.IsKeyDown(Keys.D) || state.IsKeyDown(Keys.Right))
                playerVelocity.X += 40;

            playerMoving = (playerVelocity != Vector2.Zero);

            // Execute Move command if moving; otherwise, apply friction.
            if (playerMoving)
            {
                game.GameManager.eventManager.ExecuteCommand("Move", new Dictionary<string, object>
                {
                    { "player", game.GameManager.GetEntity("player") },
                    { "velocity", playerVelocity }
                });
            }
            else
            {
                game.GameManager.eventManager.ExecuteCommand("ApplyFriction", new Dictionary<string, object>
                {
                    { "player", game.GameManager.GetEntity("player") }
                });
            }

            // Player Action Logic
            string actionType = "";
            if (state.IsKeyDown(Keys.Z)) actionType = "fire";
            if (state.IsKeyDown(Keys.D1)) actionType = "item1";
            if (state.IsKeyDown(Keys.D2)) actionType = "item2";
            if (state.IsKeyDown(Keys.D3)) actionType = "item3";
            if (state.IsKeyDown(Keys.D4)) actionType = "item4";

            // Process other key-based actions.
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "gameManager", game.GameManager },
                { "content", game.GameManager.GetContent() },
                { "player", game.GameManager.GetEntity("player") },
                { "pickupItem", game.GameManager.GetEntity("pickupItem") },
                { "blocks", game.GameManager.GetEntity("blocks") },
                { "mob", game.GameManager.GetEntity("mob") }, 
                { "actionType", actionType }
            };

            foreach (var key in keyMappings.Keys)
            {
                if (state.IsKeyDown(key) && !previousKeyboardState.IsKeyDown(key))
                {
                    game.GameManager.eventManager.ExecuteCommand(keyMappings[key], parameters);
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

            // Update the player's cannon rotation based on the current mouse position.
            Vector2 mousePosition = new Vector2(state.X, state.Y);
            if (game.GameManager.GetEntity("player") is Player player)
            {
                Vector2 playerCenter = player.GetPosition();
                Vector2 direction = mousePosition - playerCenter;
                float rotation = (float)System.Math.Atan2(direction.Y, direction.X) - MathHelper.PiOver2;
                game.GameManager.eventManager.ExecuteCommand("UpdateCannon", new Dictionary<string, object>
                {
                    { "player", player },
                    { "rotation", rotation }
                });
            }
        }
    }
}