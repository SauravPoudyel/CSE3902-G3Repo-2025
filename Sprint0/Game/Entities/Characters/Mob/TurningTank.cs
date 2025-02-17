using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Sprint0
{
    public class TurningTank : Mob
    {
        private enum TurnState { RotatingTo45, MovingForward, RotatingToOpposite, MovingBack }
        private TurnState turnState;
        private float stateTimer;
        private float targetRotation;  // Target rotation in radians
        private float bodyRotation;    // Actual rotation of the tank body

        public TurningTank(ContentManager content) : base(content)
        {
            InitializeMob();
        }

        protected override void InitializeMob()
        {
            mobType = MobType.TurningTank;
            defaultSpeed = 100f;
            shootInterval = 3f;
            currentProjectileVariables["projectileType"] = "Default";

            AnimatedSprite tankSprite = new AnimatedSprite(0.3f);
            tankSprite.LoadContent(content, "TDTanksAllSprites", 952, 569, 81, 76, 1);
            SetSprite(tankSprite);

            ISprite cannonSprite = new AnimatedSprite(0.3f);
            cannonSprite.LoadContent(content, "TDTanksAllSprites", 832, 186, 28, 64, 1);
            cannon = new Cannon(cannonSprite, this, new Vector2(14, 10), 30f,
                                0f, MathHelper.ToRadians(20), MathHelper.PiOver2, MathHelper.Pi + MathHelper.PiOver2);

            turnState = TurnState.RotatingTo45;
            stateTimer = 0f;
            bodyRotation = 0f;
            targetRotation = MathHelper.PiOver4; // 45 degrees in radians
            velocity = Vector2.Zero;
        }

        protected override void UpdateMobBehavior(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            stateTimer += elapsed;

            switch (turnState)
            {
                case TurnState.RotatingTo45:
                    {
                        float rotationSpeed = MathHelper.ToRadians(45);
                        bodyRotation += rotationSpeed * elapsed;
                        if (bodyRotation >= targetRotation)
                        {
                            bodyRotation = targetRotation;
                            turnState = TurnState.MovingForward;
                            stateTimer = 0f;
                            // Forward velocity aligned with bodyRotation.
                            velocity = new Vector2((float)Math.Cos(bodyRotation), (float)Math.Sin(bodyRotation)) * defaultSpeed;
                        }
                    }
                    break;
                case TurnState.MovingForward:
                    {
                        if (stateTimer >= 2f)
                        {
                            turnState = TurnState.RotatingToOpposite;
                            stateTimer = 0f;
                            targetRotation = bodyRotation + MathHelper.Pi;
                        }
                    }
                    break;
                case TurnState.RotatingToOpposite:
                    {
                        float rotationSpeed = MathHelper.ToRadians(90);
                        bodyRotation += rotationSpeed * elapsed;
                        if (bodyRotation >= targetRotation)
                        {
                            bodyRotation = targetRotation;
                            turnState = TurnState.MovingBack;
                            stateTimer = 0f;
                            velocity = -new Vector2((float)Math.Cos(targetRotation - MathHelper.Pi), (float)Math.Sin(targetRotation - MathHelper.Pi)) * defaultSpeed * 0.75f;
                        }
                    }
                    break;
                case TurnState.MovingBack:
                    {
                        if (stateTimer >= 3.5f)
                        {
                            turnState = TurnState.RotatingTo45;
                            stateTimer = 0f;
                            bodyRotation = 0f;
                            targetRotation = MathHelper.PiOver4;
                            velocity = Vector2.Zero;
                        }
                    }
                    break;
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

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (sprite != null)
                sprite.Draw(spriteBatch, position, SpriteEffects.None, bodyRotation);
            if (cannon != null)
                cannon.Draw(spriteBatch);
        }
    }
}
