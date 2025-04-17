using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Sprint0
{
    public class PlayerBoost {
        public bool IsBoosting { get; set; }
        private float boostDuration = 1.0f; // seconds
        private float boostCooldown = 3.0f; // seconds
        private float boostTimer = 0f;
        private float cooldownTimer = 0f;

        public void UpdateBoostState(bool tryingToBoost)
        {
            if (IsBoosting)
            {
                boostTimer -= Globals.FRAMETIME;
                if (boostTimer <= 0)
                {
                    IsBoosting = false;
                    cooldownTimer = boostCooldown;
                }
            }
            else if (cooldownTimer > 0)
            {
                cooldownTimer -= Globals.FRAMETIME;
            }

            if (tryingToBoost && cooldownTimer <= 0 && !IsBoosting)
            {
                IsBoosting = true;
                boostTimer = boostDuration;
            }
        }

        public bool CanBoost()
        {
            return IsBoosting;
        }
    }
}
