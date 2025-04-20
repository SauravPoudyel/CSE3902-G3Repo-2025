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
        private float effectRotation = 0f;
        private Entity followTarget = null;
        private Vector2 followOffset = Vector2.Zero;
        private string entityKey;
        private bool hasSentDestroyCommand = false;
        private ContentManager content;
        public bool IsFinished { get; private set; } = false;
        public EffectType effectType;
        public bool didDamage = false;     
        private bool damagesPlayer = true;  

        private int _totalLoops;
        private int _remainingLoops;
        private float fireScale = 4f;      

        public Effect(ContentManager content, Vector2 spawnPosition, string entityKey, EffectType effectType)
        {
            this.content = content;
            this.position = spawnPosition;
            this.entityKey = entityKey;
            this.effectType = effectType;

            switch (effectType)
            {
                case EffectType.Fire:
                    _totalLoops = 8;
                    InitializeFireEffect();
                    break; 
                case EffectType.BoostFire:
                    _totalLoops = 2;
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
        }

        private void InitializeFireEffect()
        {
            const string FireSheetName = "Fire";
            const int FrameWidth  = 32;
            const int FrameHeight = 32;
            const int TotalFrames = 8;
            const float FrameTime = 0.04f;

            // choose scale
            fireScale = (effectType == EffectType.BoostFire) ? 2.8f : 4f;

            effectSprite = new AnimatedSprite(FrameTime);
            effectSprite.LoadContent(content, FireSheetName, 0, 0, FrameWidth, FrameHeight, TotalFrames);

            int width = (int)(FrameWidth  * fireScale);
            int height = (int)(FrameHeight * fireScale);

            // center on spawnPosition
            position = new Vector2(
                position.X - width * 0.5f,
                position.Y - height * 0.5f
            );

            bounds = new Rectangle((int)position.X, (int)position.Y, width, height);
            effectDuration = FrameTime * TotalFrames;
        }

        private void InitializeEffect(int rowY, int frames, float cycleSpeed, int width, int height)
        {
            effectDuration = cycleSpeed * frames;
            effectSprite = new AnimatedSprite(cycleSpeed);
            effectSprite.LoadContent(content, "EffectSprites", 0, rowY, width, height, frames);
            bounds = new Rectangle((int)position.X, (int)position.Y, width, height);
        }

        private void InitializeDefaultEffect()
        {
            effectDuration = 1f;
            effectSprite = new StaticSprite();
            effectSprite.LoadContent(content, "EffectSprites", 0, 0, 128, 128, 1);
            bounds = new Rectangle((int)position.X, (int)position.Y, 128, 128);
        }

        public override void Update()
        {
            if (IsFinished) return;

            if (followTarget != null)
            {
                if (followTarget is Player p)
                    effectRotation = p.bodyRotation;

                Vector2 worldOffset = Vector2.Transform(
                    followOffset,
                    Matrix.CreateRotationZ(effectRotation)
                );

                if (effectType == EffectType.Fire || effectType == EffectType.BoostFire)
                {
                    Vector2 pivot = new Vector2(bounds.Width * 0.5f, bounds.Height * 0.5f);
                    position = followTarget.GetPosition() + worldOffset - pivot;
                }
                else
                {
                    position = followTarget.GetPosition() + worldOffset;
                }

                bounds = new Rectangle(
                    (int)position.X,
                    (int)position.Y,
                    bounds.Width,
                    bounds.Height
                );
            }

            timer += Globals.FRAMETIME;
            effectSprite.Update();

            if (timer >= effectDuration)
            {
                _remainingLoops--;
                if (_remainingLoops > 0)
                {
                    timer = 0f;
                    if (effectSprite is AnimatedSprite anim)
                        anim.ResetAnimation();
                }
                else
                {
                    IsFinished = true;
                    if (!hasSentDestroyCommand)
                    {
                        hasSentDestroyCommand = true;
                        commandQueue.Enqueue(new CommandRequest(
                            "DestroyEntity",
                            new Dictionary<string, object>{{ "destroyEntity", entityKey }}
                        ));
                    }
                }
            }

            if (effectType == EffectType.Fire)
                CheckInitialPlayerCollision();
        }

        private void CheckInitialPlayerCollision()
        {
            Player player = Player.Instance;
            if (player != null && bounds.Intersects(player.Bounds) && damagesPlayer)
            {
                didDamage = true;  // record that we damaged the player
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
            if (IsFinished) return;

            switch (effectType)
            {
                case EffectType.Fire:
                case EffectType.BoostFire:
                    DrawFire(spriteBatch);
                    break;
                default:
                    DrawDefault(spriteBatch);
                    break;
            }
        }

        private void DrawFire(SpriteBatch spriteBatch)
        {
            const int OutWidth = 32, OH = 32;
            float scale = fireScale;
            int width = (int)(OutWidth * scale), height = (int)(OH * scale);

            effectSprite.Draw(
                spriteBatch,
                position + new Vector2(width * 0.5f, height * 0.5f),
                SpriteEffects.None,
                effectRotation,
                new Vector2(OutWidth * 0.5f, OH * 0.5f),
                null,
                scale
            );
        }

        private void DrawDefault(SpriteBatch spriteBatch)
        {
            effectSprite.Draw(
                spriteBatch,
                position,
                SpriteEffects.None,
                0f,
                null,
                null,
                1f
            );
        }

        public void AttachTo(Entity target, Vector2 offset)
        {
            followTarget = target;
            followOffset = offset;
        }

        public void SetRotation(float rotation)
            => effectRotation = rotation;

        public void DisableDamage()
            => damagesPlayer = false;
    }
}
