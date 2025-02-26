using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Sprint0
{
    public class Explosion : Entity
    {
        private float timer;
        private float phaseInterval;
        private int phase;
        private List<ISprite> explosionSprites;
        private string entityKey;
        private bool hasSentDestroyCommand;
        public bool IsFinished { get; private set; }
        private ContentManager content;

        public Explosion(ContentManager content, Vector2 spawnPosition, float phaseInterval, string entityKey)
        {
            this.content = content; 
            this.position = spawnPosition; 
            this.entityKey = entityKey;
            this.phaseInterval = phaseInterval;
            this.hasSentDestroyCommand = false;
            timer = 0f;
            phase = 0;
            IsFinished = false;
            explosionSprites = LoadExplosionSprites();

            if (explosionSprites.Count > 0)
                SetSprite(explosionSprites[0]);
        }

        public override void Update()
        {
            timer += Globals.FRAMETIME;

            if (timer >= phaseInterval)
            {
                timer = 0f;
                phase++;

                if (phase < explosionSprites.Count)
                {
                    SetSprite(explosionSprites[phase]);
                }
                else if (!hasSentDestroyCommand)
                {
                    IsFinished = true;
                    hasSentDestroyCommand = true;

                    var destroyParams = new Dictionary<string, object>
                    {
                        {"destroyEntity", entityKey}
                    };

                    commandQueue.Enqueue(new CommandRequest("DestroyEntity", destroyParams));
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsFinished)
                sprite?.Draw(spriteBatch, position, SpriteEffects.None, 0f);
        }

        private List<ISprite> LoadExplosionSprites()
        {
            if (content == null)
                throw new System.NullReferenceException("ContentManager is not assigned in Explosion.");

            List<ISprite> sprites = new List<ISprite>();

            AnimatedSprite exp1 = new AnimatedSprite(0.3f);
            exp1.LoadContent(content, "TDTanksAllSprites", 765, 508, 113, 112, 1);
            sprites.Add(exp1);

            AnimatedSprite exp2 = new AnimatedSprite(0.3f);
            exp2.LoadContent(content, "TDTanksAllSprites", 642, 256, 124, 126, 1);
            sprites.Add(exp2);

            AnimatedSprite exp3 = new AnimatedSprite(0.3f);
            exp3.LoadContent(content, "TDTanksAllSprites", 641, 383, 124, 125, 1);
            sprites.Add(exp3);

            return sprites;
        }
    }
}
