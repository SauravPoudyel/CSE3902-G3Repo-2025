using System;
using System.Collections.Generic;
using System.IO;

namespace Sprint0
{
    public static class CSVParser
    {
        public static List<Dictionary<string, string>> ParseFile(string filePath)
        {
            List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();
            if (!File.Exists(filePath))
                return rows;

            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length == 0)
                return rows;

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
    }
}
