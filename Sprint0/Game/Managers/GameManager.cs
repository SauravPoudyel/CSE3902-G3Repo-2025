using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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

        public ScreenManager screenManager { get; private set; }
        public EventManager eventManager { get; private set; }

        public Game1 Game { get; private set; }

        private bool gameStarted;
        private bool gamePaused;

        private int levelNumber = 1;
        private bool gameLoading;

        public bool GameStarted
        {
            get { return gameStarted; }
            set { gameStarted = value; }
        }
        public bool GamePaused
        {
            get { return gamePaused; }
            set { gamePaused = value; }
        }
        public bool GameLoading
        {
            get { return gameLoading; }
            set
            {
                gameLoading = value;
                UpdateActiveScreen();
            }
        }

        public int LevelNumber
        {
            get { return levelNumber; }
            set { levelNumber = value; }
        }

        public GameManager(Game1 game, bool started = false)
        {
            AudioManager.LoadContent();
            AudioManager.PlayMusic(AudioManager.MusicKey.Background);
            Instance = this;

            Game = game;
            gameStarted = started;
            gamePaused = false;

            entities = new Dictionary<string, Entity>();
            tiles = new List<Tile>();

            screenManager = new ScreenManager();
            collisionManager = new CollisionManager();
            spriteManager = new SpriteManager();
            eventManager = new EventManager(game, this);
            levelManager = new LevelManager();
        }

        public ContentManager GetContent() => content;

        public Dictionary<string, Entity> GetEntities() => entities;

        public Entity GetEntity(string entityKey) =>
            entities.ContainsKey(entityKey) ? entities[entityKey] : null;

        public void SetEntity(string key, Entity entity)
        {
            entities[key] = entity;
        }

        public void RemoveEntity(string key)
        {
            if (entities.ContainsKey(key))
                entities.Remove(key);
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

            screenManager.Initialize(content, Game);
        }

        public void UpdateLevel()
        {

            LoadLevelContent();
            InitializeTiles();
            InitializeEntities();
        }

        private void UpdateActiveScreen()
        {
            screenManager.LevelNumber = levelNumber;
            screenManager.GameStarted = gameStarted;
            screenManager.IsPaused = gamePaused;
        }

        private void LoadLevelContent()
        {
            levelManager.LoadContent(content, levelNumber);
 
        }

        private void InitializeTiles()
        {
            tiles = levelManager.LoadLevelTiles();
        }

        private void InitializeEntities()
        {
            Player player = null;
            if(entities.ContainsKey("player")) {
                player = (Player)entities["player"];
            }
            entities = levelManager.LoadLevelEntities();
            if(player!=null && entities.ContainsKey("player")) {
                entities["player"] = player;
            }
            ProjectileFactory.Initialize(content);
        }

        public void Update()
        {
            UpdateActiveScreen();

            screenManager.Update();

            if (screenManager.IsInputBlocked())
                return;

            foreach (Entity entity in entities.Values)
            {
                entity.Update();
                eventManager.CollectCommandRequests(entity.GetCommandQueue());
            }

            collisionManager.Update(entities);
            spriteManager.Update();
            eventManager.ProcessCommandRequests();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var tile in tiles)
                tile.Draw(spriteBatch);

            foreach (var entity in entities.Values)
                entity.Draw(spriteBatch);

            spriteManager.Draw(spriteBatch);
            screenManager.Draw(spriteBatch);
        }
    }
}
