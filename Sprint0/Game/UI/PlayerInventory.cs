using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Sprint0
{
    public class PlayerInventory : IScreen
    {
        private List<IHUD> hudElements;

        public bool BlocksInput => false;

        public PlayerInventory(ContentManager content)
        {
            hudElements = new List<IHUD>();
            // Initialize HUD elements here
            hudElements.Add(new ShieldHUD(content.Load<Texture2D>("ShieldIcon")));
            hudElements.Add(new HealthHUD(content.Load<Texture2D>("HealthIcon")));
            hudElements.Add(new AmmoHUD(content.Load<Texture2D>("AmmoIcon")));
            hudElements.Add(new CoinHUD(content.Load<Texture2D>("PickupItemSpritesheet2")));
        }

        public void Update()
        {
            foreach (var hud in hudElements)
            {
                hud.Update();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var hud in hudElements)
            {
                hud.Draw(spriteBatch);
            }
        }

        public void HandleClick(Point clickLocation)
        {
            // No click handling needed for HUD.
        }
    }
}
