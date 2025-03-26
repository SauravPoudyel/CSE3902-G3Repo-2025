using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0
{
    class HealthHUD : IHUD
    {
        private Texture2D icon;
        private Texture2D gray;
        private Texture2D red;
        private Texture2D green;
        private Texture2D yellow;
        private PlayerData playerData;
        private int maxHealth;
        private int currentHealth;
        public HealthHUD(Texture2D texture)
        {
            this.icon = texture;
            this.playerData = Globals.PlayerData;
            this.maxHealth = playerData.GetInt("Health");

            // Initialize textures for health bar
            gray = new Texture2D(texture.GraphicsDevice, 1, 1);
            gray.SetData(new Color[] { Color.Gray });
            red = new Texture2D(texture.GraphicsDevice, 1, 1);
            red.SetData(new Color[] { Color.Red });
            green = new Texture2D(texture.GraphicsDevice, 1, 1);
            green.SetData(new Color[] { Color.Green });
            yellow = new Texture2D(texture.GraphicsDevice, 1, 1);
            yellow.SetData(new Color[] { Color.Yellow });
        }
        public void Update()
        {
            // Update health bar based on player health
            this.currentHealth = playerData.GetInt("Health");

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the health icon and health bar
            spriteBatch.Draw(icon, new Rectangle(10, 950, 50, 50), Color.White);
            spriteBatch.Draw(gray, new Rectangle(61, 950, maxHealth * 3, 50), Color.White * 0.5f);
            if ((double)currentHealth / maxHealth > 0.5)
            {
                spriteBatch.Draw(green, new Rectangle(61, 950, currentHealth * 3, 50), Color.White);
            }
            else if ((double)currentHealth / maxHealth > 0.25)
            {
                spriteBatch.Draw(yellow, new Rectangle(61, 950, currentHealth * 3, 50), Color.White);
            }
            else
            {
                spriteBatch.Draw(red, new Rectangle(61, 950, currentHealth * 3, 50), Color.White);

            }
        }
    }
}
