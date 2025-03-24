using System;
using Microsoft.Xna.Framework.Content;


namespace Sprint0
{
    public static class MobFactory
    {
        private static int currentIndex = 0;

        public static Mob CreateMob(EntityKeys.MobType type, ContentManager content)
        {
            switch (type)
            {

                case EntityKeys.MobType.BossTank: return new BossTank(content);
                case EntityKeys.MobType.SmallEnemy: return new SmallEnemy(content);
                case EntityKeys.MobType.Turret: return new Turret(content);
                case EntityKeys.MobType.Plane: return new Plane(content);
                case EntityKeys.MobType.ShieldTank: return new ShieldTank(content);
                case EntityKeys.MobType.SwarmingTank: return new SwarmingTank(content);
                case EntityKeys.MobType.HoveringTank: return new HoveringTank(content);
                case EntityKeys.MobType.HealerTank: return new HealerTank(content);
                case EntityKeys.MobType.StealthTank: return new StealthTank(content);
                case EntityKeys.MobType.ShipVertical: return new Ship(content, true);
                case EntityKeys.MobType.ShipHorizontal: return new Ship(content, false);
                default: return new BossTank(content);
            }
        }

        public static void CycleNextMob(GameManager gameManager)
        {
            currentIndex = (currentIndex + 1) % Enum.GetNames(typeof(EntityKeys.MobType)).Length;
            ReplaceMob(gameManager);
        }

        public static void CyclePreviousMob(GameManager gameManager)
        {
            currentIndex = (currentIndex - 1 + Enum.GetNames(typeof(EntityKeys.MobType)).Length) % Enum.GetNames(typeof(EntityKeys.MobType)).Length;
            ReplaceMob(gameManager);
        }

        private static void ReplaceMob(GameManager gameManager)
        {
            if (gameManager.GetEntity("mob") != null)
                gameManager.RemoveEntity("mob");

            var mobTypes = (EntityKeys.MobType[])Enum.GetValues(typeof(EntityKeys.MobType));
            var newMob = CreateMob(mobTypes[currentIndex], gameManager.GetContent());

            newMob.SetPosition(new Microsoft.Xna.Framework.Vector2(Globals.SCREENWIDTH / 2 + 100, 400));
            gameManager.SetEntity("mob", newMob);
        }
    }
}