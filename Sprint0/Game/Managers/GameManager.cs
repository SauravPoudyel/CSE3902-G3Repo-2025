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
        public PlayerInventory playerInventory { get; private set; }
        private bool gameStarted;
        private bool gamePaused;
        private int levelNumber;

        public bool GameStarted
        {
            get { return gameStarted; }
            set
            {
                gameStarted = value;
                UpdateActiveScreen();
            }
        }

        public bool GamePaused
        {
            get { return gamePaused; }
            set
            {
                gamePaused = value;
                UpdateActiveScreen();
            }
        }

        public int LevelNumber
        {
            get { return levelNumber; }
            set
            {
                levelNumber = value;
                UpdateActiveScreen();
            }
        }

        public GameManager(Game1 game, bool started = false)
        {
            AudioManager.LoadContent();
            AudioManager.PlayMusic(AudioManager.MusicKey.Background);
            Instance = this;
            Game = game;
            gameStarted = started;
            gamePaused = false;
            levelNumber = 1;
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
            LoadLevelContent();
            InitializeTiles();
            InitializeEntities();

            // Load persistent player data from CSV (located in Data\playerData.csv)
            string playerDataFile = Path.Combine(Globals.projectDirectory, "\\Data\\playerDataFile.csv");
            playerData = PlayerData.LoadData(playerDataFile);
            playerInventory = new PlayerInventory(content);
            UpdateActiveScreen();
        }

        private void UpdateActiveScreen()
        {
            screens.Clear();
            if (!gameStarted)
            {
                activeScreen = new StartMenu(Game);
                blockingScreen = activeScreen;
            } else if (gamePaused) {
                activeScreen = new PauseMenu(content, Game.GraphicsDevice, Game);
                blockingScreen = activeScreen;
            }
            else
            {
                activeScreen = playerInventory;
                blockingScreen = null;
            }
            screens.Add(activeScreen);
        }
        public void UpdateLevel(){
            LoadLevelContent();
            InitializeTiles();
            InitializeEntities();
        }
        private void LoadLevelContent() {
            levelManager.LoadContent(content, "Level"+levelNumber.ToString());
        }
        private void InitializeTiles()
        {
            tiles = levelManager.LoadLevelTiles();
        }

        private void InitializeEntities()
        {
            entities = levelManager.LoadLevelEntities();
            ProjectileFactory.Initialize(content);
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
