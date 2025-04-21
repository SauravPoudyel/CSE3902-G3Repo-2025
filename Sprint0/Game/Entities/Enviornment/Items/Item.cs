using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0
{
    public class Item : Entity
    {
        private ContentManager content;
        private float timer;
        private EntityKeys.ItemType itemType;
        private float PickupRadius = 150f;
        private const float Acceleration = 300f;
        protected Sprite animatedSprite;
        protected float frameTime = 0.15f;

        public Item(ContentManager content, EntityKeys.ItemType type)
        {
            this.content = content;
            this.itemType = type;
            LoadItemContent(content, type);
            SetItemType(type);
        }

        public void LoadItemContent(ContentManager content, EntityKeys.ItemType type)
        {
            if(content == null) {
                throw new ArgumentNullException(nameof(content), "ContentManager cannot be null.");
            }

            animatedSprite = new Sprite(frameTime);

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
                case EntityKeys.ItemType.ShopKeys:
                    animatedSprite.LoadContent(content, "PickupItemSpritesheet2", 0, 920 , 40, 40, 10);
                    break;
                default:
                    throw new ArgumentException($"Invalid ItemType: {type}");
            }

            SetSprite(animatedSprite);
        }

        public void SetItemType(EntityKeys.ItemType type)
        {
            this.itemType = type;
            SetSprite(animatedSprite);
        }

        public EntityKeys.ItemType GetItemType() => itemType;

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
                    PickupRadius = 600; 
                    Vector2 accel = Vector2.Normalize(toPlayer) * Acceleration;
                    velocity = accel; 
                } else {
                    velocity = Vector2.Zero; 
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            animatedSprite.Draw(spriteBatch, position, SpriteEffects.None, 0, null, null, 1.1f);
        }
    }
}
