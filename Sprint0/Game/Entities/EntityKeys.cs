using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace Sprint0
{
    public static class EntityKeys 
    {
        public enum ProjectileTypeEnum{
            Sniper, Rocket, Shotgun, Mine, Teleporter
        }

        public enum BlockType
        {
            Tree, Box, Oil, BarbedFence, Barrel, RedBarrel,
        }

        public enum MobType
        {
        BossTank, SmallEnemy, ExplodingTank, Turret, TurningTank, Plane, ShieldTank, SwarmingTank, HoveringTank, StealthTank, HealerTank
        }

        public enum EffectType {
            Explosion, Fire, Shield, TeleportOut, TeleportIn
        }
    }
}