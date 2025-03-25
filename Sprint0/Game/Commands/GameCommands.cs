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
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("game") && parameters["game"] is Game1 game)
                    game.Exit();
            }
        }

        public class ResetCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("game") && parameters["game"] is Game1 game)
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
                    } else if(gameManager.GamePaused || gameManager.GameLoading) {
                        gameManager.GamePaused = false;
                        gameManager.GameLoading = false;
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
                        gameManager.GameLoading = true;
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
                        gameManager.GameLoading = true;
                    }
                }
            }
        }

        public class SetLevelCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager 
                && parameters.ContainsKey("level") && parameters["level"] is int levelNum)
                {
                    gameManager.LevelNumber = levelNum;
                    gameManager.UpdateLevel();
                    // gameManager.GameLoading = true;
                }
            }
        }
    }
}
