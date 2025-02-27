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
        public EventManager eventManager { get; private set; }
        private List<IScreen> screens;
        public Game1 Game { get; private set; }

        // Persistent player data and HUD overlay
        private PlayerData playerData;
        private PlayerInventory playerInventory;

        // Flag to indicate if the game has started (i.e. start menu dismissed)
        private bool gameStarted;
        public bool GameStarted
        {
            get { return gameStarted; }
            set { gameStarted = value; }
        }

        // Allow external initialization of gameStarted (default is false)
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
            Globals.LoadGlobalFonts(content);
            Globals.LoadPlayerData(); 
            InitializeTiles();
            InitializeEntities();

            // Load persistent player data from CSV (located in Data\playerData.csv)
            string playerDataFile = Path.Combine("Data", "playerData.csv");
            playerData = PlayerData.LoadData(playerDataFile);

            // Create and load the player inventory HUD overlay (nonblocking)
            playerInventory = new PlayerInventory(content);

            // Only add the Start Menu if the game has not yet started.
            if (!gameStarted)
            {
                StartMenu startMenu = new StartMenu(content, Game.GraphicsDevice, Game);
                screens.Insert(0, startMenu);
            }
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

            // Create different block types at various positions
            List<BaseBlock> levelBlocks = new List<BaseBlock>();

            levelBlocks.Add(BlockFactory.CreateBlock(BlockSpriteKey.Tree, content, new Vector2(200, 800), 0.3f));
            levelBlocks.Add(BlockFactory.CreateBlock(BlockSpriteKey.Box, content, new Vector2(400, 800), 0.3f));
            levelBlocks.Add(BlockFactory.CreateBlock(BlockSpriteKey.OilBarrel_Red, content, new Vector2(600, 800), 0.3f));
            levelBlocks.Add(BlockFactory.CreateBlock(BlockSpriteKey.OilBarrel_Black, content, new Vector2(800, 800), 0.3f));
            levelBlocks.Add(BlockFactory.CreateBlock(BlockSpriteKey.BarbedFence, content, new Vector2(1000, 800), 0.3f));
            levelBlocks.Add(BlockFactory.CreateBlock(BlockSpriteKey.Oil, content, new Vector2(1200, 800), 0.3f));

            // Add blocks to entities dictionary
            int blockIndex = 0;
            foreach (var block in levelBlocks)
            {
                if (block is FlammableBlock flammable)
                {
                    string key = $"flammable_{Guid.NewGuid().ToString("N")}";
                    flammable.SetEntityKey(key);
                    entities.Add(key, flammable);
                }
                else
                {
                    entities.Add($"block_{blockIndex++}", block);
                }
            }

            ProjectileFactory.Initialize(content);
        }

        public void Update()
        {
            // If the game has started, update entities normally.
            if (gameStarted)
            {
                foreach (var entity in entities.Values)
                {
                    entity.Update();
                    eventManager.CollectCommandRequests(entity.GetCommandQueue());
                }
                collisionManager.Update(entities);
                spriteManager.Update();
                eventManager.ProcessCommandRequests();
                if (!screens.Contains(playerInventory))
                {
                    screens.Add(playerInventory);
                }
            }
            // Otherwise, if the game hasn't started, update entities only if no blocking screen is present.
            else
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
            }

            // Always update all screens (HUD and menus).
            foreach (var screen in screens)
                screen.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw background tiles.
            foreach (var tile in tiles)
                tile.Draw(spriteBatch);

            // Draw game entities.
            foreach (var entity in entities.Values)
                entity.Draw(spriteBatch);

            spriteManager.Draw(spriteBatch);

            // Draw overlay screens (e.g., HUD, menus).
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

        public IScreen GetBlockingScreen()
        {
            foreach (var screen in screens)
            {
                if (screen.BlocksInput)
                    return screen;
            }
            return null;
        }
    }
}
