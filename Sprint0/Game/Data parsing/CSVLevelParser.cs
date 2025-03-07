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

        private static void ParseEntities(string filePath, Level level, ContentManager content)
        {
            List<Dictionary<string, string>> rows = ParseFile(filePath);
            Console.WriteLine("Parsed " + rows.Count + " rows from CSV file: " + filePath);

            for (int i = 0; i < rows.Count; i++)
            {
                Dictionary<string, string> currentRow = rows[i];

                if (!currentRow.ContainsKey("Type"))
                {
                    Console.WriteLine("Skipping row due to missing Type: " + string.Join(",", currentRow.Values));
                    continue;
                }

                float xPos = currentRow.ContainsKey("X") && float.TryParse(currentRow["X"], out float tempX) ? tempX : 0;
                float yPos = currentRow.ContainsKey("Y") && float.TryParse(currentRow["Y"], out float tempY) ? tempY : 0;
                Vector2 position = new Vector2(xPos, yPos);

                string entityType = currentRow["Type"];
                switch (entityType)
                {
                    case "Player":
                        level.AddPlayer(content, position);
                        break;

                    case "Enemy":
                        if (currentRow.ContainsKey("Subtype") && Enum.TryParse(currentRow["Subtype"], out EntityKeys.MobType mobType))
                        {
                            level.AddEnemy(content, mobType, position);
                        }
                        else
                        {
                            Console.WriteLine($"Unknown Enemy Subtype at position ({xPos}, {yPos}). Skipping row.");
                        }
                        break;

                    case "Item":
                        if (currentRow.ContainsKey("Subtype") && Enum.TryParse(currentRow["Subtype"], out EntityKeys.ItemType itemType))
                        {
                            level.AddItem(content, position, itemType);
                        }
                        else
                        {
                            Console.WriteLine($"Unknown Item Subtype at position ({xPos}, {yPos}). Skipping row.");
                        }
                        break;

                    case "Block":
                        if (currentRow.ContainsKey("Subtype") && Enum.TryParse(currentRow["Subtype"], out EntityKeys.BlockType blockType))
                        {
                            level.AddBlock(content, position, blockType);
                        }
                        else
                        {
                            Console.WriteLine($"Unknown Block Subtype at position ({xPos}, {yPos}). Skipping row.");
                        }
                        break;

                    default:
                        Console.WriteLine($"Unknown Type '{entityType}' at position ({xPos}, {yPos}). Skipping row.");
                        break;
                }
            }
        }

        private static string[,] ParseTileFile(string filePath)
        {
            string[,] tileArray = new string[9, 16]; // hardcoded; adjust as needed
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File path does not exist: {filePath}");
                return tileArray;
            }

            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length == 0)
            {
                Console.WriteLine($"Zero lines read: {filePath}");
                return tileArray;
            }

            for (int i = 0; i < lines.Length; i++)
            {
                string[] currentRow = lines[i].Split(',');
                for (int j = 0; j < currentRow.Length; j++)
                {
                    tileArray[i, j] = currentRow[j];
                }
            }
            return tileArray;
        }

        private static void ParseTiles(string filePath, Level level, ContentManager content)
        {
            string[,] tiles = ParseTileFile(filePath);
            for (int y = 0; y < tiles.GetLength(0); y++)
            {
                for (int x = 0; x < tiles.GetLength(1); x++)
                {
                    Tile.TileType tileType = Tile.TileType.Grass; // default
                    Enum.TryParse(tiles[y, x], out tileType);
                    level.AddTile(content, tileType, new Vector2(x, y));
                }
            }
        }
    }
}
