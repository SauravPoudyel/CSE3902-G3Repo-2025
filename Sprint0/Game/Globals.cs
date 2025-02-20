using Microsoft.Xna.Framework.Content;

namespace Sprint0
{
    public static class Globals
    {
        public const int SCREENWIDTH = 1920;
        public const int SCREENHEIGHT = 1080;
        public static ISprite NULLSPRITE;

        public const float FRAMETIME = 1f/ 60f; 

        public static void LoadGlobalSprites(ContentManager content)
        {
            NULLSPRITE = new StaticSprite();
            NULLSPRITE.LoadContent(content, "TDTanksAllSprites", 129, 0, 1, 1, 1);
        }
    }
}
