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

        private ContentManager content;
        private Game1 game;
        public PlayerInventory playerInventory;

        public void Initialize(ContentManager content, Game1 game)
        {
            this.content = content;
            this.game = game;
            playerInventory = new PlayerInventory(content);
            dialogueHandler = new DialogueHandler(content, game);
        }

        private void UpdateCoreScreen()
        {
            IScreen desired;
            if (!GameStarted)
                desired = new StartMenu(game);
            else if (IsPaused)
                desired = new PauseMenu(content, game.GraphicsDevice, game);
            else
                desired = playerInventory;

            if (coreScreen == null || coreScreen.GetType() != desired.GetType())
            {
                if (coreScreen != null && screens.Contains(coreScreen))
                    screens.Remove(coreScreen);
                coreScreen = desired;
                bool blocking = (!GameStarted || IsPaused);
                AddScreen(coreScreen, blocking);
            }
        }

        public void Update()
        {
            UpdateCoreScreen();
            dialogueHandler.Update(this, LevelNumber, IsPaused, GameStarted);

            if (blockingScreen != null && blockingScreen.BlocksInput)
                blockingScreen.Update();
            else
                foreach (var screen in screens.ToList())
                    screen.Update();
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
