
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0
{
    class AmmoHUD : IHUD
    {
        private Texture2D icon;
        private PlayerData playerData;
        private int maxAmmo;
        private int currentAmmo;

        public AmmoHUD(Texture2D texture)
        {
            this.icon = texture;
            playerData = Globals.PlayerData;
            maxAmmo = playerData.GetInt("AmmoDefault");
            currentAmmo = playerData.GetInt("AmmoDefault");
        }
        public void Update()
        {
            // Update ammo count based on player ammo
            currentAmmo = playerData.GetInt("AmmoDefault");
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(icon, new Rectangle(10, 1000, 50, 50), Color.White);
            spriteBatch.DrawString(Globals.FONT, currentAmmo + "/" + maxAmmo, new Vector2(70, 1020), Color.White);
        }
    }
}

