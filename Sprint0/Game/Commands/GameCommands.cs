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
                    if (!gameManager.GameStarted)
                    {
                        gameManager.GameStarted = true; // switches to gameplay (player inventory)
                        AudioManager.StopMusic(); 
                    }
                    else if (gameManager.GamePaused)
                    {
                        gameManager.GamePaused = false;
                        // Clear the blocking pause menu so input is unblocked.
                        gameManager.screenManager.ClearBlockingScreen();
                        // Optionally, remove the PauseMenu from the screen list:
                        // gameManager.screenManager.RemoveScreen(gameManager.screenManager.GetBlockingScreen());
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
