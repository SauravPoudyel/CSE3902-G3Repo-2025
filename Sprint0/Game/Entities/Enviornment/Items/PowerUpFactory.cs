using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Sprint0
{
    public static class PowerUpFactory
    {
        public static Dictionary<PickupItemType, float> EffectTimers = new Dictionary<PickupItemType, float>();
        private static readonly Dictionary<PickupItemType, Action<Player>> expireActions = new()
        {
            { PickupItemType.SpeedBoost, ResetSpeed },
            { PickupItemType.Shield, DeactivateShield },
            { PickupItemType.FireRateIncrease, ResetFireRate },
            { PickupItemType.TimeSlow, ResetTimeSlow },
            { PickupItemType.Cloak, ResetPlayerInvis }
        };

        private static void ResetSpeed(Player player) { player.speedMultiplier = 1f; } 
        private static void DeactivateShield(Player player) { player.shieldActive = false; } 
        private static void ResetFireRate(Player player) { player.currentShootInterval = player.baseShootInterval; } 
        private static void ResetTimeSlow(Player player) { Globals.FRAMETIME = 1f / 60f; }

        private static void ResetPlayerInvis(Player player)
        {
            player.SetSprite("TankBody");
            if (player.cannon != null)
                player.cannon.SetSprite("default");
            player.isInvis = false;
        }
        
        private static void ResetCannonAngularVelocity(Player player) 
        { 
            if (player.cannon != null) 
                player.cannon.AngularVelocity = 30f; 
        } 

        public static void UpdateEffects(Player player)
        {
            var effects = new List<PickupItemType>(EffectTimers.Keys);

            foreach (var effect in effects)
            {
                EffectTimers[effect] -= Globals.FRAMETIME;
                if (EffectTimers[effect] <= 0f)
                {
                    if (expireActions.ContainsKey(effect))
                        expireActions[effect](player);
                    
                    EffectTimers.Remove(effect);
                }
            }
        }


        public static void ApplyPickupEffect(Player player, PickupItemType type)
        {
            switch (type)
            {
                case PickupItemType.SpeedBoost:
                    player.speedMultiplier = 4f;
                    EffectTimers[PickupItemType.SpeedBoost] = 5f;
                    break;
                case PickupItemType.Shield:
                    player.shieldActive = true;
                    EffectTimers[PickupItemType.Shield] = 5f;
                    break;
                case PickupItemType.Ammo_default:
                    Globals.PlayerData.TemporaryAmmoDefault++;
                    break;
                case PickupItemType.Ammo_shotgun:
                    Globals.PlayerData.TemporaryAmmoShotgun++;
                    break;
                case PickupItemType.Ammo_sniper:
                    Globals.PlayerData.TemporaryAmmoSniper++;
                    break;
                case PickupItemType.Ammo_rocket:
                    Globals.PlayerData.TemporaryAmmoRocket++;
                    break;
                case PickupItemType.Ammo_Laser:
                    Globals.PlayerData.TemporaryAmmoLaser++;
                    break;
                case PickupItemType.Ammo_Mine:
                    Globals.PlayerData.TemporaryAmmoMine++;
                    break;
                case PickupItemType.Magnet:
                    EffectTimers[PickupItemType.Magnet] = 5f;
                    break;
                case PickupItemType.FireRateIncrease:
                    player.currentShootInterval = player.baseShootInterval * 0.5f;
                    EffectTimers[PickupItemType.FireRateIncrease] = 5f;
                    break;
                case PickupItemType.MedStrong:
                    Globals.PlayerData.TemporaryHealth += 50;
                    break;
                case PickupItemType.MedWeak:
                    Globals.PlayerData.TemporaryHealth += 25;
                    break;
                case PickupItemType.TimeSlow:
                    Globals.FRAMETIME = 1f / 150f;
                    EffectTimers[PickupItemType.TimeSlow] = 5f;
                    break;
                case PickupItemType.Fly:
                    EffectTimers[PickupItemType.Fly] = 5f;
                    break;
                case PickupItemType.Cloak:
                    player.isInvis = true;
                    player.SetSprite("InvisTankBody");
                    if (player.cannon != null)
                        player.cannon.SetSprite("InvisTankCannon");
                    EffectTimers[PickupItemType.Cloak] = 5f;
                    break;
                case PickupItemType.SilverTag:
                    Globals.PlayerData.TemporaryCoins += 100;
                    break;
                case PickupItemType.GoldTag:
                    Globals.PlayerData.TemporaryCoins += 500;
                    break;
                case PickupItemType.Teleporter:
                    // Implement teleporter logic if necessary
                    break;
            }
        }
    }
}
