using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0
{
    public class BossTank : Mob
    {
        public BossTank(ContentManager content) : base(content) { }

        protected override void InitializeMob()
        {
            mobType = MobType.BossTank;
            defaultSpeed = 80f;
            shootInterval = 4f; 
            currentProjectileVariables["projectileType"] = "Shotgun";

            AnimatedSprite bossSprite = new AnimatedSprite(0.3f);
            bossSprite.LoadContent(content, "TDTanksAllSprites", 641, 661, 123, 144, 1);
            SetSprite(bossSprite);

            ISprite cannonSprite = new AnimatedSprite(0.3f);
            cannonSprite.LoadContent(content, "TDTanksAllSprites", 832, 186, 28, 64, 1);
            cannon = new Cannon(cannonSprite, this, new Vector2(14, 10), 30f,
                                0f, MathHelper.ToRadians(20), MathHelper.PiOver2, MathHelper.Pi + MathHelper.PiOver2);


            velocity = new Vector2(defaultSpeed, 0);
        }

        protected override void UpdateMobBehavior()
        {
            float rightBound = Globals.SCREENWIDTH / 2 + 300;
            float leftBound = Globals.SCREENWIDTH / 2;
            if (position.X > rightBound)
                velocity = new Vector2(-defaultSpeed, 0);
            else if (position.X < leftBound)
                velocity = new Vector2(defaultSpeed, 0);
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
