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
        public string effectType; 
        public bool didDamage = false; 
        enum EffectType {
            Explosion, Fire, Shield, TeleportOut, TeleportIn
        }

        public Effect(ContentManager content, Vector2 spawnPosition, string entityKey, string effectType)
        {
            this.content = content;
            this.position = spawnPosition;
            this.entityKey = entityKey;
            this.hasSentDestroyCommand = false;
            this.timer = 0f;
            this.IsFinished = false;
            this.effectType = effectType; 

            // Determine starting Y coordinate based on the effect type.
            int rowY = 0;
            int frames = 0; 
            float cycleSpeed = 0.8f; 
            switch(effectType)
            {
                case nameof(EffectType.Explosion):
                    rowY = 0;
                    frames = 8; 
                    cycleSpeed = 0.08f; 
                    break;
                //The fire effect here is a placeholder, might need a new spritesheet for it
                case nameof(EffectType.Fire):
                    rowY = 0;
                    frames = 1;
                    cycleSpeed = 3.2f;
                    break;
                case  nameof(EffectType.Shield):
                    rowY = 128;
                    frames = 12; 
                    cycleSpeed = 0.08f;
                    break;
                case  nameof(EffectType.TeleportOut):
                    rowY = 256;
                    frames = 12; 
                    cycleSpeed = 0.03f;  
                    break;
                case  nameof(EffectType.TeleportIn):
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
            bounds = new Rectangle((int)position.X, (int)position.Y, 128, 128);
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
