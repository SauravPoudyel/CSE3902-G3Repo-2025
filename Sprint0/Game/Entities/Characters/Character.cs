using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

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
        protected float trackTrailSpawnTimer;
        protected float trackTrailSpawnInterval = 0.2f;
        protected bool TrackTrailsEnabled { get; set; } = true;
        protected ISprite trackTrailSprite;

        public float health = 100f;
        protected bool isDead = false;

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
            trackTrailSprite = new StaticSprite();
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

        public void SetCannonRotation(float rotation) 
        {
            cannon.Rotation = rotation; 
        }

    public virtual void FireProjectile()
    {
        if (cannon == null)
            return;
 
        Vector2 tip = cannon.GetTipPosition();
        var parameters = new Dictionary<string, object>
        {
<<<<<<< HEAD
            { "projectileType", currentProjectileVariables["projectileType"] },
            { "spawnPosition", tip },
            { "cannonRotation", cannon.Rotation }
        };
        if ((string)currentProjectileVariables["projectileType"] == "Shotgun")
        {
            parameters["spreadAngle"] = MathHelper.ToRadians(10);
            parameters["numberOfProjectiles"] = 3;
=======
            if (cannon == null) return;
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
>>>>>>> origin/test
        }
        commandQueue.Enqueue(new CommandRequest("CreateProjectile", parameters));

        cannon.TriggerFiringEffect();
    }


        public void SetProjectileType(string newType)
        {
            if (newType == "Default" || newType == "Sniper" || newType == "Rocket" ||
                newType == "Shotgun" || newType == "Bomb" || newType == "Teleporter")
                currentProjectileVariables["projectileType"] = newType;
        }

        public override void Update()
        {
            float deltaTime = Globals.FRAMETIME;
            Vector2 forward = new Vector2((float)Math.Sin(bodyRotation), -(float)Math.Cos(bodyRotation));
            position += forward * -velocity.Y * speedMultiplier * deltaTime;

            // Update track trails.
            if (TrackTrailsEnabled)
                TrackTrail.UpdateTrackTrails(trackTrailList, deltaTime, position, bodyRotation, trackTrailSprite, ref trackTrailSpawnTimer, trackTrailSpawnInterval);

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
            float halfWidth = spriteWidth * 0.5f;
            float halfHeight = spriteHeight * 0.5f;
            float cosAngle = (float)Math.Cos(bodyRotation);
            float sinAngle = (float)Math.Sin(bodyRotation);
            Vector2[] corners = new Vector2[4]
            {
                new Vector2(-halfWidth, -halfHeight),
                new Vector2(halfWidth, -halfHeight),
                new Vector2(halfWidth, halfHeight),
                new Vector2(-halfWidth, halfHeight)
            };
            for (int i = 0; i < 4; i++)
            {
                float x = corners[i].X;
                float y = corners[i].Y;
                corners[i].X = x * cosAngle - y * sinAngle + position.X;
                corners[i].Y = x * sinAngle + y * cosAngle + position.Y;
            }
            float minX = corners[0].X, maxX = corners[0].X;
            float minY = corners[0].Y, maxY = corners[0].Y;
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
            var effectParams = new Dictionary<string, object>
            {
                { "spawnPosition", position },
                { "effectType", "explosion" }
            };
<<<<<<< HEAD
            commandQueue.Enqueue(new CommandRequest("SpawnEffect", effectParams));

=======
            commandQueue.Enqueue(new CommandRequest("SpawnExplosion", parameters));

            var parameters2 = new Dictionary<string, object>()
            {
                {"destroyEntity", "mob"}
            };
            commandQueue.Enqueue(new CommandRequest("DestroyEntity", parameters2));
>>>>>>> origin/test
        }

        public virtual void Damage(int damage)
        {
            health -= damage;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (var trail in trackTrailList)
                trail.Draw(spriteBatch);
            sprite?.Draw(spriteBatch, position, SpriteEffects.None, bodyRotation);
            cannon?.Draw(spriteBatch);
        }
    }

}
