using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Sprint0
{
    public class PlayerInventory : IScreen
    {
        // Reference global player data.
        private PlayerData playerData;

        private TextSprite healthText;
        private TextSprite ammoText;
        private TextSprite shieldText;
        private TextSprite coinText;

        public bool BlocksInput => false;

        public PlayerInventory()
        {
            playerData = Globals.PlayerData;
            healthText = new TextSprite("Health: " + playerData.TemporaryHealth, Color.White);
            ammoText = new TextSprite("Ammo: " + playerData.TemporaryAmmoDefault, Color.White);
            shieldText = new TextSprite("Shield: " + playerData.TemporaryShield, Color.White);
            coinText = new TextSprite("Coins: " + playerData.TemporaryCoins, Color.Yellow);
        }

        public void LoadContent(ContentManager content)
        {
            healthText.LoadContent(content, "Arial", 0, 0, 0, 0, 0);
            ammoText.LoadContent(content, "Arial", 0, 0, 0, 0, 0);
            shieldText.LoadContent(content, "Arial", 0, 0, 0, 0, 0);
            coinText.LoadContent(content, "Arial", 0, 0, 0, 0, 0);
        }

        public void Update()
        {
            healthText.SetText("Health: " + playerData.TemporaryHealth);
            ammoText.SetText("Ammo: " + playerData.TemporaryAmmoDefault);
            shieldText.SetText("Shield: " + playerData.TemporaryShield);
            coinText.SetText("Coins: " + playerData.TemporaryCoins);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D rect = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            rect.SetData(new Color[] { Color.White });
            Rectangle hudRect = new Rectangle(10, 10, 220, 100);
            spriteBatch.Draw(rect, hudRect, new Color(0, 0, 0, 150));

            healthText.Draw(spriteBatch, new Vector2(100, 20));
            ammoText.Draw(spriteBatch, new Vector2(100, 40));
            shieldText.Draw(spriteBatch, new Vector2(100, 60));
            coinText.Draw(spriteBatch, new Vector2(100, 80));
        }

        public void HandleClick(Point clickLocation)
        {
            // No click handling needed for HUD.
        }
    }
}
