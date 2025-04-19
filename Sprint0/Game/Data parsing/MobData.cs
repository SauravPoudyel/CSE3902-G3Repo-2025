using System;
using System.Collections.Generic;

namespace Sprint0
{
    public class MobData
    {
        public enum MobDifficulty { Easy, Regular, Hard, Insane }
        public enum MobModifiers { MovementSpeed, FiringInterval, AggressionRange, Health }

        // Table that maps (Difficulty, Modifier) → float
        private static readonly Dictionary<(MobDifficulty, MobModifiers), float> DifficultyModifierTable
            = new Dictionary<(MobDifficulty, MobModifiers), float>
        {
            // Easy
            { (MobDifficulty.Easy, MobModifiers.MovementSpeed), 0.75f },
            { (MobDifficulty.Easy, MobModifiers.FiringInterval), 1.25f },
            { (MobDifficulty.Easy, MobModifiers.AggressionRange), 0.75f },
            { (MobDifficulty.Easy, MobModifiers.Health), 0.75f },
            // Regular
            { (MobDifficulty.Regular, MobModifiers.MovementSpeed), 1f },
            { (MobDifficulty.Regular, MobModifiers.FiringInterval), 1f },
            { (MobDifficulty.Regular, MobModifiers.AggressionRange), 1f },
            { (MobDifficulty.Regular, MobModifiers.Health), 1f },
            // Hard
            { (MobDifficulty.Hard, MobModifiers.MovementSpeed), 1.25f },
            { (MobDifficulty.Hard, MobModifiers.FiringInterval), 0.75f },
            { (MobDifficulty.Hard, MobModifiers.AggressionRange), 1.25f },
            { (MobDifficulty.Hard, MobModifiers.Health), 1.5f },
            // Insane
            { (MobDifficulty.Insane, MobModifiers.MovementSpeed), 1.5f },
            { (MobDifficulty.Insane, MobModifiers.FiringInterval), 0.5f },
            { (MobDifficulty.Insane, MobModifiers.AggressionRange), 1.5f },
            { (MobDifficulty.Insane, MobModifiers.Health), 2f }
        };

        public MobDifficulty Difficulty { get; private set; } = MobDifficulty.Regular;

        public void SetDifficulty(MobDifficulty difficulty)
        {
            Difficulty = difficulty;
        }

        public float GetModifier(MobModifiers modifier)
        {
            if (DifficultyModifierTable.TryGetValue((Difficulty, modifier), out float value))
                return value;

            return 1f; // Default neutral modifier
        }
    }
}
