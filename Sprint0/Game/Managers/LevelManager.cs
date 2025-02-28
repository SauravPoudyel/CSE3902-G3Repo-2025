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
        string projectDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\"));
        string entityFilePath = Path.Combine(projectDirectory, "Data\\LevelParseTest1.csv");
        string tilesFilePath = Path.Combine(projectDirectory, "Data\\LevelTilesParseTest1.csv");
        level = CSVLevelParser.ParseLevel(entityFilePath, tilesFilePath, content);
    }
    
    public Dictionary<string, Entity> LoadLevelEntities() {
        return level.LevelEntities;
    }

    public List<Tile> LoadLevelTiles() {

        return level.LevelTiles;
    }
}