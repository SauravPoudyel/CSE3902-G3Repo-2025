using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Sprint0
{
    public class GameManager
    {
        private Dictionary<string, Entity> entities;
        private List<Tile> tiles;
        private CollisionManager collisionManager;
        private SpriteManager spriteManager;
        private ContentManager content;
        public EventManager eventManager { get; private set; }
        private List<IScreen> screens;
        public Game1 Game { get; private set; }

        public GameManager(Game1 game)
        {
            Game = game;
            entities = new Dictionary<string, Entity>();
            tiles = new List<Tile>();
            collisionManager = new CollisionManager();
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
            InitializeTiles();
            InitializeEntities();
        }

        private void InitializeTiles()
        {
            int tileSize = 128;
            int rows = (Globals.SCREENHEIGHT / tileSize) + 1;
            int cols = (Globals.SCREENWIDTH / tileSize) + 1;

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    Tile.TileType type = Tile.TileType.Grass; 

                    tiles.Add(new Tile(content, type, new Vector2(x * tileSize, y * tileSize)));
                }
            }
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

        public void Update()
        {
            if (GetActiveScreen() == null)
            {
                foreach (var entity in entities.Values)
                {
                    entity.Update();
                    eventManager.CollectCommandRequests(entity.GetCommandQueue());
                }
                collisionManager.Update(entities);
                spriteManager.Update();
                eventManager.ProcessCommandRequests();
            }

            foreach (var screen in screens)
                screen.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw tiles first 
            foreach (var tile in tiles)
                tile.Draw(spriteBatch);

            foreach (var entity in entities.Values)
                entity.Draw(spriteBatch);

            spriteManager.Draw(spriteBatch);

            foreach (var screen in screens)
                screen.Draw(spriteBatch);
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
            return screens.Count > 0 ? screens[0] : null;
        }
    }
}
