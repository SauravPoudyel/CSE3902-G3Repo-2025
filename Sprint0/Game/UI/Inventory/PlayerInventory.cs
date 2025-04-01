using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Sprint0
{
    public class PlayerInventory : IScreen
    {
        private List<IHUD> hudElements;
        private List<Button> buttonElements;
        Dictionary<string, object> paramters;
        public List<InventorySlot> inventorySlots;
        private ContentManager content;

        public bool BlocksInput => false;

        public PlayerInventory(Game1 game)
        {
            this.content = game.Content; // Store for later use.
            hudElements = new List<IHUD>();
            // hudElements.Add(new ShieldHUD(content.Load<Texture2D>("UI/ShieldIcon")));
            hudElements.Add(new HealthHUD(content.Load<Texture2D>("UI/HealthIcon")));
            hudElements.Add(new AmmoHUD(content.Load<Texture2D>("UI/AmmoIcon")));
            hudElements.Add(new CoinHUD(content.Load<Texture2D>("PickupItemSpritesheet2")));
            hudElements.Add(new XPHUD(game.GraphicsDevice));

            paramters = new Dictionary<string, object>
            {
                { "gameManager", game.GameManager },
                { "game", game}
            };
            buttonElements = new List<Button>();
            Vector2 shopButtonPos = new Vector2(Globals.SCREENWIDTH - 100, 20);
            buttonElements.Add(new ShopButton(content.Load<Texture2D>("UI/ShopIcon"), shopButtonPos, paramters));

            // Initialize 5 inventory slots.
            inventorySlots = new List<InventorySlot>();
            int slotCount = 5;
            int slotWidth = 64;
            int slotHeight = 64;
            int spacing = 10;
            int totalWidth = slotCount * slotWidth + (slotCount - 1) * spacing;
            int startX = (Globals.SCREENWIDTH - totalWidth) / 2;
            int y = Globals.SCREENHEIGHT - slotHeight - 50;

            string[] testTypes = new string[] { "Sniper", "Rocket", "Shotgun", "Mine", "Teleporter" };
            for (int i = 0; i < slotCount; i++)
            {
                Vector2 pos = new Vector2(startX + i * (slotWidth + spacing), y);
                inventorySlots.Add(new InventorySlot(content, i, testTypes[i], pos));
            }
        }

        public void Update()
        {
            foreach (var hud in hudElements)
                hud.Update();
            foreach (var slot in inventorySlots)
                slot.Update();
            foreach (var button in buttonElements)
                button.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var hud in hudElements)
                hud.Draw(spriteBatch);
            foreach (var slot in inventorySlots)
                slot.Draw(spriteBatch);
            foreach (var button in buttonElements)
                button.Draw(spriteBatch);
        }

        public void SelectSlot(int slotIndex)
        {
            if (slotIndex >= 0 && slotIndex < inventorySlots.Count)
            {
                string projectileType = inventorySlots[slotIndex].ProjectileType;
                if (Player.Instance != null)
                    Player.Instance.SetProjectileType(projectileType);
            }
        }

        public void ShiftEmptySlot(int emptySlotIndex)
        {
            inventorySlots.RemoveAt(emptySlotIndex);

            int slotCount = 5; // Fixed inventory size.
            int slotWidth = 64;
            int slotHeight = 64;
            int spacing = 10;
            int totalWidth = slotCount * slotWidth + (slotCount - 1) * spacing;
            int startX = (Globals.SCREENWIDTH - totalWidth) / 2;
            int y = Globals.SCREENHEIGHT - slotHeight - 50;

            for (int i = 0; i < inventorySlots.Count; i++)
            {
                Vector2 pos = new Vector2(startX + i * (slotWidth + spacing), y);
                inventorySlots[i].SetSlotIndex(i);
                inventorySlots[i].UpdatePosition(pos);
            }

            Vector2 newPos = new Vector2(startX + inventorySlots.Count * (slotWidth + spacing), y);
            inventorySlots.Add(new InventorySlot(content, inventorySlots.Count, "Empty", newPos));
        }
    }
}
