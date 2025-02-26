using System;

namespace Sprint0
{
    public class PlayerData
    {
        // Permanent data: modified by permanent powerups; persists across deaths.
        public int PermanentHealth { get; set; } = 100;
        public int PermanentAmmo { get; set; } = 50;
        public int PermanentShield { get; set; } = 25;
        public int PermanentCoins { get; set; } = 0;

        // Temporary data: resets on death or can be modified by temporary powerups.
        public int TemporaryHealth { get; set; } = 100;
        public int TemporaryAmmo { get; set; } = 50;
        public int TemporaryShield { get; set; } = 25;
        public int TemporaryCoins { get; set; } = 0;

        // When the game starts or after a death, the temporary values are reloaded
        // from the permanent ones.
        public void ResetTemporaryData()
        {
            TemporaryHealth = PermanentHealth;
            TemporaryAmmo = PermanentAmmo;
            TemporaryShield = PermanentShield;
            TemporaryCoins = 0; // Or keep coins permanently
        }

        public void SaveData(string filePath)
        {
            CSVParser.SavePlayerData(filePath, this);
        }

        public static PlayerData LoadData(string filePath)
        {
            return CSVParser.ParsePlayerData(filePath);
        }
    }
}
