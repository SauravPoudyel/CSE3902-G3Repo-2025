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
                // { Keys.Q, "Quit" },
                // { Keys.R, "Reset" },
                { Keys.D1, "PlayerAction" },
                { Keys.D2, "PlayerAction" },
                { Keys.D3, "PlayerAction" },
                { Keys.D4, "PlayerAction" },
                { Keys.D5, "PlayerAction" },
                { Keys.W, "Move" },
                { Keys.S, "Move" },
                { Keys.C, "PlayerAction" },
                { Keys.E, "PlayerAction" },
                { Keys.H, "Damage" },
                { Keys.K, "Immortality" },
                { Keys.T, "CycleBlockPrev" },
                { Keys.Y, "CycleBlockNext" },
                { Keys.U, "CycleItemPrev" },
                { Keys.I, "CycleItemNext" },
                { Keys.O, "CycleEnemyPrev" },
                { Keys.P, "CycleEnemyNext" },
                { Keys.Escape, "ShowPauseMenu" },
                { Keys.OemPlus, "AudioIncrease" },
                { Keys.OemMinus, "AudioDecrease" },
            };
        }

        public void Update(Game1 game)
        {
            KeyboardState state = Keyboard.GetState();

            // Only use W/S for forward/backward movement.
            Vector2 playerVelocity = Vector2.Zero;
            if (state.IsKeyDown(Keys.W) || state.IsKeyDown(Keys.Up))
                playerVelocity.Y -= 50;
            if (state.IsKeyDown(Keys.S) || state.IsKeyDown(Keys.Down))
                playerVelocity.Y += 50;

            bool playerMoving = (playerVelocity != Vector2.Zero);
            bool playerTryBoosting = state.IsKeyDown(Keys.LeftShift); 
            bool playerBoosting = false; 
            if (game.GameManager.GetEntity("player") is Player player)
            {
                player.UpdateBoostState(playerTryBoosting);
                playerBoosting = player.CanBoost();
                if (state.IsKeyDown(Keys.A) || state.IsKeyDown(Keys.Left))
                {
                    player.rotationInput = -1; // rotate left
                }
                if (state.IsKeyDown(Keys.D) || state.IsKeyDown(Keys.Right))
                {
                    player.rotationInput = 1; // rotate right
                }
            }

            if (!game.GameManager.screenManager.IsInputBlocked())
            {
                if (playerMoving)
                {
                    game.GameManager.eventManager.ExecuteCommand("Move", new Dictionary<string, object>
                    {
                        { "player", game.GameManager.GetEntity("player") },
                        { "velocity", new Vector2(0, playerVelocity.Y) },
                        { "playerBoosting", playerBoosting},
                        { "gameManager", game.GameManager }
                    });
                }
                else
                {
                    game.GameManager.eventManager.ExecuteCommand("ApplyFriction", new Dictionary<string, object>
                    {
                        { "player", game.GameManager.GetEntity("player") },
                        { "playerTryBoosting", playerTryBoosting },
                        { "gameManager", game.GameManager }
                    });
                }
            }

            string actionType = "fire";
            if (state.IsKeyDown(Keys.C)) actionType = "fire";
            if (state.IsKeyDown(Keys.E)) actionType = "interact";
            if (state.IsKeyDown(Keys.D1)) actionType = "item1";
            if (state.IsKeyDown(Keys.D2)) actionType = "item2";
            if (state.IsKeyDown(Keys.D3)) actionType = "item3";
            if (state.IsKeyDown(Keys.D4)) actionType = "item4";
            if (state.IsKeyDown(Keys.D5)) actionType = "item5";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "gameManager", game.GameManager },
                { "content", game.GameManager.GetContent() },
                { "player", game.GameManager.GetEntity("player") },
                { "mob", game.GameManager.GetEntity("mob") },
                { "pickupItem", game.GameManager.GetEntity("pickupItem") },
                { "blocks", game.GameManager.GetEntity("blocks") },
                { "actionType", actionType },
                { "game", game }
            };

            foreach (Keys key in keyMappings.Keys)
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
        private MouseState previousMouseState;
        public MouseController()
        {
            previousMouseState = new MouseState();
            Mouse.SetCursor(MouseCursor.Crosshair); // temporary mouse
        }
        public void Update(Game1 game)
        {
            MouseState state = Mouse.GetState();
            // If a blocking screen is active, let it handle clicks.
            if (!game.GameManager.screenManager.IsInputBlocked())
            {
                if (state.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton != ButtonState.Pressed)
                {
                    Point clickPos = new Point(state.X, state.Y);
                }
                
                Vector2 mousePosition = new Vector2(state.X, state.Y);
                if (game.GameManager.GetEntity("player") is Player player)
                {
                    Vector2 playerCenter = player.GetPosition();
                    Vector2 direction = mousePosition - playerCenter;
                    float rotation = (float)System.Math.Atan2(direction.Y, direction.X) - MathHelper.PiOver2;
                    Dictionary<string, object> parameters = new Dictionary<string, object>
                    {
                        { "player", player },
                        { "rotation", rotation },
                        { "gameManager", game.GameManager }
                    };
                    game.GameManager.eventManager.ExecuteCommand("UpdateCannon", parameters);
                }
            }
            previousMouseState = state;
        }
    }
}