using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace Sprint0
{
    public static class Globals
    {
        public const int SCREENWIDTH = 1920;
        public const int SCREENHEIGHT = 1080;
        public static StaticSprite NULLSPRITE_S;
        public static AnimatedSprite NULLSPRITE_A;
        public static float FRAMETIME = 1f / 60f;
        public const float PLAYERFRAMETIME = 1f / 60f; // just so the slow powerup works
        private static PlayerData playerData;
        public static SpriteFont FONT;
        public static string projectDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\"));

        public static PlayerData PlayerData
        {
            get
            {
                if (playerData == null)
                    LoadPlayerData();
                return playerData;
            }
            set { playerData = value; }
        }

        public static void LoadGlobalSprites(ContentManager content)
        {
            NULLSPRITE_S = new StaticSprite();
            NULLSPRITE_S.LoadContent(content, "TDTanksAllSprites", 129, 0, 1, 1, 1);

            NULLSPRITE_A = new AnimatedSprite(0.1f);
            NULLSPRITE_A.LoadContent(content, "TDTanksAllSprites", 129, 0, 1, 1, 1);
        }

        public static void LoadGlobalFonts(ContentManager content)
        {
            FONT = content.Load<SpriteFont>("Arial");
        }

        public static void LoadPlayerData()
        {
            string playerDataFile = Path.Combine("Data", "playerDataFile.csv");
            if (File.Exists(playerDataFile))
            {
                playerData = PlayerData.LoadData(playerDataFile);
            }
            else
            {
                playerData = new PlayerData();
            }
        }

        public static void SavePlayerData()
        {
            string playerDataFile = Path.Combine("Data", "playerDataFile.csv");
            CSVParser.SavePlayerData(playerDataFile, playerData);
        }
    }
}
