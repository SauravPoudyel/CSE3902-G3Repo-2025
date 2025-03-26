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
                if (parameters.ContainsKey("game") && parameters["game"] is Game1 game) {
                    Globals.SavePlayerData(); 
                    game.Exit();
                }
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
                        gameManager.shopOpen = false;
                        gameManager.GamePaused = true;
                    }
                }
            }
        }

        public class  ToggleShopCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager)
                {
                    gameManager.shopOpen = !gameManager.shopOpen;
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

        public class resetLevelCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager)
                {
                    gameManager.UpdateLevel();
                    if (!gameManager.GameStarted)
                    {
                        gameManager.GameStarted = true; // This will automatically switch to Player Inventory screen
                    }
                    else if (gameManager.GamePaused)
                    {
                        gameManager.GamePaused = false;
                    }
                }
            }
        }
    }
}
