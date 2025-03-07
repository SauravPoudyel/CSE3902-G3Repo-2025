using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0
{
    public enum PickupItemType
    { 
        SpeedBoost, Shield, Ammo_default, Ammo_shotgun, Ammo_sniper, Ammo_rocket,
        Ammo_Laser, Ammo_Mine, Magnet, FireRateIncrease, MedStrong, MedWeak, TimeSlow,
        Fly, Cloak, SilverTag, GoldTag, Teleporter
    }

    public class Item : Entity
    {
        private ContentManager content;
        private float timer;
        private PickupItemType itemType;
        private float PickupRadius = 150f;
        private const float Acceleration = 300f;
        protected AnimatedSprite animatedSprite;
        protected float frameTime = 0.15f;

        public Item(ContentManager content, PickupItemType type)
        {
            this.content = content;
            this.itemType = type;
            LoadItemContent(content, type);
            SetItemType(type);
        }

        public void LoadItemContent(ContentManager content, PickupItemType type)
        {
            animatedSprite = new AnimatedSprite(frameTime);
            switch (type)
            {
                case PickupItemType.SpeedBoost:
                    animatedSprite.LoadContent(content, "PickupItemSpritesheet2", 0, 0, 40, 40, 10);
                    break;
                case PickupItemType.Shield:
                    animatedSprite.LoadContent(content, "PickupItemSpritesheet2", 0, 40, 40, 40, 10);
                    break;
                case PickupItemType.Ammo_default:
                    animatedSprite.LoadContent(content, "PickupItemSpritesheet2", 0, 80, 40, 40, 10);
                    break;
                case PickupItemType.Ammo_shotgun:
                    animatedSprite.LoadContent(content, "PickupItemSpritesheet2", 0, 120, 40, 40, 10);
                    break;
                case PickupItemType.Ammo_sniper:
                    animatedSprite.LoadContent(content, "PickupItemSpritesheet2", 0, 160, 40, 40, 10);
                    break;
                case PickupItemType.Ammo_rocket:
                    animatedSprite.LoadContent(content, "PickupItemSpritesheet2", 0, 200, 40, 40, 10);
                    break;
                case PickupItemType.Ammo_Laser:
                    animatedSprite.LoadContent(content, "PickupItemSpritesheet2", 0, 240, 40, 40, 10);
                    break;
                case PickupItemType.Ammo_Mine:
                    animatedSprite.LoadContent(content, "PickupItemSpritesheet2", 0, 280, 40, 40, 10);
                    break;
                case PickupItemType.Magnet:
                    animatedSprite.LoadContent(content, "PickupItemSpritesheet2", 0, 320, 40, 40, 10);
                    break;
                case PickupItemType.FireRateIncrease:
                    animatedSprite.LoadContent(content, "PickupItemSpritesheet2", 0, 360, 40, 40, 10);
                    break;
                case PickupItemType.MedStrong:
                    animatedSprite.LoadContent(content, "PickupItemSpritesheet2", 0, 400, 40, 40, 10);
                    break;
                case PickupItemType.MedWeak:
                    animatedSprite.LoadContent(content, "PickupItemSpritesheet2", 0, 440, 40, 40, 10);
                    break;
                case PickupItemType.TimeSlow:
                    animatedSprite.LoadContent(content, "PickupItemSpritesheet2", 0, 480, 40, 40, 10);
                    break;
                case PickupItemType.Fly:
                    animatedSprite.LoadContent(content, "PickupItemSpritesheet2", 0, 520, 40, 40, 10);
                    break;
                case PickupItemType.Cloak:
                    animatedSprite.LoadContent(content, "PickupItemSpritesheet2", 0, 560, 40, 40, 10);
                    break;
                case PickupItemType.SilverTag:
                    animatedSprite.LoadContent(content, "PickupItemSpritesheet2", 0, 600, 40, 40, 10);
                    break;
                case PickupItemType.GoldTag:
                    animatedSprite.LoadContent(content, "PickupItemSpritesheet2", 0, 640, 40, 40, 10);
                    break;
                case PickupItemType.Teleporter:
                    animatedSprite.LoadContent(content, "PickupItemSpritesheet2", 0, 680, 40, 40, 10);
                    break;
                default:
                    break;
            }
            SetSprite(animatedSprite);
        }

        public void SetItemType(PickupItemType type)
        {
            this.itemType = type;
            SetSprite(animatedSprite);
        }

        public PickupItemType GetItemType()
        {
            return itemType;
        }

        public override void Update()
        {
            PickupRadius = PowerUpFactory.NormalPickUpRadius;
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
            animatedSprite.Draw(spriteBatch, position, SpriteEffects.None, 0, null, null, 1.5f);
        }
    }
}
