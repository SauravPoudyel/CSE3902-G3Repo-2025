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
        private Level prereqLevel;
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
        private List<BaseBlock> perimeter;
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
            prereqLevel = null;
            tilesList = new List<Tile>();
            entities = new Dictionary<string, Entity>();
            blocksList = new List<BaseBlock>();
            itemsList = new List<Item>();
            enemiesList = new List<Mob>();
            connectedLevels = new Dictionary<Direction, Level>();
            perimeter = new List<BaseBlock>();
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
        public void AddConnectedLevel(Direction direction, Level level)
        {
            if (level != null)
            {
                connectedLevels[direction] = level;
            }
        }

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

        public Level GetConnectedLevel(Direction direction)
        {
            return connectedLevels.TryGetValue(direction, out Level value) ? value : null;
        }
        public bool HasConnectedLevel(Direction direction)
        {
            return connectedLevels.ContainsKey(direction);
        }
        public void UnlockConnectedLevel(Direction direction)
        {
            List<BaseBlock> fencesToRemove = perimeter.FindAll(fence =>
            {
                Vector2 pos = fence.GetPosition();
                switch (direction)
                {
                    case Direction.Top:
                        return pos.Y < 0;
                    case Direction.Bottom:
                        return pos.Y > tileSize * 9;
                    case Direction.Left:
                        return pos.X < 0;
                    case Direction.Right:
                        return pos.X > tileSize * 16;
                    default:
                        return false;
                }
            });
            foreach (var fence in fencesToRemove)
            {
                perimeter.Remove(fence);
                if (entities.ContainsKey(fence.EntityKey))
                {
                    entities.Remove(fence.EntityKey);
                }
            }
        }
        public void AddTile(ContentManager content, Tile.TileType tileType, Vector2 position)
        {
            tilesList.Add(new Tile(content, tileType, (position * tileSize) + new Vector2(tileSize / 2, tileSize / 2)));
        }

        public void AddPlayer(ContentManager content, Vector2 position)
        {
            // Only create a new player if one doesn't already exist in the level
            if (entities.ContainsKey("player"))
                return;

            player = new Player(content);
            player.SetPosition((position * tileSize) + new Vector2(tileSize / 2, tileSize / 2));
            player.EntityKey = "player";
            player.Update();
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
        
        public void InitializePerimeter(ContentManager content)
        {
            perimeter.Clear();
            for (int x = -1; x < 16; x++) {
                if(!this.HasConnectedLevel(Direction.Top) || !this.GetConnectedLevel(Direction.Top).Unlocked)
                    AddPerimeterBlock(content, new Vector2(x, -1));
                if(!this.HasConnectedLevel(Direction.Bottom) || !this.GetConnectedLevel(Direction.Bottom).Unlocked)
                    AddPerimeterBlock(content, new Vector2(x, 9));
            }
            for (int y = -1; y < 9; y++){
                if(!this.HasConnectedLevel(Direction.Left) || !this.GetConnectedLevel(Direction.Left).Unlocked)
                    AddPerimeterBlock(content, new Vector2(-1, y));
                if(!this.HasConnectedLevel(Direction.Right) || !this.GetConnectedLevel(Direction.Right).Unlocked)
                    AddPerimeterBlock(content, new Vector2(16, y));
            }
        }

        private void AddPerimeterBlock(ContentManager content, Vector2 position)
        {
            Vector2 worldPosition = (position * tileSize) + new Vector2(tileSize / 2, tileSize / 2);
            BaseBlock perimeterBlock = BlockFactory.CreateBlock(BlockType.Boarder, content, worldPosition, 0.3f);
            perimeter.Add(perimeterBlock);
            perimeterBlock.EntityKey = "perimeterBlock_" + perimeter.Count;
            entities.Add(perimeterBlock.EntityKey, perimeterBlock);
        }
    }
}
