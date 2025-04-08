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
            return Variables.TryGetValue(key, out string stringValue) ? stringValue : defaultValue;
        }

        public int GetInt(string key, int defaultValue = 0)
        {
            if (Variables.TryGetValue(key, out string stringValue) && int.TryParse(stringValue, out int intValue))
                return intValue;
            return defaultValue;
        }

        public void SetInt(string key, int value)
        {
            Set(key, value.ToString());
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
