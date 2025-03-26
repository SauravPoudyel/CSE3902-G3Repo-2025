using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace Sprint0
{
    public class LevelManager {
        private Level activeLevel;
        private Dictionary<string, Level> levels;
        public LevelManager() {
            activeLevel = new Level();
            levels = CSVLevelParser.ParseLevelIndex();
        }
        public void LoadContent(ContentManager content, int levelNum) {
            string levelName = "Level" + levelNum;
            if(!levels.ContainsKey(levelName)) { // if level isn't in dictionary
                levels.Add(levelName, new Level());
            }  
            if(!levels[levelName].Loaded) { // if level hasn't been loaded
                    string entityFilePath = Path.Combine(Globals.projectDirectory, "Data\\Level" + levelNum + "_Entities.csv");
                    string tilesFilePath = Path.Combine(Globals.projectDirectory, "Data\\Level" + levelNum + "_Tiles.csv");
                    Level loadedLevel = CSVLevelParser.ParseLevel(entityFilePath, tilesFilePath, content);
                    loadedLevel.ConnectedLevels = levels[levelName].ConnectedLevels; // Unfortunately, current methods require this awkward handoff
                    levels[levelName] = loadedLevel;
                    levels[levelName].LevelNumber = levelNum;
                    levels[levelName].Loaded = true;
            }          
            activeLevel = levels[levelName];
        }
        public Dictionary<string, Entity> LoadLevelEntities() {
            return activeLevel.Entities;
        }
        public List<Tile> LoadLevelTiles() {
            return activeLevel.GetLevelTiles;
        }
        public void SwitchLevel(Level.Direction direction) {
            if(activeLevel.HasConnectedLevel(direction)) {
                activeLevel = activeLevel.GetConnectedLevel(direction);
            }
        }
        public void UpdateLevelEntities(Dictionary<string, Entity> entities) {
            activeLevel.Entities = entities;
        }
        public void Update(Dictionary<string, Entity> entities) {
            UpdateLevelEntities(entities);
            activeLevel.Complete = true;
            foreach (Entity entity in entities.Values)
            {
                if(entity is Mob) {
                    activeLevel.Complete = false;
                }
            }
            // Level moving logic
            if(entities.ContainsKey("player")) {
                Player player = (Player)entities["player"];
                if(player.GetPosition().Y < 0 && activeLevel.HasConnectedLevel(Level.Direction.Top)) {
                    player.SetPosition(new Vector2(player.GetPosition().X, 1080));
                    player.MoveLevel(activeLevel.GetConnectedLevel(Level.Direction.Top).LevelNumber);
                } else if(player.GetPosition().Y > 1080 && activeLevel.HasConnectedLevel(Level.Direction.Bottom)) {
                    player.SetPosition(new Vector2(player.GetPosition().X, 0));
                    player.MoveLevel(activeLevel.GetConnectedLevel(Level.Direction.Bottom).LevelNumber);
                } else if(player.GetPosition().X < 0 && activeLevel.HasConnectedLevel(Level.Direction.Left)) {
                    player.SetPosition(new Vector2(1920,player.GetPosition().Y));
                    player.MoveLevel(activeLevel.GetConnectedLevel(Level.Direction.Left).LevelNumber);
                } else if(player.GetPosition().X > 1920 && activeLevel.HasConnectedLevel(Level.Direction.Right)) {
                    player.SetPosition(new Vector2(0,player.GetPosition().Y));
                    player.MoveLevel(activeLevel.GetConnectedLevel(Level.Direction.Right).LevelNumber);
                }
            }
        }
    }
}
