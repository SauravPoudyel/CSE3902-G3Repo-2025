using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using static Sprint0.EntityKeys;

namespace Sprint0 {
    public class Level
    {
        public enum Direction { Top, Bottom, Left, Right }  
        private Dictionary<Direction, Level> connectedLevels;  
        public int tileSize = 120;
        private int levelNumber;
        private bool unlocked;
        private bool complete;
        private bool loaded;
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
        public int LevelNumber
        {
            get { return levelNumber; }
            set { levelNumber = value; }
        }
        public bool Complete   
        {
            get { return complete; }
            set { complete = value; }
        }
        public bool Unlocked   
        {
            get { return unlocked; }
            set { unlocked = value; }
        }
        public bool Loaded   
        {
            get { return loaded; }
            set { loaded = value; }
        }
        public Dictionary<Direction, Level> ConnectedLevels   
        {
            get { return connectedLevels; }
            set { connectedLevels = value; }
        }
        public Level()
        {
            complete = false;
            unlocked = true;
            loaded = false;
            levelNumber = 1;
            tilesList = new List<Tile>();
            entities = new Dictionary<string, Entity>();
            blocksList = new List<BaseBlock>();
            itemsList = new List<Item>();
            enemiesList = new List<Mob>();
            connectedLevels = new Dictionary<Direction, Level>();
        }
        public List<Mob> GetLevelEnemies() => enemiesList;
        public List<Tile> GetLevelTiles => tilesList;
        public void AddConnectedLevel(Direction direction, Level level)
        {
            if (level != null)
            {
                connectedLevels[direction] = level;
            }
        }
        public Level GetConnectedLevel(Direction direction)
        {
            return connectedLevels.ContainsKey(direction) ? connectedLevels[direction] : null;
        }
        public bool HasConnectedLevel(Direction direction)
        {
            return connectedLevels.ContainsKey(direction);
        }
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

        public void AddBlock(ContentManager content, Vector2 position, BlockType blockType, float rotation)
        {
            Vector2 worldPosition = (position * tileSize) + new Vector2(tileSize / 2, tileSize / 2);
            BaseBlock newBlock = BlockFactory.CreateBlock(blockType, content, worldPosition, 0.3f);
            newBlock.Rotation = rotation;
            newBlock.EntityKey = "block_" + blocksList.Count + "_" + blockType.ToString();
            blocksList.Add(newBlock);
            entities.Add(newBlock.EntityKey, newBlock);
        }
    }
}
