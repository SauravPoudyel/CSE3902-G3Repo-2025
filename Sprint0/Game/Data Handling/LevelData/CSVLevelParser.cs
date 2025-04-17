using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

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

        public static Dictionary<string, Level> ParseLevelIndex(){
            Dictionary<string, Level> levelDict = new Dictionary<string, Level>();
            string indexPath = Path.Combine(Globals.projectDirectory, "Data\\LevelIndex.csv");
            string[,] grid = ParseEntityGridFile(indexPath);
            if (!File.Exists(indexPath))
                Console.WriteLine($"File path does not exist: {indexPath}");
            string[] lines = File.ReadAllLines(indexPath);
            if (lines.Length == 0)
                Console.WriteLine($"Zero lines read: {indexPath}");
            // First pass: add all levels (skip header)
            for (int i = 1; i < lines.Length; i++) {
                string[] currentRow = lines[i].Split(',');
                string levelName = currentRow[0];
                if(!levelDict.ContainsKey(levelName)) {
                    levelDict.Add(levelName, new Level());
                    levelDict[levelName].LevelNumber = int.Parse(currentRow[1]);
                }
            }
            // Second pass: connect levels, set prerequisites, and set key item info.
            // Note: KeyItem info is in column 8 (index 7).
            for (int i = 1; i < lines.Length; i++)
            {
                string[] currentRow = lines[i].Split(',');
                string levelName = currentRow[0];
                if(levelDict.ContainsKey(levelName))
                {
                    for(int j=0; j<4; j++){
                        string connectedLevelName = currentRow[j+2];
                        if(levelDict.ContainsKey(connectedLevelName)) {
                            levelDict[levelName].AddConnectedLevel((Level.Direction)j, levelDict[connectedLevelName]);
                        }
                    }
                    string preReqLevelName = currentRow[6];
                    if(levelDict.ContainsKey(preReqLevelName))
                    {
                        levelDict[levelName].Unlocked = false;
                        levelDict[levelName].PrereqLevel = levelDict[preReqLevelName];
                    }
                    // Check for KeyItem info in column 8.
                    if (currentRow.Length > 7 && !string.IsNullOrWhiteSpace(currentRow[7]))
                    {
                        if (Enum.TryParse(currentRow[7], out EntityKeys.ItemType keyItemType))
                        {
                            levelDict[levelName].SetKeyItemType(keyItemType);
                        }
                        else
                        {
                            Console.WriteLine("Invalid KeyItem type in LevelIndex: " + currentRow[7]);
                        }
                    }
                }
            }
            return levelDict;
        }

        private static void ParseEntities(string filePath, Level level, ContentManager content)
        {
            string[,] grid = ParseEntityGridFile(filePath);
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);
            Console.WriteLine("Parsed " + (rows * cols) + " cells from CSV file: " + filePath);

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    string cell = grid[y, x];
                    if (string.IsNullOrWhiteSpace(cell))
                        continue;

                    // Each cell is structured as: ParentType_Subtype (or for blocks: Block_Subtype_Rotation)
                    string[] parts = cell.Split('_');
                    if (parts.Length == 0)
                        continue;
                    string parentType = parts[0];

                    // Calculate the position based on grid coordinates.
                    // Level.Add* methods already adjust for tile size.
                    Vector2 position = new Vector2(x, y);

                    switch (parentType)
                    {
                        case "Player":
                            level.AddPlayer(content, position);
                            break;

                        case "Enemy":
                            if (parts.Length >= 2 && Enum.TryParse(parts[1], out EntityKeys.MobType mobType))
                            {
                                level.AddEnemy(content, mobType, position);
                            }
                            else
                            {
                                Console.WriteLine("Unknown Enemy subtype in cell: " + cell);
                            }
                            break;

                        case "Item":
                            if (parts.Length >= 2 && Enum.TryParse(parts[1], out EntityKeys.ItemType itemType))
                            {
                                level.AddItem(content, position, itemType);
                            }
                            else
                            {
                                Console.WriteLine("Unknown Item subtype in cell: " + cell);
                            }
                            break;

                        case "Block":
                            if (parts.Length >= 2)
                            {
                                if (Enum.TryParse(parts[1], out EntityKeys.BlockType blockType))
                                {
                                    float rotation = 0f;
                                    if (parts.Length >= 3)
                                    {
                                        if (float.TryParse(parts[2], out float deg))
                                        {
                                            rotation = MathHelper.ToRadians(deg);
                                        }
                                        else
                                        {
                                            Console.WriteLine("Invalid rotation value in cell: " + cell);
                                        }
                                    }
                                    level.AddBlock(content, position, blockType, rotation);
                                }
                                else
                                {
                                    Console.WriteLine("Unknown Block subtype in cell: " + cell);
                                }
                            }
                            break;

                        // New case: register a KeyItem if found in the entities CSV.
                        case "KeyItem":
                            if (parts.Length >= 2 && Enum.TryParse(parts[1], out EntityKeys.ItemType keyType))
                            {
                                level.SetKeyItemType(keyType);
                            }
                            else
                            {
                                Console.WriteLine("Unknown KeyItem subtype in cell: " + cell);
                            }
                            break;

                        default:
                            Console.WriteLine("Unknown entity type in cell: " + cell);
                            break;
                    }
                }
            }
        }

        private static string[,] ParseEntityGridFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File path does not exist: {filePath}");
                return new string[0, 0];
            }

            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length == 0)
            {
                Console.WriteLine($"Zero lines read: {filePath}");
                return new string[0, 0];
            }

            int rows = lines.Length;
            int cols = lines[0].Split(',').Length;
            string[,] grid = new string[rows, cols];

            for (int y = 0; y < rows; y++)
            {
                string[] currentRow = lines[y].Split(',');
                for (int x = 0; x < cols; x++)
                {
                    grid[y, x] = x < currentRow.Length ? currentRow[x].Trim() : "";
                }
            }
            return grid;
        }

        private static string[,] ParseTileFile(string filePath)
        {
            string[,] tileArray = new string[9, 16];
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
