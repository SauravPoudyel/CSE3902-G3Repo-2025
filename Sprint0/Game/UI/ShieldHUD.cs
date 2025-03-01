
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0
{
    class ShieldHUD : IHUD
    {
        private Texture2D icon;
        private Texture2D gray;
        private Texture2D blue;
        private PlayerData playerData;
        private int maxShield;
        private int currentShield;
        public ShieldHUD(Texture2D texture)
        {
            this.icon = texture;
            this.playerData = Globals.PlayerData;
            this.maxShield = playerData.PermanentShield;

            // Initialize textures for shield bar
            gray = new Texture2D(texture.GraphicsDevice, 1, 1);
            gray.SetData(new Color[] { Color.Gray });
            blue = new Texture2D(texture.GraphicsDevice, 1, 1);
            blue.SetData(new Color[] { Color.Blue });
        }
        public void Update()
        {
            // Update shield bar based on player shield
            this.currentShield = playerData.TemporaryShield;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the shield icon and shield bar
            spriteBatch.Draw(icon, new Rectangle(10, 950, 50, 50), Color.White);
            spriteBatch.Draw(gray, new Rectangle(61, 950, maxShield * 3, 50), Color.White * 0.5f);
            spriteBatch.Draw(blue, new Rectangle(61, 950, currentShield * 3, 50), Color.White);
        }
    }
}
