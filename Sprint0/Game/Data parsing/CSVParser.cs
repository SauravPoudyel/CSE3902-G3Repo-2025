using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Sprint0
{
    public static class CSVParser
    {
        public static List<Dictionary<string, string>> ParseFile(string filePath)
        {
            List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File path does not exist: " + filePath);
                return rows;
            }
            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length == 0)
            {
                Console.WriteLine("Zero lines read: " + filePath);
                return rows;
            }
            string[] headers = lines[0].Split(',');
            int i, j;
            for (i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                    continue;
                string[] columns = lines[i].Split(',');
                if (columns.Length != headers.Length)
                    continue;
                Dictionary<string, string> row = new Dictionary<string, string>();
                for (j = 0; j < headers.Length; j++)
                {
                    row[headers[j].Trim()] = columns[j].Trim();
                }
                rows.Add(row);
            }
            return rows;
        }

        public static PlayerData ParsePlayerData(string filePath)
        {
            if (!File.Exists(filePath)) {
                System.Console.WriteLine("hello");
                return new PlayerData();
            }

            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length < 3)
                return new PlayerData();
            string[] headers = lines[0].Split(',');
            Dictionary<string, string> permanentData = null;
            Dictionary<string, string> temporaryData = null;
            int i, j;
            for (i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                    continue;
                string[] columns = lines[i].Split(',');
                if (columns.Length != headers.Length)
                    continue;
                Dictionary<string, string> row = new Dictionary<string, string>();
                for (j = 0; j < headers.Length; j++)
                {
                    row[headers[j].Trim()] = columns[j].Trim();
                }
                if (row.ContainsKey("Type"))
                {
                    if (row["Type"].Equals("Permanent", StringComparison.OrdinalIgnoreCase))
                        permanentData = row;
                    else if (row["Type"].Equals("Temporary", StringComparison.OrdinalIgnoreCase))
                        temporaryData = row;
                }
            }
            PlayerData data = new PlayerData();
            int tempVal;
            if (permanentData != null)
            {
                if (permanentData.ContainsKey("PermanentHealth") && int.TryParse(permanentData["PermanentHealth"], out tempVal))
                    data.PermanentHealth = tempVal;
                if (permanentData.ContainsKey("PermanentAmmoDefault") && int.TryParse(permanentData["PermanentAmmoDefault"], out tempVal))
                    data.PermanentAmmoDefault = tempVal;
                if (permanentData.ContainsKey("PermanentAmmoShotgun") && int.TryParse(permanentData["PermanentAmmoShotgun"], out tempVal))
                    data.PermanentAmmoShotgun = tempVal;
                if (permanentData.ContainsKey("PermanentAmmoSniper") && int.TryParse(permanentData["PermanentAmmoSniper"], out tempVal))
                    data.PermanentAmmoSniper = tempVal;
                if (permanentData.ContainsKey("PermanentAmmoRocket") && int.TryParse(permanentData["PermanentAmmoRocket"], out tempVal))
                    data.PermanentAmmoRocket = tempVal;
                if (permanentData.ContainsKey("PermanentAmmoLaser") && int.TryParse(permanentData["PermanentAmmoLaser"], out tempVal))
                    data.PermanentAmmoLaser = tempVal;
                if (permanentData.ContainsKey("PermanentAmmoMine") && int.TryParse(permanentData["PermanentAmmoMine"], out tempVal))
                    data.PermanentAmmoMine = tempVal;
                if (permanentData.ContainsKey("PermanentShield") && int.TryParse(permanentData["PermanentShield"], out tempVal))
                    data.PermanentShield = tempVal;
                if (permanentData.ContainsKey("PermanentCoins") && int.TryParse(permanentData["PermanentCoins"], out tempVal))
                    data.PermanentCoins = tempVal;
            }
            if (temporaryData != null)
            {
                if (temporaryData.ContainsKey("TemporaryHealth") && int.TryParse(temporaryData["TemporaryHealth"], out tempVal))
                    data.TemporaryHealth = tempVal;
                if (temporaryData.ContainsKey("TemporaryAmmoDefault") && int.TryParse(temporaryData["TemporaryAmmoDefault"], out tempVal))
                    data.TemporaryAmmoDefault = tempVal;
                if (temporaryData.ContainsKey("TemporaryAmmoShotgun") && int.TryParse(temporaryData["TemporaryAmmoShotgun"], out tempVal))
                    data.TemporaryAmmoShotgun = tempVal;
                if (temporaryData.ContainsKey("TemporaryAmmoSniper") && int.TryParse(temporaryData["TemporaryAmmoSniper"], out tempVal))
                    data.TemporaryAmmoSniper = tempVal;
                if (temporaryData.ContainsKey("TemporaryAmmoRocket") && int.TryParse(temporaryData["TemporaryAmmoRocket"], out tempVal))
                    data.TemporaryAmmoRocket = tempVal;
                if (temporaryData.ContainsKey("TemporaryAmmoLaser") && int.TryParse(temporaryData["TemporaryAmmoLaser"], out tempVal))
                    data.TemporaryAmmoLaser = tempVal;
                if (temporaryData.ContainsKey("TemporaryAmmoMine") && int.TryParse(temporaryData["TemporaryAmmoMine"], out tempVal))
                    data.TemporaryAmmoMine = tempVal;
                if (temporaryData.ContainsKey("TemporaryShield") && int.TryParse(temporaryData["TemporaryShield"], out tempVal))
                    data.TemporaryShield = tempVal;
                if (temporaryData.ContainsKey("TemporaryCoins") && int.TryParse(temporaryData["TemporaryCoins"], out tempVal))
                    data.TemporaryCoins = tempVal;
            }
            return data;
        }

        public static void SavePlayerData(string filePath, PlayerData data)
        {
            string header = "Type,PermanentHealth,PermanentAmmoDefault,PermanentAmmoShotgun,PermanentAmmoSniper,PermanentAmmoRocket,PermanentAmmoLaser,PermanentAmmoMine,PermanentShield,PermanentCoins," +
                            "TemporaryHealth,TemporaryAmmoDefault,TemporaryAmmoShotgun,TemporaryAmmoSniper,TemporaryAmmoRocket,TemporaryAmmoLaser,TemporaryAmmoMine,TemporaryShield,TemporaryCoins";
            string permanentLine = "Permanent," + data.PermanentHealth.ToString() + "," + data.PermanentAmmoDefault.ToString() + "," +
                                     data.PermanentAmmoShotgun.ToString() + "," + data.PermanentAmmoSniper.ToString() + "," +
                                     data.PermanentAmmoRocket.ToString() + "," + data.PermanentAmmoLaser.ToString() + "," +
                                     data.PermanentAmmoMine.ToString() + "," + data.PermanentShield.ToString() + "," + data.PermanentCoins.ToString();
            string temporaryLine = "Temporary," + data.TemporaryHealth.ToString() + "," + data.TemporaryAmmoDefault.ToString() + "," +
                                     data.TemporaryAmmoShotgun.ToString() + "," + data.TemporaryAmmoSniper.ToString() + "," +
                                     data.TemporaryAmmoRocket.ToString() + "," + data.TemporaryAmmoLaser.ToString() + "," +
                                     data.TemporaryAmmoMine.ToString() + "," + data.TemporaryShield.ToString() + "," + data.TemporaryCoins.ToString();
            File.WriteAllText(filePath, header + Environment.NewLine + permanentLine + Environment.NewLine + temporaryLine);
        }

        public static Level ParseLevel(string filePath, ContentManager content)
        {
            Level level = new Level();
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
                        MobType mobType = MobType.SmallEnemy;
                        if (currentRow.ContainsKey("Subtype") && Enum.TryParse(currentRow["Subtype"], out MobType parsedMobType))
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
            return level;
        }
    }
}
