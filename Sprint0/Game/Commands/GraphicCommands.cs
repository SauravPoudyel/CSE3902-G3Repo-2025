using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
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
                if (parameters.ContainsKey("pickupItem") && parameters["pickupItem"] is PickupItem pickupItem) 
                {
                    pickupItem.CycleItemPrev();
                }
            }
        }

        public class CycleItemNextCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("pickupItem") && parameters["pickupItem"] is PickupItem pickupItem) 
                {
                    pickupItem.CycleItemNext();
                }
            }
        }

        public class CycleEnemyNextCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager)
                {
                    MobFactory.CycleNextMob(gameManager);
                }
            }
        }

        public class CycleEnemyPrevCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.ContainsKey("gameManager") && parameters["gameManager"] is GameManager gameManager)
                {
                    MobFactory.CyclePreviousMob(gameManager);
                }
            }
        }

        public class SpawnExplosionCommand : ICommand
        {
            public void Execute(Dictionary<string, object> parameters)
            {
                if (parameters.TryGetValue("gameManager", out var gmObj) && gmObj is GameManager gm &&
            parameters.TryGetValue("spawnPosition", out var posObj) && posObj is Vector2 pos &&
            parameters.TryGetValue("phaseInterval", out var intervalObj) && intervalObj is float interval)
                {
                    string explosionKey = "explosion_" + Guid.NewGuid().ToString("N");
                    var explosion = new Explosion(gm.GetContent(), pos, interval, explosionKey);
                    gm.GetEntities().Add(explosionKey, explosion);
                }
            }
        }

    }
}