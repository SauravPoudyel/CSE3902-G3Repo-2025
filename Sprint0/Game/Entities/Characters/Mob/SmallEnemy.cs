using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0
{
    public class SmallEnemy : Mob
    {
        private float movementTimer;
        private int movementPhase;

        public SmallEnemy(ContentManager content) : base(content) { }

        protected override void InitializeMob()
        {
            mobType = MobType.SmallEnemy;
            defaultSpeed = 200f;
            shootInterval = 1.4f; // SmallEnemy shoots faster
            movementTimer = 0f;
            movementPhase = 0;
            currentProjectileVariables["projectileType"] = "Default";

            AnimatedSprite enemySprite = new AnimatedSprite(0.3f);
            enemySprite.LoadContent(content, "TDTanksAllSprites", 768, 256, 95, 113, 1);
            SetSprite(enemySprite);

            ISprite cannonSprite = new AnimatedSprite(0.3f);
            cannonSprite.LoadContent(content, "TDTanksAllSprites", 832, 186, 28, 64, 1);
            cannon = new Cannon(cannonSprite, this, new Vector2(14, 10), 30f,
                                0f, MathHelper.ToRadians(20), MathHelper.PiOver2, MathHelper.Pi + MathHelper.PiOver2);

            velocity = new Vector2(defaultSpeed, 0);
        }

        protected override void UpdateMobBehavior(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            movementTimer += elapsed;
            if (movementTimer > 0.6f)
            {
                movementPhase = (movementPhase + 1) % 4;
                movementTimer = 0f;
            }
            switch (movementPhase)
            {
                case 0: velocity = new Vector2(defaultSpeed, 0); break;
                case 1: velocity = new Vector2(0, defaultSpeed); break;
                case 2: velocity = new Vector2(-defaultSpeed, 0); break;
                case 3: velocity = new Vector2(0, -defaultSpeed); break;
            }
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
