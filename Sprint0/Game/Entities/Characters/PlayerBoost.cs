using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

namespace Sprint0
{
    public class PlayerBoost
    {
        public bool IsBoosting { get; private set; }

        private float boostDuration = 1.0f;
        private float boostCooldown = 3.0f;
        private float boostTimer = 0f;
        private float cooldownTimer = 0f;

        private string trailEffectKey = null;

        public void UpdateBoostState(bool tryingToBoost, Player player)
        {
            if (IsBoosting)
            {
                boostTimer -= Globals.FRAMETIME;

                // If trail effect isn't active, spawn it
                if (trailEffectKey == null)
                {
                    trailEffectKey = "boost_trail_" + Guid.NewGuid();
                    Vector2 backwardOffset = new Vector2((float)Math.Cos(player.bodyRotation + MathHelper.Pi), 
                                     (float)Math.Sin(player.bodyRotation + MathHelper.Pi)) * 30f;
                    var parameters = new Dictionary<string, object>
                    {
                        { "gameManager", GameManager.Instance },
                        { "spawnPosition", player.GetPosition() },
                        { "effectType", EntityKeys.EffectType.Fire },
                        { "followTarget", player },
                        { "offset", backwardOffset },
                        { "customKey", trailEffectKey },
                        { "rotation", player.bodyRotation }, // Optional: make the effect loop
                        { "damagesPlayer", false }
                    };

                    GameManager.Instance.eventManager.ExecuteCommand("SpawnEffect", parameters);
                }

                if (boostTimer <= 0f)
                {
                    EndBoost();
                }
            }
            else
            {
                if (cooldownTimer > 0f)
                    cooldownTimer -= Globals.FRAMETIME;

                if (tryingToBoost && cooldownTimer <= 0f)
                {
                    IsBoosting = true;
                    boostTimer = boostDuration;
                }
            }
        }

        private void EndBoost()
        {
            IsBoosting = false;
            cooldownTimer = boostCooldown;

            if (trailEffectKey != null)
            {
                var destroyParams = new Dictionary<string, object>
                {
                    { "destroyEntity", trailEffectKey }
                };

                GameManager.Instance.eventManager.ExecuteCommand("DestroyEntity", destroyParams);
                trailEffectKey = null;
            }
        }

        public bool CanBoost() => IsBoosting;
    }

}