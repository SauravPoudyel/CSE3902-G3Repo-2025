using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Sprint0
{
    public abstract class Entity : IEntity
    {
        protected Vector2 position; // protected so that subclasses can access it
        protected Vector2 velocity;
        protected  ISprite sprite;
        protected  Rectangle bounds;

        public Vector2 GetPosition()
        {
            return position;
        }

        public void SetPosition(Vector2 position)
        {
            this.position = position;
        }

        public virtual Vector2 GetVelocity()
        {
            return position;
        }

        public virtual void SetVelocity(Vector2 velocity)
        {
            this.velocity = velocity; 
        }

        public virtual void SetVelocity(float x, float y)
        {
            this.velocity = new Vector2(x, y);
        }

        public ISprite GetSprite()
        {
            return sprite;
        }

        public void SetSprite(ISprite sprite)
        {
            this.sprite = sprite;
        }

        public Rectangle GetBounds()
        {
            return bounds;
        }

        public virtual void LoadContent(ContentManager content, string assetName, int startX, int startY, int frameWidth, int frameHeight, int frameCount)
        {
            if (sprite == null)
            {
                throw new NullReferenceException("Sprite is not initialized. Call SetSprite before LoadContent.");
            }
            sprite.LoadContent(content, assetName, startX, startY, frameWidth, frameHeight, frameCount);
        }

        public abstract void LoadContent(ContentManager content);

        public virtual void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, position);
        }
    }
}
