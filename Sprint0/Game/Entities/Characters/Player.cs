using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Sprint0
{
    public class Player : Character
    {
        // effect fields
        private PlayerBoost playerBoost = new PlayerBoost();
        public void UpdateBoostState(bool tryingToBoost) 
            => playerBoost.UpdateBoostState(tryingToBoost, this);
        public bool CanBoost() 
            => playerBoost.CanBoost();
        public float speedMultiplier = 1f;   
        public bool shieldActive = false, 
                    isInvis = false, 
                    isFly = false;
        public float baseShootInterval = 1.2f;
        public float currentShootInterval;   
        ISprite effectSprite = new Sprite(0.4f); 
        public float rotationInput = 0f;
        private float timeSinceLastShot = 0f;
        public static Player Instance { get; private set; }
        public bool CanFire { get { return timeSinceLastShot >= currentShootInterval; }}
        public bool IsImmortal { get; set; } = false;

        public List<Effect> activeFireEffects = new List<Effect>();
        private float fireDamageCooldown;
        private const float FIRE_DAMAGE_INTERVAL = 1.0f; //Fire damage gap
        private const int FIRE_DAMAGE_PER_TICK = 10; //Fire damage

        public Player(ContentManager content) : base(content)
        {
            spriteWidth = 65;
            spriteHeight = 65;
            EntityKey = "player"; 
            health = 1000; 
            Instance = this;
            currentShootInterval = baseShootInterval;

            bodyRotation = 0f;
            
            AddSprite("TankBody", new Sprite(0.3f));
            sprites["TankBody"].LoadContent(content, "TDTanksAllSprites", 795, 1052, 74, 76, 1);
            SetSprite("TankBody");

            AddSprite("InvisTankBody", new Sprite(0.3f));
            sprites["InvisTankBody"].LoadContent(content, "TDTanksAllSprites", 1114, 1, 84, 84, 1);

            AddSprite("FlyingTankBody", new Sprite(0.3f));
            sprites["FlyingTankBody"].LoadContent(content, "TDTanksAllSprites", 1135, 88, 86, 92, 1);

            effectSprite =  new Sprite(0.08f);
            effectSprite.LoadContent(content, "EffectSprites", 0, 128, 128, 128, 12);

            ISprite cannonSprite = new Sprite(0.3f);
            cannonSprite.LoadContent(content, "TDTanksAllSprites", 1060, 837, 24, 60, 1);
            cannon = new Cannon(content, cannonSprite, this, new Vector2(12, 5), 50f, new Vector2(12, 60),
                    0f, MathHelper.ToRadians(20));

            ISprite cannonSpriteInvis = new Sprite(0.3f);
            cannonSpriteInvis.LoadContent(content, "TDTanksAllSprites", 1110, 88, 24, 60, 1);
            cannon.AddSprite("InvisTankCannon", cannonSpriteInvis); 
            
            trackTrailSprite.LoadContent(content, "TDTanksAllSprites", 953, 665, 73, 88, 1);

            currentProjectileVariables["projectileType"] = "Default";
        }



        public override void ChangeHealth(int change)
        {
            if(IsImmortal) return; 
            base.ChangeHealth(change);
            if(change < 0)
                isDamaged = true;
            else
                isHealed = true;
            Globals.PlayerData.UpdateVariable("Health", change); 
        }

        public override void OnDeath()
        {
            base.OnDeath();
            Dictionary<string, object> parameters2 = new Dictionary<string, object>{{ "player", this }};
            commandQueue.Enqueue(new CommandRequest("PlayerDeath", parameters2));
            this.SetPosition(new Vector2(700, 700)); // to respawn at the proper point
        }

        public void Interact() {
            Dictionary<string, object> parameters2 = new Dictionary<string, object>{{ "player", this}};
            commandQueue.Enqueue(new CommandRequest("PlayerInteract", parameters2));
        }

        public void MoveLevel(int levelNum) {
            var parameters2 = new Dictionary<string, object>();
            parameters2.Add("level", levelNum);
            commandQueue.Enqueue(new CommandRequest("SetLevel", parameters2));
        }

        public override void Update()
        {
            prevPosition = position;
            timeSinceLastShot += Globals.PLAYERFRAMETIME;
            currentShootInterval = currentShootInterval / Globals.PlayerData.GetInt("FireRateModifier");

            float turnSpeed = 1.5f;
            bodyRotation += rotationInput * turnSpeed * Globals.PLAYERFRAMETIME;

            Vector2 forwardDirection = new Vector2((float)Math.Sin(bodyRotation), -(float)Math.Cos(bodyRotation));
            float forwardSpeed = velocity.Y * Globals.PlayerData.GetInt("SpeedModifier");
            position += forwardDirection * forwardSpeed * speedMultiplier * Globals.PLAYERFRAMETIME;

            // Enqueue audio drive command
            float maxSpeed = 100f;
            Dictionary<string, object> driveParams = new Dictionary<string, object>
            {
                { "currentSpeed", Math.Abs(forwardSpeed) },
                { "maxSpeed", maxSpeed }
            };
            commandQueue.Enqueue(new CommandRequest("AudioDrive", driveParams));

            CalculateBounds(spriteWidth, spriteHeight);

            if (TrackTrailsEnabled)
                TrackTrail.UpdateTrackTrails(trackTrailList, Globals.PLAYERFRAMETIME, position, bodyRotation, trackTrailSprite, ref trackTrailSpawnTimer, trackTrailSpawnInterval);

            if (Math.Abs(forwardSpeed) > 0.1f)
                sprite.Update();

            if (health <= 0 && !isDead)
            {
                isDead = true;
                OnDeath();
            }

            PowerUpFactory.UpdateEffects(this);
            cannon.Update();

            //Fire damage logic process
            ProcessFireDamage();

            if (shieldActive)
                effectSprite.Update();

            rotationInput = 0f;
        }

        public bool IsTouchingFire(Effect effect)
        {
            return this.Bounds.Intersects(effect.Bounds);
        }

        private void ProcessFireDamage()
        {
            // Clear the fire effect if the player is not touching the fire or the fire vanishes
            activeFireEffects.RemoveAll(fire =>
                fire.IsFinished || !fire.GetBounds().Intersects(this.GetBounds()));

            // Damage logic
            if (activeFireEffects.Count > 0)
            {
                fireDamageCooldown -= Globals.PLAYERFRAMETIME;
                if (fireDamageCooldown <= 0)
                {
                    ChangeHealth(-FIRE_DAMAGE_PER_TICK);
                    fireDamageCooldown = FIRE_DAMAGE_INTERVAL;
                    isDamaged = true;
                }
            }
            else
            {
                fireDamageCooldown = 0;
            }
        }

        public override void FireProjectile()
        {
            // If not ready to fire, do nothing.
            if (timeSinceLastShot < currentShootInterval)
                return;

            if (cannon == null)
                return;

            Vector2 tip = cannon.GetTipPosition();

            if((string)currentProjectileVariables["projectileType"] == "Mine")
                tip = position; 
                
            var parametersFire = new Dictionary<string, object>
            {
                { "projectileType", currentProjectileVariables["projectileType"] },
                { "spawnPosition", tip },
                { "cannonRotation", cannon.Rotation },
                { "owner", this }
            };
            if ((string)currentProjectileVariables["projectileType"] == "Shotgun")
            {
                parametersFire["spreadAngle"] = MathHelper.ToRadians(10);
                parametersFire["numberOfProjectiles"] = 3;
            }
            commandQueue.Enqueue(new CommandRequest("CreateProjectile", parametersFire));

            if(!((string)currentProjectileVariables["projectileType"] == "Mine"))
                cannon.TriggerFiringEffect();
                
            timeSinceLastShot = 0f;
            AudioManager.PlaySound(AudioManager.SoundKey.Shoot, 0.5f);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 tankCenter = new Vector2(37, 38);
            foreach (var trail in trackTrailList)
                trail.Draw(spriteBatch);

            sprite?.Draw(spriteBatch, position, SpriteEffects.None, bodyRotation, null, changeIndicator);

            cannon.Draw(spriteBatch);

            if(shieldActive)
                effectSprite.Draw(spriteBatch, position); 
        }
    }
}
