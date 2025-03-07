using System;
using Microsoft.Xna.Framework.Content;

namespace Sprint0
{
    public static class MobFactory
    {
        private static int currentIndex = 0;

        public static Mob CreateMob(MobType type, ContentManager content)
        {
            switch (type)
            {
                case MobType.BossTank: return new BossTank(content);
                case MobType.SmallEnemy: return new SmallEnemy(content);
                case MobType.Turret: return new Turret(content);
                case MobType.Plane: return new Plane(content);
                case MobType.ShieldTank: return new ShieldTank(content);
                case MobType.SwarmingTank: return new SwarmingTank(content);
                case MobType.HoveringTank: return new HoveringTank(content);
                case MobType.StealthTank: return new StealthTank(content);
                case MobType.HealerTank: return new HealerTank(content);
                // case MobType.ShipVertical: return new Ship(content, true);
                // case MobType.ShipHorizontal: return new Ship(content, false);
                default: return new BossTank(content);
            }
        }

        public static void CycleNextMob(GameManager gameManager)
        {
            currentIndex = (currentIndex + 1) % Enum.GetNames(typeof(MobType)).Length;
            ReplaceMob(gameManager);
        }

        public static void CyclePreviousMob(GameManager gameManager)
        {
            currentIndex = (currentIndex - 1 + Enum.GetNames(typeof(MobType)).Length) % Enum.GetNames(typeof(MobType)).Length;
            ReplaceMob(gameManager);
        }

        private static void ReplaceMob(GameManager gameManager)
        {
            if (gameManager.GetEntity("mob") != null)
                gameManager.RemoveEntity("mob");

            var mobTypes = (MobType[])Enum.GetValues(typeof(MobType));
            var newMob = CreateMob(mobTypes[currentIndex], gameManager.GetContent());

            newMob.SetPosition(new Microsoft.Xna.Framework.Vector2(Globals.SCREENWIDTH / 2 + 100, 400));
            gameManager.SetEntity("mob", newMob);
        }
    }
}
