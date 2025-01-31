using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Sprint0
{
    public class StaticSprite : ISprite
    {
        private Texture2D spriteSheet;
        private Rectangle frame;

        public void LoadContent(ContentManager content, string assetName, int startX, int startY, int frameWidth, int frameHeight, int frameCount)
        {
            spriteSheet = content.Load<Texture2D>(assetName);
            frame = new Rectangle(startX, startY, frameWidth, frameHeight);
        }

        public void Update(GameTime gameTime)
        {
            // Static sprite does not need to update
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(spriteSheet, position, frame, Color.White);
        }
    }
}
