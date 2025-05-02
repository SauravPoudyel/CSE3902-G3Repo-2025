using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using static Sprint0.EntityKeys;

namespace Sprint0
{
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
        protected string aggressionLevel = "Aggressive";
        private bool neutralToggle = true;
        private bool isNeutralTaskRunning = false; // for thread task management
        public int MobXP;

        private IMobBehaviorState behaviorState;

        public Mob(ContentManager content) : base(content)
        {
            firingTimer = 0f;
            requestPlayerTimer = 0.5f;
            lastKnownPlayerPosition = Vector2.Zero;
            aggressionRange = 1000f;
            InitializeMob();
            SetBehaviorState(aggressionLevel);
        }

        protected abstract void InitializeMob();
        protected abstract void ChangeMobType(MobType type);
        protected abstract void ResetMobPosition();

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

            behaviorState.Update(this);
            base.Update();
        }

        public void SetBehaviorState(string aggression)
        {
            if (aggression == "Follow-Axis")
            {
                if (currentMobType == MobType.ShipHorizontal)
                    behaviorState = new AxisXFollowState();
                else if (currentMobType == MobType.ShipVertical)
                    behaviorState = new AxisYFollowState();
                else
                    behaviorState = new DefaultFollowState();
            }
            else
            {
                behaviorState = new DefaultFollowState();
            }
        }

        public void PointCannonPlayer()
        {
            if (lastKnownPlayerPosition != Vector2.Zero)
            {
                float distanceToPlayer = Vector2.Distance(GetPosition(), lastKnownPlayerPosition);
                if (distanceToPlayer <= aggressionRange * Globals.GlobalMobData.GetModifier(MobData.MobModifiers.AggressionRange))
                {
                    Vector2 directionToPlayer = lastKnownPlayerPosition - GetPosition();
                    float desiredCannonAngle = (float)Math.Atan2(directionToPlayer.Y, directionToPlayer.X) - MathHelper.PiOver2;
                    float currentRotation = cannon.Rotation;
                    float angleDiff = MathHelper.WrapAngle(desiredCannonAngle - currentRotation);
                    float maxTurnRadians = MathHelper.ToRadians(60) * Globals.FRAMETIME;

                    if (Math.Abs(angleDiff) > maxTurnRadians)
                        angleDiff = Math.Sign(angleDiff) * maxTurnRadians;
                    cannon.Rotation = currentRotation + angleDiff;

                    if (Math.Abs(MathHelper.WrapAngle(desiredCannonAngle - cannon.Rotation)) < 0.15f &&
                        firingTimer >= firingInterval * Globals.GlobalMobData.GetModifier(MobData.MobModifiers.FiringInterval))
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

        private async void SwitchNeutralState()
        {
            while (aggressionLevel == "Neutral")
            {
                await Task.Delay(new Random().Next(2000, 5000)); // 2–5s random toggle
                neutralToggle = !neutralToggle;
            }
            isNeutralTaskRunning = false;
        }

        public void StartNeutralCycle()
        {
            if (!isNeutralTaskRunning)
            {
                isNeutralTaskRunning = true;
                SwitchNeutralState();
            }
        }

        public void UpdateKnownPlayerPosition(Vector2 newPlayerPosition)
        {
            lastKnownPlayerPosition = newPlayerPosition;
            timeSinceLastPlayerSeen = 0f;
        }

        public override void OnDeath()
        {
            base.OnDeath();
            Globals.PlayerData.UpdateVariable("XP", (int)(MobXP * Globals.GlobalMobData.GetModifier(MobData.MobModifiers.XP)));
            if (currentMobType == MobType.ShipVertical || currentMobType == MobType.ShipHorizontal)
            {
                Globals.PlayerData.UpdateVariable("ShipKilled", 1);
            }
            Globals.PlayerData.UpdateVariable(currentMobType.ToString() + "Killed", 1);
        }
    }
}
