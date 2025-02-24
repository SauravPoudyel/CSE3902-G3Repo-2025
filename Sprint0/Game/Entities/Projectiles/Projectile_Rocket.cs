using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;


namespace Sprint0
{
     public class RocketProjectile : Projectile
    {   
        public RocketProjectile(ContentManager content, string entityKey) : base(content, entityKey)
        {
            AddSprite("Rocket", new StaticSprite());
            sprites["Rocket"].LoadContent(content, "TDTanksAllSprites", 120, 1040, 20, 20, 1); 

            SetSprite("Rocket");

            baseSpeed = 200f; 
            maxDistance = 250f; 
            colors = new Color[] { Color.Green, Color.DarkGreen, Color.Lime };
        }
        public override void Update()
        {
            // special behavior (acceleration, explosion timer) here
            base.Update();
        }
    }
}
