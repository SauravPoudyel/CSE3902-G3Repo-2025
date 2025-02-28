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
            var rows = ParseFile(filePath);
            Console.WriteLine($"Parsed {rows.Count} rows from CSV file: {filePath}");
            for(int i=0; i<rows.Count; i++)
            {
                var currentRow = rows[i];
                // retrieve and store current entity's x and y positions
                float xPos = 0, yPos = 0; // default position if left blank in csv file
                if (currentRow.ContainsKey("X") && float.TryParse(currentRow["X"], out xPos)) {}
                if (currentRow.ContainsKey("Y") && float.TryParse(currentRow["Y"], out yPos)) {}
                Vector2 position = new Vector2(xPos, yPos); 
                // retrieve current entity's type
                string temp;
                if(currentRow.ContainsKey("Type")){
                    switch(currentRow["Type"]) 
                    {
                    case "Player":
                        level.AddPlayer(content, position);
                        break;
                    case "Enemy":
                        MobType mobType = MobType.SmallEnemy; // default case if blank
                        if(currentRow.ContainsKey("Subtype")) {
                            switch(currentRow["Subtype"]) 
                            {
                                case "BossTank":
                                    mobType = MobType.BossTank;
                                    break;
                                case "SmallEnemy":
                                    mobType = MobType.SmallEnemy;
                                    break;
                                case "ExplodingTank":
                                    mobType = MobType.ExplodingTank;
                                    break;
                                case "Turret":
                                    mobType = MobType.Turret;
                                    break;
                                case "TurningTank":
                                    mobType = MobType.TurningTank;
                                    break;
                                case "Plane":
                                    mobType = MobType.Plane;
                                    break;
                            }
                        }
                        level.AddEnemy(content, mobType, position);
                        break;
                    case "Item":
                        level.AddItem(content, position);
                        break;
                    case "Block":
                        level.AddBlock(content, position);
                        break;
                    default:
                        break;
                    }
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
                    }
                    level.AddTile(content, tileType, new Vector2(x, y));
                }
            }
        }
    }
}