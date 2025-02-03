using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Sprint0
{
    public static class GraphicCommands {
        public class DisplayStaticGameCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager)
                {
                    var staticSprite = new StaticSprite();
                    staticSprite.LoadContent(gameManager.GetContent(), "LinkSpritesheet", 140, 2, 62, 62, 1);
                    gameManager.GetEntity(0).SetSprite(staticSprite);
                }
            }
        }

        public class DisplayAnimatedGameCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager)
                {
                    var animatedSprite = new AnimatedSprite(0.4f);
                    animatedSprite.LoadContent(gameManager.GetContent(), "LinkSpritesheet", 140, 2, 62, 62, 2);
                    gameManager.GetEntity(0).SetSprite(animatedSprite);
                }
            }
        }

        public class SetSpriteCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager &&
                    parameters.ContainsKey("sprite") && parameters["sprite"] is ISprite sprite)
                {
                    gameManager.GetEntity(0).SetSprite(sprite);
                }
            }
        }

        public class CycleBlockPrevCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                // Implement the logic to cycle to the previous block
            }
        }

        public class CycleBlockNextCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                // Implement the logic to cycle to the next block
            }
        }

        public class CycleItemPrevCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                // Implement the logic to cycle to the previous item
            }
        }

        public class CycleItemNextCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                // Implement the logic to cycle to the next item
            }
        }

        public class CycleEnemyPrevCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                // Implement the logic to cycle to the previous enemy/NPC
            }
        }

        public class CycleEnemyNextCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                // Implement the logic to cycle to the next enemy/NPC
            }
        }
    }
}