using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Sprint0
{
    public class TextSprite : ISprite
    {
        private string text;
        private SpriteFont font;
        private Vector2 position;
        private Color color;

        public TextSprite(Vector2 position, string text, Color color)
        {
            this.position = position;
            this.text = text;
            this.color = color;
        }

        public void LoadContent(ContentManager content, string assetName, int startX, int startY, int frameWidth, int frameHeight, int frameCount)
        {
            font = content.Load<SpriteFont>(assetName);
        }

        public void SetText(string text)
        {
            this.text = text;
        }

        public void Update(GameTime gameTime) {}

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            if (font != null && !string.IsNullOrEmpty(text))
            {
                spriteBatch.DrawString(font, text, position, color);
            }
        }
    }

}
