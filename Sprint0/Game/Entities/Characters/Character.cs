using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sprint0
{
    public abstract class Character : Entity
    {
        public Cannon cannon { get; set; }
        protected ContentManager content;
        protected Dictionary<string, object> currentProjectileVariables;
        public float bodyRotation { get; set; }
        protected float speedMultiplier = 1f;
        protected List<TrackTrail> trackTrailList;
        protected float trackTrailSpawnTimer, trackTrailSpawnInterval = 0.4f;
        public bool TrackTrailsEnabled { get; set; } = true;
        protected ISprite trackTrailSprite = new StaticSprite();
        public float health = 100f;
        public bool isDead = false, isDamaged = false, isHealed = false;
        protected Color? changeIndicator = null;

        public Character(ContentManager content) : base()
        {
            this.content = content;
            currentProjectileVariables = new Dictionary<string, object>
            {
                { "projectileSpeedModifier", 1f },
                { "projectileSpread", MathHelper.ToRadians(10) },
                { "projectileType", "Default" }
            };
            trackTrailList = new List<TrackTrail>();
            trackTrailSpawnTimer = 0f;
            trackTrailSprite.LoadContent(content, "TDTanksAllSprites", 953, 665, 73, 88, 1);
            bodyRotation = 0f;
        }

        protected void TurnTowards(float targetAngle, float turnSpeed)
        {
            float angleDiff = MathHelper.WrapAngle(targetAngle - bodyRotation);
            float maxTurn = turnSpeed * Globals.FRAMETIME;
            if (Math.Abs(angleDiff) > maxTurn)
                angleDiff = Math.Sign(angleDiff) * maxTurn;
            bodyRotation += angleDiff;
        }

        public void SetCannonRotation(float rotation) { cannon.Rotation = rotation; }

        public virtual void FireProjectile()
        {
            if (cannon == null)
                return;

            Vector2 tip = cannon.GetTipPosition();
            var parameters = new Dictionary<string, object>
            {
                { "projectileType", currentProjectileVariables["projectileType"] },
                { "spawnPosition", tip },
                { "cannonRotation", cannon.Rotation },
                { "owner", this }
            };
            if ((string)currentProjectileVariables["projectileType"] == "Shotgun")
            {
                parameters["spreadAngle"] = MathHelper.ToRadians(10);
                parameters["numberOfProjectiles"] = 3;
            }
            commandQueue.Enqueue(new CommandRequest("CreateProjectile", parameters));
            cannon.TriggerFiringEffect();
            AudioManager.PlaySound(AudioManager.SoundKey.Shoot);
        }

        public void SetProjectileType(string newType)
        {
            if (newType == "Default" || newType == "Sniper" || newType == "Rocket" ||
                newType == "Shotgun" || newType == "Mine" || newType == "Teleporter" || 
                newType == "Laser")
                currentProjectileVariables["projectileType"] = newType;
        }

        public override void Update()
        {
            float dt = Globals.FRAMETIME;
            Vector2 forward = new Vector2((float)Math.Sin(bodyRotation), -(float)Math.Cos(bodyRotation));
            position += -forward * velocity.Y * speedMultiplier * dt;
            if (TrackTrailsEnabled)
                TrackTrail.UpdateTrackTrails(trackTrailList, dt, position, bodyRotation, trackTrailSprite, ref trackTrailSpawnTimer, trackTrailSpawnInterval);
            if (sprite != null)
            {
                sprite.Update();
                CalculateBounds(spriteWidth, spriteHeight);
            }
            if (health <= 0 && !isDead)
            {
                isDead = true;
                OnDeath();
            }
            prevPosition = position;
            cannon.Update();
        }

        protected void CalculateBounds(float spriteWidth, float spriteHeight)
        {
            float halfW = spriteWidth * 0.5f, halfH = spriteHeight * 0.5f;
            float cosA = (float)Math.Cos(bodyRotation), sinA = (float)Math.Sin(bodyRotation);
            Vector2[] corners = new Vector2[4]
            {
                new Vector2(-halfW, -halfH),
                new Vector2(halfW, -halfH),
                new Vector2(halfW, halfH),
                new Vector2(-halfW, halfH)
            };
            for (int i = 0; i < 4; i++)
            {
                float x = corners[i].X, y = corners[i].Y;
                corners[i].X = x * cosA - y * sinA + position.X;
                corners[i].Y = x * sinA + y * cosA + position.Y;
            }
            float minX = corners[0].X, maxX = corners[0].X, minY = corners[0].Y, maxY = corners[0].Y;
            for (int i = 1; i < 4; i++)
            {
                if (corners[i].X < minX) minX = corners[i].X;
                if (corners[i].X > maxX) maxX = corners[i].X;
                if (corners[i].Y < minY) minY = corners[i].Y;
                if (corners[i].Y > maxY) maxY = corners[i].Y;
            }
            bounds = new Rectangle((int)minX, (int)minY, (int)(maxX - minX), (int)(maxY - minY));
        }

        public virtual void OnDeath()
        {
            commandQueue.Enqueue(new CommandRequest("SpawnEffect", new Dictionary<string, object>
            {
                { "spawnPosition", position },
                { "effectType", EntityKeys.EffectType.Explosion }
            }));
            int coinCount = Globals.random.Next(2, 5);
            Rectangle r = GetBounds();
            for (int i = 0; i < coinCount; i++)
            {
                int coinX = Globals.random.Next(r.Left, r.Right);
                int coinY = Globals.random.Next(r.Top, r.Bottom);
                Vector2 coinPos = new Vector2(coinX, coinY);
                // Create coin using Item as a base class
                Item coin = new Item(content, EntityKeys.ItemType.GoldTag);
                coin.EntityKey = "coin_" + i + Guid.NewGuid().ToString();
                commandQueue.Enqueue(new CommandRequest("CreateEntity", new Dictionary<string, object>
                {
                    { "create", coin },
                    { "entityName", coin.EntityKey },
                    { "position", coinPos },
                    { "velocity", Vector2.Zero }
                }));
            }
            commandQueue.Enqueue(new CommandRequest("DestroyEntity", new Dictionary<string, object>
            {
                { "destroyEntity", EntityKey }
            }));

            //Add command for XP increment
            
        }

        public virtual void ChangeHealth(int change)
        {
            health += change;
            if (change < 0)
            {
                isDamaged = true;
                changeIndicator = Color.Red;
                Task.Delay(450).ContinueWith(_ => { isDamaged = false; changeIndicator = null; });
            }
            else if (change > 0)
            {
                isHealed = true;
                changeIndicator = Color.Green;
                Task.Delay(450).ContinueWith(_ => { isHealed = false; changeIndicator = null; });
            }
            if (health <= 0 && !isDead)
            {
                isDead = true;
                OnDeath();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            float scaleFactor = (spriteWidth / 73f + spriteHeight / 88f) / 2f; // scale the track trail to match the sprite size
            foreach (var trail in trackTrailList)
                trail.Draw(spriteBatch, scaleFactor); 
            sprite?.Draw(spriteBatch, position, SpriteEffects.None, bodyRotation, null, changeIndicator);
            cannon?.Draw(spriteBatch);
        }
    }
}
