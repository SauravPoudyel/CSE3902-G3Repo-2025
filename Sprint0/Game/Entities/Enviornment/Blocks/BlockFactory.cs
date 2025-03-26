using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

namespace Sprint0
{
    public static class BlockFactory
    {
        private static readonly Dictionary<EntityKeys.BlockType, Func<ContentManager, EntityKeys.BlockType, float, BaseBlock>> blockCreators;

        static BlockFactory()
        {
            blockCreators = new Dictionary<EntityKeys.BlockType, Func<ContentManager, EntityKeys.BlockType, float, BaseBlock>>
            {
                // Rigid Blocks
                { EntityKeys.BlockType.Tree, CreateRigidBlock },
                { EntityKeys.BlockType.BarbedFence, CreateRigidBlock },
                { EntityKeys.BlockType.RockPile, CreateRigidBlock },
                { EntityKeys.BlockType.Factory, CreateRigidBlock },
                { EntityKeys.BlockType.RockPileVar1, CreateRigidBlock },
                { EntityKeys.BlockType.RockPileVar2, CreateRigidBlock },
                { EntityKeys.BlockType.House, CreateRigidBlock },
                { EntityKeys.BlockType.House2, CreateRigidBlock },
                { EntityKeys.BlockType.SmallTree, CreateRigidBlock },
                { EntityKeys.BlockType.Fence, CreateRigidBlock },
                { EntityKeys.BlockType.DeadTree, CreateRigidBlock },
                { EntityKeys.BlockType.Garage, CreateRigidBlock },
                { EntityKeys.BlockType.CoconutTree, CreateRigidBlock },
                { EntityKeys.BlockType.Boarder, CreateRigidBlock },
                { EntityKeys.BlockType.Tree1, CreateRigidBlock },
                { EntityKeys.BlockType.Tree2, CreateRigidBlock },

                // Pushable Blocks
                { EntityKeys.BlockType.Box, CreatePushableBlock },
                { EntityKeys.BlockType.SmallBarrel, CreatePushableBlock },

                // Flammable Blocks
                { EntityKeys.BlockType.Barrel, CreateFlammableBlock },
                { EntityKeys.BlockType.RedBarrel, CreateFlammableBlock },
                { EntityKeys.BlockType.Oil, CreateFlammableBlock }
            };
        }

        public static BaseBlock CreateBlock(EntityKeys.BlockType blockType, ContentManager content, Vector2 position, float frameTime)
        {
            if (blockCreators.ContainsKey(blockType))
            {
                BaseBlock block = blockCreators[blockType](content, blockType, frameTime);
                block.SetPosition(position);
                return block;
            }
            return null;
        }

        private static BaseBlock CreateRigidBlock(ContentManager content, EntityKeys.BlockType blockType, float frameTime) =>
            new RigidBlock(content, blockType, frameTime);

        private static BaseBlock CreatePushableBlock(ContentManager content, EntityKeys.BlockType blockType, float frameTime) =>
            new PushableBlock(content, blockType, frameTime);

        private static BaseBlock CreateFlammableBlock(ContentManager content, EntityKeys.BlockType blockType, float frameTime) =>
            new FlammableBlock(content, blockType, frameTime);
    }
}
