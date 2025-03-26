using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using static Sprint0.EntityKeys;


namespace Sprint0
{

    public abstract class Mob : Character
    {
        protected EntityKeys.MobType currentMobType;
        protected float defaultMovementSpeed;
        protected float firingTimer;
        protected float firingInterval;
        private float requestPlayerTimer;
        private float timeSinceLastPlayerSeen;
        protected Vector2 lastKnownPlayerPosition;
        protected float aggressionRange;
        protected string aggressionLevel = "Aggressive";
        private bool neutralToggle = true; 
        private bool isNeutralTaskRunning = false; // for thread task management
        public int MobXP;

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
            firingTimer += Globals.FRAMETIME;
            requestPlayerTimer -= Globals.FRAMETIME;
            if (requestPlayerTimer <= 0f)
            {
                var commandParams = new Dictionary<string, object> { { "mob", this } };
                commandQueue.Enqueue(new CommandRequest("RequestPlayerPosition", commandParams));
                requestPlayerTimer = 0.5f;
            }
            UpdateMobBehavior();
            base.Update();
        }

        public void PointCannonPlayer()
        {
            if (lastKnownPlayerPosition != Vector2.Zero)
            {
                float distanceToPlayer = Vector2.Distance(GetPosition(), lastKnownPlayerPosition);
                if (distanceToPlayer <= aggressionRange)
                {
                    Vector2 directionToPlayer = lastKnownPlayerPosition - GetPosition();
                    float desiredCannonAngle = (float)Math.Atan2(directionToPlayer.Y, directionToPlayer.X) - MathHelper.PiOver2;
                    float currentRotation = cannon.Rotation;
                    float angleDiff = MathHelper.WrapAngle(desiredCannonAngle - currentRotation);
                    float maxTurnRadians = MathHelper.ToRadians(60) * Globals.FRAMETIME;

                    if (Math.Abs(angleDiff) > maxTurnRadians)
                        angleDiff = Math.Sign(angleDiff) * maxTurnRadians;
                    cannon.Rotation = currentRotation + angleDiff;
                    if (Math.Abs(MathHelper.WrapAngle(desiredCannonAngle - cannon.Rotation)) < 0.15f && firingTimer >= firingInterval)
                    {
                        FireProjectile();
                        firingTimer = 0f;
                    }
                }
                else
                {
                    cannon.Rotation += MathHelper.ToRadians(20) * Globals.FRAMETIME;
                }
                if (timeSinceLastPlayerSeen >= 0.8f)
                    lastKnownPlayerPosition = Vector2.Zero;
            }
            else
            {
                cannon.Rotation += MathHelper.ToRadians(20) * Globals.FRAMETIME;
            }
        }

        public void FollowPlayer(string aggression)
        {
            if (lastKnownPlayerPosition != Vector2.Zero)
            {
                if (aggression.Equals("Follow-Axis"))
                {
                    // For horizontal ships, follow the player's X coordinate.
                    if (currentMobType == EntityKeys.MobType.ShipHorizontal)
                    {
                        float delta = lastKnownPlayerPosition.X - position.X;
                        if (Math.Abs(delta) > 5f)
                        {
                            // Fix the rotation so the sprite never flips (always face the same direction)
                            bodyRotation = -MathHelper.PiOver2;  // For example, always face right
                            // Since Character.Update moves using velocity.Y, use that value to encode direction:
                            // A positive velocity means move right (if facing right) and a negative velocity means move left.
                            velocity = new Vector2(0, Math.Sign(delta) * defaultMovementSpeed);
                        }
                        else
                        {
                            velocity = Vector2.Zero;
                            if (firingTimer >= firingInterval)
                            {
                                FireProjectile();
                                firingTimer = 0f;
                            }
                        }
                    }
                    // For vertical ships, follow the player's Y coordinate.
                    else if (currentMobType == EntityKeys.MobType.ShipVertical)
                    {
                        float delta = lastKnownPlayerPosition.Y - position.Y;
                        if (Math.Abs(delta) > 5f)
                        {
                            float movementMultiplier = (delta > 0) ? 1f : -1f;
                            // Vertical ship: face down (or adjust as desired)
                            bodyRotation = 0f;
                            velocity = new Vector2(0, defaultMovementSpeed * movementMultiplier);
                        }
                        else
                        {
                            velocity = Vector2.Zero;
                            if (firingTimer >= firingInterval)
                            {
                                FireProjectile();
                                firingTimer = 0f;
                            }
                        }
                    }
                    else
                    {
                        // If not an axis-following ship, fall back to default behavior.
                        DefaultFollow(aggression);
                    }
                }
                else
                {
                    // Default behavior for "Aggressive", "Passive", "Neutral"
                    DefaultFollow(aggression);
                }
            }
            else
            {
                velocity = Vector2.Zero;
            }
        }

        private void DefaultFollow(string aggression)
        {
            Vector2 dirToPlayer = lastKnownPlayerPosition - position;
            if (dirToPlayer != Vector2.Zero)
                dirToPlayer.Normalize();

            float trackingTurnRate = MathHelper.ToRadians(90) * Globals.FRAMETIME;
            float desiredAngle = (float)Math.Atan2(dirToPlayer.Y, dirToPlayer.X) - MathHelper.PiOver2;
            float angleDiff = MathHelper.WrapAngle(desiredAngle - bodyRotation);

            if (Math.Abs(angleDiff) > trackingTurnRate)
                angleDiff = Math.Sign(angleDiff) * trackingTurnRate;
            bodyRotation += angleDiff;

            float movementMultiplier = 1f;
            if (aggression.Equals("Passive"))
                movementMultiplier = -1f;
            else if (aggression.Equals("Neutral"))
            {
                if (!isNeutralTaskRunning)
                {
                    isNeutralTaskRunning = true;
                    SwitchNeutralState();
                }
                movementMultiplier = neutralToggle ? 1f : -1f;
            }
            // Default moves along the Y-axis.
            velocity = new Vector2(0, defaultMovementSpeed * movementMultiplier);
        }


        private async void SwitchNeutralState()
        {
            while (aggressionLevel.Equals("Neutral"))
            {
                await Task.Delay(new Random().Next(2000, 5000)); // Randomly switch between passive and aggressive 2-5 second delay
                neutralToggle = !neutralToggle;
            }
            isNeutralTaskRunning = false;
        }

        public void UpdateKnownPlayerPosition(Vector2 newPlayerPosition)
        {
            lastKnownPlayerPosition = newPlayerPosition;
            timeSinceLastPlayerSeen = 0f;
        }

        public override void OnDeath()
        {
            base.OnDeath();
            Globals.PlayerData.UpdateVariable("XP", MobXP);
        }

    }
}
