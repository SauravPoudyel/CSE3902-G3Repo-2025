using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Sprint0
{
    public class PickupItem : Entity
    {
        enum ItemType
        {
            Shield,
            Ammo,
            Repair,
            Speed,
            Cloak,
            Fire,
            Bounce,
            Instakill,
        }
        private ContentManager content;
        private float timer;
        private ItemType itemType;

        public PickupItem(ContentManager content) 
        {
            this.content = content;
            this.itemType = ItemType.Shield;

            AddSprite("Shield", new AnimatedSprite(0.15f));
            sprites["Shield"].LoadContent(content, "PickupItemSpritesheet", 0, 0, 100, 100, 10);    

            AddSprite("Ammo", new AnimatedSprite(0.15f));
            sprites["Ammo"].LoadContent(content, "PickupItemSpritesheet", 0, 100, 100, 100, 10);
            
            AddSprite("Repair", new AnimatedSprite(0.15f));
            sprites["Repair"].LoadContent(content, "PickupItemSpritesheet", 0, 200, 100, 100, 10);

            AddSprite("Speed", new AnimatedSprite(0.15f));
            sprites["Speed"].LoadContent(content, "PickupItemSpritesheet", 0, 300, 100, 100, 10);

            AddSprite("Cloak", new AnimatedSprite(0.15f));
            sprites["Cloak"].LoadContent(content, "PickupItemSpritesheet", 0, 400, 100, 100, 10);

            AddSprite("Fire", new AnimatedSprite(0.15f));
            sprites["Fire"].LoadContent(content, "PickupItemSpritesheet", 0, 500, 100, 100, 10);

            AddSprite("Bounce", new AnimatedSprite(0.15f));
            sprites["Bounce"].LoadContent(content, "PickupItemSpritesheet", 0, 600, 100, 100, 10);

            AddSprite("Instakill", new AnimatedSprite(0.15f));
            sprites["Instakill"].LoadContent(content, "PickupItemSpritesheet", 0, 700, 100, 100, 10);

            SetSprite(sprites[this.itemType.ToString()]);
        }

        private void SetItemType(ItemType itemType) {
            this.itemType = itemType;
            this.SetSprite(this.itemType.ToString());
        }

        public string GetItemType() {
            return itemType.ToString();
        }
        public void CycleItemNext() {
            if(itemType != ItemType.Instakill) {
                this.itemType++;
            } else {
                this.itemType = ItemType.Shield;
            }
            SetItemType(this.itemType);
        }
        public void CycleItemPrev() {
            if(itemType != ItemType.Shield) {
                this.itemType--;
            } else {
                this.itemType = ItemType.Instakill;
            }
            SetItemType(this.itemType);
        }
        public override void Update()
        {
            position += velocity * Globals.FRAMETIME; 
            bounds = new Rectangle((int)position.X, (int)position.Y, 100, 100);
            sprite.Update();
            timer += Globals.FRAMETIME; 
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            SpriteEffects effects = SpriteEffects.None;
            sprite.Draw(spriteBatch, position, effects, 0f);
        }

        public void ApplyEffect(Player player)
        {
            switch(itemType)
            {
                case ItemType.Shield:
                    player.shieldActive = true;
                    player.EffectTimers["Shield"] = 5f;
                    break;
                case ItemType.Ammo:
                    // Increase ammo count
                    break;
                case ItemType.Repair:
                    // Repair health 
                    break;
                case ItemType.Speed:
                    player.speedMultiplier = 2f;
                    player.EffectTimers["SpeedBoost"] = 5f;
                    break;
                case ItemType.Cloak:
                    // Set cloaking
                    break;
                case ItemType.Fire:
                    if (player.cannon != null)
                    {
                        player.cannon.AngularVelocity *= 2f;
                        player.EffectTimers["FireBoost"] = 5f;
                    }
                    break;
                case ItemType.Bounce:
                    // Implement bounce 
                    break;
                case ItemType.Instakill:
                    // Implement instakill
                    break;
                default:
                    break;
            }
        }
    }
}
