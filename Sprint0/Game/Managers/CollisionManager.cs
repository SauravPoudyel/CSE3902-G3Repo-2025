using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Sprint0
{
    public class CollisionManager
    {
        public void Update(GameTime gameTime, Dictionary<string, Entity> entities)
        {
            CheckCollisions(entities.Values.ToList());  
        }

        public void CheckCollisions(List<Entity> entities)
        {
            foreach (var entityActor in entities)
            {
                foreach (var entityActedUpon in entities)
                {
                    if (entityActor != entityActedUpon && entityActor.GetBounds().Intersects(entityActedUpon.GetBounds()))
                    {
                        entityActor.OnCollide(entityActedUpon); 
                    }
                }
            }
        }
    }
}