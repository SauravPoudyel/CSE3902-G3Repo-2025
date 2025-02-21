using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Sprint0
{
    public enum MobType
    {
        BossTank,
        SmallEnemy,
        ExplodingTank,
        Turret,
        TurningTank,
        Plane
    }
    public abstract class Mob : Character
    {
        protected MobType mobType;
        protected float defaultSpeed;

        public Mob(ContentManager content) : base(content)
        {
            defaultSpeed = 80f;
            shootTimer = 0f;
            InitializeMob();
        }

        protected float shootTimer;
        protected float shootInterval;

        protected abstract void InitializeMob();
        protected abstract void UpdateMobBehavior();
        protected abstract void SetEnemyType(MobType type);
        protected abstract void ResetPosition();

        public void CycleEnemyNext()
        {
            MobType[] types = (MobType[])Enum.GetValues(typeof(MobType));
            int index = Array.IndexOf(types, mobType);
            int nextIndex = (index + 1) % types.Length;
            SetEnemyType(types[nextIndex]);
            ResetPosition();
        }

        public void CycleEnemyPrev()
        {
            MobType[] types = (MobType[])Enum.GetValues(typeof(MobType));
            int index = Array.IndexOf(types, mobType);
            int prevIndex = (index - 1 + types.Length) % types.Length;
            SetEnemyType(types[prevIndex]);
            ResetPosition();
        }

        public override void Update()
        {
            UpdateMobBehavior();

            if (cannon != null)
                cannon.Update();

            // Update shoot timer and fire projectile if ready.
            float elapsed = Globals.FRAMETIME;
            shootTimer += elapsed;
            if (shootTimer >= shootInterval)
            {
                FireProjectile();
                shootTimer = 0f;
            }

            base.Update();
        }
    }
}
