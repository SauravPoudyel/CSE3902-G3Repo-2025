using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Sprint0.GameCommands;

namespace Sprint0;

public class ExitGameButton : Button
{
    public ExitGameButton(Texture2D texture, Texture2D backgroundTexture, Vector2 vector, Dictionary<string, object> paramters) {
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
            new QuitCommand().Execute(parameters);
        }
        previousMouseState = mouseState;
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        Color backgroundColor = IsHovered() ? Color.White : Color.White * 0.5f;
        spriteBatch.Draw(backgroundTexture, position, backgroundColor);
        int x = (int)position.X;
        int y = (int)position.Y;
        spriteBatch.Draw(texture, new Rectangle(x, y, 120, 120), Color.White);
    }
}