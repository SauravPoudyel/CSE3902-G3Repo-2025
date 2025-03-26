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
                {
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
                    if (!gameManager.GameStarted)
                    {
                        gameManager.GameStarted = true;
                        AudioManager.StopMusic();
                        System.Console.WriteLine("[StartGameCommand] GameStarted set to true");
                    }
                    else if (gameManager.GamePaused)
                    {
                        gameManager.GamePaused = false;
                        gameManager.screenManager.ClearBlockingScreen();
                        System.Console.WriteLine("[StartGameCommand] Resuming game, clearing blocking screen");
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
                        gameManager.screenManager.shopOpen = false;
                        gameManager.GamePaused = true;
                    }
                }
            }
        }

        public class ToggleShopCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager)
                {
                    gameManager.screenManager.shopOpen = !gameManager.screenManager.shopOpen;
                }
            }
        }

        public class IncreaseLevelCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager)
                {
                    if (gameManager.LevelNumber < 99)
                    {
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
                    if (gameManager.LevelNumber > 1)
                    {
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
                }
            }
        }
    }
}
