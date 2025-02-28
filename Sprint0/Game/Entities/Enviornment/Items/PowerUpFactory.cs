using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Sprint0
{
    public static class PowerUpFactory
    {
        private static readonly Dictionary<string, Action<Player>> expireActions = new()
        {
            { "SpeedBoost", ResetSpeed },
            { "Shield", DeactivateShield },
            { "FireBoost", ResetCannonAngularVelocity },
            { "Invis", ResetPlayerInvis },
            { "FireRateIncrease", ResetFireRate },
            { "TimeSlow", ResetTimeSlow }
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

        private static void ResetPlayerInvis(Player player)
        {
            player.SetSprite("TankBody");
            if (player.cannon != null)
                player.cannon.SetSprite("default");
            player.isInvis = false;
        }

        private static void ResetFireRate(Player player)
        {
            player.currentShootInterval = player.baseShootInterval;
        }

        private static void ResetTimeSlow(Player player)
        {
            Globals.FRAMETIME = 1f / 60f;
        }

        public static void UpdateEffects(Player player)
        {
            var effects = new List<string>(player.EffectTimers.Keys);

            foreach (var effect in effects)
            {
                player.EffectTimers[effect] -= Globals.FRAMETIME;
                if (player.EffectTimers[effect] <= 0f)
                {
                    if (expireActions.ContainsKey(effect))
                    {
                        expireActions[effect](player);
                    }
                    player.EffectTimers.Remove(effect);
                }
            }
        }

        public static void ApplyPickupEffect(Player player, string pickupType)
        {
            switch (pickupType)
            {
                case "SpeedBoost":
                    player.speedMultiplier = 4f;
                    player.EffectTimers["SpeedBoost"] = 5f;
                    break;
                case "Shield":
                    player.shieldActive = true;
                    player.EffectTimers["Shield"] = 5f;
                    break;
                case "Ammo_default":
                    Globals.PlayerData.TemporaryAmmoDefault++;
                    break;
                case "Ammo_shotgun":
                    Globals.PlayerData.TemporaryAmmoShotgun++;
                    break;
                case "Ammo_sniper":
                    Globals.PlayerData.TemporaryAmmoSniper++;
                    break;
                case "Ammo_rocket":
                    Globals.PlayerData.TemporaryAmmoRocket++;
                    break;
                case "Ammo_Laser":
                    Globals.PlayerData.TemporaryAmmoLaser++;
                    break;
                case "Ammo_Mine":
                    Globals.PlayerData.TemporaryAmmoMine++;
                    break;
                case "Magnet":
                    player.EffectTimers["Magnet"] = 5f;
                    break;
                case "FireRateIncrease":
                    player.currentShootInterval = player.baseShootInterval * 0.5f;
                    player.EffectTimers["FireRateIncrease"] = 5f;
                    break;
                case "MedStrong":
                    Globals.PlayerData.TemporaryHealth += 50;
                    break;
                case "MedWeak":
                    Globals.PlayerData.TemporaryHealth += 25;
                    break;
                case "TimeSlow":
                    Globals.FRAMETIME = 1f / 150f; // Slow down non-player entities
                    player.EffectTimers["TimeSlow"] = 5f;
                    break;
                case "Fly":
                    player.EffectTimers["Fly"] = 5f;
                    break;
                case "Cloak":
                    player.isInvis = true;
                    player.SetSprite("InvisTankBody");
                    if (player.cannon != null)
                        player.cannon.SetSprite("InvisTankCannon");
                    player.EffectTimers["Invis"] = 5f;
                    break;
                case "SilverTag":
                    Globals.PlayerData.TemporaryCoins += 100;
                    break;
                case "GoldTag":
                    Globals.PlayerData.TemporaryCoins += 500;
                    break;
                case "Teleporter":
                    break;
                default:
                    break;
            }
        }
    }
}
