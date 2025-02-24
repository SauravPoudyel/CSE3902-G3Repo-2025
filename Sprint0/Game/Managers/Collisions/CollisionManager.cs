using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Sprint0
{
    public class CollisionManager
    {
        public void Update(Dictionary<string, Entity> entities)
        {
            CheckCollisions(entities.Values.ToList());
        }

        private void CheckCollisions(List<Entity> entities)
        {
            foreach (Entity actor in entities)
            {
                Rectangle futureActorBounds = actor.PredictFutureBounds();
                foreach (Entity target in entities)
                {
                    if (actor == target)
                        continue;
                    Rectangle futureTargetBounds = target.PredictFutureBounds();
                    
                    if (futureActorBounds.Intersects(futureTargetBounds))
                        ResolveCollision(actor, target);
                }
            }
        }

        private void ResolveCollision(Entity actor, Entity target)
        {
            string commandKey = CollisionResponse.GetResponseCommand(actor.GetType(), target.GetType());
            if (commandKey != null)
            {
                var parameters = new Dictionary<string, object>
                {
                    { "actor", actor },
                    { "target", target }
                };
                actor.EnqueueCommand(commandKey, parameters);
            }
        }
    }
}
