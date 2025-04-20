using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
                    if (gameManager.screenManager.shopOpen)
                    {
                        gameManager.screenManager.shopOpen = false;
                    } 
                    else if (gameManager.screenManager.statsOpen)
                    {
                        gameManager.screenManager.statsOpen = false;
                    }
                    else if (gameManager.GameStarted)
                    {
                        gameManager.GamePaused = true;
                    }
                }
            }
        }

        public class OpenShopCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager)
                {
                    gameManager.screenManager.shopOpen = true;
                }
            }
        }

        public class CloseShopCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager)
                {
                    gameManager.screenManager.shopOpen = false;
                }
            }
        }

        public class OpenStatsCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager)
                {
                    gameManager.screenManager.statsOpen = true;
                }
            }
        }

        public class CloseStatsCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager)
                {
                    gameManager.screenManager.statsOpen = false;
                }
            }
        }

        public class OpenAchievementsCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager)
                {
                    gameManager.screenManager.achievementsOpen = true;
                }
            }
        }

        public class CloseAchievementsCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager)
                {
                    gameManager.screenManager.achievementsOpen = false;
                }
            }
        }

        
    }
}
