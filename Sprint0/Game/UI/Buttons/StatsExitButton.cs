using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Sprint0.GameCommands;

namespace Sprint0
{
    class StatsExitButton : Button
    {
        public StatsExitButton(Texture2D texture, Vector2 vector, Dictionary<string, object> paramters)
        {
            this.parameters = paramters;
            this.texture = texture;
            this.position = vector;
            this.bounds = new Rectangle((int)vector.X, (int)vector.Y, this.texture.Width, this.texture.Height);
        }
        public override void Update()
        {
            mouseState = Mouse.GetState();
            if (IsClicked())
            {
                new CloseStatsCommand().Execute(parameters);
            }
            previousMouseState = mouseState;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Color color = IsHovered() ? Color.White : Color.White * 0.5f;
            spriteBatch.Draw(texture, position, color);
        }
    }
}
