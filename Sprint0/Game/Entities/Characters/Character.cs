using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Sprint0
{
    public abstract class Character : Entity
    {
        public Cannon cannon { get; set; }
        protected ContentManager content;
        protected Dictionary<string, object> currentProjectileVariables;

        public Character(ContentManager content) : base()
        {
            this.content = content;
            currentProjectileVariables = new Dictionary<string, object>
            {
                { "projectileSpeedModifier", 1f },
                { "projectileSpread", MathHelper.ToRadians(10) },
                { "projectileType", "Default" }
            };
        }

        public void SetProjectileType(string newType)
        {
            if (newType == "Default" || newType == "Sniper" || newType == "Rocket" ||
                newType == "Shotgun" || newType == "Bomb")
                currentProjectileVariables["projectileType"] = newType;
            else
                System.Console.WriteLine("Invalid projectile type: " + newType);
        }        
        public virtual void FireProjectile()
        {
            if (cannon == null)
                return;
            // Get the tip from the cannon (computed as above).
            Vector2 tip = cannon.GetTipPosition();
            var parameters = new Dictionary<string, object>
            {
                { "projectileType", currentProjectileVariables["projectileType"] },
                { "spawnPosition", tip },
                { "cannonRotation", cannon.Rotation }
            };
            if ((string)currentProjectileVariables["projectileType"] == "Shotgun")
            {
                parameters["spreadAngle"] = MathHelper.ToRadians(10);
                parameters["numberOfProjectiles"] = 3;
            }
            commandQueue.Enqueue(new CommandRequest("CreateProjectile", parameters));
        }

        protected virtual void HandleRecoil()
        {
            string projType = (string)currentProjectileVariables["projectileType"];
            float recoilStrength = 20f;
            if (projType == "Sniper")
                recoilStrength = 30f;
            else if (projType == "Rocket")
                recoilStrength = 80f;
            Vector2 recoilDir = Vector2.Transform(new Vector2(0, 1), Matrix.CreateRotationZ(cannon.Rotation));
            velocity -= recoilDir * recoilStrength;
        }

        public void SetCannonRotation(float rotation)
        {
            if (cannon != null)
                cannon.Rotation = rotation;
        }

        public override void Update()
        {
            float elapsed = Globals.FRAMETIME;
            position += velocity * elapsed;
            if(sprite != null)
                sprite.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            
            if(sprite != null)
                sprite.Draw(spriteBatch, position);
            if(cannon != null)
                cannon.Draw(spriteBatch);
        }
    }
}
