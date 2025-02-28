using System;

namespace Sprint0
{
    public class PlayerData
    {
        // Permanent data: modified by permanent powerups; persists across deaths.
        public int PermanentHealth { get; set; } = 100;
        public int PermanentAmmoDefault { get; set; } = 50;
        public int PermanentAmmoShotgun { get; set; } = 10;
        public int PermanentAmmoSniper { get; set; } = 5;
        public int PermanentAmmoRocket { get; set; } = 5;
        public int PermanentAmmoLaser { get; set; } = 20;
        public int PermanentAmmoMine { get; set; } = 5;
        public int PermanentShield { get; set; } = 25;
        public int PermanentCoins { get; set; } = 0;

        // Temporary data: resets on death or can be modified by temporary powerups.
        public int TemporaryHealth { get; set; } = 100;
        public int TemporaryAmmoDefault { get; set; } = 50;
        public int TemporaryAmmoShotgun { get; set; } = 10;
        public int TemporaryAmmoSniper { get; set; } = 5;
        public int TemporaryAmmoRocket { get; set; } = 5;
        public int TemporaryAmmoLaser { get; set; } = 20;
        public int TemporaryAmmoMine { get; set; } = 5;
        public int TemporaryShield { get; set; } = 25;
        public int TemporaryCoins { get; set; } = 0;

        // When the game starts or after a death, the temporary values are reloaded
        // from the permanent ones.
        public void ResetTemporaryData()
        {
            TemporaryHealth = PermanentHealth;
            TemporaryAmmoDefault = PermanentAmmoDefault;
            TemporaryAmmoShotgun = PermanentAmmoShotgun;
            TemporaryAmmoSniper = PermanentAmmoSniper;
            TemporaryAmmoRocket = PermanentAmmoRocket;
            TemporaryAmmoLaser = PermanentAmmoLaser;
            TemporaryAmmoMine = PermanentAmmoMine;
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
