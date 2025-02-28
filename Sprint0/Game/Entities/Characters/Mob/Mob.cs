using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

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
        protected MobType currentMobType;
        protected float defaultMovementSpeed;
        protected float firingTimer;
        protected float firingInterval;
        private float requestPlayerTimer;
        private float timeSinceLastPlayerSeen;
        protected Vector2 lastKnownPlayerPosition;
        protected float aggressionRange;


        public Mob(ContentManager content) : base(content)
        {
            firingTimer = 0f;
            requestPlayerTimer = 0.5f;
            lastKnownPlayerPosition = Vector2.Zero;
            aggressionRange = 1000f;
            InitializeMob();
        }

        protected abstract void InitializeMob();
        protected abstract void UpdateMobBehavior();
        protected abstract void ChangeMobType(MobType type);
        protected abstract void ResetMobPosition();

        public void NextMobType()
        {
            MobType[] allTypes = (MobType[])Enum.GetValues(typeof(MobType));
            int index = Array.IndexOf(allTypes, currentMobType);
            int nextIndex = (index + 1) % allTypes.Length;
            ChangeMobType(allTypes[nextIndex]);
            ResetMobPosition();
        }

        public void PreviousMobType()
        {
            MobType[] allTypes = (MobType[])Enum.GetValues(typeof(MobType));
            int index = Array.IndexOf(allTypes, currentMobType);
            int prevIndex = (index - 1 + allTypes.Length) % allTypes.Length;
            ChangeMobType(allTypes[prevIndex]);
            ResetMobPosition();
        }

        public override void Update()
        {
            float elapsed = Globals.FRAMETIME;
            firingTimer += elapsed;
            requestPlayerTimer -= elapsed;

            if (requestPlayerTimer <= 0f)
            {
                var commandParams = new Dictionary<string, object> { { "mob", this } };
                commandQueue.Enqueue(new CommandRequest("RequestPlayerPosition", commandParams));
                requestPlayerTimer = 0.5f;
            }

            if (lastKnownPlayerPosition != Vector2.Zero)
            {
                float distanceToPlayer = Vector2.Distance(GetPosition(), lastKnownPlayerPosition);
                if (distanceToPlayer <= aggressionRange)
                {
                    Vector2 directionToPlayer = lastKnownPlayerPosition - GetPosition();
                    float desiredCannonAngle = (float)Math.Atan2(directionToPlayer.Y, directionToPlayer.X) - MathHelper.PiOver2;
                    float currentRotation = cannon.Rotation;
                    float angleDiff = MathHelper.WrapAngle(desiredCannonAngle - currentRotation);
                    float maxTurnRadians = MathHelper.ToRadians(60) * elapsed;
                    if (Math.Abs(angleDiff) > maxTurnRadians)
                        angleDiff = Math.Sign(angleDiff) * maxTurnRadians;
                    cannon.Rotation = currentRotation + angleDiff;
                    if (Math.Abs(MathHelper.WrapAngle(desiredCannonAngle - cannon.Rotation)) < 0.1f && firingTimer >= firingInterval)
                    {
                        FireProjectile();
                        firingTimer = 0f;
                    }
                }
                else
                {
                    cannon.Rotation += MathHelper.ToRadians(20) * elapsed;
                }

                if (timeSinceLastPlayerSeen >= 0.8f)
                    lastKnownPlayerPosition = Vector2.Zero;
            }
            else
            {
                cannon.Rotation += MathHelper.ToRadians(20) * elapsed;
            }

            UpdateMobBehavior();
            base.Update();
        }

        public void UpdateKnownPlayerPosition(Vector2 newPlayerPosition)
        {
            lastKnownPlayerPosition = newPlayerPosition;
            timeSinceLastPlayerSeen = 0f;
        }
    }
}
