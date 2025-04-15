using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using static Sprint0.EntityKeys;

namespace Sprint0 {
    public class Level
    {
        public enum Direction { Top, Bottom, Left, Right }  
        private LevelConnections connections;
        private Level prereqLevel;
        private int levelNumber;
        private bool unlocked;
        private bool complete;
        private bool loaded;
        private List<Tile> tilesList;
        private Dictionary<string, Entity> entities;
        private Player player;
        private LevelPerimeter levelPerimeter;

        public IEnumerable<Mob> GetEnemies() => entities.Values.OfType<Mob>();
        public IEnumerable<Item> GetItems() => entities.Values.OfType<Item>();
        public IEnumerable<BaseBlock> GetBlocks() => entities.Values.OfType<BaseBlock>();
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
        public Level PrereqLevel 
        {
            get { return prereqLevel; }
            set { prereqLevel = value; }
        }
        public LevelConnections ConnectedLevels => connections;
        public Level()
        {
            complete = false;
            unlocked = true;
            loaded = false;
            levelNumber = 1;
            prereqLevel = null;
            tilesList = new List<Tile>();
            entities = new Dictionary<string, Entity>();
            connections = new LevelConnections();
            levelPerimeter = new LevelPerimeter(this, entities);
        }
        public bool HasEnemies() {
            bool hasEnemies = false;
            foreach(Entity entity in entities.Values) {
                if(entity is Mob) {
                    hasEnemies = true;
                }
            }
            return hasEnemies;
        }
        public List<Tile> GetLevelTiles => tilesList;
        

        public bool HasKeyItem { get; private set; } = false;
        public EntityKeys.ItemType KeyItemType { get; private set; }
        private bool keyItemDropped = false;
        public void SetKeyItemType(EntityKeys.ItemType keyItemType)
        {
            HasKeyItem = true;
            KeyItemType = keyItemType;
        }

        // When the level is complete, drop the key item in the center.
        public void DropKeyItem(ContentManager content)
        {
            System.Console.WriteLine(Complete + " " + HasKeyItem + " " + keyItemDropped);
            if (Complete && HasKeyItem)
            {
                AddItem(content, new Vector2(8, 5), KeyItemType);
                keyItemDropped = true;
            }
        }

        public void AddConnectedLevel(Direction dir, Level level)
            => connections.AddConnectedLevel(dir, level);

        public bool HasConnectedLevel(Direction dir)
            => connections.HasConnectedLevel(dir);

        public Level GetConnectedLevel(Direction dir)
            => connections.GetConnectedLevel(dir);

        public void UnlockConnectedLevel(Direction direction)
        {
            levelPerimeter.Unlock(direction);
        }

        public void AddTile(ContentManager content, Tile.TileType tileType, Vector2 position)
        {
            tilesList.Add(new Tile(content, tileType, (position * Globals.TILESIZE) + new Vector2(Globals.TILESIZE / 2, Globals.TILESIZE / 2)));
        }

        public void AddPlayer(ContentManager content, Vector2 position)
        {
            // Only create a new player if one doesn't already exist in the level
            if (entities.ContainsKey("player"))
                return;

            player = new Player(content);
            player.SetPosition((position * Globals.TILESIZE) + new Vector2(Globals.TILESIZE / 2, Globals.TILESIZE / 2));
            player.EntityKey = "player";
            player.Update();
            entities.Add("player", player);
        }
        public void AddItem(ContentManager content, Vector2 position, EntityKeys.ItemType itemType)
        {
            Item newItem = new Item(content, itemType);
            newItem.SetPosition((position * Globals.TILESIZE) + new Vector2(Globals.TILESIZE / 2, Globals.TILESIZE / 2));
            newItem.EntityKey = "item_" + this.GetItems().Count() + "_" + itemType.ToString();
            entities.Add(newItem.EntityKey, newItem);
        }

        public void AddEnemy(ContentManager content, MobType mobType, Vector2 position)
        {
            Mob newEnemy = MobFactory.CreateMob(mobType, content);
            newEnemy.SetPosition((position * Globals.TILESIZE) + new Vector2(Globals.TILESIZE / 2, Globals.TILESIZE / 2));
            newEnemy.EntityKey = "enemy_" + this.GetEnemies().Count() + "_" + mobType.ToString();
            entities.Add(newEnemy.EntityKey, newEnemy);
        }

        public void AddBlock(ContentManager content, Vector2 position, BlockType blockType, float rotation)
        {
            Vector2 worldPosition = (position * Globals.TILESIZE) + new Vector2(Globals.TILESIZE / 2, Globals.TILESIZE / 2);
            BaseBlock newBlock = BlockFactory.CreateBlock(blockType, content, worldPosition, 0.3f);
            newBlock.Rotation = rotation;
            newBlock.EntityKey = "block_" + this.GetBlocks().Count() + "_" + blockType.ToString();
            entities.Add(newBlock.EntityKey, newBlock);
        }
        public void InitializePerimeter(ContentManager content)
        {
            levelPerimeter.Initialize(content);
        }
    }
}
