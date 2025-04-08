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
        private ContentManager content;
        public Level ActiveLevel
        {
            get { return activeLevel; }
        }
        private Dictionary<string, Level> levels;
        public LevelManager() {
            activeLevel = new Level();
            levels = CSVLevelParser.ParseLevelIndex();
        }
        public void LoadContent(ContentManager content, int levelNum) {
            this.content = content; 
            string levelName = "Level" + levelNum;

            if (!levels.TryGetValue(levelName, out Level value)) {
                value = new Level();
                levels.Add(levelName, value);
            }

            if (!value.Loaded) {
                string entityFilePath = Path.Combine(Globals.projectDirectory, "Data\\Level" + levelNum + "_Entities.csv");
                string tilesFilePath = Path.Combine(Globals.projectDirectory, "Data\\Level" + levelNum + "_Tiles.csv");
                Level loadedLevel = CSVLevelParser.ParseLevel(entityFilePath, tilesFilePath, content);

                Level indexLevel = value;
                if (indexLevel.HasKeyItem) {
                    loadedLevel.SetKeyItemType(indexLevel.KeyItemType);
                }

                loadedLevel.ConnectedLevels = indexLevel.ConnectedLevels; // still needed
                levels[levelName] = loadedLevel;
                levels[levelName].LevelNumber = levelNum;
                levels[levelName].Loaded = true;

                loadedLevel.InitializePerimeter(content);
            }

            activeLevel = levels[levelName];
        }
        public Dictionary<string, Entity> LoadLevelEntities()
        {
            var loadedEntities = activeLevel.Entities;

            if (GameManager.Instance.GetEntities().ContainsKey("player"))
            {
                Player persistentPlayer = (Player)GameManager.Instance.GetEntities()["player"];
                persistentPlayer.SetVelocity(Vector2.Zero); // Also stop any movement.
                loadedEntities["player"] = persistentPlayer;
            }

            return loadedEntities;
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
            if(!activeLevel.Complete && activeLevel.Loaded && !activeLevel.HasEnemies())
            {
                activeLevel.Complete = true;
                // Drop the key item if applicable.
                activeLevel.DropKeyItem(content);
                foreach(Level level in levels.Values) {
                    if(level.PrereqLevel != null && level.PrereqLevel.LevelNumber == activeLevel.LevelNumber)
                        level.Unlocked = true;
                    if(activeLevel.ConnectedLevels.Values.Contains(level)) {
                        var direction = activeLevel.ConnectedLevels.FirstOrDefault(x => x.Value == level).Key;
                        activeLevel.UnlockConnectedLevel(direction);
                    }
                }
            }
            // Level moving logic
            if (entities.TryGetValue("player", out Entity value))
            {
                Player player = (Player)value;
                int halfTile = activeLevel.tileSize / 2; // 60 if tileSize is 120

                if (player.GetPosition().Y < 0 && activeLevel.HasConnectedLevel(Level.Direction.Top))
                {
                    player.SetPosition(new Vector2(player.GetPosition().X, 1080 - halfTile));
                    player.MoveLevel(activeLevel.GetConnectedLevel(Level.Direction.Top).LevelNumber);
                }
                else if (player.GetPosition().Y > 1080 && activeLevel.HasConnectedLevel(Level.Direction.Bottom))
                {
                    player.SetPosition(new Vector2(player.GetPosition().X, halfTile));
                    player.MoveLevel(activeLevel.GetConnectedLevel(Level.Direction.Bottom).LevelNumber);
                }
                else if (player.GetPosition().X < 0 && activeLevel.HasConnectedLevel(Level.Direction.Left))
                {
                    player.SetPosition(new Vector2(1920 - halfTile, player.GetPosition().Y));
                    player.MoveLevel(activeLevel.GetConnectedLevel(Level.Direction.Left).LevelNumber);
                }
                else if (player.GetPosition().X > 1920 && activeLevel.HasConnectedLevel(Level.Direction.Right))
                {
                    player.SetPosition(new Vector2(halfTile, player.GetPosition().Y));
                    player.MoveLevel(activeLevel.GetConnectedLevel(Level.Direction.Right).LevelNumber);
                }
            }

        }
    }
}
