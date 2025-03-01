using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace Sprint0
{
    public interface ICommand
    {
        void Execute(Dictionary<string, object> parameters);
    }

    public static class GameCommands
    {
        public class QuitCommand : ICommand
        {
            private Game1 game;

            public QuitCommand(Game1 game)
            {
                this.game = game;
            }

            public void Execute(Dictionary<string, object> parameters)
            {
                game.Exit();
            }
        }

        public class ResetCommand : ICommand
        {
            private Game1 game;

            public ResetCommand(Game1 game)
            {
                this.game = game;
            }

            public void Execute(Dictionary<string, object> parameters)
            {
                game.ResetGame();
            }
        }

        public class StartGameCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager)
                {
                    gameManager.GameStarted = true; // This will automatically switch to Player Inventory screen
                }
            }
        }

        public class ShowStartMenuCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager)
                {
                    if (!gameManager.GameStarted)
                    {
                        gameManager.GameStarted = false; // Ensures Start Menu is the only screen if game hasn't started
                    }
                }
            }
        }
    }
}
