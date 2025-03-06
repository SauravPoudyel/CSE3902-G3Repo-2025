using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sprint0
{
    class textButton : Button
    {
        private SpriteFont font;
        private string text;
        private ICommand command;
        public textButton(Texture2D buttonTexture, Rectangle bound, string text, ICommand command, Dictionary<string, object> gameParams)
        {
            this.font = Globals.FONT;
            this.texture = buttonTexture;
            this.bounds = bound;
            this.text = text;
            this.command = command;
            this.parameters = gameParams;
        }
        override public void Update()
        {
            mouseState = Mouse.GetState();
            if (IsClicked())
            {
                command.Execute(parameters);
            }
            previousMouseState = mouseState;
        }

        override public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, bounds, Color.White);
            Vector2 textSize = font.MeasureString(text);
            Vector2 textPosition = new Vector2(bounds.X + bounds.Width / 2 - textSize.X / 2, bounds.Y + bounds.Height / 2 - textSize.Y / 2);
            spriteBatch.DrawString(font, text, textPosition, Color.Black);
        }
    }
}
