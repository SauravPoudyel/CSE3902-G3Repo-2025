using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Sprint0
{
    public class StatsScreen : IScreen
    {
        private Dictionary<string, textButton> buttons;
        private Rectangle windowRectangle;
        private Texture2D backgroundTexture;
        private GraphicsDevice graphicsDevice;
        public bool BlocksInput => true;
        SpriteFont font = Globals.FONT;
        public StatsScreen(GraphicsDevice graphicsDevice, Game1 game) 
        {
            this.content = content;

            //Set size of the window
            int width = Globals.SCREENWIDTH/2;
            int height = Globals.SCREENHEIGHT/2;
            int x = (Globals.SCREENWIDTH - width) / 2;
            int y = (Globals.SCREENHEIGHT - height) / 2;
            windowRectangle = new Rectangle(x, y, width, height);
            
            //Set the background of the window
            backgroundTexture = new Texture2D(graphicsDevice, 1, 1);
            backgroundTexture.SetData(new Color[] { Color.LightGray});

            //Add Text and Images of rectangles
            Rectangle BossTankRect = new Rectangle(0, 160, 40, 40);




            //Add buttons

        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}