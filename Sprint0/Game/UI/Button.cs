using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Sprint0
{
    public class Button
    {
        public Rectangle Bounds;
        public Texture2D Texture;
        public SpriteFont Font;
        public string Text;
        public ICommand Command;
        public Dictionary<string, object> Parameters;

        public Button(Texture2D texture, SpriteFont font, Rectangle bounds, string text, ICommand command, Dictionary<string, object> parameters)
        {
            Texture = texture;
            Font = font;
            Bounds = bounds;
            Text = text;
            Command = command;
            Parameters = parameters;
        }

        public bool ContainsPoint(Point p)
        {
            return Bounds.Contains(p);
        }

        public void Click()
        {
            if (Command != null)
            {
                Command.Execute(Parameters);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Bounds, Color.White);
            Vector2 textSize = Font.MeasureString(Text);
            Vector2 textPosition = new Vector2(Bounds.X + (Bounds.Width - textSize.X) / 2,
                                               Bounds.Y + (Bounds.Height - textSize.Y) / 2);
            spriteBatch.DrawString(Font, Text, textPosition, Color.Black);
        }
    }
}
