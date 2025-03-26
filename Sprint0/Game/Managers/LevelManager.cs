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
            levels = new Dictionary<string, Level>();
        }
        public void LoadContent(ContentManager content, int levelNum) {
            string entityFilePath = Path.Combine(Globals.projectDirectory, "Data\\Level" + levelNum + "_Entities.csv");
            string tilesFilePath = Path.Combine(Globals.projectDirectory, "Data\\Level" + levelNum + "_Tiles.csv");
            Level newLevel = CSVLevelParser.ParseLevel(entityFilePath, tilesFilePath, content);
            newLevel.LevelNumber = levelNum;
            if(!levels.ContainsKey("Level" + levelNum)) {
                levels.Add("Level" + levelNum, newLevel);
            }
            activeLevel = levels["Level" + levelNum];
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
            if(entities.ContainsKey("player")) {
                Player player = (Player)entities["player"];
                if(activeLevel.Complete && player.GetPosition().X > 1920) {
                    player.SetPosition(new Vector2(0,player.GetPosition().Y));
                    player.MoveLevel(activeLevel.LevelNumber+1);
                } else if(player.GetPosition().X<0) {
                    player.SetPosition(new Vector2(1920,player.GetPosition().Y));
                    player.MoveLevel(activeLevel.LevelNumber-1);
                }
                /* 
                if(player.GetPosition().X > 1920 && activeLevel.HasConnectedLevel(Level.Direction.Right)) {
                    player.SetPosition(new Vector2(0,player.GetPosition().Y));
                    player.MoveLevel(activeLevel.GetLevelNumber()+1); // Placeholder! just increments activeLevel number
                } else if(player.GetPosition().X < 0 && activeLevel.HasConnectedLevel(Level.Direction.Left)) {
                    player.SetPosition(new Vector2(1920,player.GetPosition().Y));
                    player.MoveLevel(activeLevel.GetLevelNumber()+1);
                } else if(player.GetPosition().Y > 1080 && activeLevel.HasConnectedLevel(Level.Direction.Bottom)) {
                    player.SetPosition(new Vector2(player.GetPosition().X, 0));
                    player.MoveLevel(activeLevel.GetLevelNumber()+1);
                } else if(player.GetPosition().Y < 0 && activeLevel.HasConnectedLevel(Level.Direction.Top)) {
                    player.SetPosition(new Vector2(player.GetPosition().X, 1080));
                    player.MoveLevel(activeLevel.GetLevelNumber()+1);
                } */
            }
        }
    }
}
