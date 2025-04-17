using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0
{
    public class ProceduralHandler
    {
        public bool pendingProceduralLevelLoad { get; set; } = false;
        public float proceduralLoadTimer { get; set; } = 0f;
        public LoadingScreen proceduralLoadingScreen { get; set; } = null;

        // Generate the procedural level using the ProceduralGenerator.
        public Level GenerateProceduralLevel(ContentManager content)
        {
            // Create generator for a 16x9 grid.
            ProceduralGenerator generator = new ProceduralGenerator(16, 9, maxMobs: 6, maxItems: 4);
            generator.Generate();

            Level procLevel = new Level();

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
                                    Console.WriteLine($"Adding block {blockType} at grid ({c},{r}) with rotation {rotation}");
                                    procLevel.AddBlock(content, new Vector2(c, r), blockType, rotation);
                                }
                                break;
                            case "Enemy":
                                if (parts.Length >= 2 && Enum.TryParse(parts[1], out EntityKeys.MobType mobType))
                                {
                                    Vector2 gridPos = new Vector2(c, r);
                                    int tileSize = procLevel.tileSize; // Get the tile size from the level.
                                    Vector2 worldPos = (gridPos * tileSize) + new Vector2(tileSize / 2, tileSize / 2);
                                    Console.WriteLine($"Adding enemy {mobType} at grid ({c},{r}), world pos: {worldPos}");
                                    procLevel.AddEnemy(content, mobType, gridPos);
                                }
                                break;
                            case "Item":
                                // Use PickupItemType (as defined in Item.cs) instead of EntityKeys.ItemType.
                                if (parts.Length >= 2 && Enum.TryParse(parts[1], out EntityKeys.ItemType pickupItemType))
                                {
                                    Vector2 gridPos = new Vector2(c, r);
                                    int tileSize = procLevel.tileSize;
                                    Vector2 worldPos = (gridPos * tileSize) + new Vector2(tileSize / 2, tileSize / 2);
                                    Console.WriteLine($"Adding item {pickupItemType} at grid ({c},{r}), world pos: {worldPos}");
                                    // Note: Level.AddItem() handles conversion from grid to world coordinates.
                                    procLevel.AddItem(content, gridPos, pickupItemType);
                                }
                                break;
                            case "Player":
                                Console.WriteLine($"Adding player at grid ({c},{r})");
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

        // During the update cycle, once the procedural level is loaded,
        // update the game manager's entities and tiles.
        public void Update(GameManager gameManager)
        {
            if (pendingProceduralLevelLoad)
            {
                proceduralLoadTimer += (float)gameManager.Game.TargetElapsedTime.TotalSeconds;
                if (proceduralLoadTimer >= 2.0f && gameManager.LevelManager.ActiveLevel.Loaded)
                {
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
}
