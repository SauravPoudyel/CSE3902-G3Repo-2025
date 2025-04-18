using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0
{
    class BoostHUD : IHUD
    {
        private Texture2D icon;
        private Texture2D gray;
        private Texture2D cyan;
        private PlayerData playerData;

        private int maxWidth = 300;  // max width of the bar
        private int currentBoost;

        public BoostHUD(Texture2D texture)
        {
            this.icon = texture;
            this.playerData = Globals.PlayerData;

            // Initialize textures for boost bar
            gray = new Texture2D(texture.GraphicsDevice, 1, 1);
            gray.SetData(new Color[] { Color.Gray });

            cyan = new Texture2D(texture.GraphicsDevice, 1, 1);
            cyan.SetData(new Color[] { Color.Cyan });
        }

        public void Update()
        {
            currentBoost = playerData.GetInt("Boost");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Bar position above health (e.g., 900 instead of 950)
            int x = 61;
            int y = 900;
            int height = 30;
            int filledWidth = 3*currentBoost;

            // Draw gray background bar
            spriteBatch.Draw(gray, new Rectangle(x, y, maxWidth, height), Color.White * 0.5f);

            // Draw cyan active portion
            spriteBatch.Draw(cyan, new Rectangle(x, y, filledWidth, height), Color.White);
        }
    }
}
