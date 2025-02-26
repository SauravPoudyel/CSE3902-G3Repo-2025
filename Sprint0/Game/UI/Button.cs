using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Sprint0
{
    public class Button
    {
        public Rectangle Bounds;
        private Texture2D Texture;
        private SpriteFont Font;
        private string Text;
        private ICommand Command;
        private Dictionary<string, object> Parameters;
        private Color defaultColor = Color.White;
        private Color hoverColor = Color.LightGray;
        private Color clickColor = Color.DarkGray;
        private Color currentColor;

        private bool isHovered;
        private bool isClicked;

        public Button(Texture2D texture, SpriteFont font, Rectangle bounds, string text, ICommand command, Dictionary<string, object> parameters)
        {
            Texture = texture;
            Font = font;
            Bounds = bounds;
            Text = text;
            Command = command;
            Parameters = parameters;
            currentColor = defaultColor;
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

        public void Update(MouseState mouseState)
        {
            isHovered = Bounds.Contains(mouseState.Position);
            isClicked = isHovered && mouseState.LeftButton == ButtonState.Pressed;

            if (isClicked)
            {
                currentColor = clickColor;
            }
            else if (isHovered)
            {
                currentColor = hoverColor;
            }
            else
            {
                currentColor = defaultColor;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Bounds, currentColor);
            Vector2 textSize = Font.MeasureString(Text);
            Vector2 textPosition = new Vector2(Bounds.X + (Bounds.Width - textSize.X) / 2,
                                               Bounds.Y + (Bounds.Height - textSize.Y) / 2);
            spriteBatch.DrawString(Font, Text, textPosition, Color.Black);
        }
    }
}
