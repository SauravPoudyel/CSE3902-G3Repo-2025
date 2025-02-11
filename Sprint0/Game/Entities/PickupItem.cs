using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Threading;

namespace Sprint0
{
    public class PickupItem : Entity
    {
        enum ItemType
        {
            Ammo,
            Repair,
            Shield
        }
        private ContentManager content;
        private float timer;

        private ItemType itemType;

        public PickupItem(ContentManager content) 
        {
            this.content = content;
            this.itemType = ItemType.Ammo;

            AddSprite("Ammo", new AnimatedSprite(0.3f));
            sprites["Ammo"].LoadContent(content, "2DTanksSprites", 459, 120, 110, 100, 1);
            
            AddSprite("Repair", new AnimatedSprite(0.3f));
            sprites["Repair"].LoadContent(content, "2DTanksSprites", 464, 651, 110, 100, 1);

            AddSprite("Shield", new AnimatedSprite(0.3f));
            sprites["Shield"].LoadContent(content, "2DTanksSprites", 468, 322, 110, 100, 1);    

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
            if(itemType == ItemType.Ammo) {
                SetItemType(ItemType.Repair);
            } else if (itemType == ItemType.Repair) {
                SetItemType(ItemType.Shield);
            } else if (itemType == ItemType.Shield) {
                SetItemType(ItemType.Ammo);
            }            
        }
        public void CycleItemPrev() {
            if(itemType == ItemType.Ammo) {
                SetItemType(ItemType.Shield);
            } else if (itemType == ItemType.Repair) {
                SetItemType(ItemType.Ammo);
            } else if (itemType == ItemType.Shield) {
                SetItemType(ItemType.Repair);
            }            
        }
        public override void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            SpriteEffects effects = SpriteEffects.None;

            sprite.Draw(spriteBatch, position, effects, 0f);
        }
    }
}
