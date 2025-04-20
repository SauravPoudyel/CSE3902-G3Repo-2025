using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0
{
    class BoostHUD : IHUD
    {
        private Texture2D icon;
        private Texture2D gray;
        private Texture2D cyan;
        private PlayerData playerData;
        private Player player => Player.Instance;

        private float currentBoost = 100f;               // 0–100%
        private const float rechargeRate = 100f / 3f;     // % per second (3s cooldown)

        private int maxWidth = 300;
        private int height = 30;
        private int x = 61;
        private int y = 900;

        public BoostHUD(Texture2D texture)
        {
            this.icon = texture;
            this.playerData = Globals.PlayerData;

            gray = new Texture2D(texture.GraphicsDevice, 1, 1);
            gray.SetData(new[] { Color.Gray });
            cyan = new Texture2D(texture.GraphicsDevice, 1, 1);
            cyan.SetData(new[] { Color.Cyan });
        }

        public void Update()
        {
            int raw = playerData.GetInt("Boost");           // 0→100 during boost
            raw = MathHelper.Clamp(raw, 0, 100);

            if (player != null && player.CanBoost())
            {
                // bar decreases from full→empty while boosting
                currentBoost = 100 - raw;
            }
            else
            {
                // recharge back to full when not boosting
                currentBoost = MathHelper.Clamp(
                    currentBoost + rechargeRate * Globals.FRAMETIME,
                    0f, 100f
                );
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int filledWidth = (int)(maxWidth * (currentBoost / 100f));

            // gray background
            spriteBatch.Draw(
                gray,
                new Rectangle(x, y, maxWidth, height),
                Color.White * 0.5f
            );

            // cyan fill
            spriteBatch.Draw(
                cyan,
                new Rectangle(x, y, filledWidth, height),
                Color.White
            );
        }
    }
}
