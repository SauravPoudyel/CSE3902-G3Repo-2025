using System;

namespace Sprint0
{
    public class PlayerData
    {
        // Permanent data.
        public int PermanentHealth { get; set; } = 100;
        public int PermanentAmmoDefault { get; set; } = 50;
        public int PermanentAmmoShotgun { get; set; } = 10;
        public int PermanentAmmoSniper { get; set; } = 5;
        public int PermanentAmmoRocket { get; set; } = 5;
        public int PermanentAmmoLaser { get; set; } = 20;
        public int PermanentAmmoMine { get; set; } = 5;
        public int PermanentAmmoTeleporter { get; set; } = 5;
        public int PermanentShield { get; set; } = 25;
        public int PermanentCoins { get; set; } = 0;

        // Temporary data.
        public int TemporaryHealth { get; set; } = 100;
        public int TemporaryAmmoDefault { get; set; } = 50;
        public int TemporaryAmmoShotgun { get; set; } = 10;
        public int TemporaryAmmoSniper { get; set; } = 5;
        public int TemporaryAmmoRocket { get; set; } = 5;
        public int TemporaryAmmoLaser { get; set; } = 20;
        public int TemporaryAmmoMine { get; set; } = 5;
        public int TemporaryAmmoTeleporter { get; set; } = 5;
        public int TemporaryShield { get; set; } = 25;
        public int TemporaryCoins { get; set; } = 0;

        public void ResetTemporaryData()
        {
            TemporaryHealth = PermanentHealth;
            TemporaryAmmoDefault = PermanentAmmoDefault;
            TemporaryAmmoShotgun = PermanentAmmoShotgun;
            TemporaryAmmoSniper = PermanentAmmoSniper;
            TemporaryAmmoRocket = PermanentAmmoRocket;
            TemporaryAmmoLaser = PermanentAmmoLaser;
            TemporaryAmmoMine = PermanentAmmoMine;
            TemporaryAmmoTeleporter = PermanentAmmoTeleporter;
            TemporaryShield = PermanentShield;
            TemporaryCoins = 0;
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
