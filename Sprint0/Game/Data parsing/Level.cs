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
        private Level prereqLevel;
        private int levelNumber;
        private bool unlocked;
        private bool complete;
        private bool loaded;
        private List<Tile> tilesList;
        private LevelEntities levelEntities;
        private LevelConnections connections;
        private LevelPerimeter levelPerimeter;

        public IEnumerable<Mob> GetEnemies() => levelEntities.GetEntitiesOfType<Mob>();
        public IEnumerable<Item> GetItems() => levelEntities.GetEntitiesOfType<Item>();
        public IEnumerable<BaseBlock> GetBlocks() => levelEntities.GetEntitiesOfType<BaseBlock>();

        public Dictionary<string, Entity> Entities
        {
            get => levelEntities.Entities;
            set
            {
                foreach (var kvp in value)
                    levelEntities.Entities[kvp.Key] = kvp.Value;
            }
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
            connections = new LevelConnections();
            levelPerimeter = new LevelPerimeter(this, levelEntities.Entities);
            levelEntities = new LevelEntities();
        }
        public bool HasEnemies() => GetEnemies().Any();
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

        public void AddPlayer(ContentManager content, Vector2 gridPosition)
            => levelEntities.AddPlayer(content, gridPosition);
        public void AddItem(ContentManager content, Vector2 gridPosition, EntityKeys.ItemType itemType) 
            => levelEntities.AddItem(content, gridPosition, itemType);
        public void AddEnemy(ContentManager content, Vector2 gridPosition, MobType mobType) 
            => levelEntities.AddEnemy(content, gridPosition, mobType);
        public void AddBlock(ContentManager content, Vector2 gridPosition, BlockType blockType, float rotation) 
            => levelEntities.AddBlock(content, gridPosition, blockType, rotation);
        
        public void InitializePerimeter(ContentManager content)
        {
            levelPerimeter.Initialize(content);
        }
    }
}
