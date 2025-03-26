using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sprint0;

public abstract class Button
{
    public Rectangle bounds;
    public Texture2D texture;
    public Texture2D backgroundTexture;
    public Vector2 position;
    public Dictionary<string, object> parameters;
    public MouseState previousMouseState;
    public MouseState mouseState;
    public bool IsHovered()
    {
        return bounds.Contains(mouseState.Position);
    }

    public bool IsClicked()
    {
        bool clicked = false;
        if (IsHovered())
        {
            clicked = mouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed;
        }
        return clicked;
    }
    public abstract void Update();
    public abstract void Draw(SpriteBatch spriteBatch);

}