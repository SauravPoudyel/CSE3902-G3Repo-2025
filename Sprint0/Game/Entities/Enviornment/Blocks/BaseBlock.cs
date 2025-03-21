using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0
{
    public abstract class BaseBlock : Entity
    {
        protected AnimatedSprite animatedSprite;
        protected float frameTime = 0.3f;
        public float Rotation {get; set;} = 0f; 
        public float Scale { get; set; } = 1.0f;

        public void SetFrameTime(float newFrameTime)
        {
            frameTime = newFrameTime;
            animatedSprite?.SetFrameTime(frameTime);
        }

        public abstract void LoadBlockContent(ContentManager content, EntityKeys.BlockType blockType);

        public override void Update()
        {
            prevPosition = position;
            animatedSprite?.Update();
            position += velocity * Globals.FRAMETIME;
            UpdateBounds();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            animatedSprite?.Draw(spriteBatch, position, SpriteEffects.None, Rotation, null, null, Scale);
        }
    }
}
