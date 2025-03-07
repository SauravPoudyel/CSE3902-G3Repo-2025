using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Sprint0.GameCommands;

namespace Sprint0;

public class StartButton : Button
{
    public StartButton(Texture2D texture, Texture2D backgroundTexture, Vector2 vector, Dictionary<string, object> paramters) {
        this.parameters = paramters;
        this.texture = texture;
        this.backgroundTexture = backgroundTexture;
        this.position = vector;
        this.bounds = new Rectangle((int)vector.X, (int)vector.Y, backgroundTexture.Width, backgroundTexture.Height);
    }
    public override void Update() {
        mouseState = Mouse.GetState();
        if (IsClicked())
        {
            new StartGameCommand().Execute(parameters);
        }
        previousMouseState = mouseState;
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        Color backgroundColor = IsHovered() ? Color.White : Color.White * 0.5f;
        spriteBatch.Draw(backgroundTexture, position, backgroundColor);
        int x = (int)position.X + (backgroundTexture.Width - texture.Width) / 2;
        int y = (int)position.Y + (backgroundTexture.Height - texture.Height) / 2;
        spriteBatch.Draw(texture, new Rectangle(x, y, 68, 68), Color.White);
    }
}