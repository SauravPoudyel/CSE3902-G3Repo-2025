using System;

namespace Sprint0
{
    public static class EntityKeys
    {
        public enum ProjectileTypeEnum
        {
            Sniper, Rocket, Shotgun, Mine, Teleporter, Laser, Default
        }

        public enum BlockType
        {
            Tree, Box, Barrel, RedBarrel, BarbedFence, Oil, RockPile, Factory, RockPileVar1, RockPileVar2, House, House2, SmallTree, Fence, DeadTree, Garage, CoconutTree, SmallBarrel, Boarder, Tree1, Tree2, DesertHouse, Tent, Shop,
            CliffHorizontalLeft,
            CliffHorizontalRight,
            CliffVerticalTop,
            CliffVerticalBottom
        }

        public enum MobType
        {
            BossTank, SmallEnemy, Turret, Plane, ShieldTank, SwarmingTank, HoveringTank, StealthTank, HealerTank, ShipVertical, ShipHorizontal
        }

        public enum EffectType
        {
            Explosion, Fire, Shield, TeleportOut, TeleportIn, BoostFire
        }

        public enum ItemType
        {
            SpeedBoost, Shield, AmmoDefault, AmmoShotgun, AmmoSniper, AmmoRocket, AmmoLaser, AmmoMine,
            Magnet, FireRateIncrease, MedStrong, MedWeak, TimeSlow, Fly, Cloak, SilverTag, GoldTag, Teleporter, 
            ShopKeys
        }
    }
}
