using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace Sprint0
{
    public class GameManager
    {
        public static GameManager Instance { get; private set; }

        private Dictionary<string, Entity> entities;
        private List<Tile> tiles;
        private CollisionManager collisionManager;
        private SpriteManager spriteManager;
        private ContentManager content;
        private LevelManager levelManager;
        public EventManager eventManager { get; private set; }
        public Game1 Game { get; private set; }
        private List<IScreen> screens;
        private IScreen activeScreen;
        private IScreen blockingScreen;
        private PlayerData playerData;
        private PlayerInventory playerInventory;
        private bool gameStarted;

        public bool GameStarted
        {
            get { return gameStarted; }
            set
            {
                gameStarted = value;
                UpdateActiveScreen();
            }
        }

        public GameManager(Game1 game, bool started = false)
        {
            Instance = this;
            Game = game;
            gameStarted = started;
            entities = new Dictionary<string, Entity>();
            tiles = new List<Tile>();
            collisionManager = new CollisionManager();
            spriteManager = new SpriteManager();
            eventManager = new EventManager(game, this);
            levelManager = new LevelManager();
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
            if (entities.ContainsKey(entityKey))
            {
                return entities[entityKey];
            }
            return null;
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
            Globals.LoadGlobalFonts(content);
            Globals.LoadPlayerData();
            InitializeTiles();
            InitializeEntities();
            string playerDataFile = "C:\\Users\\saura\\OneDrive - The Ohio State University\\Documents\\OHIO STATE DOCS\\SP 2025\\CSE3902-G3Repo-2025\\Sprint0\\Data\\playerDataFile.csv";
            playerData = PlayerData.LoadData(playerDataFile);
            playerInventory = new PlayerInventory(content);
            UpdateActiveScreen();
        }

        private void UpdateActiveScreen()
        {
            screens.Clear();
            if (!gameStarted)
            {
                activeScreen = new StartMenu(content, Game.GraphicsDevice, Game);
                blockingScreen = activeScreen;
            }
            else
            {
                activeScreen = playerInventory;
                blockingScreen = null;
            }
            screens.Add(activeScreen);
        }

        private void InitializeTiles()
        {
            int tileSize = 128;
            int rows = (Globals.SCREENHEIGHT / tileSize) + 1;
            int cols = (Globals.SCREENWIDTH / tileSize) + 1;
            int x, y;
            for (y = 0; y < rows; y++)
            {
                for (x = 0; x < cols; x++)
                {
                    Tile.TileType type = Tile.TileType.Grass;
                    Tile tile = new Tile(content, type, new Vector2(x * tileSize, y * tileSize));
                    tiles.Add(tile);
                }
            }
        }

        private void InitializeEntities()
        {
            levelManager.LoadContent(content);
            entities = levelManager.LoadLevelEntities();

            Mob mob = MobFactory.CreateMob(content);
            mob.SetPosition(new Vector2(Globals.SCREENWIDTH / 2 + 100, 400));
            entities.Add(mob.EntityKey, mob);

            PickupItem pickupItem = new PickupItem(content);
            pickupItem.SetPosition(new Vector2(Globals.SCREENWIDTH / 2 + 170, 180));
            entities.Add(pickupItem.EntityKey, pickupItem);

            Blocks blocks = new Blocks(content);
            blocks.SetPosition(new Vector2(Globals.SCREENWIDTH / 2 - 300, 480));
            entities.Add(blocks.EntityKey, blocks);

            ProjectileFactory.Initialize(content);

            List<BaseBlock> levelBlocks = new List<BaseBlock>();
            levelBlocks.Add(BlockFactory.CreateBlock(BlockSpriteKey.Tree, content, new Vector2(200, 800), 0.3f));
            levelBlocks.Add(BlockFactory.CreateBlock(BlockSpriteKey.Box, content, new Vector2(400, 800), 0.3f));
            levelBlocks.Add(BlockFactory.CreateBlock(BlockSpriteKey.OilBarrel_Red, content, new Vector2(600, 800), 0.3f));
            levelBlocks.Add(BlockFactory.CreateBlock(BlockSpriteKey.OilBarrel_Black, content, new Vector2(800, 800), 0.3f));
            levelBlocks.Add(BlockFactory.CreateBlock(BlockSpriteKey.BarbedFence, content, new Vector2(1000, 800), 0.3f));
            levelBlocks.Add(BlockFactory.CreateBlock(BlockSpriteKey.Oil, content, new Vector2(1200, 800), 0.3f));
            int i;
            for (i = 0; i < levelBlocks.Count; i++)
            {
                BaseBlock block = levelBlocks[i];
                entities.Add(block.EntityKey, block);
            }
        }

        public void Update()
        {
            if (blockingScreen != null && blockingScreen.BlocksInput)
            {
                blockingScreen.Update();
                return;
            }
            foreach (Entity entity in entities.Values)
            {
                entity.Update();
                eventManager.CollectCommandRequests(entity.GetCommandQueue());
            }
            collisionManager.Update(entities);
            spriteManager.Update();
            playerInventory.Update();
            eventManager.ProcessCommandRequests();
            if (activeScreen != null)
            {
                activeScreen.Update();
            }
            int j;
            for (j = 0; j < screens.Count; j++)
            {
                screens[j].Update();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int i;
            for (i = 0; i < tiles.Count; i++)
            {
                tiles[i].Draw(spriteBatch);
            }
            foreach (Entity entity in entities.Values)
            {
                entity.Draw(spriteBatch);
            }
            spriteManager.Draw(spriteBatch);
            if (activeScreen != null)
            {
                activeScreen.Draw(spriteBatch);
            }
            if (blockingScreen != null)
            {
                blockingScreen.Draw(spriteBatch);
            }
            int j;
            for (j = 0; j < screens.Count; j++)
            {
                screens[j].Draw(spriteBatch);
            }
        }

        public void AddScreen(IScreen screen)
        {
            if (!screens.Contains(screen))
            {
                screens.Add(screen);
                activeScreen = screen;
            }
        }

        public void RemoveScreen(IScreen screen)
        {
            if (screens.Contains(screen))
            {
                screens.Remove(screen);
                if (screens.Count > 0)
                {
                    activeScreen = screens[screens.Count - 1];
                }
                else
                {
                    activeScreen = null;
                }
            }
        }

        public IScreen GetActiveScreen()
        {
            return activeScreen;
        }

        public IScreen GetBlockingScreen()
        {
            return blockingScreen;
        }

        public void SetBlockingScreen(IScreen screen)
        {
            blockingScreen = screen;
        }

        public void ClearBlockingScreen()
        {
            blockingScreen = null;
        }
    }
}
