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
        string projectDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\"));
        string filePath2 = Path.Combine(projectDirectory, "Content\\LevelParseTest1.csv");
        // string filePath = Path.Combine(AppDomain.CurrentDomain.RelativeSearchPath, "Data", "LevelParseTest1.csv");
        
        // string filePath = content.RootDirectory + "\\LevelParseTest1.csv";
        // C:\Users\manzl\Documents\GitHub\CSE3902-G3Repo-2025\Sprint0\Content\LevelParseTest1.csv
        // string filePath = "GitHub\\CSE3902-G3Repo-2025\\Sprint0\\Content\\LevelParseTest1.csv";
        level = CSVParser.ParseLevel(filePath2, content);
    }
    public Dictionary<string, Entity> LoadLevelEntities() {
        return level.LevelEntities;
    }
}