using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Sprint0;
using static Sprint0.Level;
using static Sprint0.EntityKeys;
public class LevelEntities
{
    private Dictionary<string, Entity> entities = new();
    private int entityIdCounter = 0;
    public Dictionary<string, Entity> Entities => entities;

    public IEnumerable<T> GetEntitiesOfType<T>() where T : Entity
        => entities.Values.OfType<T>();

    public Vector2 ToWorldPosition(Vector2 gridPosition)
        => (gridPosition * Globals.TILESIZE) + new Vector2(Globals.TILESIZE / 2f);

    private string GenerateEntityKey(string prefix)
        => $"{prefix}_{entityIdCounter++}";

    public void AddPlayer(ContentManager content, Vector2 gridPosition)
    {
        if (entities.ContainsKey("player")) return;

        Player player = new Player(content);
        player.SetPosition(ToWorldPosition(gridPosition));
        player.EntityKey = "player";
        player.Update();
        entities[player.EntityKey] = player;
    }

    public void AddItem(ContentManager content, Vector2 gridPosition, EntityKeys.ItemType itemType)
    {
        Item item = new Item(content, itemType);
        item.SetPosition(ToWorldPosition(gridPosition));
        item.EntityKey = GenerateEntityKey("item");
        entities[item.EntityKey] = item;
    }

    public void AddEnemy(ContentManager content, Vector2 gridPosition, MobType mobType)
    {
        Mob enemy = MobFactory.CreateMob(mobType, content);
        enemy.SetPosition(ToWorldPosition(gridPosition));
        enemy.EntityKey = GenerateEntityKey("enemy");
        entities[enemy.EntityKey] = enemy;
    }

    public void AddBlock(ContentManager content, Vector2 gridPosition, BlockType blockType, float rotation)
    {
        BaseBlock block = BlockFactory.CreateBlock(blockType, content, ToWorldPosition(gridPosition), 0.3f);
        block.Rotation = rotation;
        block.EntityKey = GenerateEntityKey("block");
        entities[block.EntityKey] = block;
    }

    public void Remove(string entityKey)
    {
        if (entities.ContainsKey(entityKey))
            entities.Remove(entityKey);
    }
}
