using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Sprint0;
using System.Collections.Generic;
public class Level
{
    private ContentManager content;
    private bool completed; // boolean to track whether level is completed
    private List<Entity> entitiesList;
    private Dictionary<string, Entity> entities;
    private Player player;
    private List<PickupItem> itemsList;
    private List<Mob> enemiesList;
    private List<Blocks> blocksList;
    public Level(ContentManager content)
    {
        this.content = content;
        itemsList = new List<PickupItem>();
        enemiesList = new List<Mob>();
        blocksList = new List<Blocks>();
    }
    public void AddPlayer (Vector2 position) {
        player = new Player(content);
        player.SetPosition(position);
        entitiesList.Add(player);
        entities.Add("Player", player);
    }
    public void AddItem (Vector2 position)
    {
        PickupItem newItem = new PickupItem(content);
        newItem.SetPosition(position);
        itemsList.Add(newItem);
        entitiesList.Add(newItem);
        entities.Add(string.Concat("Item",(itemsList.Count).ToString()), newItem);
    }
    public void AddEnemy(MobType mobType, Vector2 position)
    {
        Mob newEnemy = MobFactory.CreateMob(mobType, content);
        newEnemy.SetPosition(position);
        enemiesList.Add(newEnemy);
        entitiesList.Add(newEnemy);
        entities.Add(string.Concat("Enemy",(enemiesList.Count).ToString()), newEnemy);
    }
    public void AddBlock(Vector2 position)
    {
        Blocks newBlock = new Blocks(content);
        newBlock.SetPosition(position);
        blocksList.Add(newBlock);
        entitiesList.Add(newBlock);
        entities.Add(string.Concat("Block",(enemiesList.Count).ToString()), newBlock);
    }
}