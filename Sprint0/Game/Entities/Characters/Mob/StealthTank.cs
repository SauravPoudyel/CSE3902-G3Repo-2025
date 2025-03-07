using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Threading.Tasks;

namespace Sprint0
{
    public class StealthTank : Mob
    {
        private bool isInvisible = false;
        private float normalSpeed;
        private float aggressiveSpeed;
        private ISprite originalTankSprite;
        private ISprite originalCannonSprite;
        private bool isInvisibilityTaskRunning = false;

        public StealthTank(ContentManager content) : base(content)
        {
            TrackTrailsEnabled = true;
            aggressionLevel = "Passive";
            spriteWidth = 84;
            spriteHeight = 80;
        }

        protected override void InitializeMob()
        {
            currentMobType = MobType.StealthTank; // Use ShieldTank to represent the stealth enemy
            defaultMovementSpeed = 50f;
            normalSpeed = defaultMovementSpeed;
            aggressiveSpeed = normalSpeed * 1.7f;
            firingInterval = 2f; // fire slow when not in invisible state

            var tankSprite = new AnimatedSprite(0.3f);
            tankSprite.LoadContent(content, "TDTanksAllSprites", 876, 783, 84, 80, 1);
            SetSprite(tankSprite);

            originalTankSprite = tankSprite;

            var cannonSprite = new AnimatedSprite(0.3f);
            cannonSprite.LoadContent(content, "TDTanksAllSprites", 1104, 152, 16, 52, 1);
            cannon = new Cannon(content, cannonSprite, this, new Vector2(8, 5), 50f, new Vector2(8, 60), 0f, MathHelper.ToRadians(20));

            originalCannonSprite = cannon.GetSprite();

            StartInvisibilityCycle();
        }

        protected override void UpdateMobBehavior()
        {
            if (isInvisible)
            {
                FollowPlayer("Aggressive");
                PointCannonPlayer();
            }
            else
            {
                FollowPlayer("Passive");
                PointCannonPlayer();
            }
        }

        protected override void ChangeMobType(MobType type)
        {
            currentMobType = type;
            InitializeMob();
        }

        protected override void ResetMobPosition()
        {
            position = new Vector2(Globals.SCREENWIDTH / 2 + 100, 400);
        }

        private async void StartInvisibilityCycle()
        {
            if (isInvisibilityTaskRunning) return;
            isInvisibilityTaskRunning = true;

            while (true) // Optionally condition this on enemy health or existence
            {
                int delay = Globals.random.Next(3000, 6000); // Wait 3-5 seconds
                await Task.Delay(delay);

                // Go invisible: increase speed and switch aggression
                isInvisible = true;
                firingInterval = 0.6f; // fire fast when in invisible state
                aggressionLevel = "Aggressive";
                defaultMovementSpeed = aggressiveSpeed;
                SetSprite(Globals.NULLSPRITE_S);

                if (cannon != null)
                    cannon.SetSprite(Globals.NULLSPRITE_S);
                int delay2 = Globals.random.Next(4000, 5500); // Wait 4-5.5 seconds
                await Task.Delay(delay2);

                // Revert to visible state
                isInvisible = false;
                firingInterval = 2f; 
                aggressionLevel = "Passive";
                defaultMovementSpeed = normalSpeed;
                SetSprite(originalTankSprite);
                if (cannon != null)
                    cannon.SetSprite(originalCannonSprite);
            }
        }
    }
}
