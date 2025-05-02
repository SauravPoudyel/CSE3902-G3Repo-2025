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
        public ISprite originalTankSprite;
        public ISprite originalCannonSprite;
        private bool isInvisibilityTaskRunning = false;

        public StealthTank(ContentManager content) : base(content)
        {
            TrackTrailsEnabled = true;
            aggressionLevel = "Passive";
            spriteWidth = 84;
            spriteHeight = 80;
            MobXP = 30;
        }

        protected override void InitializeMob()
        {
            currentMobType = EntityKeys.MobType.StealthTank; // Use ShieldTank to represent the stealth enemy
            defaultMovementSpeed = 50f;
            normalSpeed = defaultMovementSpeed;
            aggressiveSpeed = normalSpeed * 1.7f;
            firingInterval = 2f; // fire slow when not in invisible state
            StartInvisibilityCycle();
        }

        protected override void ChangeMobType(EntityKeys.MobType type)
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
