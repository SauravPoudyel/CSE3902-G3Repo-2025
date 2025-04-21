using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;

namespace Sprint0
{
    public class ProceduralGenerator
    {
        public int Width  { get; private set; }
        public int Height { get; private set; }

        public Tile.TileType[,] TileMatrix   { get; private set; }
        public string[,] EntityMatrix { get; private set; }

        private int maxMobs;
        private int maxItems;

        private EntityKeys.MobType[]  candidateMobs  = {EntityKeys.MobType.SmallEnemy,  EntityKeys.MobType.Turret,  EntityKeys.MobType.Plane,  
                                                        EntityKeys.MobType.ShieldTank,  EntityKeys.MobType.SwarmingTank,  EntityKeys.MobType.HoveringTank,  EntityKeys.MobType.StealthTank, 
                                                        EntityKeys.MobType.HealerTank};
        private EntityKeys.ItemType[] candidateItems = { EntityKeys.ItemType.AmmoDefault, EntityKeys.ItemType.SpeedBoost, EntityKeys.ItemType.Cloak, EntityKeys.ItemType.FireRateIncrease, 
                                                        EntityKeys.ItemType.MedStrong, EntityKeys.ItemType.MedWeak, EntityKeys.ItemType.TimeSlow, EntityKeys.ItemType.Fly, 
                                                        EntityKeys.ItemType.Magnet, EntityKeys.ItemType.SpeedBoost, EntityKeys.ItemType.Shield, EntityKeys.ItemType.AmmoDefault, 
                                                        EntityKeys.ItemType.AmmoShotgun, EntityKeys.ItemType.AmmoSniper, EntityKeys.ItemType.AmmoRocket };

        public ProceduralGenerator(int width, int height, int maxMobs = 6, int maxItems = 4)
        {
            Width  = width;
            Height = height;
            this.maxMobs  = maxMobs;
            this.maxItems = maxItems;
            TileMatrix   = new Tile.TileType[Height, Width];
            EntityMatrix = new string[Height, Width];
        }

        public void Generate()
        {
            // Paths and template dims
            string tileCsv   = Path.Combine(Globals.projectDirectory, "Data\\ProceduralTileTemplates.csv");
            string entityCsv = Path.Combine(Globals.projectDirectory, "Data\\ProceduralEntityTemplates.csv");
            int tplRows = 3, tplCols = 4;

            // Load *all* 3×4 templates from each CSV
            var tileTemplates   = LoadTemplates(tileCsv,   tplRows, tplCols);
            var entityTemplates = LoadTemplates(entityCsv, tplRows, tplCols);

            int tileTplCount   = tileTemplates.Count;
            int entityTplCount = entityTemplates.Count;

            int blockRows = Height / tplRows;   // e.g. 9/3 = 3
            int blockCols = Width  / tplCols;   // e.g. 16/4 = 4

            // Pre-pick a Globals.random.m template for each block
            int[,] tilePick   = new int[blockRows, blockCols];
            int[,] entityPick = new int[blockRows, blockCols];
            for (int br = 0; br < blockRows; br++)
            for (int bc = 0; bc < blockCols; bc++)
            {
                tilePick[br,bc] = Globals.random.Next(tileTplCount);
                entityPick[br,bc] = Globals.random.Next(entityTplCount);
            }

            // blocks
            for (int r = 0; r < Height; r++)
            {
                for (int c = 0; c < Width; c++)
                {
                    int br = r / tplRows, bc = c / tplCols;
                    int lr = r % tplRows, lc = c % tplCols;

                    // Tile
                    var chosenTileTpl = tileTemplates[tilePick[br,bc]];
                    TileMatrix[r, c]  = ParseTileType(chosenTileTpl[lr,lc]);

                    // Block (or empty)
                    var chosenEntTpl  = entityTemplates[entityPick[br,bc]];
                    string cell       = chosenEntTpl[lr,lc]?.Trim();
                    EntityMatrix[r,c] = string.IsNullOrWhiteSpace(cell) 
                                        ? "" 
                                        : cell;
                }
            }

            // mobs
            int mobCount = 0;
            for (int r = 0; r < Height; r++) {
                for (int c = 0; c < Width; c++) {
                    if (string.IsNullOrWhiteSpace(EntityMatrix[r,c]) 
                        && mobCount < maxMobs 
                        && Globals.random.NextDouble() < 0.1)
                    {
                        var m = candidateMobs[Globals.random.Next(candidateMobs.Length)];
                        EntityMatrix[r,c] = $"Enemy_{m}";
                        mobCount++;
                    }
                }
            }

            // items
            int itemCount = 0;
            for (int r = 0; r < Height; r++) {
                for (int c = 0; c < Width; c++) {
                    if (string.IsNullOrWhiteSpace(EntityMatrix[r,c]) 
                        && itemCount < maxItems 
                        && Globals.random.NextDouble() < 0.05)
                    {
                        var it = candidateItems[Globals.random.Next(candidateItems.Length)];
                        EntityMatrix[r,c] = $"Item_{it}";
                        itemCount++;
                    }
                }
            }
        }

        // Reads the entire CSV (rows x cols), slices it into multiple 3×4 templates.
        private List<string[,]> LoadTemplates(string path, int rows, int cols)
        {
            var lines = File.ReadAllLines(path);
            int totalRows = lines.Length;
            int tplCount  = totalRows / rows;
            var list = new List<string[,]>();

            for (int t = 0; t < tplCount; t++)
            {
                var tpl = new string[rows, cols];
                for (int r = 0; r < rows; r++)
                {
                    var tokens = lines[t*rows + r].Split(',');
                    for (int c = 0; c < cols; c++)
                        tpl[r, c] = tokens[c].Trim();
                }
                list.Add(tpl);
            }
            return list;
        }

        private Tile.TileType ParseTileType(string v)
        {
            if (v.Equals("Grass",  StringComparison.OrdinalIgnoreCase)) return Tile.TileType.Grass;
            if (v.Equals("Dirt",   StringComparison.OrdinalIgnoreCase)) return Tile.TileType.Dirt;
            if (v.Equals("Forest", StringComparison.OrdinalIgnoreCase)) return Tile.TileType.Forest;
            if (v.Equals("Sand",   StringComparison.OrdinalIgnoreCase)) return Tile.TileType.Sand;
            return Tile.TileType.Grass;
        }
    }
}
