using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;

namespace Sprint0
{
    public class ProceduralGenerator
    {
        public int Width  { get; }
        public int Height { get; }

        public Tile.TileType[,] TileMatrix   { get; }
        public string[,]          EntityMatrix { get; }

        private readonly int maxMobs;
        private readonly int maxItems;

        private static readonly EntityKeys.MobType[]  candidateMobs  =
        {
            EntityKeys.MobType.SmallEnemy, EntityKeys.MobType.Turret, EntityKeys.MobType.Plane,
            EntityKeys.MobType.ShieldTank, EntityKeys.MobType.SwarmingTank, EntityKeys.MobType.HoveringTank,
            EntityKeys.MobType.StealthTank, EntityKeys.MobType.HealerTank
        };

        private static readonly EntityKeys.ItemType[] candidateItems =
        {
            EntityKeys.ItemType.AmmoDefault, EntityKeys.ItemType.SpeedBoost, EntityKeys.ItemType.Cloak,
            EntityKeys.ItemType.FireRateIncrease, EntityKeys.ItemType.MedStrong, EntityKeys.ItemType.MedWeak,
            EntityKeys.ItemType.TimeSlow, EntityKeys.ItemType.Fly, EntityKeys.ItemType.Magnet,
            EntityKeys.ItemType.SpeedBoost, EntityKeys.ItemType.Shield, EntityKeys.ItemType.AmmoDefault,
            EntityKeys.ItemType.AmmoShotgun, EntityKeys.ItemType.AmmoSniper, EntityKeys.ItemType.AmmoRocket
        };

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
            // 1) load templates
            var tileTemplates   = LoadTemplates("Data\\ProceduralTileTemplates.csv",   3, 4);
            var entityTemplates = LoadTemplates("Data\\ProceduralEntityTemplates.csv", 3, 4);

            int blockRows = Height / 3;
            int blockCols = Width  / 4;

            // 2) pick which template goes in each block
            var tilePick   = PickTemplateIndices(blockRows, blockCols, tileTemplates.Count);
            var entityPick = PickTemplateIndices(blockRows, blockCols, entityTemplates.Count);

            // 3) fill the raw matrices
            PopulateTilesAndEntities(tileTemplates, entityTemplates, tilePick, entityPick);

            // 4) sprinkle mobs and items
            PopulateRandomMobs();
            PopulateRandomItems();
        }

        private List<string[,]> LoadTemplates(string relativePath, int rows, int cols)
        {
            string path = Path.Combine(Globals.projectDirectory, relativePath);
            var lines = File.ReadAllLines(path);
            int templateCount = lines.Length / rows;
            var list = new List<string[,]>(templateCount);

            for (int t = 0; t < templateCount; t++)
            {
                var tpl = new string[rows, cols];
                for (int r = 0; r < rows; r++)
                {
                    var tokens = lines[t * rows + r].Split(',');
                    for (int c = 0; c < cols; c++)
                        tpl[r, c] = tokens[c].Trim();
                }
                list.Add(tpl);
            }

            return list;
        }

        private int[,] PickTemplateIndices(int blockRows, int blockCols, int templateCount)
        {
            var picks = new int[blockRows, blockCols];
            for (int br = 0; br < blockRows; br++)
            for (int bc = 0; bc < blockCols; bc++)
                picks[br, bc] = Globals.random.Next(templateCount);
            return picks;
        }

        private void PopulateTilesAndEntities(
            List<string[,]> tileTemplates,
            List<string[,]> entityTemplates,
            int[,] tilePick,
            int[,] entityPick)
        {
            const int tplRows = 3, tplCols = 4;

            for (int r = 0; r < Height; r++)
            for (int c = 0; c < Width; c++)
            {
                int br = r / tplRows, bc = c / tplCols;
                int lr = r % tplRows, lc = c % tplCols;

                // tile
                var tileTpl = tileTemplates[tilePick[br, bc]];
                TileMatrix[r, c] = ParseTileType(tileTpl[lr, lc]);

                // entity key
                var entTpl = entityTemplates[entityPick[br, bc]];
                string key = entTpl[lr, lc]?.Trim();
                EntityMatrix[r, c] = string.IsNullOrWhiteSpace(key) ? "" : key;
            }
        }

        private void PopulateRandomMobs()
        {
            int count = 0;
            for (int r = 0; r < Height && count < maxMobs; r++)
            for (int c = 0; c < Width && count < maxMobs; c++)
            {
                if (string.IsNullOrWhiteSpace(EntityMatrix[r, c]) &&
                    Globals.random.NextDouble() < 0.1)
                {
                    var mob = candidateMobs[Globals.random.Next(candidateMobs.Length)];
                    EntityMatrix[r, c] = $"Enemy_{mob}";
                    count++;
                }
            }
        }

        private void PopulateRandomItems()
        {
            int count = 0;
            for (int r = 0; r < Height && count < maxItems; r++)
            for (int c = 0; c < Width && count < maxItems; c++)
            {
                if (string.IsNullOrWhiteSpace(EntityMatrix[r, c]) &&
                    Globals.random.NextDouble() < 0.05)
                {
                    var item = candidateItems[Globals.random.Next(candidateItems.Length)];
                    EntityMatrix[r, c] = $"Item_{item}";
                    count++;
                }
            }
        }

        private Tile.TileType ParseTileType(string v)
        {
            return v.Equals("Dirt",   StringComparison.OrdinalIgnoreCase) ? Tile.TileType.Dirt   :
                   v.Equals("Forest", StringComparison.OrdinalIgnoreCase) ? Tile.TileType.Forest :
                   v.Equals("Sand",   StringComparison.OrdinalIgnoreCase) ? Tile.TileType.Sand   :
                                                                            Tile.TileType.Grass;
        }
    }
}
