using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint0
{
    public class Blocks : Entity
    {
        enum BlockType
        {
            Tree,
            Box,
            Oil
        }
        private ContentManager content;
        private float timer;

        private BlockType blocktype;

        public Blocks(ContentManager content)
        {
            this.content = content;
            this.blocktype = BlockType.Tree;

            AddSprite("Tree", new AnimatedSprite(0.3f));
            sprites["Tree"].LoadContent(content, "TDTanksAllSprites", 128, 0, 128, 128, 1);

            AddSprite("Box", new AnimatedSprite(0.3f));
            sprites["Box"].LoadContent(content, "TDTanksAllSprites", 960, 753, 56, 56, 1);

            AddSprite("Oil", new AnimatedSprite(0.3f));
            sprites["Oil"].LoadContent(content, "TDTanksAllSprites", 524, 1024, 100, 100, 1);

            SetSprite(sprites[this.blocktype.ToString()]);
        }

        private void SetBlockType(BlockType blockType)
        {
            this.blocktype = blockType;
            this.SetSprite(this.blocktype.ToString());
        }

        public string GetBlockType()
        {
            return blocktype.ToString();
        }

        public void CycleBlockNext()
        {
            if (blocktype == BlockType.Tree)
            {
                SetBlockType(BlockType.Box);
            }
            else if (blocktype == BlockType.Box)
            {
                SetBlockType(BlockType.Oil);
            }
            else if (blocktype == BlockType.Oil)
            {
                SetBlockType(BlockType.Tree);
            }
        }

        public void CycleBlockPrev()
        {
            if (blocktype == BlockType.Tree)
            {
                SetBlockType(BlockType.Oil);
            }
            else if (blocktype == BlockType.Box)
            {
                SetBlockType(BlockType.Tree);
            }
            else if (blocktype == BlockType.Oil)
            {
                SetBlockType(BlockType.Box);
            }
        }

        public override void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            SpriteEffects effects = SpriteEffects.None;
            sprite.Draw(spriteBatch, position, effects);
        }
    }
}
