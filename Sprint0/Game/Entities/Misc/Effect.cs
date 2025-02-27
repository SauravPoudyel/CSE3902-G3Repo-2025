using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Sprint0
{
    public class Effect : Entity
    {
        private float timer;
        private float effectDuration;
        private ISprite effectSprite;
        private string entityKey;
        private bool hasSentDestroyCommand;
        private ContentManager content;
        public bool IsFinished { get; private set; }

        public Effect(ContentManager content, Vector2 spawnPosition, string entityKey, string effectType)
        {
            this.content = content;
            this.position = spawnPosition;
            this.entityKey = entityKey;
            this.hasSentDestroyCommand = false;
            this.timer = 0f;
            this.IsFinished = false;

            // Determine starting Y coordinate based on the effect type.
            int rowY = 0;
            int frames = 0; 
            float cycleSpeed = 0.8f; 
            switch(effectType)
            {
                case "explosion":
                    rowY = 0;
                    frames = 8; 
                    cycleSpeed = 0.08f; 
                    break;
                case "shield":
                    rowY = 128;
                    frames = 12; 
                    cycleSpeed = 0.08f;
                    break;
                case "teleportOut":
                    rowY = 256;
                    frames = 12; 
                    cycleSpeed = 0.03f;  
                    break;
                case "teleportIn":
                    rowY = 384;
                    frames = 12; 
                    cycleSpeed = 0.07f;
                    break;
                default:
                    rowY = 0;
                    effectDuration = 1f; 
                    break;
            }

            effectDuration = cycleSpeed * frames;

            effectSprite = new AnimatedSprite(cycleSpeed);
            effectSprite.LoadContent(content, "EffectSprites", 0, rowY, 128, 128, frames);
            sprite = effectSprite;
        }

        public override void Update()
        {
            if (IsFinished) return;

            timer += Globals.FRAMETIME;
            effectSprite.Update();

            // Once the animation has played completely, enqueue a destroy command.
            if (timer >= effectDuration)
            {
                IsFinished = true;
                if (!hasSentDestroyCommand)
                {
                    hasSentDestroyCommand = true;
                    var destroyParams = new Dictionary<string, object>
                    {
                        { "destroyEntity", entityKey }
                    };
                    commandQueue.Enqueue(new CommandRequest("DestroyEntity", destroyParams));
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsFinished)
            {
                effectSprite.Draw(spriteBatch, position, SpriteEffects.None, 0f);
            }
        }
    }
}
