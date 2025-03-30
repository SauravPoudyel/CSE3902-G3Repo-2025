using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
                    loadedLevel.InitializePerimeter(content);
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
            if(!activeLevel.Complete && activeLevel.Loaded && !activeLevel.HasEnemies()) {
                activeLevel.Complete = true;
                foreach(Level level in levels.Values) {
                    if(level.PrereqLevel!=null && level.PrereqLevel.LevelNumber == activeLevel.LevelNumber) 
                        level.Unlocked = true;
                        if(activeLevel.ConnectedLevels.Values.Contains(level)) {
                            var direction = activeLevel.ConnectedLevels.FirstOrDefault(x => x.Value == level).Key;
                            activeLevel.UnlockConnectedLevel(direction);
                        }
                }
            }
            // Level moving logic
            if(entities.ContainsKey("player")) {
                Player player = (Player)entities["player"];
                Vector2 playerPosition = player.GetPosition();
                if(playerPosition.Y < 0 && activeLevel.HasConnectedLevel(Level.Direction.Top) 
                && activeLevel.GetConnectedLevel(Level.Direction.Top).Unlocked) {
                    player.MoveLevel(activeLevel.GetConnectedLevel(Level.Direction.Top).LevelNumber);
                    player.SetPosition(new Vector2(playerPosition.X, 1080));
                } else if(playerPosition.Y > 1080 && activeLevel.HasConnectedLevel(Level.Direction.Bottom) 
                && activeLevel.GetConnectedLevel(Level.Direction.Bottom).Unlocked) {
                    player.MoveLevel(activeLevel.GetConnectedLevel(Level.Direction.Bottom).LevelNumber);
                    player.SetPosition(new Vector2(playerPosition.X, 0));
                } else if(playerPosition.X < 0 && activeLevel.HasConnectedLevel(Level.Direction.Left) 
                && activeLevel.GetConnectedLevel(Level.Direction.Left).Unlocked) {
                    player.MoveLevel(activeLevel.GetConnectedLevel(Level.Direction.Left).LevelNumber);
                    player.SetPosition(new Vector2(1920,playerPosition.Y));
                } else if(playerPosition.X > 1920 && activeLevel.HasConnectedLevel(Level.Direction.Right) 
                && activeLevel.GetConnectedLevel(Level.Direction.Right).Unlocked) {
                    player.MoveLevel(activeLevel.GetConnectedLevel(Level.Direction.Right).LevelNumber);
                    player.SetPosition(new Vector2(0,playerPosition.Y));
                }
            }
        }
    }
}
