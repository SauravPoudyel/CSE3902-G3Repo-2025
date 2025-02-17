using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Sprint0
{
    public class Cannon : Entity
    {
        public IEntity Owner { get; private set; }
        public float Rotation { get; set; }
        public float AngularVelocity { get; set; }
        public float LowerBound { get; set; }
        public float UpperBound { get; set; }
        public Vector2 Pivot { get; set; }   // cannon’s rotation center
        public float TipDistance { get; set; }   // Distance from Pivot to spawn the projectile
        public SpriteEffects CannonEffects { get; set; }

        // Primary constructor (all parameters specified)
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

        // Overload with default tip distance of 30 pixels.
        public Cannon(ISprite sprite, IEntity owner, Vector2 pivot,
                      float initialRotation, float angularVelocity, float lowerBound, float upperBound)
            : this(sprite, owner, pivot, 30f, initialRotation, angularVelocity, lowerBound, upperBound)
        {
        }

        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
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
            Vector2 direction = Vector2.Transform(Vector2.UnitY, Matrix.CreateRotationZ(Rotation)) * TipDistance;
            return Owner.GetPosition() + Pivot + direction;
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 ownerPos = Owner.GetPosition();
            sprite.Draw(spriteBatch, ownerPos, CannonEffects, Rotation, Pivot);
        }
    }
}
