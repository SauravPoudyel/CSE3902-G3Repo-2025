using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint0.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint0.Sprites
{
    public class Link : ISprite
    {
        public Vector2 Position { get; private set; }
        public Direction Facing { get; private set; } = Direction.Down;
        public bool IsAttacking { get; private set; }
        public bool IsDamaged { get; private set; }

        private const float MoveSpeed = 8f;
        private float damageTimer;
        private const float DamageDuration = 0.3f;

        private AnimationManager animationManager;
        private Texture2D texture;
        private float attackTimer;
        private const float AttackDuration = 0.3f;

        public void LoadContent(Texture2D texture)
        {
            this.texture = texture;
            InitializeAnimations();
        }

        private void InitializeAnimations()
        {
            animationManager = new AnimationManager();

            // Walk animation
            var walkUp = new Animation(texture, 69, 11, 16, 16, 2, 0.2f);
            var walkDown = new Animation(texture, 1, 11, 16, 16, 2, 0.2f);
            var walkRight = new Animation(texture, 35, 11, 16, 16, 2, 0.2f);

            // Attack animation
            var attackUp = new Animation(texture, 1, 97, 16, 28, 4, 0.08f, SpriteEffects.None, new Vector2(0, -26));
            var attackDown = new Animation(texture, 1, 47, 16, 27, 4, 0.08f, SpriteEffects.None, new Vector2(0, 26));
            var attackRight = new Animation(texture, 1, 77, 27, 16, 4, 0.08f, SpriteEffects.None, new Vector2(0, 0));

            animationManager.AddAnimation("Walk_Up", walkUp);
            animationManager.AddAnimation("Walk_Down", walkDown);
            animationManager.AddAnimation("Walk_Left", walkRight);
            animationManager.AddAnimation("Walk_Right", walkRight);

            animationManager.AddAnimation("Attack_Up", attackUp);
            animationManager.AddAnimation("Attack_Down", attackDown);
            animationManager.AddAnimation("Attack_Left", attackRight);
            animationManager.AddAnimation("Attack_Right", attackRight);
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update attack timer
            if (IsAttacking)
            {
                attackTimer -= deltaTime;
                if (attackTimer <= 0)
                {
                    IsAttacking = false;
                }
            }

            UpdateAnimationState();
            animationManager.Update(gameTime);
        }

        private void UpdateAnimationState()
        {
            if (IsAttacking)
            {
                string baseAnim = Facing == Direction.Left ? "Attack_Right" : $"Attack_{Facing}";
                animationManager.Play(baseAnim);
            }
            else if (!IsDamaged)
            {
                string baseAnim = Facing == Direction.Left ? "Walk_Right" : $"Walk_{Facing}";
                animationManager.Play(baseAnim);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (animationManager.currentAnimation == null)
                return;

            var currentFrame = animationManager.currentAnimation.Frames[animationManager.currentFrame];
            Vector2 origin = new Vector2(currentFrame.Width / 2f, currentFrame.Height / 2f);

            // Effect when hurt
            Color color = IsDamaged ? Color.Red * 0.5f : Color.White;
            SpriteEffects effects = Facing == Direction.Left ?
        SpriteEffects.FlipHorizontally :
        SpriteEffects.None;

            animationManager.Draw(spriteBatch, Position, color, 0f, origin, 4f, effects);
        }

        public void Move(Direction direction)
        {
            if (IsAttacking || IsDamaged) return;

            Facing = direction;
            Position += direction.ToVector2() * MoveSpeed;
        }

        public void Attack()
        {
            if (!IsAttacking && !IsDamaged)
            {
                IsAttacking = true;
                attackTimer = AttackDuration;
            }
        }

        public void TakeDamage()
        {
            if (!IsDamaged)
            {
                IsDamaged = true;
                damageTimer = DamageDuration;
                // Health lose logic in the future
            }
        }

        public void UseItem(int itemIndex)
        {
            if (!IsAttacking && !IsDamaged)
            {
                // Item system in the future
            }
        }
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public static class DirectionExtensions
    {
        public static Vector2 ToVector2(this Direction direction)
        {
            return direction switch
            {
                Direction.Up => new Vector2(0, -1),
                Direction.Down => new Vector2(0, 1),
                Direction.Left => new Vector2(-1, 0),
                Direction.Right => new Vector2(1, 0),
                _ => Vector2.Zero
            };
        }
    }
}
