using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Sprint0
{
    public class SniperProjectile : Projectile
    {
        public SniperProjectile(ContentManager content, string entityKey, Character owner) : base(content, entityKey, owner)
        {
            AddSprite("Sniper", new StaticSprite());
            sprites["Sniper"].LoadContent(content, "TDTanksAllSprites", 120, 1040, 20, 20, 1); 

            SetSprite("Sniper");

            baseSpeed = 1500f; 
            maxDistance = 1000f; 
            colors = new Color[] { Color.Blue, Color.LightBlue, Color.DarkBlue }; 
            damage = 50;
        }

        public override void Update()
        {
            // special behavior (acceleration, explosion timer) here
            base.Update();
        }
    }
}
