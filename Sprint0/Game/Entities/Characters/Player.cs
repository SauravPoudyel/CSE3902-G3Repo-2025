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
        public bool shieldActive = false;
        public bool isInvis;
        public float baseShootInterval = 1.2f;    
        public float currentShootInterval;   
        ISprite effectSprite = new StaticSprite(); 
        public float rotationInput = 0f;
        private float timeSinceLastShot = 0f;
        public static Player Instance { get; private set; }

        public Player(ContentManager content) : base(content)
        {
            EntityKey = "player"; 
            Instance = this;
            currentShootInterval = baseShootInterval;

            bodyRotation = 0f;
            
            AddSprite("TankBody", new AnimatedSprite(0.3f));
            sprites["TankBody"].LoadContent(content, "TDTanksAllSprites", 795, 1052, 74, 76, 1);
            SetSprite("TankBody");

            AddSprite("InvisTankBody", new AnimatedSprite(0.3f));
            sprites["InvisTankBody"].LoadContent(content, "TDTanksAllSprites", 1114, 1, 84, 84, 1);

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

        public override void Damage(int damage)
        {
            health -= damage;
            Globals.PlayerData.TemporaryHealth -= damage; 
        }

        public override void OnDeath()
        {
            base.OnDeath();
            var parameters2 = new Dictionary<string, object>();
            commandQueue.Enqueue(new CommandRequest("DestroyEntity", parameters2));
        }

        public override void Update()
        {
            prevPosition = position;
            timeSinceLastShot += Globals.PLAYERFRAMETIME;

            // Use dedicated rotation input
            float turnSpeed = 1.5f; // Tweak for responsiveness
            bodyRotation += rotationInput * turnSpeed * Globals.PLAYERFRAMETIME;

            Vector2 forwardDirection = new Vector2((float)Math.Sin(bodyRotation), -(float)Math.Cos(bodyRotation));
            float forwardSpeed = -velocity.Y;
            position += forwardDirection * forwardSpeed * speedMultiplier * Globals.PLAYERFRAMETIME;

            CalculateBounds(74, 76);
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
            // If not enough time has passed, ignore input.
            if (timeSinceLastShot < currentShootInterval)
                return;

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

            timeSinceLastShot = 0f;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 tankCenter = new Vector2(37, 38);
            foreach (var trail in trackTrailList)
                trail.Draw(spriteBatch);
            sprite.Draw(spriteBatch, position, SpriteEffects.FlipVertically, bodyRotation, tankCenter, Color.White);
            cannon.Draw(spriteBatch);

            if(shieldActive)
                effectSprite.Draw(spriteBatch, position); 
        }
    }
}
