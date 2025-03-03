using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0
{
    public class Cannon : Entity
    {
        public IEntity Owner { get; private set; }
        public float Rotation { get; set; }
        public float AngularVelocity { get; set; }
        public float LowerBound { get; set; }
        public float UpperBound { get; set; }
        public Vector2 Pivot { get; set; }
        public float TipDistance { get; set; }  // desired distance from the pivot
        public Vector2 TipOffset { get; set; } = new Vector2(12, 60);  
        public bool HasFiringEffect { get; set; } = true; 
        public SpriteEffects CannonEffects { get; set; }

        private AnimatedSprite firingEffectSprite;
        private bool showFiringEffect;
        private float effectTimer;
        private const float defaultLowerBound = MathHelper.PiOver2;
        private const float defaultUpperBound = MathHelper.Pi + MathHelper.PiOver2;

        // Added parameter tipOffset with a default value that worked for the player.
        public Cannon(ContentManager content, ISprite sprite, IEntity owner, Vector2 pivot, float tipDistance, Vector2 tipOffset,
                      float initialRotation, float angularVelocity, float lowerBound = defaultLowerBound, float upperBound = defaultUpperBound)
        {
            this.sprite = sprite;
            sprites.Add("default", sprite); // if you need to reset it
            Owner = owner;
            Pivot = pivot;
            TipDistance = tipDistance;
            Rotation = initialRotation;
            AngularVelocity = angularVelocity;
            LowerBound = lowerBound;
            UpperBound = upperBound;
            CannonEffects = SpriteEffects.None;

            showFiringEffect = false;
            effectTimer = 0f;

            firingEffectSprite = new AnimatedSprite(0.1f);
            firingEffectSprite.LoadContent(content, "TDTanksAllSprites", 1025, 56, 39, 50, 1);
            firingEffectSprite.AddFrame(1033, 215, 32, 62);

            TipOffset = tipOffset;
        }

        public override void Update()
        {
            if (showFiringEffect)
            {
                firingEffectSprite.Update();
                effectTimer -= Globals.FRAMETIME;
                if (effectTimer <= 0f)
                {
                    showFiringEffect = false;
                    effectTimer = 0f;
                    firingEffectSprite.ResetAnimation();
                }
            }
        }

        public void TriggerFiringEffect()
        {
            showFiringEffect = true;
            effectTimer = 0.2f;
        }

        public Vector2 GetTipPosition()
        {
            // Calculate the local offset from the pivot to the tip using the provided tipOffset.
            Vector2 localOffset = TipOffset - Pivot;
            float scale = TipDistance / localOffset.Length();
            Vector2 desiredLocalOffset = localOffset * scale;
            Vector2 rotatedOffset = Vector2.Transform(desiredLocalOffset, Matrix.CreateRotationZ(Rotation));
            return Owner.GetPosition() + rotatedOffset;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, Owner.GetPosition(), CannonEffects, Rotation, Pivot);
            if (showFiringEffect && HasFiringEffect)
            {
                Vector2 tip = GetTipPosition(); 
                firingEffectSprite.Draw(spriteBatch, tip, SpriteEffects.None, Rotation, Pivot);
            }
        }
    }
}
