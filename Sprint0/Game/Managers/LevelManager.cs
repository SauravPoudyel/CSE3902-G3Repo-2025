using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace Sprint0
{
    public class LevelManager {
        private Level level;
        public LevelManager() {
            level = new Level();
        }
        public void LoadContent(ContentManager content) {

            string entityFilePath = Path.Combine(Globals.projectDirectory, "Data\\Sprint3FuncLevelParse.csv"); //Sprint0\Data\LevelParseTest2.csv
            string tilesFilePath = Path.Combine(Globals.projectDirectory, "Data\\LevelTilesParseTest2.csv");
            level = CSVLevelParser.ParseLevel(entityFilePath, tilesFilePath, content);
        }
        
        public Dictionary<string, Entity> LoadLevelEntities() {
            return level.GetLevelEntities();
        }

        public List<Tile> LoadLevelTiles() {

            return level.GetLevelTiles;
        }
    }
}
