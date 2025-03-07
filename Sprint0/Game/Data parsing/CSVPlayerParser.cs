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
            if (!File.Exists(filePath))
                return new PlayerData();

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
                    row[headers[j].Trim()] = columns[j].Trim();
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
                // (existing fields...)
                if (permanentData.ContainsKey("PermanentAmmoMine") && int.TryParse(permanentData["PermanentAmmoMine"], out tempVal))
                    data.PermanentAmmoMine = tempVal;
                if (permanentData.ContainsKey("PermanentAmmoTeleporter") && int.TryParse(permanentData["PermanentAmmoTeleporter"], out tempVal))
                    data.PermanentAmmoTeleporter = tempVal;
                if (permanentData.ContainsKey("PermanentShield") && int.TryParse(permanentData["PermanentShield"], out tempVal))
                    data.PermanentShield = tempVal;
                if (permanentData.ContainsKey("PermanentCoins") && int.TryParse(permanentData["PermanentCoins"], out tempVal))
                    data.PermanentCoins = tempVal;
            }
            if (temporaryData != null)
            {
                // (existing fields...)
                if (temporaryData.ContainsKey("TemporaryAmmoMine") && int.TryParse(temporaryData["TemporaryAmmoMine"], out tempVal))
                    data.TemporaryAmmoMine = tempVal;
                if (temporaryData.ContainsKey("TemporaryAmmoTeleporter") && int.TryParse(temporaryData["TemporaryAmmoTeleporter"], out tempVal))
                    data.TemporaryAmmoTeleporter = tempVal;
                if (temporaryData.ContainsKey("TemporaryShield") && int.TryParse(temporaryData["TemporaryShield"], out tempVal))
                    data.TemporaryShield = tempVal;
                if (temporaryData.ContainsKey("TemporaryCoins") && int.TryParse(temporaryData["TemporaryCoins"], out tempVal))
                    data.TemporaryCoins = tempVal;
            }
            return data;
        }

        public static void SavePlayerData(string filePath, PlayerData data)
        {
            string header = "Type,PermanentHealth,PermanentAmmoDefault,PermanentAmmoShotgun,PermanentAmmoSniper,PermanentAmmoRocket,PermanentAmmoLaser,PermanentAmmoMine,PermanentAmmoTeleporter,PermanentShield,PermanentCoins," +
                            "TemporaryHealth,TemporaryAmmoDefault,TemporaryAmmoShotgun,TemporaryAmmoSniper,TemporaryAmmoRocket,TemporaryAmmoLaser,TemporaryAmmoMine,TemporaryAmmoTeleporter,TemporaryShield,TemporaryCoins";
            string permanentLine = "Permanent," + data.PermanentHealth + "," + data.PermanentAmmoDefault + "," +
                                     data.PermanentAmmoShotgun + "," + data.PermanentAmmoSniper + "," +
                                     data.PermanentAmmoRocket + "," + data.PermanentAmmoLaser + "," +
                                     data.PermanentAmmoMine + "," + data.PermanentAmmoTeleporter + "," +
                                     data.PermanentShield + "," + data.PermanentCoins;
            string temporaryLine = "Temporary," + data.TemporaryHealth + "," + data.TemporaryAmmoDefault + "," +
                                     data.TemporaryAmmoShotgun + "," + data.TemporaryAmmoSniper + "," +
                                     data.TemporaryAmmoRocket + "," + data.TemporaryAmmoLaser + "," +
                                     data.TemporaryAmmoMine + "," + data.TemporaryAmmoTeleporter + "," +
                                     data.TemporaryShield + "," + data.TemporaryCoins;
            File.WriteAllText(filePath, header + Environment.NewLine + permanentLine + Environment.NewLine + temporaryLine);
        }
    }
}