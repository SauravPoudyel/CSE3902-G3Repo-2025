using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Sprint0
{
    public class GameManager
    {
        private Dictionary<string, Entity> entities;
        private PhysicsManager physicsManager;
        private SpriteManager spriteManager;
        private ContentManager content;
        public EventManager eventManager { get; private set; }
        private List<IScreen> screens;
        public Game1 Game { get; private set; }

        public GameManager(Game1 game)
        {
            Game = game;
            entities = new Dictionary<string, Entity>();
            physicsManager = new PhysicsManager();
            spriteManager = new SpriteManager();
            eventManager = new EventManager(game, this);
            screens = new List<IScreen>();
        }

        public ContentManager GetContent()
        {
            return content;
        }

        public Dictionary<string, Entity> GetEntities()
        {
            return entities;
        }

        public Entity GetEntity(string entityKey)
        {
            return entities.ContainsKey(entityKey) ? entities[entityKey] : null;
        }

        public void SetEntity(string key, Entity entity)
        {
            entities[key] = entity;
        }

        public void RemoveEntity(string key)
        {
            if (entities.ContainsKey(key))
            {
                entities.Remove(key);
            }
        }

        public void LoadContent(ContentManager contentManager)
        {
            content = contentManager;
            Globals.LoadGlobalSprites(content);
            InitializeEntities();
        }

        private void InitializeEntities()
        {
            Entity player = new Player(content);
            player.SetPosition(new Vector2(Globals.SCREENWIDTH / 2, 300));
            entities.Add("player", player);

            Mob mob = MobFactory.CreateMob(content);
            mob.SetPosition(new Vector2(Globals.SCREENWIDTH / 2 + 100, 400));
            entities.Add("mob", mob);

            PickupItem pickupItem = new PickupItem(content);
            pickupItem.SetPosition(new Vector2(Globals.SCREENWIDTH / 2 + 170, 180));
            entities.Add("pickupItem", pickupItem);

            Blocks blocks = new Blocks(content);
            blocks.SetPosition(new Vector2(Globals.SCREENWIDTH / 2 - 300, 480));
            entities.Add("blocks", blocks);

            ProjectileFactory.Initialize(content);
        }

        public void Update(GameTime gameTime)
        {
            if (GetActiveScreen() == null)
            {
                foreach (var entity in entities.Values)
                {
                    entity.Update(gameTime);
                    eventManager.CollectCommandRequests(entity.GetCommandQueue());
                }
                physicsManager.Update(gameTime, entities);
                spriteManager.Update(gameTime);
                eventManager.ProcessCommandRequests();
            }
            for (int i = 0; i < screens.Count; i++)
                screens[i].Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var entity in entities.Values)
                entity.Draw(spriteBatch);
            spriteManager.Draw(spriteBatch);
            for (int i = 0; i < screens.Count; i++)
                screens[i].Draw(spriteBatch);
        }

        public void AddScreen(IScreen screen)
        {
            screens.Add(screen);
        }

        public void RemoveScreen(IScreen screen)
        {
            screens.Remove(screen);
        }
        public IScreen GetActiveScreen()
        {
            if (screens.Count > 0)
                return screens[0];
            return null;
        }

    }
}
