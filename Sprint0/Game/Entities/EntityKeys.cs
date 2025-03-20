using System;

namespace Sprint0
{
    public static class EntityKeys
    {
        public enum ProjectileTypeEnum
        {
            Sniper, Rocket, Shotgun, Mine, Teleporter, Laser
        }

        public enum BlockType
        {
            Tree, Box, Barrel, RedBarrel, BarbedFence, Oil, RockPile, Factory, RockPileVar1, RockPileVar2, House, House2, SmallTree, Fence, DeadTree, Garage, CoconutTree, SmallBarrel
        }

        public enum MobType
        {
            BossTank, SmallEnemy, Turret, Plane, ShieldTank, SwarmingTank, HoveringTank, StealthTank, HealerTank, ShipVertical, ShipHorizontal
        }

        public enum EffectType
        {
            Explosion, Fire, Shield, TeleportOut, TeleportIn
        }

        public enum ItemType
        {
            SpeedBoost, Shield, Ammo_default, Ammo_shotgun, Ammo_sniper, Ammo_rocket, Ammo_Laser, Ammo_Mine,
            Magnet, FireRateIncrease, MedStrong, MedWeak, TimeSlow, Fly, Cloak, SilverTag, GoldTag, Teleporter
        }
    }
}
