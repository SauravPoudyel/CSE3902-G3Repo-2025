using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Sprint0;
public interface IEntity
    {
        Vector2 GetPosition();
        void SetPosition(Vector2 position);
        Vector2 GetVelocity();
        void SetVelocity(Vector2 velocity);
        Rectangle GetBounds();
        void LoadContent(ContentManager content, string assetName, int startX, int startY, int frameWidth, int frameHeight, int frameCount);
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }