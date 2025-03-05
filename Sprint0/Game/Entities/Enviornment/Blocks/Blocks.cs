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
            Oil,
            BarbedFence,
            Barrel,
            RedBarrel,
            RockPile,
            Factory,
            RockPileVar1,
            RockPileVar2,
            Hosue,
            House2,
            SmallTree,
            Fence,
            DeadTree,
            Garage,
            CoconutTree,
            SmallBarrel,
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

            AddSprite("BarbedFence", new AnimatedSprite(0.3f));
            sprites["BarbedFence"].LoadContent(content, "TDTanksAllSprites", 958, 1048, 56, 56, 1);

            AddSprite("Barrel", new AnimatedSprite(0.3f));
            sprites["Barrel"].LoadContent(content, "2DTanksSprites", 485, 1523, 80, 99, 1);

            AddSprite("RedBarrel", new AnimatedSprite(0.3f));
            sprites["RedBarrel"].LoadContent(content, "2DTanksSprites", 485, 1622, 80, 99, 1);

            AddSprite("RockPile", new AnimatedSprite(0.3f));
            sprites["RockPile"].LoadContent(content, "TDTanksAllSprites", 950, 1593, 76, 70, 1);

            AddSprite("Factory", new AnimatedSprite(0.3f));
            sprites["Factory"].LoadContent(content, "TDTanksAllSprites", 766, 1768, 104, 96, 1);

            AddSprite("RockPileVar1", new AnimatedSprite(0.3f));
            sprites["RockPileVar1"].LoadContent(content, "TDTanksAllSprites", 950, 1523, 76, 70, 1);

            AddSprite("RockPileVar2", new AnimatedSprite(0.3f));
            sprites["RockPileVar2"].LoadContent(content, "TDTanksAllSprites", 950, 1453, 76, 70, 1);

            AddSprite("Hosue", new AnimatedSprite(0.3f));
            sprites["Hosue"].LoadContent(content, "TDTanksAllSprites", 862, 1431, 88, 96, 1);

            AddSprite("House2", new AnimatedSprite(0.3f));
            sprites["House2"].LoadContent(content, "TDTanksAllSprites", 871, 1128, 88, 120, 1);

            AddSprite("SmallTree", new AnimatedSprite(0.3f));
            sprites["SmallTree"].LoadContent(content, "TDTanksAllSprites", 954, 362, 72, 72, 1);

            AddSprite("Fence", new AnimatedSprite(0.3f));
            sprites["Fence"].LoadContent(content, "TDTanksAllSprites", 2159, 1095, 128, 48, 1);

            AddSprite("DeadTree", new AnimatedSprite(0.3f));
            sprites["DeadTree"].LoadContent(content, "TDTanksAllSprites", 128, 512, 128, 128, 1);

            AddSprite("Garage", new AnimatedSprite(0.3f));
            sprites["Garage"].LoadContent(content, "TDTanksAllSprites", 869, 1864, 88, 120, 1);

            AddSprite("CoconutTree", new AnimatedSprite(0.3f));
            sprites["CoconutTree"].LoadContent(content, "TDTanksAllSprites", 774, 2748, 120, 120, 1);

            AddSprite("SmallBarrel", new AnimatedSprite(0.3f));
            sprites["SmallBarrel"].LoadContent(content, "TDTanksAllSprites", 1016, 510, 40, 56, 1);

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
            if(blocktype != BlockType.RedBarrel) {
                this.blocktype++;
            } else {
                this.blocktype = BlockType.Tree;
            }
            SetBlockType(this.blocktype);
        }

        public void CycleBlockPrev()
        {
            if(blocktype != BlockType.Tree) {
                this.blocktype--;
            } else {
                this.blocktype = BlockType.RedBarrel;
            }
            SetBlockType(this.blocktype);
        }

        public override void Update()
        {
            position += velocity * Globals.FRAMETIME; 
            sprite.Update();
            bounds = new Rectangle((int)position.X, (int)position.Y, 80, 80);
            
            timer += Globals.FRAMETIME; 
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            SpriteEffects effects = SpriteEffects.None;

            sprite.Draw(spriteBatch, position, effects, 0f);
        }
    }
}
