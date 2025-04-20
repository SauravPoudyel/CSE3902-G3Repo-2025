using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sprint0
{
    public class LevelManager
    {
        private Level activeLevel;
        private ContentManager content;

        public Level ActiveLevel => activeLevel;

        // === Procedural‑level support ===
        public bool ProcedurallyLoading { get; set; } = false;
        public ProceduralHandler proceduralHandler { get; set; } = new ProceduralHandler();
        private bool portalAdded = false;
        private bool sawEnemies  = false;

        // === Hand‑crafted levels ===
        private Dictionary<string, Level> levels;

        public LevelManager()
        {
            activeLevel = new Level();
            levels      = CSVLevelParser.ParseLevelIndex();
        }

        // Standard CSV level load
        public void LoadContent(ContentManager content, int levelNum)
        {
            this.content = content;
            string lvlName = "Level" + levelNum;

            if (!levels.TryGetValue(lvlName, out Level placeholder))
            {
                placeholder = new Level();
                levels[lvlName] = placeholder;
            }

            if (!placeholder.Loaded)
            {
                string ePath = Path.Combine(Globals.projectDirectory, $"Data\\Level{levelNum}_Entities.csv");
                string tPath = Path.Combine(Globals.projectDirectory, $"Data\\Level{levelNum}_Tiles.csv");
                Level parsed = CSVLevelParser.ParseLevel(ePath, tPath, content);

                // carry over index metadata (key‑item, connections)
                if (placeholder.KeyItem.Exists)
                    parsed.SetKeyItemType(placeholder.KeyItem.Type);

                parsed.ConnectedLevels.SetAll(
                    placeholder.ConnectedLevels
                               .GetAll()
                               .ToDictionary(kv => kv.Key, kv => kv.Value)
                );

                parsed.LevelNumber = levelNum;
                parsed.Loaded      = true;
                parsed.InitializePerimeter(content);

                levels[lvlName] = parsed;
            }

            activeLevel = levels[lvlName];
        }

        // Called by GameManager to pull entities out of the Level
        public Dictionary<string, Entity> LoadLevelEntities()
        {
            var ents = activeLevel.Entities;

            // Preserve the existing player instance if there is one
            var gmEnts = GameManager.Instance.GetEntities();
            if (gmEnts.ContainsKey("player"))
            {
                Player p = (Player)gmEnts["player"];
                p.SetVelocity(Vector2.Zero);
                ents["player"] = p;
            }

            return ents;
        }

        // Called by GameManager to pull tiles out of the Level
        public List<Tile> LoadLevelTiles() => activeLevel.GetLevelTiles;

        // Manual level switch (edge transitions)
        public void SwitchLevel(Level.Direction dir)
        {
            if (activeLevel.HasConnectedLevel(dir))
                activeLevel = activeLevel.GetConnectedLevel(dir);
        }
        
        public void UpdateLevelEntities(Dictionary<string, Entity> entities)
        {
            activeLevel.Entities = entities;
        }

        public void LoadProceduralLevel()
        {
            Level proc = proceduralHandler.GenerateProceduralLevel(content);
            activeLevel = proc;

            activeLevel.LevelNumber = -1;
            activeLevel.Loaded      = true;
            activeLevel.Complete    = false;
            activeLevel.InitializePerimeter(content);
            ProcedurallyLoading = true;

            portalAdded = false;
            sawEnemies  = false;
        }

        public void Update(Dictionary<string, Entity> entities)
        {
            var gm = GameManager.Instance;

            if (proceduralHandler.pendingProceduralLevelLoad)
            {
                proceduralHandler.Update(gm);
                if (!proceduralHandler.pendingProceduralLevelLoad)
                {
                    gm.entities = LoadLevelEntities();
                    gm.tiles    = LoadLevelTiles();
                }
                return;
            }

            UpdateLevelEntities(entities);

            activeLevel.CheckAndMarkCompletion(content);

            if (activeLevel.HasEnemies())
            {
                sawEnemies = true;
            }
            else if (sawEnemies && !portalAdded && ProcedurallyLoading)
            {
                // place exactly one portal
                proceduralHandler.AddPortal(activeLevel, content, gm);
                portalAdded = true;
            }

            if (!ProcedurallyLoading && activeLevel.Complete)
            {
                // (key‑drop already happened in CheckAndMarkCompletion)
                foreach (var lvl in levels.Values)
                {
                    if (lvl.PrereqLevel?.LevelNumber == activeLevel.LevelNumber)
                        lvl.Unlocked = true;

                    foreach (var kv in activeLevel.ConnectedLevels.GetAll())
                    {
                        if (kv.Value == lvl)
                            activeLevel.UnlockConnectedLevel(kv.Key);
                    }
                }
            }

            if (entities.TryGetValue("player", out Entity ent) && ent is Player pl)
                HandlePlayerPortal(pl);
        }

        // Wrap or portal‐transition logic
        private void HandlePlayerPortal(Player player)
        {
            int half = Globals.TILESIZE / 2;
            Vector2 pos = player.GetPosition();

            if (pos.Y < 0 && activeLevel.HasConnectedLevel(Level.Direction.Top))
            {
                player.SetPosition(new Vector2(pos.X, 1080 - half));
                player.MoveLevel(activeLevel.GetConnectedLevel(Level.Direction.Top).LevelNumber);
            }
            else if (pos.Y > 1080 && activeLevel.HasConnectedLevel(Level.Direction.Bottom))
            {
                player.SetPosition(new Vector2(pos.X, half));
                player.MoveLevel(activeLevel.GetConnectedLevel(Level.Direction.Bottom).LevelNumber);
            }
            else if (pos.X < 0 && activeLevel.HasConnectedLevel(Level.Direction.Left))
            {
                player.SetPosition(new Vector2(1920 - half, pos.Y));
                player.MoveLevel(activeLevel.GetConnectedLevel(Level.Direction.Left).LevelNumber);
            }
            else if (pos.X > 1920 && activeLevel.HasConnectedLevel(Level.Direction.Right))
            {
                player.SetPosition(new Vector2(half, pos.Y));
                player.MoveLevel(activeLevel.GetConnectedLevel(Level.Direction.Right).LevelNumber);
            }
        }
    }
}
