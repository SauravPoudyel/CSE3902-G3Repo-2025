using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Sprint0
{
    public class GameManager
    {
        private ISprite currentSprite;
        private TextSprite creditsText;
        private ContentManager content;
        private Dictionary<string, ICommand> commandMap;

        public GameManager(Game1 game)
        {
            string creditsString = "SPRINT 0 CREDITS:\nProgram Made By: Saurav Poudyel\nSprites From: https://www.spriters-resource.com/nes/legendofzelda/sheet/8366/";
            creditsText = new TextSprite(new Vector2(Globals.SCREENWIDTH / 2 - 400, Globals.SCREENHEIGHT / 2 + 150), creditsString, Color.White);

            commandMap = new Dictionary<string, ICommand>
            {
                {"Quit", new GameCommands.QuitCommand(game)},
                {"Static", new GameCommands.DisplayStaticGameCommand()},
                {"Animated", new GameCommands.DisplayAnimatedGameCommand()},
                {"Moving", new GameCommands.DisplayMovingGameCommand()},
                {"MovingAnimated", new GameCommands.DisplayMovingAnimatedGameCommand()}
            };

            currentSprite = new StaticSprite(new Vector2(Globals.SCREENWIDTH / 2 - 20, Globals.SCREENHEIGHT / 2 - 20));
        }

        public void ExecuteCommand(string commandKey)
        {
            if (commandMap.ContainsKey(commandKey))
            {
                commandMap[commandKey].Execute(this);
            }
        }

        public void SetSprite(ISprite sprite)
        {
            currentSprite = sprite;
        }

        public ContentManager GetContent()
        {
            return content;
        }

        public void LoadContent(ContentManager contentManager)
        {
            content = contentManager;
            currentSprite.LoadContent(content, "LinkSpritesheet", 140, 2, 62, 62, 1);
            creditsText.LoadContent(content, "Arial");
        }

        public void Update(GameTime gameTime)
        {
            if (currentSprite != null)
            {
                currentSprite.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (currentSprite != null)
            {
                currentSprite.Draw(spriteBatch);
            }
            creditsText.Draw(spriteBatch);
        }
    }
}
