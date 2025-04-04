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

        public class ToggleStatsCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager)
                {
                    gameManager.screenManager.statsOpen = !gameManager.screenManager.statsOpen;
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

                    if (gameManager.LevelNumber == 2 && Globals.PlayerData.GetInt("BaseDialogueCount") == 1)
                        gameManager.screenManager.dialogueHandler.AddDialogueByKey("Base1",  gameManager.screenManager);

                }
            }
        }

        public class PlayerDeathCommand : ICommand
        {
            public async void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager &&
                    parameters.ContainsKey("player") && parameters["player"] is Player player)
                {
                    // Fade the screen to black over 2 seconds.
                    ScreenFader.FadeToBlack(2f);

                    await Task.Delay(2000); // Wait 2 seconds to allow fade to complete.
                    Thread.Sleep(600); //wait an extra 0.6 seconds
                    gameManager.DayNightCycle.AdvanceDayNightCycle(0.5f);
                    AudioManager.PlaySound(AudioManager.SoundKey.FixDeath);

                    gameManager.LevelNumber = 2;
                    gameManager.UpdateLevel();
                    
                    // Update the player
                    gameManager.GetEntity("player").SetPosition(new Vector2(700, 700)); // to respawn at the proper point
                    gameManager.GetEntity("player").SetVelocity(Vector2.Zero); // Also stop any movement.

                    // Update the player Data
                    int maxHealth = Globals.PlayerData.GetInt("MaxHealth");
                    Globals.PlayerData.UpdateVariable("Health", maxHealth);
    

                    await Task.Delay(1500);
                    
                    ScreenFader.FadeToNormal(1.5f);

                    await Task.Delay(1500); // Wait 1.5 seconds to allow fade to complete.
                    gameManager.screenManager.dialogueHandler.AddDialogueByKey("Death", gameManager.screenManager);


                }
            }
        }
    }
}
