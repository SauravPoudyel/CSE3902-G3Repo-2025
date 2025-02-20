using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0
{
    public class ExplodingTank : Mob
    {
        private float explosionTimer;
        private int explosionPhase;
        private readonly string[] explosionSpriteNames = { "Explosion1", "Explosion2", "Explosion3" };
        private bool isExploding;

        public ExplodingTank(ContentManager content) : base(content)
        {
        }

        protected override void InitializeMob()
        {
            mobType = MobType.ExplodingTank;
            defaultSpeed = 80f;
            shootInterval = 3f; // Shoots every 3 seconds
            explosionTimer = 0f;
            explosionPhase = 0;
            isExploding = false;
            currentProjectileVariables["projectileType"] = "Default";

            AnimatedSprite tankSprite = new AnimatedSprite(0.3f);
            tankSprite.LoadContent(content, "TDTanksAllSprites", 952, 569, 81, 76, 1);
            SetSprite(tankSprite);

            ISprite cannonSprite = new AnimatedSprite(0.3f);
            cannonSprite.LoadContent(content, "TDTanksAllSprites", 832, 186, 28, 64, 1);
            cannon = new Cannon(cannonSprite, this, new Vector2(14, 10), 30f,
                                0f, MathHelper.ToRadians(20), MathHelper.PiOver2, MathHelper.Pi + MathHelper.PiOver2);

            velocity = new Vector2(defaultSpeed, 0);
        }

        protected override void UpdateMobBehavior()
        {
            float elapsed = Globals.FRAMETIME;

            // Stop shooting while exploding
            if (!isExploding)
            {
                float rightBound = Globals.SCREENWIDTH / 2 + 300;
                float leftBound = Globals.SCREENWIDTH / 2;
                if (position.X > rightBound)
                    velocity = new Vector2(-defaultSpeed, 0);
                else if (position.X < leftBound)
                    velocity = new Vector2(defaultSpeed, 0);
            }
            else
            {
                shootTimer = 0; 
                velocity = Vector2.Zero; // Stops movement while exploding
            }

            // Explosion logic:
            explosionTimer += elapsed;
            if (!isExploding && explosionTimer > 2f)
            {
                isExploding = true;
                explosionTimer = 0f;
                explosionPhase = 0;
                cannon.SetSprite(Globals.NULLSPRITE); // Remove cannon while exploding
                SetSprite(LoadExplosionSprite(explosionPhase));
            }

            if (isExploding)
            {
                if (explosionTimer > 0.5f && explosionPhase < explosionSpriteNames.Length)
                {
                    SetSprite(LoadExplosionSprite(explosionPhase));
                    explosionPhase++;
                    explosionTimer = 0f;
                }

                if (explosionPhase >= explosionSpriteNames.Length)
                {
                    isExploding = false;
                    InitializeMob(); 
                }
            }
        }

        private ISprite LoadExplosionSprite(int phase)
        {
            AnimatedSprite explosionSprite = new AnimatedSprite(0.3f);
            switch (phase)
            {
                case 0:
                    explosionSprite.LoadContent(content, "TDTanksAllSprites", 765, 508, 113, 112, 1);
                    break;
                case 1:
                    explosionSprite.LoadContent(content, "TDTanksAllSprites", 642, 256, 124, 126, 1);
                    break;
                case 2:
                    explosionSprite.LoadContent(content, "TDTanksAllSprites", 641, 383, 124, 125, 1);
                    break;
            }
            return explosionSprite;
        }

        protected override void SetEnemyType(MobType type)
        {
            mobType = type;
            InitializeMob();
        }

        protected override void ResetPosition()
        {
            position = new Vector2(Globals.SCREENWIDTH / 2 + 100, 400);
        }
    }
}
