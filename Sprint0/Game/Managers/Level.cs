using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Sprint0 {
    public class Level
    {
        public int tileSize = 120;
        private List<Tile> tilesList;
        private bool completed;
        private List<Entity> entitiesList;
        private Dictionary<string, Entity> entities;
        private Player player;
        private List<Item> itemsList;
        private List<Mob> enemiesList;
        private List<BaseBlock> blocksList;

        public Level()
        {
            tilesList = new List<Tile>();
            entitiesList = new List<Entity>();
            entities = new Dictionary<string, Entity>();
            itemsList = new List<Item>();
            enemiesList = new List<Mob>();
            blocksList = new List<BaseBlock>();
        }

        public Dictionary<string, Entity> GetLevelEntities()
        {
            return entities;
        }

        public List<Tile> GetLevelTiles
        {
            get => tilesList;
        }

        public void AddTile(ContentManager content, Tile.TileType tileType, Vector2 position)
        {
            tilesList.Add(new Tile(content, tileType, (position * tileSize)+(new Vector2(tileSize/2, tileSize/2))));
        }

        public void AddPlayer(ContentManager content, Vector2 position)
        {
            player = new Player(content);
            player.SetPosition((position * tileSize)+(new Vector2(tileSize/2, tileSize/2)));
            entitiesList.Add(player);
            entities.Add("player", player);
        }

        public void AddItem(ContentManager content, Vector2 position, PickupItemType type)
        {
            Item newItem = new Item(content, type);
            newItem.SetPosition((position * tileSize)+(new Vector2(tileSize/2, tileSize/2)));
            newItem.EntityKey = "item_" + itemsList.Count + "_"  + type.ToString();
            itemsList.Add(newItem); 
            entitiesList.Add(newItem);
            entities.Add(newItem.EntityKey, newItem);
        }

        public void AddEnemy(ContentManager content, MobType mobType, Vector2 position)
        {
            Mob newEnemy = MobFactory.CreateMob(mobType, content);
            newEnemy.SetPosition((position * tileSize)+(new Vector2(tileSize/2, tileSize/2)));
            newEnemy.EntityKey = "enemy_" + enemiesList.Count + "_" + mobType.ToString();
            enemiesList.Add(newEnemy);
            entitiesList.Add(newEnemy);
            entities.Add(newEnemy.EntityKey, newEnemy);
        }

        public void AddBlock(ContentManager content, Vector2 position, BlockSpriteKey blockSpriteKey)
        {
            BaseBlock newBlock = BlockFactory.CreateBlock(blockSpriteKey, content, (position * tileSize)+(new Vector2(tileSize/2, tileSize/2)), 0.3f);
            newBlock.EntityKey = "block_" + blocksList.Count + "_" + blockSpriteKey.ToString();
            blocksList.Add(newBlock);
            entitiesList.Add(newBlock);
            entities.Add(newBlock.EntityKey, newBlock);
        }
    }
}
