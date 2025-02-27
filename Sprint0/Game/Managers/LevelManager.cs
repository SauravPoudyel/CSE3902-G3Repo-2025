using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Sprint0;
using System.Collections.Generic;

public class LevelManager
{
    private Level level;
    public LevelManager() {
        level = new Level();
    }
    public void LoadContent(ContentManager content) {
        level.AddPlayer(content, new Vector2(100, 100));
        level.AddBlock(content, new Vector2(300, 300));
        level.AddEnemy(content, MobType.BossTank, new Vector2(500, 500));
        level.AddItem(content, new Vector2(700, 700));
    }
    public Dictionary<string, Entity> LoadLevelEntities() {
        return level.LevelEntities;
    }
}