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
        protected float speedMultiplier { get; set; } = 1f;
        protected List<TrackTrail> trackTrailList;
        protected float trackTrailSpawnTimer, trackTrailSpawnInterval = 0.4f;
        public bool TrackTrailsEnabled { get; set; } = true;
        protected ISprite trackTrailSprite = new Sprite(0.4f);
        public float health { protected get; set; } = 100f;
        public bool isDead { protected get; set; } = false; 
        public bool isDamaged { get; set; } = false; 
        public bool isHealed { get; set; } = false;
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
            AudioManager.PlaySound(AudioManager.SoundKey.Shoot, 0.5f);
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
                bounds = CollisionBoundCalculator.Calculate(position, bodyRotation, spriteWidth, spriteHeight);
            }
            if (health <= 0 && !isDead)
            {
                isDead = true;
                OnDeath();
            }
            prevPosition = position;
            cannon.Update();
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
