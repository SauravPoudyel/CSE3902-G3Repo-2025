using Microsoft.Xna.Framework;
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
    }
}
