using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Sprint0
{
    public class PickupItem : Entity
    {
        private ContentManager content;
        private float timer;
        private PickupItemType itemType;
        private const float PickupRadius = 150f;   
        private const float Acceleration = 300f;     

        public PickupItem(ContentManager content)
        {
            this.content = content;
            this.itemType = PickupItemType.SpeedBoost;

            // Load sprites from the 40×40 PickupItemSpritesheet2.
            // Each sprite uses 10 frames.
            AddSprite("SpeedBoost", new AnimatedSprite(0.15f));
            sprites["SpeedBoost"].LoadContent(content, "PickupItemSpritesheet2", 0, 0, 40, 40, 10);

            AddSprite("Shield", new AnimatedSprite(0.15f));
            sprites["Shield"].LoadContent(content, "PickupItemSpritesheet2", 0, 40, 40, 40, 10);

            AddSprite("Ammo_default", new AnimatedSprite(0.15f));
            sprites["Ammo_default"].LoadContent(content, "PickupItemSpritesheet2", 0, 80, 40, 40, 10);

            AddSprite("Ammo_shotgun", new AnimatedSprite(0.15f));
            sprites["Ammo_shotgun"].LoadContent(content, "PickupItemSpritesheet2", 0, 120, 40, 40, 10);

            AddSprite("Ammo_sniper", new AnimatedSprite(0.15f));
            sprites["Ammo_sniper"].LoadContent(content, "PickupItemSpritesheet2", 0, 160, 40, 40, 10);

            AddSprite("Ammo_rocket", new AnimatedSprite(0.15f));
            sprites["Ammo_rocket"].LoadContent(content, "PickupItemSpritesheet2", 0, 200, 40, 40, 10);

            AddSprite("Ammo_Laser", new AnimatedSprite(0.15f));
            sprites["Ammo_Laser"].LoadContent(content, "PickupItemSpritesheet2", 0, 240, 40, 40, 10);

            AddSprite("Ammo_Mine", new AnimatedSprite(0.15f));
            sprites["Ammo_Mine"].LoadContent(content, "PickupItemSpritesheet2", 0, 280, 40, 40, 10);

            AddSprite("Magnet", new AnimatedSprite(0.15f));
            sprites["Magnet"].LoadContent(content, "PickupItemSpritesheet2", 0, 320, 40, 40, 10);

            AddSprite("FireRateIncrease", new AnimatedSprite(0.15f));
            sprites["FireRateIncrease"].LoadContent(content, "PickupItemSpritesheet2", 0, 360, 40, 40, 10);

            AddSprite("MedStrong", new AnimatedSprite(0.15f));
            sprites["MedStrong"].LoadContent(content, "PickupItemSpritesheet2", 0, 400, 40, 40, 10);

            AddSprite("MedWeak", new AnimatedSprite(0.15f));
            sprites["MedWeak"].LoadContent(content, "PickupItemSpritesheet2", 0, 440, 40, 40, 10);

            AddSprite("TimeSlow", new AnimatedSprite(0.15f));
            sprites["TimeSlow"].LoadContent(content, "PickupItemSpritesheet2", 0, 480, 40, 40, 10);

            AddSprite("Fly", new AnimatedSprite(0.15f));
            sprites["Fly"].LoadContent(content, "PickupItemSpritesheet2", 0, 520, 40, 40, 10);

            AddSprite("Cloak", new AnimatedSprite(0.15f));
            sprites["Cloak"].LoadContent(content, "PickupItemSpritesheet2", 0, 560, 40, 40, 10);

            AddSprite("SilverTag", new AnimatedSprite(0.15f));
            sprites["SilverTag"].LoadContent(content, "PickupItemSpritesheet2", 0, 600, 40, 40, 10);

            AddSprite("GoldTag", new AnimatedSprite(0.15f));
            sprites["GoldTag"].LoadContent(content, "PickupItemSpritesheet2", 0, 640, 40, 40, 10);

            AddSprite("Teleporter", new AnimatedSprite(0.15f));
            sprites["Teleporter"].LoadContent(content, "PickupItemSpritesheet2", 0, 680, 40, 40, 10);

            SetSprite(sprites[this.itemType.ToString()]);
        }

        public void SetItemType(PickupItemType type)
        {
            this.itemType = type;
            SetSprite(sprites[this.itemType.ToString()]);
        }

        public PickupItemType GetItemType()
        {
            return itemType; 
        }

        public void CycleItemNext()
        {
            int numTypes = Enum.GetNames(typeof(PickupItemType)).Length;
            int current = (int)itemType;
            current = (current + 1) % numTypes;
            itemType = (PickupItemType)current;
            SetSprite(sprites[itemType.ToString()]);
        }

        public void CycleItemPrev()
        {
            int numTypes = Enum.GetNames(typeof(PickupItemType)).Length;
            int current = (int)itemType;
            current = (current - 1 + numTypes) % numTypes;
            itemType = (PickupItemType)current;
            SetSprite(sprites[itemType.ToString()]);
        }

        public override void Update()
        {
            position += velocity * Globals.FRAMETIME;
            bounds = new Rectangle((int)position.X, (int)position.Y, 40, 40);
            sprite.Update();
            timer += Globals.FRAMETIME;

            if (Player.Instance != null)
            {
                Vector2 toPlayer = Player.Instance.GetPosition() - position;
                float distance = toPlayer.Length();
                if (distance < PickupRadius && distance > 0)
                {
                    Vector2 accel = Vector2.Normalize(toPlayer) * Acceleration;
                    velocity += accel * Globals.FRAMETIME;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            SpriteEffects effects = SpriteEffects.None;
            //sprite.Draw(spriteBatch, position, effects, 0f);
            sprite.Draw(spriteBatch, position, effects, 0, null,null, 100f);

        }
    }
}
