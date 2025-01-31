using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Sprint0.Managers
{
    public class PhysicsManager
    {
        public void Update(GameTime gameTime, List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                // Implement physics updates for each entity
            }

            CheckCollisions(entities); 
            
        }

        private void CheckCollisions(List<Entity> entities)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                for (int j = i + 1; j < entities.Count; j++)
                {
                    if (entities[i].GetBounds().Intersects(entities[j].GetBounds()))
                    {
                        // Handle collision between entities
                    }
                }
            }
        }
    }
}