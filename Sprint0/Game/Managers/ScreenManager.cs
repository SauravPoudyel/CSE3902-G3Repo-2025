using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0
{
    public class ScreenManager
    {
        private List<IScreen> screens = new List<IScreen>();
        private IScreen coreScreen;
        private IScreen blockingScreen;
        public DialogueHandler dialogueHandler { get; private set; }
        public int LevelNumber { get; set; }
        public bool IsPaused { get; set; }
        public bool GameStarted { get; set; }
        public bool shopOpen { get; set; }  // When true, shop is active
        public bool statsOpen { get; set; }
        public bool achievementsOpen { get; set;}

        private ContentManager content;
        private Game1 game;
        public PlayerInventory playerInventory;
        private Shop shop;
        private StatsScreen statsScreen;
        private Achievements achievements;
        private MiniMap miniMap;

        // Persistent core screens
        private StartMenu startMenu;
        private PauseMenu pauseMenu;

        public void Initialize(ContentManager content, Game1 game)
        {
            this.content = content;
            this.game = game;
            // Create persistent instances
            playerInventory = new PlayerInventory(game);
            shop = new Shop(game);   // Shop now implements IScreen
            shop.LoadContent();
            dialogueHandler = new DialogueHandler(content, game);
            statsScreen = new StatsScreen(game);
            achievements = new Achievements(game);
            miniMap = new MiniMap(content, game.GameManager);
            startMenu = new StartMenu(game);
            pauseMenu = new PauseMenu(content, game.GraphicsDevice, game);
            GameStarted = false;
            IsPaused = false;
            shopOpen = false;
            statsOpen = false;
            achievementsOpen = false;

        }

        private void UpdateCoreScreen()
        {
            IScreen desired;
            if (shopOpen)
            {
                desired = shop;
            } 
            else if (statsOpen)
            {
                desired = statsScreen;
            }
            else if (achievementsOpen)
            {
                desired = achievements;
            }
            else if (!GameStarted)
            {
                desired = startMenu;
            }
            else if (IsPaused)
            {
                desired = pauseMenu;
            }
            else
            {
                desired = playerInventory;
            }

            // Remove any core screens that do not match the desired type (leave overlays intact)
            foreach (var s in screens.ToList())
            {
                if (!(s is DialogueToScreenAdapter) && s.GetType() != desired.GetType())
                    RemoveScreen(s);
            }

            // Force-remove StartMenu if the game has started.
            if (GameStarted && coreScreen is StartMenu)
            {
                RemoveScreen(coreScreen);
            }

            coreScreen = desired;
            // If desired screen is not already in the list, add it with blocking if needed.
            bool blocking = (!GameStarted || IsPaused || shopOpen || statsOpen);
            if (!screens.Contains(desired))
                AddScreen(desired, blocking);


        }

        public void Update()
        {
            UpdateCoreScreen();
            dialogueHandler.Update(this, LevelNumber, IsPaused, GameStarted);

            if (blockingScreen != null && blockingScreen.BlocksInput)
            {
                blockingScreen.Update();
            }
            else
            {
                foreach (var screen in screens.ToList())
                    screen.Update();
            }

            miniMap.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var screen in screens)
                screen.Draw(spriteBatch);
            miniMap.Draw(spriteBatch);
        }

        public void AddScreen(IScreen screen, bool isBlocking = false)
        {
            if (!screens.Contains(screen))
            {
                screens.Add(screen);
                if (isBlocking)
                    blockingScreen = screen;
            }
        }

        public void RemoveScreen(IScreen screen)
        {
            if (screens.Contains(screen))
            {
                screens.Remove(screen);
                if (screen == blockingScreen)
                    blockingScreen = null;
                if (screen == coreScreen)
                    coreScreen = null;
            }
        }

        public void Clear()
        {
            screens.Clear();
            coreScreen = null;
            blockingScreen = null;
        }

        public void ClearBlockingScreen()
        {
            blockingScreen = null;
        }

        public bool IsInputBlocked() => blockingScreen != null && blockingScreen.BlocksInput;
    }
}
