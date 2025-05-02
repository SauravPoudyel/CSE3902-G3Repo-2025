using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using static Sprint0.EntityKeys;

namespace Sprint0
{
    public static class MobFactory
    {
        private static int currentIndex = 0;

        public static Mob CreateMob(MobType type, ContentManager content)
        {
            Mob mob = type switch
            {
                MobType.BossTank      => new BossTank(content),
                MobType.SmallEnemy    => new SmallEnemy(content),
                MobType.Turret        => new Turret(content),
                MobType.Plane         => new Plane(content),
                MobType.ShieldTank    => new ShieldTank(content),
                MobType.SwarmingTank  => new SwarmingTank(content),
                MobType.HoveringTank  => new HoveringTank(content),
                MobType.HealerTank    => new HealerTank(content),
                MobType.StealthTank   => new StealthTank(content),
                MobType.ShipVertical   => new Ship(content, true),
                MobType.ShipHorizontal => new Ship(content, false),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };

            MobSpriteFactory.Initialize(mob, content);
            return mob;
        }

        public static void CycleNextMob(GameManager gameManager)
        {
            var types = (MobType[])Enum.GetValues(typeof(MobType));
            currentIndex = (currentIndex + 1) % types.Length;
            ReplaceMob(gameManager, types);
        }

        public static void CyclePreviousMob(GameManager gameManager)
        {
            var types = (MobType[])Enum.GetValues(typeof(MobType));
            currentIndex = (currentIndex - 1 + types.Length) % types.Length;
            ReplaceMob(gameManager, types);
        }

        private static void ReplaceMob(GameManager gameManager, MobType[] types)
        {
            if (gameManager.GetEntity("mob") != null)
                gameManager.RemoveEntity("mob");

            Mob newMob = CreateMob(types[currentIndex], gameManager.GetContent());
            newMob.SetPosition(new Vector2(Globals.SCREENWIDTH / 2 + 100, 400));
            gameManager.SetEntity("mob", newMob);
        }
    }
}
