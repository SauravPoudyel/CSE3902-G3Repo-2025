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
        protected MobType mobType;
        protected float defaultSpeed;

        protected float shootTimer;
        protected float shootInterval;
        private float positionRequestTimer;
        private float lastSeenPlayerTimer;
        private Vector2 lastKnownPlayerPosition;
        protected float aggressionDistance; // Mob fires only if the player is within this distance


        public Mob(ContentManager content) : base(content)
        {
            shootTimer = 0f;
            positionRequestTimer = 0.5f; // Request player position every 0.5 seconds
            lastKnownPlayerPosition = Vector2.Zero;
            aggressionDistance = 1000f; // Default aggression range; override in subclass if needed
            InitializeMob();
        }

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
            float elapsed = Globals.FRAMETIME;
            shootTimer += elapsed;
            positionRequestTimer -= elapsed;

            if (positionRequestTimer <= 0f)
            {
                var parameters = new Dictionary<string, object>{{ "mob", this } };
                commandQueue.Enqueue(new CommandRequest("RequestPlayerPosition", parameters));
                positionRequestTimer = 0.5f;
            }

            if (lastKnownPlayerPosition != Vector2.Zero)
            {
                float distanceToPlayer = Vector2.Distance(GetPosition(), lastKnownPlayerPosition);
                if (distanceToPlayer <= aggressionDistance)
                {
                    Vector2 directionToPlayer = lastKnownPlayerPosition - GetPosition();
                    // Compute desired cannon angle (in radians) from mob position to player.
                    float desiredCannonAngle = (float)Math.Atan2(directionToPlayer.Y, directionToPlayer.X) - (float)Math.PI/2;
                    float currentRotation = cannon.Rotation;
                    float deltaAngle = MathHelper.WrapAngle(desiredCannonAngle - currentRotation);
                    float maxRotationSpeed = MathHelper.ToRadians(90) * elapsed;
                    if (Math.Abs(deltaAngle) > maxRotationSpeed)
                        deltaAngle = Math.Sign(deltaAngle) * maxRotationSpeed;
                    cannon.Rotation = currentRotation + deltaAngle;

                    // Fire only if cannon is nearly aligned.
                    if (Math.Abs(MathHelper.WrapAngle(desiredCannonAngle - cannon.Rotation)) < 0.1f && shootTimer >= shootInterval)
                    {
                        FireProjectile();
                        shootTimer = 0f;
                    }
                }
                else
                {
                    // Player is out of aggression range: scanning state.
                    cannon.Rotation += MathHelper.ToRadians(30) * elapsed;
                }

                // If it's been too long since last update, reset position
                if (lastSeenPlayerTimer >= 0.8f)
                    lastKnownPlayerPosition = Vector2.Zero;
            }
            else
            {
                // No known player position: scanning state.
                cannon.Rotation += MathHelper.ToRadians(30) * elapsed;
            }

            UpdateMobBehavior();
            base.Update();
        }

        public void UpdateKnownPlayerPosition(Vector2 playerPosition)
        {
            lastKnownPlayerPosition = playerPosition;
            lastSeenPlayerTimer = 0f;
        }
    }
}
