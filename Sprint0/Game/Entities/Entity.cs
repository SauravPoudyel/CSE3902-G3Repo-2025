using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Sprint0
{
    public interface IEntity
    {
        Vector2 GetPosition();
        void SetPosition(Vector2 position);
        Vector2 GetVelocity();
        void SetVelocity(Vector2 velocity);
        Rectangle GetBounds();
        void LoadContent(ContentManager content, string assetName, int startX, int startY, int frameWidth, int frameHeight, int frameCount);
        void Update();
        void Draw(SpriteBatch spriteBatch);
        void SetSprite(string key);
        ISprite GetSprite();
        void OnCollide(Entity ActedUponEntity); 
    }
    
    public class Entity : IEntity
    {
        protected Vector2 position; // protected so that subclasses can access it
        protected Vector2 prevPosition; // protected so that subclasses can access it
        protected Vector2 velocity;
        protected ISprite sprite;
        protected bool hasSprite = true; // default to every entity having a sprite, set to false if not
        protected Dictionary<string, ISprite> sprites;
        protected Rectangle bounds;

        protected Queue<CommandRequest> commandQueue;

        public Entity()
        {
            sprites = new Dictionary<string, ISprite>(); 
            commandQueue = new Queue<CommandRequest>();
        }
        public Vector2 GetPosition()
        {
            return position;
        }

        public Vector2 GetPreviousPosition()
        {
            return prevPosition;
        }

        public virtual void SetPosition(Vector2 position)
        {
            this.position = position;
        }

        public Vector2 GetVelocity()
        {
            return velocity;
        }

        public void SetVelocity(Vector2 velocity)
        {
            this.velocity = velocity; 
        }
        public Queue<CommandRequest> GetCommandQueue()
        {
            return commandQueue;
        }

        public void EnqueueCommand(string commandKey, Dictionary<string, object> parameters)
        {
            commandQueue.Enqueue(new CommandRequest(commandKey, parameters));
        }
        public ISprite GetSprite()
        {
            return sprite;
        }

        public void SetSprite(ISprite sprite)
        {
            this.sprite = sprite;
        }

        public void AddSprite(string key, ISprite sprite)
        {
            sprites[key] = sprite;
        }

        public void SetSprite(string key)
        {
            if (sprites.ContainsKey(key))
            {
                sprite = sprites[key];
            }
        }
        
        public Rectangle GetBounds()
        {
            return bounds; 
        }

        public Rectangle PredictFutureBounds()
        {
            Vector2 nextPosition = position + velocity * Globals.FRAMETIME; 
            return new Rectangle((int)nextPosition.X,(int)nextPosition.Y, bounds.Width, bounds.Height); 
        }

        public virtual void LoadContent(ContentManager content, string assetName, int startX, int startY, int frameWidth, int frameHeight, int frameCount)
        {
            if (sprite == null)
            {
                throw new NullReferenceException("Sprite is not initialized. Call SetSprite before LoadContent.");
            }
            sprite.LoadContent(content, assetName, startX, startY, frameWidth, frameHeight, frameCount);
        }

        public virtual void Update()
        {
            if (hasSprite && sprite != null)
            {
                sprite.Update();
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (hasSprite && sprite != null)
            {
                sprite.Draw(spriteBatch, position);
            }
        }

        public virtual void OnCollide(Entity entityActedUpon)
        {
        }
    }
}
