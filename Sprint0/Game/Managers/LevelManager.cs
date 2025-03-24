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
        public void LoadContent(ContentManager content, string levelName) {
            //string entityFilePath = Path.Combine(Globals.projectDirectory, "Data\\Sprint3FuncLevelParse.csv"); //Sprint0\Data\LevelParseTest2.csv
            //string tilesFilePath = Path.Combine(Globals.projectDirectory, "Data\\LevelTilesParseTest3.csv");
            string entityFilePath = Path.Combine(Globals.projectDirectory, "Data\\" + levelName + "_Entities.csv");
            string tilesFilePath = Path.Combine(Globals.projectDirectory, "Data\\" + levelName + "_Tiles.csv");
            level = CSVLevelParser.ParseLevel(entityFilePath, tilesFilePath, content);
        }
        
        public Dictionary<string, Entity> LoadLevelEntities() {
            return level.GetLevelEntities();
        }

        public List<Tile> LoadLevelTiles() {
            return level.GetLevelTiles;
        }
        public void UpdateLevel() {
            // tiles = levelManager.LoadLevelTiles();
            // entities = levelManager.LoadLevelEntities();
        }

        public void Update(Dictionary<string, Entity> entities) {
            level.Complete = true;
            foreach (Entity entity in entities.Values)
            {
                if(entity is Mob) {
                    level.Complete = false;
                }
            }
            if(entities.ContainsKey("player")) {
                Player player = (Player)entities["player"];
                if(level.Complete && player.GetPosition().X > 1920) {
                    player.SetPosition(new Vector2(0,player.GetPosition().Y));
                    player.MoveLevel(1);
                } else if(player.GetPosition().X<0) {
                    player.SetPosition(new Vector2(1920,player.GetPosition().Y));
                    player.MoveLevel(-1);
                }
            }
        }
    }
}
