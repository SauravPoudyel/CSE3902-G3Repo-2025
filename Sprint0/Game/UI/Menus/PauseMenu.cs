using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Sprint0
{
    public class PauseMenu : IScreen
    {
        private Dictionary<string, textButton> buttons;
        private Color overlayColor;
        private ContentManager content;
        private GraphicsDevice graphicsDevice;
        private Game1 game;
        private int pauseButtonWidth = 200;
        // PauseMenu should block game input.
        public bool BlocksInput => true;

        public PauseMenu(ContentManager content, GraphicsDevice graphicsDevice, Game1 game)
        {
            this.content = content;
            this.game = game;

            buttons = new Dictionary<string, textButton>();
            overlayColor = new Color(0, 0, 0, 180);

            SpriteFont font = Globals.FONT;
            Texture2D buttonTexture = new Texture2D(graphicsDevice, 1, 1);
            buttonTexture.SetData(new Color[] { Color.Gray });

            Dictionary<string, object> gameParams = new Dictionary<string, object>
            {
                { "gameManager", this.game.GameManager },
                { "game", this.game }
            };
            textButton restartButton = new textButton(buttonTexture,
                new Rectangle(Globals.SCREENWIDTH / 2 - 100, Globals.SCREENHEIGHT / 2 - 140, pauseButtonWidth, 40),
                "Restart", new GameCommands.ResetCommand(), gameParams);
            textButton menuButton = new textButton(buttonTexture,
                new Rectangle(Globals.SCREENWIDTH / 2 - 100, Globals.SCREENHEIGHT / 2 - 210, pauseButtonWidth, 40),
                "Main Menu", new GameCommands.ResetCommand(), gameParams);
            textButton quitButton = new textButton(buttonTexture,
                new Rectangle(Globals.SCREENWIDTH / 2 - 100, Globals.SCREENHEIGHT / 2 + 210, pauseButtonWidth, 40),
                "Quit", new GameCommands.QuitCommand(), gameParams);

            Dictionary<string, object> screenParams = new Dictionary<string, object>
            {
                { "gameManager", this.game.GameManager },
                { "screen", this }
            };
            // Note: The Start button uses "StartGameCommand" to remove the start menu.
            textButton resumeButton = new textButton(buttonTexture,
                new Rectangle(Globals.SCREENWIDTH / 2 - 100, Globals.SCREENHEIGHT / 2 - 80, pauseButtonWidth, 40),
                "Resume", new GameCommands.StartGameCommand(), screenParams);

            textButton increaseLevelButton = new textButton(buttonTexture,
                new Rectangle(Globals.SCREENWIDTH / 2 - 100 + 210, Globals.SCREENHEIGHT / 2 + 40, 40, 40),
                "+", new GameCommands.IncreaseLevelCommand(), screenParams);
            textButton decreaseLevelButton = new textButton(buttonTexture,
                new Rectangle(Globals.SCREENWIDTH / 2 - 100 - 50, Globals.SCREENHEIGHT / 2 + 40, 40, 40),
                "-", new GameCommands.DecreaseLevelCommand(), screenParams);
            textButton levelButton = new textButton(buttonTexture,
                new Rectangle(Globals.SCREENWIDTH / 2 - 100, Globals.SCREENHEIGHT / 2 + 40, pauseButtonWidth, 40),
                "Level: " + game.GameManager.LevelNumber.ToString(), null, screenParams);

            textButton increaseDifficultyButton = new textButton(buttonTexture,
                new Rectangle(Globals.SCREENWIDTH / 2 - 100 + 210, Globals.SCREENHEIGHT / 2 - 20, 40, 40),
                "+", new GameCommands.IncreaseDifficultyCommand(), screenParams);
            textButton decreaseDifficultyButton = new textButton(buttonTexture,
                new Rectangle(Globals.SCREENWIDTH / 2 - 100 - 50, Globals.SCREENHEIGHT / 2 - 20, 40, 40),
                "-", new GameCommands.DecreaseDifficultyCommand(), screenParams);
            textButton difficultyButton = new textButton(buttonTexture,
                new Rectangle(Globals.SCREENWIDTH / 2 - 100, Globals.SCREENHEIGHT / 2 - 20, pauseButtonWidth, 40),
                "Difficulty: " + Globals.GlobalMobData.Difficulty.ToString(), null, screenParams);

            textButton increaseVolumeButton = new textButton(buttonTexture,
                new Rectangle(Globals.SCREENWIDTH / 2 - 100 + 210, Globals.SCREENHEIGHT / 2 + 100, 40, 40),
                "+", new AudioCommands.AudioIncreaseCommand(), screenParams);
            textButton decreaseVolumeButton = new textButton(buttonTexture,
                new Rectangle(Globals.SCREENWIDTH / 2 - 100 - 50, Globals.SCREENHEIGHT / 2 + 100, 40, 40),
                "-", new AudioCommands.AudioDecreaseCommand(), screenParams);
            textButton volumeButton = new textButton(buttonTexture,
                new Rectangle(Globals.SCREENWIDTH / 2 - 100, Globals.SCREENHEIGHT / 2 + 100, pauseButtonWidth, 40),
                "Volume: " + (int)(AudioManager.Volume * 100), new AudioCommands.AudioMuteCommand(), screenParams);
            buttons.Add("Restart", restartButton);
            buttons.Add("Menu", menuButton);
            buttons.Add("Quit", quitButton);
            buttons.Add("Resume", resumeButton);
            buttons.Add("Difficulty", difficultyButton);
            buttons.Add("DecDifficulty", decreaseDifficultyButton);
            buttons.Add("IncDifficulty", increaseDifficultyButton);
            buttons.Add("Level", levelButton);
            buttons.Add("DecLevel", decreaseLevelButton);
            buttons.Add("IncLevel", increaseLevelButton);
            buttons.Add("Volume", volumeButton);
            buttons.Add("DecVolume", decreaseVolumeButton);
            buttons.Add("IncVolume", increaseVolumeButton);
        }

        public void Update()
        {
            foreach (textButton button in buttons.Values)
            {
                button.Update();
            }
            buttons.GetValueOrDefault("Volume").UpdateText("Volume: " + (int)Math.Ceiling(AudioManager.Volume * 10)*10);
            buttons.GetValueOrDefault("Level").UpdateText("Level: " + (game.GameManager.LevelNumber.ToString()));
            buttons.GetValueOrDefault("Difficulty").UpdateText("Difficulty: " + Globals.GlobalMobData.Difficulty.ToString());
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (textButton button in buttons.Values)
            {
                button.Draw(spriteBatch);
            }
        }
    }
}
