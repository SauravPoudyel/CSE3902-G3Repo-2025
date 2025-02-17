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

        public GameManager(Game1 game)
        {
            entities = new Dictionary<string, Entity>();
            physicsManager = new PhysicsManager();
            spriteManager = new SpriteManager();
            eventManager = new EventManager(game, this);
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
            return entities[entityKey];
        }

        public void LoadContent(ContentManager contentManager)
        {
            content = contentManager;
            InitializeEntities();
        }

        private void InitializeEntities()
        {
            Entity player = new Player(content);
            player.SetPosition(new Vector2(Globals.SCREENWIDTH / 2, 300));
            entities.Add("player", player);

            Entity mob = new Mob(content);
            mob.SetPosition(new Vector2(Globals.SCREENWIDTH / 2 + 100, 400));
            entities.Add("mob", mob);

            PickupItem pickupItem = new PickupItem(content);
            pickupItem.SetPosition(new Vector2(Globals.SCREENWIDTH / 2 + 170, 180));
            entities.Add("pickupItem", pickupItem);

            Blocks blocks = new Blocks(content);
            blocks.SetPosition(new Vector2(Globals.SCREENWIDTH / 2 - 300, 480));
            entities.Add("blocks", blocks);

            ProjectileFactory projectileFactory = new ProjectileFactory(content);
            projectileFactory.SetPosition(new Vector2(Globals.SCREENWIDTH / 2, 300));
            entities.Add("projectileFactory", projectileFactory);
        }

        public void Update(GameTime gameTime)
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

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var entity in entities.Values)
            {
                entity.Draw(spriteBatch);
            }
            spriteManager.Draw(spriteBatch);
        }

        public void SetSprite(ISprite sprite)
        {
            spriteManager.SetSprite(sprite);
        }
    }
}
