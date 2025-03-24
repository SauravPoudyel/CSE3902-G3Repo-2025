using System;
using System.Collections.Generic;
using System.IO;

namespace Sprint0
{
    public static class CSVPlayerParser
    {
        public static PlayerData ParsePlayerData(string filePath)
        {
            PlayerData data = new PlayerData();
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File path does not exist: " + filePath);
                return data;
            }
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                // Expecting each line to have at least 2 columns: key, value
                string[] parts = line.Split(',');
                if (parts.Length < 2)
                    continue;
                string key = parts[0].Trim();
                string value = parts[1].Trim();
                data.Set(key, value);
            }
            return data;
        }

        public static void SavePlayerData(string filePath, PlayerData data)
        {
            List<string> lines = new List<string>();
            foreach (var kv in data.Variables)
            {
                lines.Add($"{kv.Key}, {kv.Value}");
            }
            File.WriteAllLines(filePath, lines);
        }
    }
}
