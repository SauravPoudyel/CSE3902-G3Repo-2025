using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using static Sprint0.EntityKeys;

namespace Sprint0 {
    public class Level
    {
        public int tileSize = 120;
        private bool complete;
        private List<Tile> tilesList;
        private Dictionary<string, Entity> entities;
        private List<BaseBlock> blocksList;
        private Player player;
        private List<Item> itemsList;
        private List<Mob> enemiesList;
        public Dictionary<string, Entity> Entities 
        {
            get { return entities; }
            set { entities = value; }
        }
        public bool Complete   
        {
            get { return complete; }
            set { complete = value; }
        }

        public Level()
        {
            complete = false;
            tilesList = new List<Tile>();
            entities = new Dictionary<string, Entity>();
            blocksList = new List<BaseBlock>();
            itemsList = new List<Item>();
            enemiesList = new List<Mob>();
        }

        public Dictionary<string, Entity> GetLevelEntities() => entities;
        public List<Mob> GetLevelEnemies() => enemiesList;
        public List<Tile> GetLevelTiles => tilesList;
        public void AddTile(ContentManager content, Tile.TileType tileType, Vector2 position)
        {
            tilesList.Add(new Tile(content, tileType, (position * tileSize) + new Vector2(tileSize / 2, tileSize / 2)));
        }

        public void AddPlayer(ContentManager content, Vector2 position)
        {
            player = new Player(content);
            player.SetPosition((position * tileSize) + new Vector2(tileSize / 2, tileSize / 2));
            entities.Add("player", player);
        }

        public void AddItem(ContentManager content, Vector2 position, EntityKeys.ItemType itemType)
        {
            Item newItem = new Item(content, itemType);
            newItem.SetPosition((position * tileSize) + new Vector2(tileSize / 2, tileSize / 2));
            newItem.EntityKey = "item_" + itemsList.Count + "_" + itemType.ToString();
            itemsList.Add(newItem);
            entities.Add(newItem.EntityKey, newItem);
        }

        public void AddEnemy(ContentManager content, MobType mobType, Vector2 position)
        {
            Mob newEnemy = MobFactory.CreateMob(mobType, content);
            newEnemy.SetPosition((position * tileSize) + new Vector2(tileSize / 2, tileSize / 2));
            newEnemy.EntityKey = "enemy_" + enemiesList.Count + "_" + mobType.ToString();
            enemiesList.Add(newEnemy);
            entities.Add(newEnemy.EntityKey, newEnemy);
        }

        public void AddBlock(ContentManager content, Vector2 position, BlockType blockType)
        {
            BaseBlock newBlock = BlockFactory.CreateBlock(blockType, content, (position * tileSize) + new Vector2(tileSize / 2, tileSize / 2), 0.3f);
            newBlock.EntityKey = "block_" + blocksList.Count + "_" + blockType.ToString();
            blocksList.Add(newBlock);
            entities.Add(newBlock.EntityKey, newBlock);
        }
    }
}
