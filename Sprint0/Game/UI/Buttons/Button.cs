using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sprint0;

public abstract class Button
{
    public Rectangle bounds {get; protected set; }
    public Texture2D texture {get; protected set; }
    public Texture2D backgroundTexture {get; protected set; }
    public Vector2 position {get; protected set; }
    public Dictionary<string, object> parameters {get; protected set; }
    public MouseState previousMouseState {get; protected set; }    
    public MouseState mouseState {get; protected set; }
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