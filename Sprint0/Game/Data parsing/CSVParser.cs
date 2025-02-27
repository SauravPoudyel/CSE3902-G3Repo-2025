using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Microsoft.Xna.Framework.Content;

namespace Sprint0
{
    public static class CSVParser
    {
        public static List<Dictionary<string, string>> ParseFile(string filePath)
        {
            List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();
            if (!File.Exists(filePath)) {
                Console.WriteLine($"File path does not exist: {filePath}");
                return rows; }

            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length == 0) {
                Console.WriteLine($"Zero lines read: {filePath}");
                return rows; }

            string[] headers = lines[0].Split(',');

            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                    continue;

                string[] columns = lines[i].Split(',');
                if (columns.Length != headers.Length)
                    continue;

                Dictionary<string, string> row = new Dictionary<string, string>();
                for (int j = 0; j < headers.Length; j++)
                    row[headers[j].Trim()] = columns[j].Trim();
                rows.Add(row);
            }
            return rows;
        }

        public static PlayerData ParsePlayerData(string filePath)
        {
            var rows = ParseFile(filePath);
            if (rows.Count > 0)
            {
                var row = rows[0];
                PlayerData data = new PlayerData();
                int temp;
                if (row.ContainsKey("PermanentHealth") && int.TryParse(row["PermanentHealth"], out temp))
                    data.PermanentHealth = temp;
                if (row.ContainsKey("PermanentAmmo") && int.TryParse(row["PermanentAmmo"], out temp))
                    data.PermanentAmmo = temp;
                if (row.ContainsKey("PermanentShield") && int.TryParse(row["PermanentShield"], out temp))
                    data.PermanentShield = temp;
                if (row.ContainsKey("PermanentCoins") && int.TryParse(row["PermanentCoins"], out temp))
                    data.PermanentCoins = temp;

                if (row.ContainsKey("TemporaryHealth") && int.TryParse(row["TemporaryHealth"], out temp))
                    data.TemporaryHealth = temp;
                if (row.ContainsKey("TemporaryAmmo") && int.TryParse(row["TemporaryAmmo"], out temp))
                    data.TemporaryAmmo = temp;
                if (row.ContainsKey("TemporaryShield") && int.TryParse(row["TemporaryShield"], out temp))
                    data.TemporaryShield = temp;
                if (row.ContainsKey("TemporaryCoins") && int.TryParse(row["TemporaryCoins"], out temp))
                    data.TemporaryCoins = temp;

                return data;
            }
            return new PlayerData();
        }

        public static void SavePlayerData(string filePath, PlayerData data)
        {
            string header = "PermanentHealth,PermanentAmmo,PermanentShield,PermanentCoins,TemporaryHealth,TemporaryAmmo,TemporaryShield,TemporaryCoins";
            string line = $"{data.PermanentHealth},{data.PermanentAmmo},{data.PermanentShield},{data.PermanentCoins}," +
                          $"{data.TemporaryHealth},{data.TemporaryAmmo},{data.TemporaryShield},{data.TemporaryCoins}";
            File.WriteAllText(filePath, header + Environment.NewLine + line);
        }

        public static Level ParseLevel(string filePath, ContentManager content)
        {
            Level level = new Level();
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
            return level;
        }
    }
}
