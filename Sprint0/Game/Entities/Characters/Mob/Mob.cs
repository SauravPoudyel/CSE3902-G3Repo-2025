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
                    if (Math.Abs(MathHelper.WrapAngle(desiredCannonAngle - cannon.Rotation)) < 0.1f && firingTimer >= firingInterval)
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
                if (aggression.Equals("FollowX-Axis"))
                {
                    float deltaX = lastKnownPlayerPosition.X - position.X;
                    // Move along X only if the difference is significant.
                    if (Math.Abs(deltaX) > 5f)
                    {
                        velocity = new Vector2(Math.Sign(deltaX) * defaultMovementSpeed, 0);
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
                    // Face right (0 rad) if player is to the right, left (PI) if to the left.
                    bodyRotation = (deltaX >= 0) ? 0f : MathHelper.Pi;
                }
                else if (aggression.Equals("FollowY-Axis"))
                {
                    // Calculate difference in Y between the player and the enemy.
                    float deltaY = lastKnownPlayerPosition.Y - position.Y;
                    
                    // If the difference is larger than a small threshold, move toward the player's Y.
                    if (Math.Abs(deltaY) > 5f)
                    {
                        // Set velocity only in the Y direction.
                        velocity = new Vector2(0, Math.Sign(deltaY) * defaultMovementSpeed);
                    }
                    else
                    {
                        // Close enough on Y-axis: stop moving and fire if possible.
                        velocity = Vector2.Zero;
                        if (firingTimer >= firingInterval)
                        {
                            FireProjectile();
                            firingTimer = 0f;
                        }
                    }
                    
                    // Set the facing: if player is below, face down (Pi/2); if above, face up (-Pi/2).
                    bodyRotation = (deltaY >= 0) ? MathHelper.PiOver2 : -MathHelper.PiOver2;
                }
                else
                {
                    // Default behavior for aggression levels such as "Aggressive", "Passive", "Neutral"
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

                    // In the default case we move along the Y-axis.
                    velocity = new Vector2(0, defaultMovementSpeed * movementMultiplier);
                }
            }
            else
            {
                velocity = Vector2.Zero;
            }
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
    }
}
