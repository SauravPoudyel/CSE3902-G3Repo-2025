using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0
{
    public class DialogueHandler
    {
        private bool tutorialAdded;
        private ContentManager content;
        private Game1 game;
        private DialogueToScreenAdapter tutorialOverlay;

        public DialogueHandler(ContentManager content, Game1 game)
        {
            this.content = content;
            this.game = game;
            tutorialAdded = false;
        }

        public void Update(ScreenManager screenManager, int levelNumber, bool isPaused, bool gameStarted)
        {
            // Always check for removal if overlay exists.
            if (tutorialOverlay != null && tutorialOverlay.IsFinished)
            {
                screenManager.RemoveScreen(tutorialOverlay);
                System.Console.WriteLine("[TutorialOverlay] Removed");
                tutorialOverlay = null;
            }

            // Only attempt to add the overlay if game is active and unpaused.
            if (!gameStarted || isPaused)
                return;

            if (levelNumber == 1 && Globals.PlayerData.GetInt("HasSeenTutorial") == 0 && tutorialOverlay == null)
            {
                StaticSprite circleSprite = new StaticSprite();
                circleSprite.LoadContent(content, "CharacterCircle", 150, 0, 150, 150, 1);

                Texture2D rectangleTexture = new Texture2D(game.GraphicsDevice, 1, 1);
                rectangleTexture.SetData(new Color[] { Color.White });
                SpriteFont font = Globals.FONT;

                Dialogue overlay = new Dialogue(
                    circleSprite,
                    rectangleTexture,
                    font,
                    "Commander:\n\nWe're losing the war " + Globals.PlayerData.GetString("Name") +"! Destroy your combatants & recoup at base by using WASD to move and z to fire. Click to acknowledge. "
                );

                tutorialOverlay = new DialogueToScreenAdapter(overlay);
                screenManager.AddScreen(tutorialOverlay, true);
                Globals.PlayerData.UpdateVariable("HasSeenTutorial", 1);
                tutorialAdded = true;
                System.Console.WriteLine("[TutorialOverlay] Added");
            }
        }
    }
}
