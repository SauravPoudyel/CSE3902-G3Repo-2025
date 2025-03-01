using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Sprint0 {
    public class Level
    {
        private bool completed;
        private List<Entity> entitiesList;
        private Dictionary<string, Entity> entities;
        private Player player;
        private List<Item> itemsList;
        private List<Mob> enemiesList;
        private List<BaseBlock> blocksList;

        public Level()
        {
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

        public void AddPlayer(ContentManager content, Vector2 position)
        {
            player = new Player(content);
            player.SetPosition(position);
            entitiesList.Add(player);
            entities.Add("player", player);
        }

        public void AddItem(ContentManager content, Vector2 position, PickupItemType type)
        {
            Item newItem = new Item(content, type);
            newItem.SetPosition(position);
            newItem.EntityKey = "item_" + itemsList.Count + "_"  + type.ToString();
            itemsList.Add(newItem); 
            entitiesList.Add(newItem);
            entities.Add(newItem.EntityKey, newItem);
        }

        public void AddEnemy(ContentManager content, MobType mobType, Vector2 position)
        {
            Mob newEnemy = MobFactory.CreateMob(mobType, content);
            newEnemy.SetPosition(position);
            newEnemy.EntityKey = "enemy_" + enemiesList.Count + "_" + mobType.ToString();
            enemiesList.Add(newEnemy);
            entitiesList.Add(newEnemy);
            entities.Add(newEnemy.EntityKey, newEnemy);
        }

        public void AddBlock(ContentManager content, Vector2 position, BlockSpriteKey blockSpriteKey)
        {
            BaseBlock newBlock = BlockFactory.CreateBlock(blockSpriteKey, content, position, 0.3f);
            newBlock.EntityKey = "block_" + blocksList.Count + "_" + blockSpriteKey.ToString();
            blocksList.Add(newBlock);
            entitiesList.Add(newBlock);
            entities.Add(newBlock.EntityKey, newBlock);
        }
    }
}
