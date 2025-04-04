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
        private DialogueHandler dialogueHandler;
        public int LevelNumber { get; set; }
        public bool IsPaused { get; set; }
        public bool GameStarted { get; set; }
        public bool shopOpen { get; set; }  // When true, shop is active
        public bool statsOpen { get; set; }

        private ContentManager content;
        private Game1 game;
        public PlayerInventory playerInventory;
        private Shop shop;  
        private ShopScreenAdapter shopAdapter; // Adapter to wrap Shop
        private StatsScreen statsScreen;

        // Persistent instances for core screens:
        private StartMenu startMenu;
        private PauseMenu pauseMenu;

        public void Initialize(ContentManager content, Game1 game)
        {
            this.content = content;
            this.game = game;
            // Persist player inventory (already created in Initialize)
            playerInventory = new PlayerInventory(game);
            // Create and load shop content, and its adapter
            shop = new Shop(game);
            shop.LoadContent();
            shopAdapter = new ShopScreenAdapter(shop);
            dialogueHandler = new DialogueHandler(content, game);
            statsScreen = new StatsScreen(game);
            // Initialize persistent core screens as null; they'll be created on demand.
            startMenu = null;
            pauseMenu = null;
            GameStarted = false;
            IsPaused = false;
            shopOpen = false;
            statsOpen = false;

        }

        private void UpdateCoreScreen()
        {
            IScreen desired;
            if (shopOpen)
            {
                desired = shopAdapter;
            }
            if (statsOpen)
            {
                desired = new StatsScreen(game);
            }
            else if (!GameStarted)
            {
                if (startMenu == null)
                {
                    startMenu = new StartMenu(game);
                }
                desired = startMenu;
            }
            else if (IsPaused)
            {
                if (pauseMenu == null)
                {
                    pauseMenu = new PauseMenu(content, game.GraphicsDevice, game);
                }
                desired = pauseMenu;

            }
            else
            {
                desired = playerInventory;
            }

            // Remove any core screens that don’t match the desired type (leave overlays intact)
            foreach (var s in screens.ToList())
            {
                if (!(s is DialogueToScreenAdapter) && s.GetType() != desired.GetType())
                {
                    RemoveScreen(s);
                }
            }

            // Force-remove StartMenu if game has started.
            if (GameStarted && coreScreen is StartMenu)
            {
                RemoveScreen(coreScreen);
            }

            coreScreen = desired;
            bool blocking = (!GameStarted || IsPaused || shopOpen);
            if (!screens.Contains(desired))
            {

                AddScreen(desired, blocking);
            }
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
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var screen in screens)
                screen.Draw(spriteBatch);
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
                {
                    blockingScreen = null;
                }
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
            System.Console.WriteLine("[ScreenManager] ClearBlockingScreen() called");
        }

        public bool IsInputBlocked() => blockingScreen != null && blockingScreen.BlocksInput;
    }
}
