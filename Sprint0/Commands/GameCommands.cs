using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Sprint0
{
    public interface ICommand
    {
        void Execute(GameManager gameManager);
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

            public void Execute(GameManager gameManager)
            {
                game.Exit();
            }
        }

        public class DisplayStaticGameCommand : ICommand
        {
            public void Execute(GameManager gameManager)
            {
                var staticSprite = new StaticSprite(new Vector2(Globals.SCREENWIDTH / 2 - 20, Globals.SCREENHEIGHT / 2 - 20));
                staticSprite.LoadContent(gameManager.GetContent(), "LinkSpritesheet", 140, 2, 62, 62, 1);
                gameManager.SetSprite(staticSprite);
            }
        }

        public class DisplayAnimatedGameCommand : ICommand
        {
            public void Execute(GameManager gameManager)
            {
                var animatedSprite = new AnimatedSprite(new Vector2(Globals.SCREENWIDTH / 2 - 20, Globals.SCREENHEIGHT / 2 - 20), 0.4f);
                animatedSprite.LoadContent(gameManager.GetContent(), "LinkSpritesheet", 140, 2, 62, 62, 2);
                gameManager.SetSprite(animatedSprite);
            }
        }

        public class DisplayMovingGameCommand : ICommand
        {
            public void Execute(GameManager gameManager)
            {
                var movingSprite = new MovingSprite(new Vector2(Globals.SCREENWIDTH / 2 - 20, Globals.SCREENHEIGHT / 2 - 20), 1);
                movingSprite.LoadContent(gameManager.GetContent(), "LinkSpritesheet", 140, 2, 62, 62, 1);
                gameManager.SetSprite(movingSprite);
            }
        }

        public class DisplayMovingAnimatedGameCommand : ICommand
        {
            public void Execute(GameManager gameManager)
            {
                var movingAnimatedSprite = new MovingAnimatedSprite(new Vector2(Globals.SCREENWIDTH / 2 - 20, Globals.SCREENHEIGHT / 2 - 20), 1, 0.4f);
                movingAnimatedSprite.LoadContent(gameManager.GetContent(), "LinkSpritesheet", 140, 2, 62, 62, 2);
                gameManager.SetSprite(movingAnimatedSprite);
            }
        }
    }
}
