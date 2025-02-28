using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Sprint0;
using System;
using System.Collections.Generic;
public class Level
{
    public int tileSize = 120;
    private bool completed; // boolean to track whether level is completed
    private List<Tile> tilesList;
    private List<Entity> entitiesList;
    private Dictionary<string, Entity> entities;
    private Player player;
    private List<PickupItem> itemsList;
    private List<Mob> enemiesList;
    private List<Blocks> blocksList;
    public Level()
    {
        completed = false;
        tilesList = new List<Tile>();
        entitiesList = new List<Entity>();
        entities = new Dictionary<string, Entity>();
        itemsList = new List<PickupItem>();
        enemiesList = new List<Mob>();
        blocksList = new List<Blocks>();
    }
    public Dictionary<string, Entity> LevelEntities
    {
        get => entities;
    }
    public List<Tile> LevelTiles
    {
        get => tilesList;
    }
    public void AddTile(ContentManager content, Tile.TileType tileType, Vector2 position)
        {
            tilesList.Add(new Tile(content, tileType, (position * tileSize)+(new Vector2(tileSize/2, tileSize/2))));
        }
    public void AddPlayer (ContentManager content, Vector2 position) {
        player = new Player(content);
        player.SetPosition(position);
        entitiesList.Add(player);
        entities.Add("player", player);
    }
    public void AddItem (ContentManager content, Vector2 position)
    {
        PickupItem newItem = new PickupItem(content);
        newItem.SetPosition(position);
        itemsList.Add(newItem);
        entitiesList.Add(newItem);
        entities.Add(string.Concat("Item",(itemsList.Count).ToString()), newItem);
    }
    public void AddEnemy(ContentManager content, MobType mobType, Vector2 position)
    {
        Mob newEnemy = MobFactory.CreateMob(mobType, content);
        newEnemy.SetPosition(position);
        enemiesList.Add(newEnemy);
        entitiesList.Add(newEnemy);
        entities.Add(string.Concat("Enemy",(enemiesList.Count).ToString()), newEnemy);
    }
    public void AddBlock(ContentManager content, Vector2 position)
    {
        Blocks newBlock = new Blocks(content);
        newBlock.SetPosition(position);
        blocksList.Add(newBlock);
        entitiesList.Add(newBlock);
        entities.Add(string.Concat("Block",(enemiesList.Count).ToString()), newBlock);
    }
}