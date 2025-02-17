using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Sprint0
{
    public class Cannon : Entity
    {
        public IEntity Owner { get; set; }
        public SpriteEffects cannonEffects = SpriteEffects.None;
        public float Rotation { get; set; }
        public float AngularVelocity { get; set; }
        public float? LowerBound { get; set; }
        public float? UpperBound { get; set; }
        public Vector2 Pivot { get; set; }  // Attachment offset from the owner’s position.
        public Vector2 TipOffset { get; set; }  // Offset from the Pivot to the cannon tip.

        public Cannon() { }

        public Cannon(ISprite sprite, IEntity owner, Vector2 pivot, Vector2 tipOffset, float initialRotation = 0f, float angularVelocity = 0f, float? lowerBound = null, float? upperBound = null)
        {
            this.sprite = sprite;
            Owner = owner;
            Pivot = pivot;
            TipOffset = tipOffset;
            Rotation = initialRotation;
            AngularVelocity = angularVelocity;
            LowerBound = lowerBound;
            UpperBound = upperBound;
        }

        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (LowerBound.HasValue && UpperBound.HasValue)
            {
                Rotation += AngularVelocity * elapsed;
                if (Rotation >= UpperBound.Value)
                {
                    Rotation = UpperBound.Value;
                    AngularVelocity = -Math.Abs(AngularVelocity);
                }
                else if (Rotation <= LowerBound.Value)
                {
                    Rotation = LowerBound.Value;
                    AngularVelocity = Math.Abs(AngularVelocity);
                }
            }
        }

        public Vector2 GetTipPosition()
        {
            // The tip is computed from the owner's position plus the pivot and then the rotated tip offset.
            return Owner.GetPosition() + Pivot + Vector2.Transform(TipOffset, Matrix.CreateRotationZ(Rotation));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw the cannon at the owner's position + Pivot, using Pivot as the origin.
            Vector2 ownerPos = Owner.GetPosition();
            sprite.Draw(spriteBatch, ownerPos, cannonEffects, Rotation, Pivot);
        }
    }
}
