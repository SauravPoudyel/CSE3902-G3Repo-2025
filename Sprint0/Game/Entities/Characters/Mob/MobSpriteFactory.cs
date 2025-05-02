using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using static Sprint0.EntityKeys;

namespace Sprint0
{
    public static class MobSpriteFactory
    {
        private static readonly Dictionary<MobType, Action<Mob, ContentManager>> SpriteInitializers =
            new Dictionary<MobType, Action<Mob, ContentManager>>
        {
            {MobType.Plane, InitializePlaneVisuals},
            {MobType.ShieldTank, InitializeShieldTankVisuals},
            {MobType.ShipVertical, (mob, content) => InitializeShipVisuals(mob, true,  content)},
            {MobType.ShipHorizontal, (mob, content) => InitializeShipVisuals(mob, false, content)},
            {MobType.SmallEnemy, InitializeSmallEnemyVisuals},
            {MobType.StealthTank, InitializeStealthTankVisuals},
            {MobType.SwarmingTank, InitializeSwarmingTankVisuals},
            {MobType.Turret, InitializeTurretVisuals},
            {MobType.HoveringTank, InitializeHoveringTankVisuals},
            {MobType.HealerTank, InitializeHealerTankVisuals},
            {MobType.BossTank, InitializeBossTankVisuals},
        };

        public static void Initialize(Mob mob, ContentManager content)
        {
            if (SpriteInitializers.TryGetValue(mob.currentMobType, out var init))
                init(mob, content);
        }

        private static void InitializePlaneVisuals(Mob mob, ContentManager content)
        {
            Plane plane = (Plane)mob;
            Sprite body = new Sprite(0.3f);
            body.LoadContent(content, "TDTowerDefenseSprites", 2183, 1411, 135, 134, 1);
            plane.SetSprite(body);
            plane.cannon = new Cannon(content, Globals.NULLSPRITE_A, plane, new Vector2(14, 10), 30f, Vector2.Zero, 0f, MathHelper.ToRadians(20), 0f, 0f);
        }

        private static void InitializeShieldTankVisuals(Mob mob, ContentManager content)
        {
            ShieldTank tank = (ShieldTank)mob;
            Sprite body = new Sprite(0.3f);
            body.LoadContent(content, "TDTanksAllSprites", 768, 0, 94, 97, 1);
            tank.SetSprite(body);
            tank.cannon = new Cannon(content, Globals.NULLSPRITE_A, tank, new Vector2(14, 10), 30f, Vector2.Zero, 0f, 0f, 0f, 0f);
        }

        private static void InitializeShipVisuals(Mob mobBase, bool vertical, ContentManager content)
        {
            Ship ship = (Ship)mobBase;
            Sprite body = new Sprite(0.3f);
            body.LoadContent(content, "TDTanksAllSprites", 1135, 840, 68, 116, 1);
            ship.SetSprite(body);
            ship.bodyRotation = vertical ? 0f : -MathHelper.PiOver2;
            Vector2 offset = vertical ? new Vector2(25, 60) : new Vector2(60, 25);
            ship.cannon = new Cannon(content, Globals.NULLSPRITE_A, ship, offset, 30f, Vector2.Zero, 0f, MathHelper.ToRadians(20), 0f, 0f);
        }

        private static void InitializeSmallEnemyVisuals(Mob mob, ContentManager content)
        {
            SmallEnemy enemy = (SmallEnemy)mob;
            Sprite body = new Sprite(0.3f);
            body.LoadContent(content, "TDTanksAllSprites", 768, 256, 95, 113, 1);
            enemy.SetSprite(body);
            Sprite cannonSprite = new Sprite(0.3f);
            cannonSprite.LoadContent(content, "TDTanksAllSprites", 832, 186, 28, 64, 1);
            enemy.cannon = new Cannon(content, cannonSprite, enemy, new Vector2(14, 10), 50f, new Vector2(12, 70), 0f, MathHelper.ToRadians(20), MathHelper.PiOver2, MathHelper.Pi + MathHelper.PiOver2);
        }

        private static void InitializeStealthTankVisuals(Mob mob, ContentManager content)
        {
            StealthTank stealth = (StealthTank)mob;
            Sprite tankSprite = new Sprite(0.3f);
            tankSprite.LoadContent(content, "TDTanksAllSprites", 876, 783, 84, 80, 1);
            stealth.SetSprite(tankSprite);
            stealth.originalTankSprite = tankSprite;
            Sprite cannonSprite = new Sprite(0.3f);
            cannonSprite.LoadContent(content, "TDTanksAllSprites", 1104, 152, 16, 52, 1);
            stealth.cannon = new Cannon(content, cannonSprite, stealth, new Vector2(8, 5), 50f, new Vector2(8, 60), 0f, MathHelper.ToRadians(20));
            stealth.originalCannonSprite = stealth.cannon.GetSprite();
        }

        private static void InitializeSwarmingTankVisuals(Mob mob, ContentManager content)
        {
            SwarmingTank swarm = (SwarmingTank)mob;
            Sprite body = new Sprite(0.3f);
            body.LoadContent(content, "TDTanksAllSprites", 1126, 275, 53, 56, 2);
            swarm.SetSprite(body);
            swarm.cannon = new Cannon(content, Globals.NULLSPRITE_A, swarm, new Vector2(14, 10), 30f, Vector2.Zero, 0f, 0f, 0f, 0f);
        }

        private static void InitializeTurretVisuals(Mob mob, ContentManager content)
        {
            Turret turret = (Turret)mob;
            Sprite baseSprite = new Sprite(0.3f);
            baseSprite.LoadContent(content, "TDTowerDefenseSprites", 2444, 908, 104, 104, 1);
            turret.SetSprite(baseSprite);
            Sprite cannonSprite = new Sprite(0.3f);
            cannonSprite.LoadContent(content, "TDTowerDefenseSprites", 2455, 1290, 85, 110, 1);
            turret.cannon = new Cannon(content, cannonSprite, turret, new Vector2(42, 34), -70f, new Vector2(42, -80), 0f, MathHelper.ToRadians(30), MathHelper.PiOver2, MathHelper.Pi + MathHelper.PiOver2);
            turret.cannon.CannonEffects = SpriteEffects.FlipVertically;
            turret.cannon.HasFiringEffect = false;
        }

        private static void InitializeHoveringTankVisuals(Mob mob, ContentManager content)
        {
            HoveringTank hover = (HoveringTank)mob;
            Sprite body = new Sprite(0.3f);
            body.LoadContent(content, "TDTanksAllSprites", 1135, 180, 86, 92, 1);
            hover.SetSprite(body);
            Sprite cannonSprite = new Sprite(0.3f);
            cannonSprite.LoadContent(content, "TDTanksAllSprites", 832, 186, 28, 64, 1);
            hover.cannon = new Cannon(content, cannonSprite, hover, new Vector2(14, 10), 50f, new Vector2(12, 70), 0f, MathHelper.ToRadians(20), MathHelper.PiOver2, MathHelper.Pi + MathHelper.PiOver2);
        }

        private static void InitializeHealerTankVisuals(Mob mob, ContentManager content)
        {
            HealerTank healer = (HealerTank)mob;
            Sprite normal = new Sprite(0.3f);
            normal.LoadContent(content, "TDTanksAllSprites", 1126, 334, 76, 72, 1);
            healer.normalSprite = normal;
            Sprite heal = new Sprite(0.3f);
            heal.LoadContent(content, "TDTanksAllSprites", 1202, 334, 76, 72, 1);
            healer.healSprite = heal;
            healer.SetSprite(normal);
            healer.cannon = new Cannon(content, Globals.NULLSPRITE_A, healer, Vector2.Zero, 30f, Vector2.Zero, 0f, 0f, 0f, 0f);
        }

        private static void InitializeBossTankVisuals(Mob mob, ContentManager content)
        {
            BossTank boss = (BossTank)mob;
            Sprite body = new Sprite(0.3f);
            body.LoadContent(content, "TDTanksAllSprites", 641, 661, 123, 144, 1);
            boss.SetSprite(body);
            Sprite cannonSprite = new Sprite(0.3f);
            cannonSprite.LoadContent(content, "TDTanksAllSprites", 832, 186, 28, 64, 1);
            boss.cannon = new Cannon(content, cannonSprite, boss, new Vector2(14, 10), 50f, new Vector2(12, 70), 0f, MathHelper.ToRadians(20), MathHelper.PiOver2, MathHelper.Pi + MathHelper.PiOver2);
        }
    }
}
