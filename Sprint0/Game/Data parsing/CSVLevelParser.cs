using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Microsoft.Xna.Framework.Content;
using static Sprint0.CSVParser;

namespace Sprint0
{
    public static class CSVLevelParser
    {
        public static Level ParseLevel(string entityFilePath, string tileFilePath, ContentManager content)
        {
            Level level = new Level();
            ParseEntities(entityFilePath, level, content);
            ParseTiles(tileFilePath, level, content);
            return level;
        }
        private static void ParseEntities(string filePath, Level level, ContentManager content) {
            List<Dictionary<string, string>> rows = ParseFile(filePath);
            Console.WriteLine("Parsed " + rows.Count.ToString() + " rows from CSV file: " + filePath);
            int i, j;
            for (i = 0; i < rows.Count; i++)
            {
                Dictionary<string, string> currentRow = rows[i];
                float xPos = 0, yPos = 0;
                if (currentRow.ContainsKey("X") && float.TryParse(currentRow["X"], out float tempX))
                {
                    xPos = tempX;
                }
                if (currentRow.ContainsKey("Y") && float.TryParse(currentRow["Y"], out float tempY))
                {
                    yPos = tempY;
                }
                Vector2 position = new Vector2(xPos, yPos);
                if (!currentRow.ContainsKey("Type"))
                {
                    Console.WriteLine("Skipping row due to missing Type: " + string.Join(",", currentRow.Values));
                    continue;
                }
                string entityType = currentRow["Type"];
                switch (entityType)
                {
                    case "Player":
                        level.AddPlayer(content, position);
                        break;
                    case "Enemy":
                        EntityKeys.MobType mobType = EntityKeys.MobType.SmallEnemy;
                        if (currentRow.ContainsKey("Subtype") && Enum.TryParse(currentRow["Subtype"], out EntityKeys.MobType parsedMobType))
                        {
                            mobType = parsedMobType;
                        }
                        level.AddEnemy(content, mobType, position);
                        break;
                    case "Item":
                        PickupItemType pickupItemType = PickupItemType.SpeedBoost;
                        if (currentRow.ContainsKey("Subtype") && Enum.TryParse(currentRow["Subtype"], out PickupItemType parsedItemType))
                        {
                            pickupItemType = parsedItemType;
                        }
                        level.AddItem(content, position, pickupItemType);
                        break;
                    case "Block":
                        BlockSpriteKey blockType = BlockSpriteKey.Tree;
                        if (currentRow.ContainsKey("Subtype") && Enum.TryParse(currentRow["Subtype"], out BlockSpriteKey parsedBlockType))
                        {
                            blockType = parsedBlockType;
                        }
                        level.AddBlock(content, position, blockType);
                        break;
                    default:
                        Console.WriteLine("Unknown Type '" + entityType + "' at position (" + xPos.ToString() + ", " + yPos.ToString() + "). Skipping row.");
                        break;
                }
            }
        }
        private static string[,] ParseTileFile(string filePath)
        {
            string[,] tileArray = new string[9, 16]; //currently hardcoded size; either use globals or replace with parsed size
            if (!File.Exists(filePath)) {
                Console.WriteLine($"File path does not exist: {filePath}");
                return tileArray; }

            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length == 0) {
                Console.WriteLine($"Zero lines read: {filePath}");
                return tileArray; }

            for(int i=0; i<lines.Length; i++) {
                string[] currentRow = lines[i].Split(',');
                for(int j=0; j<currentRow.Length; j++) {
                    tileArray[i,j] = currentRow[j];
                }
            }
            return tileArray;
        }
        private static void ParseTiles(string filePath, Level level, ContentManager content) {
            string[,] tiles = ParseTileFile(filePath);
            for (int y = 0; y < tiles.GetLength(0); y++)
            {
                for (int x = 0; x < tiles.GetLength(1); x++)
                {
                    Tile.TileType tileType = Tile.TileType.Grass; // default is grass
                    switch(tiles[y,x]) {
                        case "G":
                            tileType = Tile.TileType.Grass;
                            break;
                        case "S":
                            tileType = Tile.TileType.Sand;
                            break;
                        case "C":
                            tileType = Tile.TileType.Concrete;
                            break;
                        case "D":
                            tileType = Tile.TileType.Dirt;
                            break;
                        case "LeftGrassRightSand":
                            tileType = Tile.TileType.LeftGrassRightSand;
                            break;
                        case "LeftGrassRightSandRoad":
                            tileType = Tile.TileType.LeftGrassRightSandRoad;
                            break;
                        case "HorizontalGrassRoad":
                            tileType = Tile.TileType.HorizontalGrassRoad;
                            break;
                        case "HorizontalSandRoad":
                            tileType = Tile.TileType.HorizontalSandRoad;
                            break;
                    }
                    level.AddTile(content, tileType, new Vector2(x, y));
                }
            }
        }
    }
}