using System;
using System.Collections.Generic;
using System.IO;

namespace Sprint0
{
    public class DialogueData
    {
        public string Key { get; set; }
        public string Character { get; set; }
        public string Text { get; set; }
        public string Trigger { get; set; }  // e.x. "HasSeenTutorial"
    }

    public static class CSVDialogueParser
    {
        public static Dictionary<string, DialogueData> ParseDialogueCSV(string filePath)
        {
            Dictionary<string, DialogueData> dialogues = new Dictionary<string, DialogueData>();
            if (!File.Exists(filePath))
            {
                System.Console.WriteLine("Dialogue CSV file not found: " + filePath);
                return dialogues;
            }
            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length <= 1)
            {
                System.Console.WriteLine("Dialogue CSV file is empty or missing header: " + filePath);
                return dialogues;
            }
            // Skip header line
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                string[] parts = line.Split(',');
                if (parts.Length < 4)
                {
                    System.Console.WriteLine("Invalid dialogue line: " + line);
                    continue;
                }
                DialogueData data = new DialogueData
                {
                    Key = parts[0].Trim(),
                    Character = parts[1].Trim(),
                    Text = parts[2].Trim(),
                    Trigger = parts[3].Trim() // may be empty
                };
                if (!dialogues.ContainsKey(data.Key))
                    dialogues.Add(data.Key, data);
                else
                    System.Console.WriteLine("Duplicate dialogue key: " + data.Key);
            }
            return dialogues;
        }
    }
}
