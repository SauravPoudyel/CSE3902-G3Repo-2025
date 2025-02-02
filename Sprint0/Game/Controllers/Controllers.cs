using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Sprint0.Interfaces;
using System.Collections.Generic;
using Sprint0.Sprites;
using Sprint0.Commands;

namespace Sprint0.Controllers
{
    public class KeyboardController : IController
    {
        private readonly Dictionary<Keys, ICommand> keyBindings = new();
        private readonly Link player;

        public KeyboardController(Link player)
        {
            this.player = player;
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            // Move command
            keyBindings.Add(Keys.Up, new MoveCommand(player, Direction.Up));
            keyBindings.Add(Keys.W, new MoveCommand(player, Direction.Up));
            keyBindings.Add(Keys.Down, new MoveCommand(player, Direction.Down));
            keyBindings.Add(Keys.S, new MoveCommand(player, Direction.Down));
            keyBindings.Add(Keys.Left, new MoveCommand(player, Direction.Left));
            keyBindings.Add(Keys.A, new MoveCommand(player, Direction.Left));
            keyBindings.Add(Keys.Right, new MoveCommand(player, Direction.Right));
            keyBindings.Add(Keys.D, new MoveCommand(player, Direction.Right));

            // Attack command
            keyBindings.Add(Keys.Z, new AttackCommand(player));
            keyBindings.Add(Keys.N, new AttackCommand(player));

            // Use item command
            for (Keys key = Keys.D1; key <= Keys.D9; key++)
            {
                int itemIndex = key - Keys.D1;
                keyBindings.Add(key, new UseItemCommand(player, itemIndex));
            }

            // Damaged command
            keyBindings.Add(Keys.E, new DamageCommand(player));
        }

        public void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            foreach (var binding in keyBindings)
            {
                if (keyboardState.IsKeyDown(binding.Key))
                {
                    binding.Value.Execute();
                }
            }
        }
    }
}
