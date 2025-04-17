using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;

namespace Sprint0
{
    // ProceduralLevel extends Level and builds itself by parsing template CSV files.
    public class ProceduralLevel : Level
    {
        private const int levelRows = 9;
        private const int levelCols = 16;
        private const int templateRows = 3;
        private const int templateCols = 4;

        private List<string[,]> tileTemplates;
        private List<string[,]> entityTemplates;

        public ProceduralLevel(ContentManager content) : base()
        {
            tileTemplates = LoadTemplates(Path.Combine(Globals.projectDirectory, "Data\\ProceduralTileTemplates.csv"), templateRows, templateCols);
            entityTemplates = LoadTemplates(Path.Combine(Globals.projectDirectory, "Data\\ProceduralEntityTemplates.csv"), templateRows, templateCols);

            int blocksVert = levelRows / templateRows;
            int blocksHoriz = levelCols / templateCols;
            Random rand = new Random();

            // Assemble the level grid by choosing a random template for each block.
            for (int blockRow = 0; blockRow < blocksVert; blockRow++)
            {
                for (int blockCol = 0; blockCol < blocksHoriz; blockCol++)
                {
                    // Process tile template.
                    string[,] chosenTileTemplate = tileTemplates[rand.Next(tileTemplates.Count)];
                    for (int r = 0; r < templateRows; r++)
                    {
                        for (int c = 0; c < templateCols; c++)
                        {
                            string tileTypeStr = chosenTileTemplate[r, c];
                            Tile.TileType tileType;
                            if (!Enum.TryParse(tileTypeStr, out tileType))
                            {
                                tileType = Tile.TileType.Grass;
                            }
                            Vector2 pos = new Vector2(blockCol * templateCols + c, blockRow * templateRows + r);
                            this.AddTile(content, tileType, pos);
                        }
                    }
                    // Process entity template.
                    string[,] chosenEntityTemplate = entityTemplates[rand.Next(entityTemplates.Count)];
                    for (int r = 0; r < templateRows; r++)
                    {
                        for (int c = 0; c < templateCols; c++)
                        {
                            string cell = chosenEntityTemplate[r, c];
                            if (!string.IsNullOrWhiteSpace(cell))
                            {
                                string[] parts = cell.Split('_');
                                Vector2 pos = new Vector2(blockCol * templateCols + c, blockRow * templateRows + r);
                                switch (parts[0])
                                {
                                    case "Player":
                                        this.AddPlayer(content, pos);
                                        break;
                                    case "Enemy":
                                        if (parts.Length >= 2 && Enum.TryParse(parts[1], out EntityKeys.MobType mobType))
                                        {
                                            this.AddEnemy(content, mobType, pos);
                                        }
                                        break;
                                    case "Item":
                                        if (parts.Length >= 2 && Enum.TryParse(parts[1], out EntityKeys.ItemType itemType))
                                        {
                                            this.AddItem(content, pos, itemType);
                                        }
                                        break;
                                    case "Block":
                                        if (parts.Length >= 2 && Enum.TryParse(parts[1], out EntityKeys.BlockType blockType))
                                        {
                                            float rotation = 0f;
                                            if (parts.Length >= 3 && float.TryParse(parts[2], out float deg))
                                            {
                                                rotation = MathHelper.ToRadians(deg);
                                            }
                                            this.AddBlock(content, pos, blockType, rotation);
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }
            }

            this.Loaded = true;
            this.InitializePerimeter(content);
        }

        private List<string[,]> LoadTemplates(string filePath, int rows, int cols)
        {
            List<string[,]> templates = new List<string[,]>();
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Template file not found: " + filePath);
                return templates;
            }
            string[] allLines = File.ReadAllLines(filePath);
            List<string> currentTemplateLines = new List<string>();

            foreach (string line in allLines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    if (currentTemplateLines.Count == rows)
                        templates.Add(ParseTemplate(currentTemplateLines, rows, cols));
                    currentTemplateLines.Clear();
                }
                else
                {
                    currentTemplateLines.Add(line);
                    if (currentTemplateLines.Count == rows)
                    {
                        templates.Add(ParseTemplate(currentTemplateLines, rows, cols));
                        currentTemplateLines.Clear();
                    }
                }
            }
            return templates;
        }

        private string[,] ParseTemplate(List<string> lines, int rows, int cols)
        {
            string[,] templateGrid = new string[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                string[] cells = lines[i].Split(',');
                for (int j = 0; j < cols && j < cells.Length; j++)
                {
                    templateGrid[i, j] = cells[j].Trim();
                }
            }
            return templateGrid;
        }
    }
}
