using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0
{
    public class Turret : Mob
    {
        public Turret(ContentManager content) : base(content)
        {
        }

        protected override void InitializeMob()
        {
            mobType = MobType.Turret;
            defaultSpeed = 0f;
            shootInterval = 2.5f; // Shoots every 2.5 seconds
            currentProjectileVariables["projectileType"] = "Rocket";

            AnimatedSprite turretSprite = new AnimatedSprite(0.3f);
            turretSprite.LoadContent(content, "TDTowerDefenseSprites", 2444, 908, 104, 104, 1);
            SetSprite(turretSprite);

            ISprite turretCannonSprite = new AnimatedSprite(0.3f);
            turretCannonSprite.LoadContent(content, "TDTowerDefenseSprites", 2455, 1290, 85, 110, 1);
            cannon = new Cannon(turretCannonSprite, this, new Vector2(42, 34), 30f,
                            0f, MathHelper.ToRadians(20), MathHelper.PiOver2, MathHelper.Pi + MathHelper.PiOver2);
            cannon.CannonEffects = SpriteEffects.FlipVertically; 
            velocity = Vector2.Zero;
        }

        protected override void UpdateMobBehavior(GameTime gameTime)
        {
            velocity = Vector2.Zero;
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
