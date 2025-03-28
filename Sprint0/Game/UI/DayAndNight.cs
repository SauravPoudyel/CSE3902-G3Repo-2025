using Microsoft.Xna.Framework;
using System;

namespace Sprint0
{
    public class DayNightCycle
    {
        public float CycleDuration { get; set; } = 60f; // in seconds
        public float MaxOverlayAlpha { get; set; } = 0.7f; // 0 means no darkening; 1 would be completely dark.

        private float timer = 0f;

        public DayNightCycle(float startTime = 0f, float cycleDuration = 60f, float maxOverlayAlpha = 0.7f)
        {
            CycleDuration = cycleDuration;
            MaxOverlayAlpha = maxOverlayAlpha;
            timer = startTime % CycleDuration;
        }

        public void Update(TimeSpan elapsedGameTime)
        {
            timer += (float)elapsedGameTime.TotalSeconds;
            timer %= CycleDuration;
        }

        public float GetOverlayAlpha()
        {
            // Cosine wave: cos(2π(t/CycleDuration - 0.5)) gives 1 at noon, -1 at midnight. 
            // math from https://www.reddit.com/r/gamedev/comments/7cti1q/2d_daynight_cycles/
            float normalized = (float)Math.Cos(2 * Math.PI * (timer / CycleDuration - 0.5));
            float alpha = (1f - normalized) / 2f;
            return alpha * MaxOverlayAlpha;
        }

        public void AdvanceDayNightCycle(float cycleFraction)
        {
            float offset = cycleFraction * CycleDuration;
            timer = (timer + offset) % CycleDuration;
        }
    }
}
