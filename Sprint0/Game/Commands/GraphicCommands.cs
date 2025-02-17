using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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
                    gameManager.GetEntity("player").SetSprite(staticSprite);
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
                    gameManager.GetEntity("player").SetSprite(animatedSprite);
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
                    gameManager.GetEntity("player").SetSprite(sprite);
                }
            }
        }

        public class UpdateCannonCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("player") && parameters["player"] is Player player &&
                    parameters.ContainsKey("rotation") && parameters["rotation"] is float rotation)
                {
                    player.SetCannonRotation(rotation);
                }
            }
        }

        public class CycleBlockPrevCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("blocks") && parameters["blocks"] is Blocks blocks)
                {
                    blocks.CycleBlockPrev();
                }
            }
        }

        public class CycleBlockNextCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("blocks") && parameters["blocks"] is Blocks blocks)
                {
                    blocks.CycleBlockNext();
                }
            }
        }

        public class CycleItemPrevCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                // Implement the logic to cycle to the previous item
                if (parameters.ContainsKey("pickUpItem") && parameters["pickUpItem"] is PickupItem pickupItem) 
                {
                    pickupItem.CycleItemPrev();
                }
            }
        }

        public class CycleItemNextCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("pickUpItem") && parameters["pickUpItem"] is PickupItem pickupItem) 
                {
                    pickupItem.CycleItemNext();
                }
            }
        }

        public class CycleEnemyPrevCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                // Implement the logic to cycle to the next enemy/NPC
                if (parameters.ContainsKey("mob") && parameters["mob"] is Mob mob) 
                {
                    mob.CycleEnemyPrev();
                }
            }
        }

        public class CycleEnemyNextCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                // Implement the logic to cycle to the next enemy/NPC
                if (parameters.ContainsKey("mob") && parameters["mob"] is Mob mob) 
                {
                    mob.CycleEnemyNext();
                }
            }
        }
    }
}