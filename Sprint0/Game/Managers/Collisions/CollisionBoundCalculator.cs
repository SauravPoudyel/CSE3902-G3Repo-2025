using System;
using Microsoft.Xna.Framework;

namespace Sprint0
{
    public static class CollisionBoundCalculator
    {
        public static Rectangle Calculate(Vector2 position, float bodyRotation, float spriteWidth, float spriteHeight)
        {
            float halfW = spriteWidth * 0.5f, halfH = spriteHeight * 0.5f;
            float cos = (float)Math.Cos(bodyRotation), sin = (float)Math.Sin(bodyRotation);

            Vector2[] corners = new Vector2[]
            {
                new Vector2(-halfW, -halfH),
                new Vector2( halfW, -halfH),
                new Vector2( halfW,  halfH),
                new Vector2(-halfW,  halfH)
            };

            for (int i = 0; i < corners.Length; i++)
            {
                float x = corners[i].X, y = corners[i].Y;
                corners[i] = new Vector2(
                    x * cos - y * sin + position.X,
                    x * sin + y * cos + position.Y
                );
            }

            float minX = corners[0].X, maxX = minX, minY = corners[0].Y, maxY = minY;
            for (int i = 1; i < corners.Length; i++)
            {
                minX = Math.Min(minX, corners[i].X);
                maxX = Math.Max(maxX, corners[i].X);
                minY = Math.Min(minY, corners[i].Y);
                maxY = Math.Max(maxY, corners[i].Y);
            }

            return new Rectangle((int)minX, (int)minY, (int)(maxX - minX), (int)(maxY - minY));
        }
    }
}
