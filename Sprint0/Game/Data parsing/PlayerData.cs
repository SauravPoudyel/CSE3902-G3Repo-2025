using System;
using System.Collections.Generic;

namespace Sprint0
{
    public class PlayerData
    {
        // The dictionary holds all stored variables.
        // Keys can be "Name", "Health", "ammo", "coins", "XP", "SmallEnemy Killed", etc.
        public Dictionary<string, string> Variables { get; private set; }

        public PlayerData()
        {
            Variables = new Dictionary<string, string>();
        }

        public string GetString(string key, string defaultValue = "")
        {
            return Variables.ContainsKey(key) ? Variables[key] : defaultValue;
        }

        public int GetInt(string key, int defaultValue = 0)
        {
            if (Variables.ContainsKey(key) && int.TryParse(Variables[key], out int value))
                return value;
            return defaultValue;
        }

        public void UpdateVariable(string key, int amount = 1)
        {
            int current = GetInt(key, 0);
            Set(key, (current + amount).ToString());
        }

        public void Set(string key, string value)
        {
            Variables[key] = value;
        }
    }
}
