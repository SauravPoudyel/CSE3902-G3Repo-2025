using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sprint0
{
    public class HealerTank : Mob
    {
        private ISprite normalSprite;
        private ISprite healSprite;
        private float healRadius = 400;
        private int healAmount = 20;

        public HealerTank(ContentManager content) : base(content)
        {
            TrackTrailsEnabled = true;
            spriteWidth = 76;
            spriteHeight = 72;
            aggressionLevel = "Passive";
            MobXP = 15;
        }

        protected override void InitializeMob()
        {
            currentMobType = EntityKeys.MobType.ShieldTank; 
            defaultMovementSpeed = 30f; 
            firingInterval = 0f; 
            
            normalSprite = new AnimatedSprite(0.3f);
            normalSprite.LoadContent(content, "TDTanksAllSprites", 1126, 334, 76, 72, 1);

            healSprite = new AnimatedSprite(0.3f);
            healSprite.LoadContent(content, "TDTanksAllSprites", 1202, 334, 76, 72, 1);
            SetSprite(normalSprite);

            cannon = new Cannon(content, Globals.NULLSPRITE_A, this, new Vector2(14, 10), 30f, new Vector2(0, 0),
                    0f, 0f, 0f, 0f);

            position = new Vector2(Globals.SCREENWIDTH / 2 + 100, 400);
            StartHealingCycle();
        }

        protected override void UpdateMobBehavior()
        {
            FollowPlayer(aggressionLevel);
        }

        protected override void ChangeMobType(EntityKeys.MobType type)
        {
            currentMobType = type;
            InitializeMob();
        }

        protected override void ResetMobPosition()
        {
            position = new Vector2(Globals.SCREENWIDTH / 2 + 100, 400);
        }

        private async void StartHealingCycle()
        {
            while (true)
            {
                int delay = Globals.random.Next(3000, 5000);
                await Task.Delay(delay);

                // kind of a stinky way of flashing the sprites but it works
                int flashDuration = 1000;
                int flashInterval = 100;
                int iterations = flashDuration / flashInterval;
                for (int i = 0; i < iterations; i++)
                {
                    if (i % 2 == 0)
                        SetSprite(healSprite);
                    else
                        SetSprite(normalSprite);
                    await Task.Delay(flashInterval);
                }
                var parameters = new Dictionary<string, object>()
                {
                    { "healOrigin", GetPosition() },
                    { "healRadius", healRadius },
                    { "healAmount", healAmount }
                };
                commandQueue.Enqueue(new CommandRequest("HealRadius", parameters));

                SetSprite(healSprite);
                await Task.Delay(2000);
                SetSprite(normalSprite);
            }
        }
    }
}
