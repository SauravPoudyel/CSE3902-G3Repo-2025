using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Sprint0
{
    public static class PowerUpFactory
    {
        public static Dictionary<EntityKeys.ItemType, float> EffectTimers = new Dictionary<EntityKeys.ItemType, float>();
        public static float NormalPickUpRadius = 150f;
        private static readonly Dictionary<EntityKeys.ItemType, Action<Player>> expireActions = new()
        {
            { EntityKeys.ItemType.SpeedBoost, ResetSpeed },
            { EntityKeys.ItemType.Shield, DeactivateShield },
            { EntityKeys.ItemType.FireRateIncrease, ResetFireRate },
            { EntityKeys.ItemType.TimeSlow, ResetTimeSlow },
            { EntityKeys.ItemType.Cloak, ResetPlayerInvis },
            { EntityKeys.ItemType.Fly, ResetPlayerFly },
            { EntityKeys.ItemType.Magnet, ResetMagnet }
        };

        private static void ResetSpeed(Player player) { player.speedMultiplier = 1f; }
        private static void DeactivateShield(Player player) { player.shieldActive = false; }
        private static void ResetFireRate(Player player) { player.currentShootInterval = player.baseShootInterval; }
        private static void ResetTimeSlow(Player player) { Globals.FRAMETIME = 1f / 60f; }
        private static void ResetMagnet(Player player) { NormalPickUpRadius = 150f; }
        private static void ResetPlayerInvis(Player player)
        {
            player.SetSprite("TankBody");
            if (player.cannon != null)
                player.cannon.SetSprite("default");
            player.isInvis = false;
        }
        private static void ResetPlayerFly(Player player)
        {
            player.SetSprite("TankBody");
            if (player.cannon != null)
                player.cannon.SetSprite("default");
            player.isFly = false;
            player.TrackTrailsEnabled = true;
        }
        
        public static void UpdateEffects(Player player)
        {
            var effects = new List<EntityKeys.ItemType>(EffectTimers.Keys);

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

        public static void ApplyPickupEffect(Player player, EntityKeys.ItemType type)
        {
            switch (type)
            {
                case EntityKeys.ItemType.SpeedBoost:
                    player.speedMultiplier = 2.2f;
                    EffectTimers[EntityKeys.ItemType.SpeedBoost] = 5f;
                    break;
                case EntityKeys.ItemType.Shield:
                    player.shieldActive = true;
                    EffectTimers[EntityKeys.ItemType.Shield] = 5f;
                    break;
                case EntityKeys.ItemType.AmmoDefault:
                    Globals.PlayerData.UpdateVariable("AmmoDefault"); 
                    break;
                case EntityKeys.ItemType.AmmoShotgun:
                    Globals.PlayerData.UpdateVariable("AmmoShotgun"); 
                    break;
                case EntityKeys.ItemType.AmmoSniper:
                    Globals.PlayerData.UpdateVariable("AmmoSniper"); 
                    break;
                case EntityKeys.ItemType.AmmoRocket:
                    Globals.PlayerData.UpdateVariable("AmmoRocket"); 
                    break;
                case EntityKeys.ItemType.AmmoLaser:
                    Globals.PlayerData.UpdateVariable("AmmoLaser"); 
                    break;
                case EntityKeys.ItemType.AmmoMine:
                    Globals.PlayerData.UpdateVariable("AmmoMine"); 
                    break;
                case EntityKeys.ItemType.Teleporter:
                    Globals.PlayerData.UpdateVariable("AmmoTeleporter"); 
                    break;
                case EntityKeys.ItemType.Magnet:
                    NormalPickUpRadius = 450;
                    EffectTimers[EntityKeys.ItemType.Magnet] = 8f;
                    break;
                case EntityKeys.ItemType.FireRateIncrease:
                    player.currentShootInterval = player.baseShootInterval * 0.3f;
                    EffectTimers[EntityKeys.ItemType.FireRateIncrease] = 5f;
                    break;
                case EntityKeys.ItemType.MedStrong:
                    player.ChangeHealth(50);
                    break;
                case EntityKeys.ItemType.MedWeak:
                    player.ChangeHealth(25);
                    break;
                case EntityKeys.ItemType.TimeSlow:
                    Globals.FRAMETIME = 1f / 150f;
                    EffectTimers[EntityKeys.ItemType.TimeSlow] = 5f;
                    break;
                case EntityKeys.ItemType.Fly:
                    player.isFly = true;
                    player.SetSprite("FlyingTankBody");
                    player.TrackTrailsEnabled = false;
                    EffectTimers[EntityKeys.ItemType.Fly] = 5f;
                    break;
                case EntityKeys.ItemType.Cloak:
                    player.isInvis = true;
                    player.SetSprite("InvisTankBody");
                    if (player.cannon != null)
                        player.cannon.SetSprite("InvisTankCannon");
                    EffectTimers[EntityKeys.ItemType.Cloak] = 5f;
                    break;
                case EntityKeys.ItemType.SilverTag:
                    Globals.PlayerData.UpdateVariable("Coins", 50); 
                    break;
                case EntityKeys.ItemType.GoldTag:
                    Globals.PlayerData.UpdateVariable("Coins", 100);
                    break;
            }
        }
    }
}
