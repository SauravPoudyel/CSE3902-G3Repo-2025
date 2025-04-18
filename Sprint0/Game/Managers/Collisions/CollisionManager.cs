using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using static Sprint0.CollisionCommands;
using static Sprint0.EntityKeys;

namespace Sprint0
{
    public class CollisionManager
    {
        private Dictionary<Entity, bool> previousCollisionStates = new Dictionary<Entity, bool>();

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

            CheckFireCollisions(entities);
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

        private void CheckFireCollisions(List<Entity> entities)
        {
            /* Player player = entities.OfType<Player>().FirstOrDefault();
            if (player == null) return; */
            foreach(Character character in entities.OfType<Character>()) {
                foreach (Effect effect in entities.OfType<Effect>().Where(e => e.effectType == EffectType.Fire))
                {
                    bool isCollidingNow = effect.GetBounds().Intersects(character.GetBounds());
                    bool wasCollidingPreviously = previousCollisionStates.TryGetValue(effect, out bool state) && state;

                    if (isCollidingNow && !wasCollidingPreviously)
                    {
                        // Trigger enter event
                        var parameters = new Dictionary<string, object> { { "actor", character }, { "target", effect } };
                        new CollisionHurtCommand().Execute(parameters);
                    }
                    else if (!isCollidingNow && wasCollidingPreviously)
                    {
                        // Trigger exit event
                        var parameters = new Dictionary<string, object> { { "actor", character }, { "target", effect } };
                        new FireCollisionExitCommand().Execute(parameters);
                    }

                    previousCollisionStates[effect] = isCollidingNow;
                }
            }
        }
    }
}
