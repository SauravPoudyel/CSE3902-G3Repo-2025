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
                { EntityKeys.BlockType.RockPileVar1, CreateRigidBlock },
                { EntityKeys.BlockType.RockPileVar2, CreateRigidBlock },
                { EntityKeys.BlockType.House, CreateRigidBlock },
                { EntityKeys.BlockType.House2, CreateRigidBlock },
                { EntityKeys.BlockType.Factory, CreateRigidBlock },
                { EntityKeys.BlockType.SmallTree, CreateRigidBlock },
                { EntityKeys.BlockType.Fence, CreateRigidBlock },
                { EntityKeys.BlockType.DeadTree, CreateRigidBlock },
                { EntityKeys.BlockType.Garage, CreateRigidBlock },
                { EntityKeys.BlockType.CoconutTree, CreateRigidBlock },
                { EntityKeys.BlockType.Boarder, CreateRigidBlock },
                { EntityKeys.BlockType.Tree1, CreateRigidBlock },
                { EntityKeys.BlockType.Tree2, CreateRigidBlock },
                { EntityKeys.BlockType.DesertHouse, CreateRigidBlock },
                { EntityKeys.BlockType.Tent, CreateRigidBlock },

                // Pushable Blocks
                { EntityKeys.BlockType.Box, CreatePushableBlock },

                // PushableDestructible Blocks
                { EntityKeys.BlockType.SmallBarrel, CreatePushableDestructibleBlock },

                // Flammable Blocks
                { EntityKeys.BlockType.Barrel, CreateFlammableBlock },
                { EntityKeys.BlockType.RedBarrel, CreateFlammableBlock },
                { EntityKeys.BlockType.Oil, CreateFlammableBlock },

                { EntityKeys.BlockType.Shop, CreateInteractableBlock },
                { EntityKeys.BlockType.ProceduralPortal, CreateInteractableBlock },

                { EntityKeys.BlockType.CliffHorizontalLeft, CreateCliffBlock },
                { EntityKeys.BlockType.CliffHorizontalRight, CreateCliffBlock },
                { EntityKeys.BlockType.CliffVerticalTop, CreateCliffBlock },
                { EntityKeys.BlockType.CliffVerticalBottom, CreateCliffBlock },
            };
        }

        public static BaseBlock CreateBlock(EntityKeys.BlockType blockType, ContentManager content, Vector2 position, float frameTime)
        {
            // Adjust spawn location based on cliff types
            switch (blockType)
            {
                case EntityKeys.BlockType.CliffHorizontalRight:
                    position.X += 44;
                    break;
                case EntityKeys.BlockType.CliffVerticalBottom:
                    position.Y += 20;
                    break;
                case EntityKeys.BlockType.CliffVerticalTop:
                    position.Y -= 46;
                    break;
                case EntityKeys.BlockType.CliffHorizontalLeft:
                    position.X -= 32;
                    break;
                default:
                    break; // No shift for other types
            }

            if (blockCreators.TryGetValue(blockType, out Func<ContentManager, EntityKeys.BlockType, float, BaseBlock> value))
            {
                BaseBlock block = value(content, blockType, frameTime);
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

        private static BaseBlock CreatePushableDestructibleBlock(ContentManager content, EntityKeys.BlockType blockType, float frameTime) =>
            new PushableDestructibleBlock(content, blockType, frameTime);
        
        private static BaseBlock CreateInteractableBlock(ContentManager content, EntityKeys.BlockType blockType, float frameTime) =>
            new InteractableBlock(content, blockType, frameTime);

        private static BaseBlock CreateCliffBlock(ContentManager content, EntityKeys.BlockType blockType, float frameTime) =>
            new CliffBlock(content, blockType, frameTime);
    }
}
