using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0
{
    public class ProceduralHandler
    {
        public bool pendingProceduralLevelLoad { get; set; } = true;
        public float proceduralLoadTimer { get; set; } = 0f;
        public LoadingScreen proceduralLoadingScreen { get; set; } = null;

        public ProceduralLevel GenerateProceduralLevel(ContentManager content)
        {
            // Create generator for a 16x9 grid.
            ProceduralGenerator generator = new ProceduralGenerator(16, 9, maxMobs: 6, maxItems: 4);
            generator.Generate();

            ProceduralLevel procLevel = new ProceduralLevel();

            // Loop over the grid dimensions (using generator.Height and generator.Width)
            for (int r = 0; r < generator.Height; r++)
            {
                for (int c = 0; c < generator.Width; c++)
                {
                    // Add the tile from the generator.
                    procLevel.AddTile(content, generator.TileMatrix[r, c], new Vector2(c, r));

                    // Inside the loop over grid cells in GenerateProceduralLevel:
                    string cell = generator.EntityMatrix[r, c];
                    if (!string.IsNullOrWhiteSpace(cell))
                    {
                        string[] parts = cell.Split('_');
                        switch (parts[0])
                        {
                            case "Block":
                                if (parts.Length >= 2 && Enum.TryParse(parts[1], out EntityKeys.BlockType blockType))
                                {
                                    float rotation = 0f;
                                    if (parts.Length >= 3 && float.TryParse(parts[2], out float deg))
                                        rotation = MathHelper.ToRadians(deg);
                                    procLevel.AddBlock(content, new Vector2(c, r), blockType, rotation);
                                }
                                break;
                            case "Enemy":
                                if (parts.Length >= 2 && Enum.TryParse(parts[1], out EntityKeys.MobType mobType))
                                {
                                    Vector2 gridPos = new Vector2(c, r);
                                    int tileSize = procLevel.tileSize; // Get the tile size from the level.
                                    Vector2 worldPos = (gridPos * tileSize) + new Vector2(tileSize / 2, tileSize / 2);
                                    procLevel.AddEnemy(content, gridPos, mobType);
                                }
                                break;
                            case "Item":
                                // Use PickupItemType (as defined in Item.cs) instead of EntityKeys.ItemType.
                                if (parts.Length >= 2 && Enum.TryParse(parts[1], out EntityKeys.ItemType pickupItemType))
                                {
                                    Vector2 gridPos = new Vector2(c, r);
                                    int tileSize = procLevel.tileSize;
                                    Vector2 worldPos = (gridPos * tileSize) + new Vector2(tileSize / 2, tileSize / 2);
                                    // Note: Level.AddItem() handles conversion from grid to world coordinates.
                                    procLevel.AddItem(content, gridPos, pickupItemType);
                                }
                                break;
                            case "Player":
                                procLevel.AddPlayer(content, new Vector2(c, r));
                                break;
                        }
                    }

                }
            }
            // Mark the level as loaded and procedural (using -1 as its level number).
            procLevel.Loaded = true;
            procLevel.LevelNumber = -1;
            return procLevel;
        }

        public void AddPortal(Level level, ContentManager content, GameManager gameManager)
        {
            int W = 16;
            int H = 9;
            int centerX = W / 2;   // 8
            int centerY = H / 2;   // 4

            // Build a HashSet of occupied grid positions
            var occupied = new HashSet<(int x, int y)>();
            foreach (var kvp in level.Entities)
            {
                var pos = kvp.Value.GetPosition();
                // convert back to grid coords
                int gx = (int)((pos.X - level.tileSize/2) / level.tileSize);
                int gy = (int)((pos.Y - level.tileSize/2) / level.tileSize);
                occupied.Add((gx, gy));
            }

            // Spiral out from center until we find an empty spot
            Vector2? spot = null;
            int maxRadius = Math.Max(centerX, centerY);
            for (int r = 0; r <= maxRadius && spot == null; r++)
            {
                for (int dx = -r; dx <= r && spot == null; dx++)
                {
                    foreach (int dy in new[] { -r, r })
                    {
                        int x = centerX + dx, y = centerY + dy;
                        if (x >= 0 && x < W && y >= 0 && y < H && !occupied.Contains((x, y)))
                        {
                            spot = new Vector2(x, y);
                            break;
                        }
                    }
                }
                for (int dy = -r+1; dy <= r-1 && spot == null; dy++)
                {
                    foreach (int dx in new[] { -r, r })
                    {
                        int x = centerX + dx, y = centerY + dy;
                        if (x >= 0 && x < W && y >= 0 && y < H && !occupied.Contains((x, y)))
                        {
                            spot = new Vector2(x, y);
                            break;
                        }
                    }
                }
            }

            // Fallback
            Vector2 gridPos = spot ?? new Vector2(0, 0);
            float ts = level.tileSize;
            Vector2 worldPos = (gridPos * ts) + new Vector2(ts/2, ts/2);

            // Create and insert the portal
            BaseBlock portal = BlockFactory.CreateBlock(
                EntityKeys.BlockType.ProceduralPortal,
                content,
                worldPos,
                0f
            );
            portal.EntityKey = "procedural_portal";

            level.Entities[portal.EntityKey]      = portal;
            gameManager.entities[portal.EntityKey] = portal;
        }

        

        public void Update(GameManager gameManager)
        {
            if (pendingProceduralLevelLoad)
            {
                proceduralLoadTimer += (float)gameManager.Game.TargetElapsedTime.TotalSeconds;
                // Update GameManager's collections with the new procedural level data.
                gameManager.entities = gameManager.LevelManager.LoadLevelEntities();
                gameManager.tiles = gameManager.LevelManager.LoadLevelTiles();

                // Remove the loading screen.
                gameManager.screenManager.RemoveScreen(proceduralLoadingScreen);
                proceduralLoadingScreen = null;
                pendingProceduralLevelLoad = false;
                proceduralLoadTimer = 0f;
            }
        }
    }
}
