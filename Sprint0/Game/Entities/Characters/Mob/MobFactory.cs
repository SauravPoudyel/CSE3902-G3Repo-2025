using Microsoft.Xna.Framework.Content;

namespace Sprint0
{
    public static class MobFactory
    {
        private static MobType[] availableMobTypes = { MobType.BossTank, MobType.SmallEnemy, MobType.ExplodingTank, MobType.Turret, MobType.TurningTank, MobType.Plane };
        private static int currentIndex = 0;

        public static Mob CreateMob(ContentManager content)
        {
            return CreateMob(availableMobTypes[currentIndex], content);
        }

        public static Mob CreateMob(MobType type, ContentManager content)
        {
            switch (type)
            {
                case MobType.BossTank:
                    return new BossTank(content);
                case MobType.SmallEnemy:
                    return new SmallEnemy(content);
                case MobType.ExplodingTank:
                    return new ExplodingTank(content);
                case MobType.Turret:
                    return new Turret(content);
                case MobType.TurningTank:
                    return new TurningTank(content);
                case MobType.Plane:
                    return new Plane(content);
                default:
                    return new BossTank(content);
            }
        }

        public static void CycleNextMob(GameManager gameManager)
        {
            currentIndex = (currentIndex + 1) % availableMobTypes.Length;
            ReplaceMob(gameManager);
        }

        public static void CyclePreviousMob(GameManager gameManager)
        {
            currentIndex = (currentIndex - 1 + availableMobTypes.Length) % availableMobTypes.Length;
            ReplaceMob(gameManager);
        }

        private static void ReplaceMob(GameManager gameManager)
        {
            if (gameManager.GetEntity("mob") != null)
                gameManager.RemoveEntity("mob");
            Mob newMob = CreateMob(availableMobTypes[currentIndex], gameManager.GetContent());
            newMob.SetPosition(new Microsoft.Xna.Framework.Vector2(Globals.SCREENWIDTH / 2 + 100, 400));
            gameManager.SetEntity("mob", newMob);
        }
    }
}
