using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

namespace Sprint0
{
    public static class BlockFactory
    {
        private static Dictionary<BlockSpriteKey, Func<ContentManager, BlockSpriteKey, float, BaseBlock>> blockCreators;

        static BlockFactory()
        {
            blockCreators = new Dictionary<BlockSpriteKey, Func<ContentManager, BlockSpriteKey, float, BaseBlock>>();
            blockCreators.Add(BlockSpriteKey.Tree, CreateRigidBlock);
            blockCreators.Add(BlockSpriteKey.BarbedFence, CreateRigidBlock);
            blockCreators.Add(BlockSpriteKey.Box, CreatePushableBlock);
            blockCreators.Add(BlockSpriteKey.OilBarrel_Red, CreateFlammableBlock);
            blockCreators.Add(BlockSpriteKey.OilBarrel_Black, CreateFlammableBlock);
            blockCreators.Add(BlockSpriteKey.Oil, CreateFlammableBlock);
        }

        public static BaseBlock CreateBlock(BlockSpriteKey spriteKey, ContentManager content, Vector2 position, float frameTime)
        {
            if (blockCreators.ContainsKey(spriteKey))
            {
                BaseBlock block = blockCreators[spriteKey](content, spriteKey, frameTime);
                block.SetPosition(position);
                return block;
            }
            return null;
        }

        private static BaseBlock CreateRigidBlock(ContentManager content, BlockSpriteKey spriteKey, float frameTime)
        {
            return new RigidBlock(content, spriteKey, frameTime);
        }

        private static BaseBlock CreatePushableBlock(ContentManager content, BlockSpriteKey spriteKey, float frameTime)
        {
            return new PushableBlock(content, spriteKey, frameTime);
        }

        private static BaseBlock CreateFlammableBlock(ContentManager content, BlockSpriteKey spriteKey, float frameTime)
        {
            return new FlammableBlock(content, spriteKey, frameTime);
        }
    }
}
