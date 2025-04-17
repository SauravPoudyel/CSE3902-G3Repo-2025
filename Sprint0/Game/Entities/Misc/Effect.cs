using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using static Sprint0.CollisionCommands;
using static Sprint0.EntityKeys;

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
        public EntityKeys.EffectType effectType; 
        public bool didDamage = false;

        private int _totalLoops;
        private int _remainingLoops;

        public Effect(ContentManager content, Vector2 spawnPosition, string entityKey, EntityKeys.EffectType effectType)
        {
            this.content = content;
            this.position = spawnPosition;
            this.entityKey = entityKey;
            this.hasSentDestroyCommand = false;
            this.timer = 0f;
            this.IsFinished = false;
            this.effectType = effectType;

            switch (effectType)
            {
                case EffectType.Fire:
                    _totalLoops = 8;
                    InitializeFireEffect();
                    break;

                case EffectType.Explosion:
                    InitializeEffect(0, 8, 0.08f, 128, 128);
                    break;

                case EffectType.Shield:
                    InitializeEffect(128, 12, 0.08f, 128, 128);
                    break;

                case EffectType.TeleportOut:
                    InitializeEffect(256, 12, 0.03f, 128, 128);
                    break;

                case EffectType.TeleportIn:
                    InitializeEffect(384, 12, 0.07f, 128, 128);
                    break;

                default:
                    _totalLoops = 1;
                    InitializeDefaultEffect();
                    break;
            }

            _remainingLoops = _totalLoops;
            bounds = new Rectangle((int)position.X, (int)position.Y, 128, 128);
        }

        private void InitializeFireEffect()
        {
            const string FireSheetName = "Fire";
            const int FrameWidth = 32;
            const int FrameHeight = 32;
            const int TotalFrames = 8;
            const float FrameTime = 0.04f;
            const float FireScale = 4.0f;

            effectSprite = new Sprite(FrameTime);
            effectSprite.LoadContent(
                content,
                FireSheetName,
                0,
                0,
                FrameWidth,
                FrameHeight,
                TotalFrames
            );

            int scaledWidth = (int)(FrameWidth * FireScale);
            int scaledHeight = (int)(FrameHeight * FireScale);

            position = new Vector2(position.X - (scaledWidth / 2), position.Y - (scaledHeight / 2));

            bounds = new Rectangle((int)position.X, (int)position.Y, scaledWidth, scaledHeight);

            effectDuration = FrameTime * TotalFrames;
        }

        private void InitializeEffect(int rowY, int frames, float cycleSpeed, int width, int height)
        {
            effectDuration = cycleSpeed * frames;
            effectSprite = new Sprite(cycleSpeed);
            effectSprite.LoadContent(content, "EffectSprites", 0, rowY, width, height, frames);
        }

        private void InitializeDefaultEffect()
        {
            const float DefaultDuration = 1f;
            effectDuration = DefaultDuration;
            effectSprite = new Sprite();
            effectSprite.LoadContent(content, "EffectSprites", 0, 0, 128, 128, 1);
        }

        public override void Update()
        {
            if (IsFinished) return;

            timer += Globals.FRAMETIME;
            effectSprite.Update();

            if (timer >= effectDuration)
            {
                _remainingLoops--;

                if (_remainingLoops > 0)
                {
                    timer = 0f;
                    if (effectSprite is Sprite animatedSprite)
                    {
                        animatedSprite.ResetAnimation();
                    }
                }
                else
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

            if (effectType == EffectType.Fire)
            {
                CheckInitialPlayerCollision();
            }
        }

        private void CheckInitialPlayerCollision()
        {
            Player player = Player.Instance;
            if (player != null && this.Bounds.Intersects(player.Bounds))
            {
                // Trigger collision event manually
                var parameters = new Dictionary<string, object>
            {
                { "actor", player },
                { "target", this }
            };
                new CollisionHurtCommand().Execute(parameters);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsFinished)
            {
                switch (effectType)
                {
                    case EffectType.Fire:
                        DrawFire(spriteBatch);
                        break;
                    default:
                        DrawDefault(spriteBatch);
                        break;
                }
            }
        }

        private void DrawFire(SpriteBatch spriteBatch)
        {
            const int OriginalWidth = 32;
            const int OriginalHeight = 32;
            const float Scale = 4f;
            const int scaledWidth = (int) (OriginalWidth * Scale);
            const int scaledHeight = (int)(OriginalWidth * Scale);

            effectSprite.Draw(
        spriteBatch,
        position + new Vector2(scaledWidth / 2, scaledHeight / 2),
        effects: SpriteEffects.None,
        scale: Scale,
        pivot: new Vector2(OriginalWidth / 2, OriginalHeight / 2)
    );
        }

        private void DrawDefault(SpriteBatch spriteBatch)
        {
            effectSprite.Draw(
                spriteBatch,
                position,
                effects: SpriteEffects.None,
                scale: 1f
            );
        }
    }
}
