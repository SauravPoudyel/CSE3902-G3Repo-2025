using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;

namespace Sprint0
{
    public class TurningTank : Mob
    {
        private enum TurnState { RotateTo45, MoveForward, RotateOpposite, MoveBackward }
        private TurnState currentTurnState;
        private float turnStateTimer;
        private float desiredRotation;

        public TurningTank(ContentManager content) : base(content)
        {
            TrackTrailsEnabled = true; 
            spriteWidth = 81;
            spriteHeight = 76;
        }

        protected override void InitializeMob()
        {
            currentMobType = MobType.TurningTank;
            defaultMovementSpeed = 80f;
            firingInterval = 3f;
            currentProjectileVariables["projectileType"] = "Default";
            var turningBodySprite = new AnimatedSprite(0.3f);
            turningBodySprite.LoadContent(content, "TDTanksAllSprites", 952, 569, 81, 76, 1);
            SetSprite(turningBodySprite);
            var turningCannonSprite = new AnimatedSprite(0.3f);
            turningCannonSprite.LoadContent(content, "TDTanksAllSprites", 832, 186, 28, 64, 1);
            cannon = new Cannon(content, turningCannonSprite, this, new Vector2(14, 10), 30f,
                                0f, MathHelper.ToRadians(20), MathHelper.PiOver2, MathHelper.Pi + MathHelper.PiOver2);
            currentTurnState = TurnState.RotateTo45;
            turnStateTimer = 0f;
            bodyRotation = 0f;
            desiredRotation = MathHelper.PiOver4;
            velocity = Vector2.Zero;
        }

        protected override void UpdateMobBehavior()
        {
            float eTime = Globals.FRAMETIME;
            turnStateTimer += eTime;
            switch (currentTurnState)
            {
                case TurnState.RotateTo45:
                    bodyRotation += MathHelper.ToRadians(25) * eTime;
                    if (bodyRotation >= desiredRotation)
                    {
                        bodyRotation = desiredRotation;
                        currentTurnState = TurnState.MoveForward;
                        turnStateTimer = 0f;
                        velocity.X = 0f;
                        velocity.Y = -defaultMovementSpeed;
                    }
                    break;
                case TurnState.MoveForward:
                    if (turnStateTimer >= 2f)
                    {
                        currentTurnState = TurnState.RotateOpposite;
                        turnStateTimer = 0f;
                        desiredRotation = bodyRotation + MathHelper.Pi;
                    }
                    break;
                case TurnState.RotateOpposite:
                    bodyRotation += MathHelper.ToRadians(50) * eTime;
                    if (bodyRotation >= desiredRotation)
                    {
                        bodyRotation = desiredRotation;
                        currentTurnState = TurnState.MoveBackward;
                        turnStateTimer = 0f;
                        velocity.X = 0f;
                        velocity.Y = -(defaultMovementSpeed * 0.5f);
                    }
                    break;
                case TurnState.MoveBackward:
                    if (turnStateTimer >= 3.5f)
                    {
                        currentTurnState = TurnState.RotateTo45;
                        turnStateTimer = 0f;
                        bodyRotation = 0f;
                        desiredRotation = MathHelper.PiOver4;
                        velocity = Vector2.Zero;
                    }
                    break;
            }
        }

        protected override void ChangeMobType(MobType type)
        {
            currentMobType = type;
            InitializeMob();
        }

        protected override void ResetMobPosition()
        {
            position = new Vector2(Globals.SCREENWIDTH / 2 + 100, 400);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            if (sprite != null) sprite.Draw(spriteBatch, position, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, bodyRotation);
            cannon?.Draw(spriteBatch);
        }
    }
}
