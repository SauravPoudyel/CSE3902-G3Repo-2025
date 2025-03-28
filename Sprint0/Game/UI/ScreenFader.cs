using System;
using Microsoft.Xna.Framework;

namespace Sprint0
{
    public static class ScreenFader
    {
        // Current overlay opacity (0 = transparent, 1 = fully opaque black).
        public static float FadeAlpha { get; private set; } = 0f;

        // Internal state
        private static float targetAlpha = 0f;
        private static float fadeDuration = 0f;  // Duration in seconds.
        private static float elapsedTime = 0f;
        private static float startAlpha = 0f;
        private static bool isFading = false;

        // Begins a fade toward the given target alpha over 'duration' seconds.
        private static void StartFadeTo(float target, float duration)
        {
            startAlpha = FadeAlpha;
            targetAlpha = MathHelper.Clamp(target, 0f, 1f);
            fadeDuration = duration;
            elapsedTime = 0f;
            isFading = true;
        }

        // Fades the screen to black over the specified duration (in seconds).
        public static void FadeToBlack(float duration = 1f)
        {
            StartFadeTo(1f, duration);
        }

        // Fades the screen back to clear over the specified duration (in seconds).
        public static void FadeToNormal(float duration = 1f)
        {
            StartFadeTo(0f, duration);
        }

        // Call this method in your Update loop to progress the fade.
        public static void Update(GameTime gameTime)
        {
            if (!isFading)
                return;

            elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            float progress = MathHelper.Clamp(elapsedTime / fadeDuration, 0f, 1f);
            FadeAlpha = MathHelper.Lerp(startAlpha, targetAlpha, progress);

            if (progress >= 1f)
                isFading = false;
        }
    }
}
