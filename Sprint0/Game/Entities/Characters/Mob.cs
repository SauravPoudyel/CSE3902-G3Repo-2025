using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Sprint0
{
    public class Mob : Entity
    {
        private enum MobType { BossTank, SmallEnemy, ExplodingTank, Turret }
        private static readonly MobType[] MobTypesArray = { MobType.BossTank, MobType.SmallEnemy, MobType.ExplodingTank, MobType.Turret };

        private Dictionary<string, float> timers;
        private ContentManager content;
        private MobType mobType;
        private int mobPhase;
        private bool isExploding;
        private int explosionPhase;
        private readonly string[] explosionSpriteNames = { "Explosion1", "Explosion2", "Explosion3" };
        private Cannon cannon;

        public Mob(ContentManager content)
        {
            this.content = content;
            velocity = new Vector2(80, 0);
            mobType = MobType.BossTank;
            timers = new Dictionary<string, float>
            {
                {"shoot", 0f},
                {"movement", 0f},
                {"explosionDelay", 0f},
                {"explosionFrame", 0f}
            };
            isExploding = false;
            explosionPhase = 0;
            LoadSprites();

            // Create a default cannon instance; its configuration is completed in SetEnemyType.
            cannon = new Cannon(sprites["Cannon"], this, new Vector2(14, 10), new Vector2(0, 48));

            SetEnemyType(mobType);
        }

        private void LoadSprites()
        {
            AddSprite("BossTank", new AnimatedSprite(0.3f));
            sprites["BossTank"].LoadContent(content, "TDTanksAllSprites", 641, 661, 123, 144, 1);

            AddSprite("SmallEnemy", new AnimatedSprite(0.3f));
            sprites["SmallEnemy"].LoadContent(content, "TDTanksAllSprites", 768, 256, 95, 113, 1);

            AddSprite("Cannon", new AnimatedSprite(0.3f));
            sprites["Cannon"].LoadContent(content, "TDTanksAllSprites", 832, 186, 28, 64, 1);

            AddSprite("ExplodingTank", new AnimatedSprite(0.3f));
            sprites["ExplodingTank"].LoadContent(content, "TDTanksAllSprites", 952, 569, 81, 76, 1);

            AddSprite("Explosion1", new AnimatedSprite(0.3f));
            sprites["Explosion1"].LoadContent(content, "TDTanksAllSprites", 765, 508, 113, 112, 1);

            AddSprite("Explosion2", new AnimatedSprite(0.3f));
            sprites["Explosion2"].LoadContent(content, "TDTanksAllSprites", 642, 256, 124, 126, 1);

            AddSprite("Explosion3", new AnimatedSprite(0.3f));
            sprites["Explosion3"].LoadContent(content, "TDTanksAllSprites", 641, 383, 124, 125, 1);

            AddSprite("Turret", new AnimatedSprite(0.3f));
            sprites["Turret"].LoadContent(content, "TDTowerDefenseSprites", 2444, 908, 104, 104, 1);

            AddSprite("TurretCannon", new AnimatedSprite(0.3f));
            sprites["TurretCannon"].LoadContent(content, "TDTowerDefenseSprites", 2455, 1290, 85, 110, 1);

            AddSprite("NULL", new AnimatedSprite(0.3f));
            sprites["NULL"].LoadContent(content, "TDTanksAllSprites", 129, 0, 12, 12, 1);
        }

        private void InitializeCannonSettings()
        {
            if (mobType == MobType.SmallEnemy)
            {
                cannon.AngularVelocity = MathHelper.ToRadians(80);
                cannon.LowerBound = MathHelper.ToRadians(120);
                cannon.UpperBound = MathHelper.ToRadians(240);
            }
            else
            {
                cannon.AngularVelocity = MathHelper.ToRadians(20);
                cannon.LowerBound = MathHelper.PiOver2;
                cannon.UpperBound = MathHelper.Pi + MathHelper.PiOver2;
            }
        }

        private void SetEnemyType(MobType type)
        {
            mobType = type;
            SetSprite(sprites[type.ToString()]);
            isExploding = false;
            explosionPhase = 0;
            timers["explosionDelay"] = 0f;
            timers["explosionFrame"] = 0f;
            timers["shoot"] = 0f;
            if (type == MobType.Turret)
            {
                cannon.SetSprite(sprites["TurretCannon"]);
                cannon.cannonEffects = SpriteEffects.FlipVertically;
                cannon.Pivot = new Vector2(42, 34);
                cannon.TipOffset = new Vector2(0, 48);
            }
            else
            {
                cannon.SetSprite(sprites["Cannon"]);
                cannon.cannonEffects = SpriteEffects.None;
                cannon.Pivot = new Vector2(14, 10);
                cannon.TipOffset = new Vector2(0, 48);
            }
            InitializeCannonSettings();
        }

        public string GetEnemyType()
        {
            return mobType.ToString();
        }

        private void ResetPosition()
        {
            position = new Vector2(Globals.SCREENWIDTH / 2 + 100, 400);
        }

        public void CycleEnemyNext()
        {
            int index = Array.IndexOf(MobTypesArray, mobType);
            SetEnemyType(MobTypesArray[(index + 1) % MobTypesArray.Length]);
            ResetPosition();
        }

        public void CycleEnemyPrev()
        {
            int index = Array.IndexOf(MobTypesArray, mobType);
            SetEnemyType(MobTypesArray[(index - 1 + MobTypesArray.Length) % MobTypesArray.Length]);
            ResetPosition();
        }

        public override void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
            if (mobType == MobType.ExplodingTank)
                UpdateExplodingTank(gameTime);
            else
                UpdateMobBehavior(gameTime);
            UpdateCannon(gameTime);
            UpdatePosition(gameTime);
        }

        private void UpdatePosition(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            position += velocity * elapsed;
        }

        private void UpdateMobBehavior(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (mobType == MobType.BossTank)
            {
                float rightBound = Globals.SCREENWIDTH / 2 + 300;
                float leftBound = Globals.SCREENWIDTH / 2;
                if (position.X > rightBound)
                    velocity = new Vector2(-80, 0);
                else if (position.X < leftBound)
                    velocity = new Vector2(80, 0);
            }
            else if (mobType == MobType.SmallEnemy)
            {
                timers["movement"] += elapsed;
                if (timers["movement"] > 0.6f)
                {
                    mobPhase = (mobPhase + 1) % 4;
                    timers["movement"] = 0f;
                }
                if (mobPhase == 0) velocity = new Vector2(200, 0);
                else if (mobPhase == 1) velocity = new Vector2(0, 200);
                else if (mobPhase == 2) velocity = new Vector2(-200, 0);
                else if (mobPhase == 3) velocity = new Vector2(0, -200);
            }
            else if (mobType == MobType.Turret)
            {
                velocity = Vector2.Zero;
            }
        }

        private void UpdateExplodingTank(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float rightBound = Globals.SCREENWIDTH / 2 + 300;
            float leftBound = Globals.SCREENWIDTH / 2;

            if (position.X > rightBound)
                velocity = new Vector2(-80, 0);
            else if (position.X < leftBound)
                velocity = new Vector2(80, 0);

            timers["explosionDelay"] += elapsed;

            // **Start Explosion & Remove Cannon Immediately**
            if (!isExploding && timers["explosionDelay"] > 2f)
            {
                isExploding = true;
                timers["explosionFrame"] = 0f;
                explosionPhase = 0;
                timers["explosionDelay"] = 0f;
                
                cannon.SetSprite(sprites["NULL"]); 
                SetSprite(sprites[explosionSpriteNames[0]]); 
            }

            if (isExploding)
            {
                timers["explosionFrame"] += elapsed;
                if (timers["explosionFrame"] > 0.5f && explosionPhase < explosionSpriteNames.Length)
                {
                    SetSprite(sprites[explosionSpriteNames[explosionPhase]]);
                    explosionPhase++;
                    timers["explosionFrame"] = 0f;
                }

                if (explosionPhase >= explosionSpriteNames.Length)
                {
                    isExploding = false;
                    SetEnemyType(mobType);  
                }
            }
        }


        private void UpdateCannon(GameTime gameTime)
        {
            cannon.Update(gameTime);
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (!isExploding)
            {
                timers["shoot"] += elapsed;
                if (timers["shoot"] > 4f)
                {
                    FireProjectile();
                    timers["shoot"] = 0f;
                }
            }
        }

        public void FireProjectile()
        {
            Vector2 cannonTip = cannon.GetTipPosition();
            Dictionary<MobType, string> projectileMap = new Dictionary<MobType, string>
            {
                { MobType.BossTank, "Shotgun" },
                { MobType.SmallEnemy, "Default" },
                { MobType.ExplodingTank, "Bomb" },
                { MobType.Turret, "Rocket" }
            };
            string projectileType = projectileMap.ContainsKey(mobType) ? projectileMap[mobType] : "Default";
            var parameters = new Dictionary<string, object>
            {
                { "projectileType", projectileType },
                { "spawnPosition", cannonTip },
                { "cannonRotation", cannon.Rotation },
                { "shooterVelocity", velocity }
            };
            if (projectileType.Equals("Shotgun"))
            {
                parameters["spreadAngle"] = MathHelper.ToRadians(10);
                parameters["numberOfProjectiles"] = 3;
            }
            commandQueue.Enqueue(new CommandRequest("CreateProjectile", parameters));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, position, SpriteEffects.None, 0f);
            if(!isExploding)
            cannon.Draw(spriteBatch);
        }
    }
}
