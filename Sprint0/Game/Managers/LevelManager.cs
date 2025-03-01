using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace Sprint0
{
    private Level level;
    public LevelManager() {
        level = new Level();
    }
    public void LoadContent(ContentManager content) {
        string projectDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\"));
        string entityFilePath = Path.Combine(projectDirectory, "Data\\LevelParseTest1.csv");
        string tilesFilePath = Path.Combine(projectDirectory, "Data\\LevelTilesParseTest2.csv");
        level = CSVLevelParser.ParseLevel(entityFilePath, tilesFilePath, content);
    }
    
    public Dictionary<string, Entity> LoadLevelEntities() {
        return level.LevelEntities;
    }

    public List<Tile> LoadLevelTiles() {

        return level.LevelTiles;
    }
}
