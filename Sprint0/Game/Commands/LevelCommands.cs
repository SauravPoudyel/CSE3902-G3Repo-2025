    using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sprint0
{
    public class LevelCommands
    {
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

        public class GenerateProceduralLevelCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("gameManager", out object gmObj) && gmObj is GameManager gameManager)
                {
                    ContentManager content = parameters.TryGetValue("content", out object cmObj) && cmObj is ContentManager
                                                ? (ContentManager)cmObj
                                                : gameManager.GetContent();

                    // Set the pending flag and reset the timer in the ProceduralHandler.
                    gameManager.LevelManager.proceduralHandler.pendingProceduralLevelLoad = true;
                    gameManager.LevelManager.proceduralHandler.proceduralLoadTimer = 0f;
                    
                    // Create and add the loading screen.
                    gameManager.LevelManager.proceduralHandler.proceduralLoadingScreen = new LoadingScreen(gameManager.Game);
                    gameManager.screenManager.AddScreen(gameManager.LevelManager.proceduralHandler.proceduralLoadingScreen, true);

                    // Load the new procedural level synchronously.
                    gameManager.LevelManager.LoadProceduralLevel();
                }
            }
        }

    }
}