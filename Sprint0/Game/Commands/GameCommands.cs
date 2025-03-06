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
                    if(!gameManager.GameStarted) {
                        gameManager.GameStarted = true; // This will automatically switch to Player Inventory screen
                    } else if(gameManager.GamePaused) {
                        gameManager.GamePaused = false;
                    }
                }
            }
        }

        public class ShowPauseMenuCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager)
                {
                    if (gameManager.GameStarted)
                    {
                        gameManager.GamePaused = true;
                    }
                }
            }
        }

        public class IncreaseLevelCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager)
                {
                    if(gameManager.LevelNumber<99) {
                        gameManager.LevelNumber++;
                        gameManager.UpdateLevel();
                    }
                }
            }
        }

        public class DecreaseLevelCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager)
                {
                    if(gameManager.LevelNumber>1) {
                        gameManager.LevelNumber--;
                        gameManager.UpdateLevel();
                    }
                }
            }
        }
    }
}
