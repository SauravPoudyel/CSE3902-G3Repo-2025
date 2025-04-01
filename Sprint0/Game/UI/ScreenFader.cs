using System;
using Microsoft.Xna.Framework;

namespace Sprint0
{
    public static class ScreenFader
    {
        // 0 = transparent, 1 = fully opaque black
        public static float FadeAlpha { get; private set; } = 0f;
        private static float targetAlpha = 0f;
        private static float fadeDuration = 0f;  // Duration in seconds.
        private static float elapsedTime = 0f;
        private static float startAlpha = 0f;
        private static bool isFading = false;

        private static void StartFadeTo(float target, float duration)
        {
            startAlpha = FadeAlpha;
            targetAlpha = MathHelper.Clamp(target, 0f, 1f);
            fadeDuration = duration;
            elapsedTime = 0f;
            isFading = true;
        }

        public static void FadeToBlack(float duration = 1f)
        {
            StartFadeTo(1f, duration);
        }
        
        public static void FadeToNormal(float duration = 1f)
        {
            StartFadeTo(0f, duration);
        }

        public static void Update(GameTime gameTime)
        {
            if (!isFading)
                return;

            elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            float progress = MathHelper.Clamp(elapsedTime / fadeDuration, 0f, 1f);
            FadeAlpha = MathHelper.Lerp(startAlpha, targetAlpha, progress); // same stuff as in DayNightScycle

            if (progress >= 1f)
                isFading = false;
        }
    }
}
