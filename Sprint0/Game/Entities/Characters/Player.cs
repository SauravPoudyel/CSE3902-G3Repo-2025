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
        public float speedMultiplier = 1f;   
        public bool shieldActive = false, 
                    isInvis = false, 
                    isFly = false;
        public float baseShootInterval = 1.2f;
        public float currentShootInterval;   
        ISprite effectSprite = new StaticSprite(); 
        public float rotationInput = 0f;
        private float timeSinceLastShot = 0f;
        public static Player Instance { get; private set; }
        public bool CanFire { get { return timeSinceLastShot >= currentShootInterval; }}

        public Player(ContentManager content) : base(content)
        {
            spriteWidth = 65;
            spriteHeight = 65;
            EntityKey = "player"; 
            Instance = this;
            currentShootInterval = baseShootInterval;

            bodyRotation = 0f;
            
            AddSprite("TankBody", new AnimatedSprite(0.3f));
            sprites["TankBody"].LoadContent(content, "TDTanksAllSprites", 795, 1052, 74, 76, 1);
            SetSprite("TankBody");

            AddSprite("InvisTankBody", new AnimatedSprite(0.3f));
            sprites["InvisTankBody"].LoadContent(content, "TDTanksAllSprites", 1114, 1, 84, 84, 1);

            AddSprite("FlyingTankBody", new AnimatedSprite(0.3f));
            sprites["FlyingTankBody"].LoadContent(content, "TDTanksAllSprites", 1135, 88, 86, 92, 1);

            effectSprite =  new AnimatedSprite(0.08f);
            effectSprite.LoadContent(content, "EffectSprites", 0, 128, 128, 128, 12);

            ISprite cannonSprite = new AnimatedSprite(0.3f);
            cannonSprite.LoadContent(content, "TDTanksAllSprites", 1060, 837, 24, 60, 1);
            cannon = new Cannon(content, cannonSprite, this, new Vector2(12, 5), 50f, new Vector2(12, 60),
                    0f, MathHelper.ToRadians(20));

            ISprite cannonSpriteInvis = new AnimatedSprite(0.3f);
            cannonSpriteInvis.LoadContent(content, "TDTanksAllSprites", 1110, 88, 24, 60, 1);
            cannon.AddSprite("InvisTankCannon", cannonSpriteInvis); 
            
            trackTrailSprite.LoadContent(content, "TDTanksAllSprites", 953, 665, 73, 88, 1);

            currentProjectileVariables["projectileType"] = "Default";
        }



        public override void ChangeHealth(int change)
        {
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
            var parameters2 = new Dictionary<string, object>();
            commandQueue.Enqueue(new CommandRequest("Reset", parameters2));
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

            // Use dedicated rotation input
            float turnSpeed = 1.5f; // Tweak for responsiveness
            bodyRotation += rotationInput * turnSpeed * Globals.PLAYERFRAMETIME;

            Vector2 forwardDirection = new Vector2((float)Math.Sin(bodyRotation), -(float)Math.Cos(bodyRotation));
            float forwardSpeed = velocity.Y * Globals.PlayerData.GetInt("SpeedModifier"); // Use velocity.Y directly for both forward and backward movement


            position += forwardDirection * forwardSpeed * speedMultiplier * Globals.PLAYERFRAMETIME;

            CalculateBounds(spriteWidth, spriteHeight);
            
            if(TrackTrailsEnabled)
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

            if(shieldActive)
                effectSprite.Update(); 

            // Reset rotation input after applying it.
            rotationInput = 0f;
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
            AudioManager.PlaySound(AudioManager.SoundKey.Shoot);
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
