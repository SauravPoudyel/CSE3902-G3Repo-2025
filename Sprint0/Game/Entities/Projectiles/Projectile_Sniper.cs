using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Sprint0
{
    public class SniperProjectile : Projectile, IRicochet
    {
        public int RicochetCount { get; set; }

        public SniperProjectile(ContentManager content, string entityKey, Character owner)
            : base(content, entityKey, owner)
        {
            AddSprite("Sniper", new Sprite(0.4f));
            sprites["Sniper"].LoadContent(content, "TDTanksAllSprites", 120, 1040, 20, 20, 1);
            SetSprite("Sniper");

            baseSpeed = 800f;
            maxDistance = 1000f;
            colors = new Color[] { Color.Blue, Color.LightBlue, Color.DarkBlue };
            damage = 50;

            // Allow the projectile to ricochet 3 times before being destroyed.
            RicochetCount = 3;
        }

        public override void Update()
        {
            // Special behavior (acceleration, explosion timer) could be added here.
            base.Update();
        }
    }
}
