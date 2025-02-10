using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Sprint0
{
    public class Projectile : Entity
    {
        private float colorChangeTimer;
        private readonly Color[] colors = { Color.Red, Color.Yellow, Color.Purple, Color.Orange };
        private int colorIndex;

        public Projectile(ContentManager content)
        {
            // Use a static sprite from the sprite sheet
            AddSprite("Default", new StaticSprite());
            sprites["Default"].LoadContent(content, "TDTanksAllSprites", 0, 1028, 34, 32, 1); 
            SetSprite("Default");

            velocity = new Vector2(200, 0); // Projectile speed
            colorIndex = 0;
            colorChangeTimer = 0f;
        }

        public override void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
            position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Flashing effect - switch colors periodically
            colorChangeTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (colorChangeTimer > 0.1f)
            {
                colorIndex = (colorIndex + 1) % colors.Length;
                colorChangeTimer = 0f;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            SpriteEffects effects = SpriteEffects.None;
            Color flashColor = colors[colorIndex];

            // Pass the color effect directly into sprite.Draw()
            sprite.Draw(spriteBatch, position, effects, flashColor);
        }
    }
}
