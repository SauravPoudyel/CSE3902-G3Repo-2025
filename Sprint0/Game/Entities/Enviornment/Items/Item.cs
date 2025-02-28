using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0
{
    public enum PickupItemType
    {
        SpeedBoost,
        Shield,
        Ammo_default,
        Ammo_shotgun,
        Ammo_sniper,
        Ammo_rocket,
        Ammo_Laser,
        Ammo_Mine,
        Magnet,
        FireRateIncrease,
        MedStrong,
        MedWeak,
        TimeSlow,
        Fly,
        Cloak,
        SilverTag,
        GoldTag, 
        Teleporter
    }

    public abstract class BaseItem : Entity
    {
        protected AnimatedSprite animatedSprite;
        protected float frameTime = 0.15f; // default frame time

        // Derived classes must implement content loading based on type.
        public abstract void LoadItemContent(ContentManager content, PickupItemType itemType);

        public override void Update()
        {
            animatedSprite?.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            animatedSprite?.Draw(spriteBatch, position, SpriteEffects.None, 0f);
        }
    }
}
