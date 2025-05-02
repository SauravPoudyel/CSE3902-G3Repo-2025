using System;
using static Sprint0.EntityKeys;

namespace Sprint0
{
    // this is a utility class to cycle through the mob types for testing purposes (but we removed the logic from mob for better isolation)
    public static class MobTypeCycler
    {
        public static void Next(Mob mob)
        {
            var all = (MobType[])Enum.GetValues(typeof(MobType));
            int idx = Array.IndexOf(all, mob.CurrentMobType);
            var next = all[(idx + 1) % all.Length];
            mob.ChangeMobType(next);
            mob.ResetMobPosition();
        }

        public static void Previous(Mob mob)
        {
            var all = (MobType[])Enum.GetValues(typeof(MobType));
            int idx = Array.IndexOf(all, mob.CurrentMobType);
            var prev = all[(idx - 1 + all.Length) % all.Length];
            mob.ChangeMobType(prev);
            mob.ResetMobPosition();
        }
    }
}
