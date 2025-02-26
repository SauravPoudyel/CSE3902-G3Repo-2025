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
                if (parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager &&
                    parameters.ContainsKey("screen") && parameters["screen"] is IScreen screen)
                {
                    gameManager.RemoveScreen(screen);
                    gameManager.GameStarted = true; // Mark game as started so future resets don't show the start menu.
                }
            }
        }

        public class ShowStartMenuCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager &&
                    parameters.ContainsKey("content") && parameters["content"] is ContentManager content &&
                    parameters.ContainsKey("game") && parameters["game"] is Game1 game)
                {
                    // Only show the start menu if the game has not already started.
                    if (!gameManager.GameStarted)
                    {
                        StartMenu menu = new StartMenu(content, game.GraphicsDevice, game);
                        gameManager.AddScreen(menu);
                    }
                }
            }
        }
    }
}
