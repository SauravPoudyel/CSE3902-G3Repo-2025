using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Sprint0;
using System;
using System.Collections.Generic;
using System.IO;

public class LevelManager
{
    private Level level;
    public LevelManager() {
        level = new Level();
    }
    public void LoadContent(ContentManager content) {
        // Commenting out manual level loading while testing Level parser
        /* level.AddPlayer(content, new Vector2(100, 100));
        level.AddBlock(content, new Vector2(300, 300));
        level.AddEnemy(content, MobType.BossTank, new Vector2(500, 500));
        level.AddItem(content, new Vector2(700, 700)); */

        // string filePath = Path.Combine("Sprint0", "Data", "LevelParseTest1.csv");
        // string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sprint0", "Data", "LevelParseTest1.csv");
        string filePath = content.RootDirectory + "\\LevelParseTest1.csv";
        level = CSVParser.ParseLevel(filePath, content);
    }
    public Dictionary<string, Entity> LoadLevelEntities() {
        return level.LevelEntities;
    }
}