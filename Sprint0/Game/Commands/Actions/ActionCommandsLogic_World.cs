using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Sprint0
{
    public static class ActionCommandsLogic_World
    {
        public static void HandleRequestPlayerPositionCommand(
            GameManager gameManager,
            Mob mob)
        {
            if (!(gameManager.GetEntity("player") is Player player) || player.isInvis)
                return;

            var playerPos = player.GetPosition();
            var playerBounds = player.GetBounds();
            var blockers = new List<Entity>();

            foreach (var entity in gameManager.GetEntities().Values)
                if (entity is IObtuse)
                    blockers.Add(entity);

            if (RayTracer.IsPlayerFullyExposed(
                    mob.GetPosition(),
                    playerBounds,
                    blockers
                ))
            {
                mob.UpdateKnownPlayerPosition(playerPos);
            }
        }

        public static void HandleHealRadiusCommand(
            GameManager gameManager,
            Vector2 origin,
            float healRadius,
            int healAmount)
        {
            foreach (var entity in gameManager.GetEntities().Values)
            {
                if (entity is Mob mob && Vector2.Distance(origin, mob.GetPosition()) <= healRadius)
                    mob.ChangeHealth(healAmount);
            }
        }
    }
}
