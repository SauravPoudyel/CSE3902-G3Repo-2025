using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Sprint0
{
    public static class Globals
    {
        public const int SCREENWIDTH = 1920;
        public const int SCREENHEIGHT = 1080;
        public static ISprite NULLSPRITE;
        public static SpriteFont FONT;
        public const float FRAMETIME = 1f / 60f;

        private static PlayerData playerData;

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
            NULLSPRITE = new StaticSprite();
            NULLSPRITE.LoadContent(content, "TDTanksAllSprites", 129, 0, 1, 1, 1);
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
