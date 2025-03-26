using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Sprint0
{
    /* 
     * THIS CLASS IS JUST FOR DEBUG PURPOSES. IT CYCLES THROUGH ALL THE PICKUP ITEMS, it is not intended to be in the game itself
    */
    public class PickupItem : Entity
    {
        private ContentManager content;
        private float timer;
        private EntityKeys.ItemType itemType;
        private const float PickupRadius = 150f;
        private const float Acceleration = 300f;
        protected AnimatedSprite animatedSprite;
        protected float frameTime = 0.15f;

        public PickupItem(ContentManager content, EntityKeys.ItemType type)
        {
            this.content = content;
            this.itemType = type;
            LoadItemContent(content, type);
            SetItemType(type);
        }

        public void LoadItemContent(ContentManager content, EntityKeys.ItemType type)
        {
            animatedSprite = new AnimatedSprite(frameTime);
            switch (type)
            {
                case EntityKeys.ItemType.SpeedBoost:
                    animatedSprite.LoadContent(content, "PickupItemSpritesheet2", 0, 0, 40, 40, 10);
                    break;
                case EntityKeys.ItemType.Shield:
                    animatedSprite.LoadContent(content, "PickupItemSpritesheet2", 0, 40, 40, 40, 10);
                    break;
                case EntityKeys.ItemType.AmmoDefault:
                    animatedSprite.LoadContent(content, "PickupItemSpritesheet2", 0, 80, 40, 40, 10);
                    break;
                case EntityKeys.ItemType.AmmoShotgun:
                    animatedSprite.LoadContent(content, "PickupItemSpritesheet2", 0, 120, 40, 40, 10);
                    break;
                case EntityKeys.ItemType.AmmoSniper:
                    animatedSprite.LoadContent(content, "PickupItemSpritesheet2", 0, 160, 40, 40, 10);
                    break;
                case EntityKeys.ItemType.AmmoRocket:
                    animatedSprite.LoadContent(content, "PickupItemSpritesheet2", 0, 200, 40, 40, 10);
                    break;
                case EntityKeys.ItemType.AmmoLaser:
                    animatedSprite.LoadContent(content, "PickupItemSpritesheet2", 0, 240, 40, 40, 10);
                    break;
                case EntityKeys.ItemType.AmmoMine:
                    animatedSprite.LoadContent(content, "PickupItemSpritesheet2", 0, 280, 40, 40, 10);
                    break;
                case EntityKeys.ItemType.Magnet:
                    animatedSprite.LoadContent(content, "PickupItemSpritesheet2", 0, 320, 40, 40, 10);
                    break;
                case EntityKeys.ItemType.FireRateIncrease:
                    animatedSprite.LoadContent(content, "PickupItemSpritesheet2", 0, 360, 40, 40, 10);
                    break;
                case EntityKeys.ItemType.MedStrong:
                    animatedSprite.LoadContent(content, "PickupItemSpritesheet2", 0, 400, 40, 40, 10);
                    break;
                case EntityKeys.ItemType.MedWeak:
                    animatedSprite.LoadContent(content, "PickupItemSpritesheet2", 0, 440, 40, 40, 10);
                    break;
                case EntityKeys.ItemType.TimeSlow:
                    animatedSprite.LoadContent(content, "PickupItemSpritesheet2", 0, 480, 40, 40, 10);
                    break;
                case EntityKeys.ItemType.Fly:
                    animatedSprite.LoadContent(content, "PickupItemSpritesheet2", 0, 520, 40, 40, 10);
                    break;
                case EntityKeys.ItemType.Cloak:
                    animatedSprite.LoadContent(content, "PickupItemSpritesheet2", 0, 560, 40, 40, 10);
                    break;
                case EntityKeys.ItemType.SilverTag:
                    animatedSprite.LoadContent(content, "PickupItemSpritesheet2", 0, 600, 40, 40, 10);
                    break;
                case EntityKeys.ItemType.GoldTag:
                    animatedSprite.LoadContent(content, "PickupItemSpritesheet2", 0, 640, 40, 40, 10);
                    break;
                case EntityKeys.ItemType.Teleporter:
                    animatedSprite.LoadContent(content, "PickupItemSpritesheet2", 0, 680, 40, 40, 10);
                    break;
                default:
                    break;
            }
            SetSprite(animatedSprite);
        }

        public void SetItemType(EntityKeys.ItemType type)
        {
            this.itemType = type;
            SetSprite(sprites[this.itemType.ToString()]);
        }

        public EntityKeys.ItemType GetItemType() => itemType;

        public void CycleItemNext()
        {
            int numTypes = Enum.GetNames(typeof(EntityKeys.ItemType)).Length;
            int current = (int)itemType;
            current = (current + 1) % numTypes;
            itemType = (EntityKeys.ItemType)current;
            SetSprite(sprites[itemType.ToString()]);
        }

        public void CycleItemPrev()
        {
            int numTypes = Enum.GetNames(typeof(EntityKeys.ItemType)).Length;
            int current = (int)itemType;
            current = (current - 1 + numTypes) % numTypes;
            itemType = (EntityKeys.ItemType)current;
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
            animatedSprite.Draw(spriteBatch, position, SpriteEffects.None, 0, null, null, 1.1f);
        }
    }
}
