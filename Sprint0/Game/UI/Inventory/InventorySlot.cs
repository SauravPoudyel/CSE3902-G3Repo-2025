using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using static Sprint0.EntityKeys;
using static Sprint0.Globals;

namespace Sprint0
{
    public class InventorySlot : IHUD
    {
        public int SlotIndex { get; private set; }
        public string ProjectileType { get; private set; }
        private Texture2D slotTexture;
        private Texture2D ammoSprite;
        private Rectangle bounds;
        private ContentManager content;
        private SpriteFont font;
        private const int SlotWidth = 64;
        private const int SlotHeight = 64;
        private Rectangle ammoSourceRect;

        public InventorySlot(ContentManager content, int slotIndex, string projectileType, Vector2 position)
        {
            this.content = content;
            SlotIndex = slotIndex;
            ProjectileType = projectileType;
            bounds = new Rectangle((int)position.X, (int)position.Y, SlotWidth, SlotHeight);

            // Create a 1x1 white texture for drawing a grey box.
            var graphicsDeviceService = (IGraphicsDeviceService)content.ServiceProvider.GetService(typeof(IGraphicsDeviceService));
            if (graphicsDeviceService != null)
            {
                slotTexture = new Texture2D(graphicsDeviceService.GraphicsDevice, 1, 1);
                slotTexture.SetData(new Color[] { Color.White });
            }

            // Load the common ammo sprite sheet.
            ammoSprite = content.Load<Texture2D>("PickupItemSpritesheet2");

            // Set the source rectangle based on the projectile type.
            switch (projectileType)
            {
                case nameof(ProjectileTypeEnum.Sniper):
                    ammoSourceRect = new Rectangle(0, 160, 40, 40);
                    break;
                case nameof(ProjectileTypeEnum.Rocket):
                    ammoSourceRect = new Rectangle(0, 200, 40, 40);
                    break;
                case nameof(ProjectileTypeEnum.Shotgun):
                    ammoSourceRect = new Rectangle(0, 120, 40, 40);
                    break;
                case nameof(ProjectileTypeEnum.Mine):
                    ammoSourceRect = new Rectangle(0, 280, 40, 40);
                    break;
                case nameof(ProjectileTypeEnum.Teleporter):
                    ammoSourceRect = new Rectangle(0, 680, 40, 40);
                    break;
                default:
                    ammoSourceRect = new Rectangle(0, 80, 40, 40);
                    break;
            }

            font = Globals.FONT;
        }

        public void Update()
        {
            // selection or animation logic.
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (slotTexture != null)
                spriteBatch.Draw(slotTexture, bounds, Color.Gray);

            // Determine which sprite to draw.
            Texture2D spriteToDraw = ammoSprite;
            Rectangle sourceRect = ammoSourceRect;
            if (ProjectileType == "Empty")
            {
                spriteToDraw = Globals.NULLSPRITE_S.spriteSheet;
                // For empty, fill the slot with the null sprite stretched to the slot dimensions.
                sourceRect = new Rectangle(2800, 2800, SlotWidth, SlotHeight);
            }

            // Draw the ammo (or null) sprite centered in the slot.
            Vector2 ammoPosition = new Vector2(
                bounds.X + (SlotWidth - sourceRect.Width) / 2,
                bounds.Y + (SlotHeight - sourceRect.Height) / 2);
            spriteBatch.Draw(spriteToDraw, ammoPosition, sourceRect, Color.White);

            // Draw the ammo count as text.
            int ammoCount = GetAmmoCount();
            string countText = ammoCount.ToString();
            Vector2 textSize = font.MeasureString(countText);
            Vector2 textPosition = new Vector2(bounds.Right - textSize.X - 2, bounds.Bottom - textSize.Y - 2);
            spriteBatch.DrawString(font, countText, textPosition, Color.White);
        }

        private int GetAmmoCount()
        {
            if (ProjectileType == "Empty")
                return 0;

            switch (ProjectileType)
            {
                case "Sniper":
                    return Globals.PlayerData.GetInt("AmmoSniper");
                case "Rocket":
                    return Globals.PlayerData.GetInt("AmmoRocket");
                case "Shotgun":
                    return Globals.PlayerData.GetInt("AmmoShotgun");
                case "Mine":
                    return Globals.PlayerData.GetInt("AmmoMine");
                case "Teleporter":
                    return Globals.PlayerData.GetInt("AmmoTeleporter");
                default:
                    return Globals.PlayerData.GetInt("AmmoDefault");
            }
        }

        public int AmmoCount
        {
            get { return GetAmmoCount(); }
        }

        public void UpdatePosition(Vector2 position)
        {
            bounds = new Rectangle((int)position.X, (int)position.Y, SlotWidth, SlotHeight);
        }

        public void SetSlotIndex(int newIndex)
        {
            SlotIndex = newIndex;
        }
    }
}
