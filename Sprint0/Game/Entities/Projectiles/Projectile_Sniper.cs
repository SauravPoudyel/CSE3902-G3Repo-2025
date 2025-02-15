using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Sprint0
{
    public class SniperProjectile : Projectile
    {
        public SniperProjectile(ContentManager content, string entityKey) : base(content, entityKey)
        {
            AddSprite("Sniper", new StaticSprite());
            sprites["Sniper"].LoadContent(content, "TDTanksAllSprites", 0, 1060, 34, 32, 1);

            SetSprite("Sniper");

            baseSpeed = 1500f; 
            maxDistance = 1000f; 
            colors = new Color[] { Color.Blue, Color.LightBlue, Color.DarkBlue }; 
        }

        public override void Update(GameTime gameTime)
        {
            // special behavior (acceleration, explosion timer) here
            base.Update(gameTime);
        }
    }
}
