using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Sprint0
{
    public static class EffectFactory
    {
        private static readonly Dictionary<string, Action<Player>> expireActions = new()
        {
            { "SpeedBoost", ResetSpeed },
            { "Shield", DeactivateShield },
            { "FireBoost", ResetCannonAngularVelocity }
        };

        private static void ResetSpeed(Player player)
        {
            player.speedMultiplier = 1f;
        }

        private static void DeactivateShield(Player player)
        {
            player.shieldActive = false;
        }

        private static void ResetCannonAngularVelocity(Player player)
        {
            if (player.cannon != null)
            {
                player.cannon.AngularVelocity = 30f;
            }
        }

        public static void UpdateEffects(Player player)
        {
            // store the player effect timers
            var effects = new List<string>(player.EffectTimers.Keys);

            foreach (var effect in effects)
            {
                player.EffectTimers[effect] -= Globals.FRAMETIME;
                if (player.EffectTimers[effect] <= 0f)
                {
                    // Once the effect timer has expired, reset the player properties to normal
                    if (expireActions.ContainsKey(effect))
                    {
                        expireActions[effect](player);
                    }
                    player.EffectTimers.Remove(effect);
                }
            }
        }
    }
}
