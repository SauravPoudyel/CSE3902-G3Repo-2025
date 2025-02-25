using Microsoft.Xna.Framework.Content;

namespace Sprint0
{
    public static class MobFactory
    {
        private static MobType[] allMobTypes = { MobType.BossTank, MobType.SmallEnemy, MobType.ExplodingTank, MobType.Turret, MobType.TurningTank, MobType.Plane };
        private static int currentIndex = 0;

        public static Mob CreateMob(ContentManager content)
        {
            return CreateMob(allMobTypes[currentIndex], content);
        }

        public static Mob CreateMob(MobType type, ContentManager content)
        {
            switch (type)
            {
                case MobType.BossTank: return new BossTank(content);
                case MobType.SmallEnemy: return new SmallEnemy(content);
                case MobType.ExplodingTank: return new ExplodingTank(content);
                case MobType.Turret: return new Turret(content);
                case MobType.TurningTank: return new TurningTank(content);
                case MobType.Plane: return new Plane(content);
                default: return new BossTank(content);
            }
        }

        public static void CycleNextMob(GameManager gm)
        {
            currentIndex = (currentIndex + 1) % allMobTypes.Length;
            ReplaceMob(gm);
        }

        public static void CyclePreviousMob(GameManager gm)
        {
            currentIndex = (currentIndex - 1 + allMobTypes.Length) % allMobTypes.Length;
            ReplaceMob(gm);
        }

        private static void ReplaceMob(GameManager gm)
        {
            if (gm.GetEntity("mob") != null)
                gm.RemoveEntity("mob");
            var newMob = CreateMob(allMobTypes[currentIndex], gm.GetContent());
            newMob.SetPosition(new Microsoft.Xna.Framework.Vector2(Globals.SCREENWIDTH / 2 + 100, 400));
            gm.SetEntity("mob", newMob);
        }
    }
}
