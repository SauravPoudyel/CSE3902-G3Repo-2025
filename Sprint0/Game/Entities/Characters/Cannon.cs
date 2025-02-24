using System;
using Microsoft.Xna.Framework;
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
        public float TipDistance { get; set; }   // Desired distance from center = 30f
        public SpriteEffects CannonEffects { get; set; }

        public Cannon(ISprite sprite, IEntity owner, Vector2 pivot, float tipDistance,
                      float initialRotation, float angularVelocity, float lowerBound, float upperBound)
        {
            this.sprite = sprite;
            Owner = owner;
            Pivot = pivot;
            TipDistance = tipDistance;
            Rotation = initialRotation;
            AngularVelocity = angularVelocity;
            LowerBound = lowerBound;
            UpperBound = upperBound;
            CannonEffects = SpriteEffects.None;
        }

        public override void Update()
        {    
            float elapsed = Globals.FRAMETIME;
            Rotation += AngularVelocity * elapsed;
            if (Rotation > UpperBound)
            {
                Rotation = UpperBound;
                AngularVelocity = -Math.Abs(AngularVelocity);
            }
            else if (Rotation < LowerBound)
            {
                Rotation = LowerBound;
                AngularVelocity = Math.Abs(AngularVelocity);
            }
        }

        public Vector2 GetTipPosition()
        {
            Vector2 spriteTip = new Vector2(12, 60);
            Vector2 localOffset = spriteTip - Pivot;
            float scale = TipDistance / localOffset.Length();
            Vector2 desiredLocalOffset = localOffset * scale;

            // Rotate this offset by the cannon's rotation.
            Vector2 rotatedOffset = Vector2.Transform(desiredLocalOffset, Matrix.CreateRotationZ(Rotation));

            // Owner.GetPosition() is the player's center.
            return Owner.GetPosition() + rotatedOffset;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw the cannon with its pivot at the owner's center.
            sprite.Draw(spriteBatch, Owner.GetPosition(), CannonEffects, Rotation, Pivot);
        }
    }
}
