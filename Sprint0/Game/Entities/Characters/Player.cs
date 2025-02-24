using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Sprint0
{
    public class Player : Character
    {
        private float bodyRotation;

        // store effect remporary effect/powerup timers in one dictionary
        public Dictionary<string, float> EffectTimers = new Dictionary<string, float>();

        // properties directly modified by pickups.
        public float speedMultiplier = 1f;
        public bool shieldActive = false;

        ISprite trailSprite  = new StaticSprite(); 

        public Player(ContentManager content) : base(content)
        {
            bodyRotation = 0f;

            AddSprite("TankBody", new AnimatedSprite(0.3f));
            sprites["TankBody"].LoadContent(content, "TDTanksAllSprites", 795, 1052, 74, 76, 1);
            SetSprite("TankBody");

            ISprite cannonSprite = new AnimatedSprite(0.3f);
            cannonSprite.LoadContent(content, "TDTanksAllSprites", 1060, 837, 24, 60, 1);
            cannon = new Cannon(cannonSprite, this, new Vector2(12, 5), 50f,
                    0f, MathHelper.ToRadians(20), MathHelper.PiOver2, MathHelper.Pi + MathHelper.PiOver2);


            trailSprite.LoadContent(content, "TDTanksAllSprites", 952, 645, 73, 100, 1);

            currentProjectileVariables["projectileType"] = "Default";
        }

        public override void Update()
        {
            prevPosition = position;

            float turnSpeed = 0.05f;  
            bodyRotation += velocity.X * turnSpeed * Globals.FRAMETIME;

            Vector2 forwardDirection = new Vector2((float)System.Math.Sin(bodyRotation), -(float)System.Math.Cos(bodyRotation));
            float movementSpeed = -velocity.Y;
            position += forwardDirection * movementSpeed * speedMultiplier * Globals.FRAMETIME;

            float spriteWidth = 74f;
            float spriteHeight = 76f;
            float halfWidth = spriteWidth * 0.5f;
            float halfHeight = spriteHeight * 0.5f;

            float cosRotation = (float)System.Math.Cos(bodyRotation);
            float sinRotation = (float)System.Math.Sin(bodyRotation);

            // Define the four corners of the bounding box relative to the sprite's center
            Vector2[] boundingBoxCorners = new Vector2[4];
            boundingBoxCorners[0] = new Vector2(-halfWidth, -halfHeight);
            boundingBoxCorners[1] = new Vector2(halfWidth, -halfHeight);
            boundingBoxCorners[2] = new Vector2(halfWidth, halfHeight);
            boundingBoxCorners[3] = new Vector2(-halfWidth, halfHeight);

            // Apply rotation transformation to each corner
            for (int i = 0; i < 4; i++)
            {
                float x = boundingBoxCorners[i].X;
                float y = boundingBoxCorners[i].Y;
                boundingBoxCorners[i].X = x * cosRotation - y * sinRotation + position.X;
                boundingBoxCorners[i].Y = x * sinRotation + y * cosRotation + position.Y;
            }

            // Compute the smallest and largest X and Y values to get the rotated bounding box dimensions
            float minX = boundingBoxCorners[0].X, maxX = boundingBoxCorners[0].X;
            float minY = boundingBoxCorners[0].Y, maxY = boundingBoxCorners[0].Y;

            for (int i = 1; i < 4; i++)
            {
                if (boundingBoxCorners[i].X < minX) minX = boundingBoxCorners[i].X;
                if (boundingBoxCorners[i].X > maxX) maxX = boundingBoxCorners[i].X;
                if (boundingBoxCorners[i].Y < minY) minY = boundingBoxCorners[i].Y;
                if (boundingBoxCorners[i].Y > maxY) maxY = boundingBoxCorners[i].Y;
            }

            bounds = new Rectangle((int)minX, (int)minY, (int)(maxX - minX), (int)(maxY - minY));

            if (System.Math.Abs(movementSpeed) > 0.1f)
                sprite.Update();

            // Update all active effect timers using the helper class
            EffectFactory.UpdateEffects(this);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 tankCenter = new Vector2(37, 38);
            if(trailSprite != null){
                trailSprite.Draw(spriteBatch, position -  (new Vector2(1, 1) * bodyRotation), SpriteEffects.None, bodyRotation, tankCenter, Color.White);
            }
            sprite.Draw(spriteBatch, position, SpriteEffects.FlipVertically, bodyRotation, tankCenter, Color.White);
            cannon.Draw(spriteBatch);
        }
    }
}
