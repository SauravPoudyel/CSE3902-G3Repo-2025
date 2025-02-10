using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Sprint0
{
    public class PhysicsManager
    {
        public void Update(GameTime gameTime, Dictionary<string, Entity> entities)
        {
            foreach (var entity in entities.Values)
            {
                // Implement physics updates for each entity
            }

            CheckCollisions(entities.Values.ToList()); 
            
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