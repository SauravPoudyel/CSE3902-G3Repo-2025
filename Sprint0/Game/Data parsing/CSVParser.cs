using System;
using System.Collections.Generic;
using System.IO;

namespace Sprint0
{
    public static class CSVParser
    {
        // Reads a CSV file expecting a header row and then two data rows (one for Permanent, one for Temporary)
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

            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                    continue;

                string[] columns = lines[i].Split(',');
                if (columns.Length != headers.Length)
                    continue;

                Dictionary<string, string> row = new Dictionary<string, string>();
                for (int j = 0; j < headers.Length; j++)
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
            // Build CSV with two rows (Permanent and Temporary)
            string header = "Type,PermanentHealth,PermanentAmmoDefault,PermanentAmmoShotgun,PermanentAmmoSniper,PermanentAmmoRocket,PermanentAmmoLaser,PermanentAmmoMine,PermanentShield,PermanentCoins," +
                            "TemporaryHealth,TemporaryAmmoDefault,TemporaryAmmoShotgun,TemporaryAmmoSniper,TemporaryAmmoRocket,TemporaryAmmoLaser,TemporaryAmmoMine,TemporaryShield,TemporaryCoins";
            string permanentLine = $"Permanent,{data.PermanentHealth},{data.PermanentAmmoDefault},{data.PermanentAmmoShotgun},{data.PermanentAmmoSniper},{data.PermanentAmmoRocket},{data.PermanentAmmoLaser},{data.PermanentAmmoMine},{data.PermanentShield},{data.PermanentCoins}";
            string temporaryLine = $"Temporary,{data.TemporaryHealth},{data.TemporaryAmmoDefault},{data.TemporaryAmmoShotgun},{data.TemporaryAmmoSniper},{data.TemporaryAmmoRocket},{data.TemporaryAmmoLaser},{data.TemporaryAmmoMine},{data.TemporaryShield},{data.TemporaryCoins}";
            File.WriteAllText(filePath, header + Environment.NewLine + permanentLine + Environment.NewLine + temporaryLine);
        }
    }
}
