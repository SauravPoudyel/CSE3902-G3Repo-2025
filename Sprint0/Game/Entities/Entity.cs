using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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
    }

    public class Entity : IEntity
    {
        protected Vector2 position;
        protected Vector2 prevPosition;
        protected Vector2 velocity;
        protected ISprite sprite;
        protected int spriteHeight, spriteWidth; // Used for collision detection
        protected bool hasSprite = true;
        protected Dictionary<string, ISprite> sprites;
        protected Rectangle bounds;
        public Rectangle Bounds => bounds;
        protected Queue<CommandRequest> commandQueue;
        public Entity Owner { get; set; }

        // Unique key for this entity.
        public string EntityKey { get; set; }
        private static int _entityCounter = 0;

        public Entity()
        {
            sprites = new Dictionary<string, ISprite>();
            commandQueue = new Queue<CommandRequest>();
            // Automatically assign a unique key.
            EntityKey = "Entity_" + _entityCounter.ToString();
            _entityCounter++;
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
            UpdateBounds();
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
            UpdateBounds();
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
                UpdateBounds();
            }
        }

        public Rectangle GetBounds()
        {
            return bounds;
        }

        public Rectangle PredictFutureBounds()
        {
            Vector2 nextPos = position + velocity * Globals.FRAMETIME;
            return new Rectangle((int)(nextPos.X - spriteWidth / 2), (int)(nextPos.Y - spriteHeight / 2), spriteWidth, spriteHeight);
        }

        public void UpdateBounds()
        {

            bounds = new Rectangle(
                (int)(position.X - spriteWidth / 2),
                (int)(position.Y - spriteHeight / 2),
                spriteWidth,
                spriteHeight
            );
        }

        public virtual void LoadContent(ContentManager content, string assetName, int startX, int startY, int frameWidth, int frameHeight, int frameCount)
        {
            if (sprite == null)
            {
                throw new System.NullReferenceException("Sprite is not initialized. Call SetSprite before LoadContent.");
            }
            sprite.LoadContent(content, assetName, startX, startY, frameWidth, frameHeight, frameCount);
            UpdateBounds();
        }

        public virtual void Update()
        {
            prevPosition = position;
            position += velocity * Globals.FRAMETIME;
            UpdateBounds();

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
    }
}
  
