using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Sprint0
{
    public class MiniMap : IScreen
    {
        private Texture2D mapTexture;
        private Texture2D mapIcon;
        private Rectangle mapFrameRectangle;
        private Rectangle mapIconRectangle;
        private Rectangle mapRectangle;
        private GameManager gameManager;
        private Dictionary<int, Rectangle> levelRectangles;
        private int currentLevel;

        public bool BlocksInput => false;

        public MiniMap(ContentManager content, GameManager gameManager)
        {
            this.gameManager = gameManager;

            // Load texture
            mapTexture = content.Load<Texture2D>("UI/Minimap");
            mapIcon = content.Load<Texture2D>("UI/MinimapIcon");

            // Define the size and position of the mini-map
            int mapFrameWidth = 300; 
            int mapFrameHeight = 300; 
            int mapFrameX = Globals.SCREENWIDTH - mapFrameWidth - 20; // Position in the bottom-right corner
            int mapFrameY = Globals.SCREENHEIGHT - mapFrameHeight - 20;

            // Define the size and position of the map icon
            int mapIconWidth = 100;
            int mapIconHeight = 100;
            int mapIconX = mapFrameX + (mapFrameWidth - mapIconWidth)/2;
            int mapIconY = mapFrameY - mapIconHeight/2;

            mapFrameRectangle = new Rectangle(mapFrameX, mapFrameY, mapFrameWidth, mapFrameHeight);
            mapIconRectangle = new Rectangle(mapIconX, mapIconY, mapIconWidth, mapIconHeight);
            initLevelRectangles();

        }

        public void Update()
        {
            currentLevel = gameManager.LevelNumber;
            if (levelRectangles.TryGetValue(currentLevel, out Rectangle levelRect))
            {
                mapRectangle = new Rectangle(
                    levelRect.X - (mapFrameRectangle.Width - levelRect.Width) / 2,
                    levelRect.Y - (mapFrameRectangle.Height - levelRect.Height) / 2,
                    mapFrameRectangle.Width,
                    mapFrameRectangle.Height
                );
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mapTexture, mapFrameRectangle, mapRectangle, Color.White * 0.7f);
            spriteBatch.Draw(mapIcon, mapIconRectangle, Color.White * 0.7f);
            if (levelRectangles.TryGetValue(currentLevel, out Rectangle levelRect))
            {
                foreach (var entity in gameManager.GetEntities().Values)
                {
                    if (entity is Mob)
                    {
                        DrawEntityOnMap(spriteBatch, entity, levelRect, Color.Red); // Enemies are red
                    }
                    if (entity is Player)
                    {
                        DrawEntityOnMap(spriteBatch, entity, levelRect, Color.Blue); // Player is blue
                    }
                }
            }
        }

        private void DrawEntityOnMap(SpriteBatch spriteBatch, Entity entity, Rectangle levelRect, Color color)
        {
            Vector2 ratio = new(entity.GetPosition().X / Globals.SCREENWIDTH, entity.GetPosition().Y / Globals.SCREENHEIGHT);
       
            Vector2 scaledPosition = new Vector2(mapFrameRectangle.X, mapFrameRectangle.Y);
            scaledPosition.X += (mapFrameRectangle.Width - levelRect.Width)/2 + levelRect.Width*ratio.X;
            scaledPosition.Y += (mapFrameRectangle.Height - levelRect.Height) / 2 + levelRect.Height * ratio.Y;


            // Draw the entity as a small rectangle or dot
            Rectangle entityRect = new Rectangle((int)scaledPosition.X, (int)scaledPosition.Y, 10, 10);
            spriteBatch.Draw(mapTexture, entityRect, color);
        }

        private void  initLevelRectangles() {
            this.levelRectangles = new Dictionary<int, Rectangle>
            {
                { 1, new Rectangle(600,355,189,115) },
                { 2, new Rectangle(600,242,189,112) },
                { 3, new Rectangle(0598,470,189,113) },
                { 4, new Rectangle(788,469,191,114) },
                { 5, new Rectangle(980,585,188,113) },
                { 6, new Rectangle(789,584,190,113) },
                { 7, new Rectangle(598,1041,190,114) },
                { 8, new Rectangle(790,1040,189,114) },
                { 9, new Rectangle(599,1154,190,113) },
                { 10,new Rectangle(788,1153,192,118) },
                { 11,new Rectangle(789,926,191,115) },
                { 12,new Rectangle(979,1154,191,111) },
                { 13,new Rectangle(980,698,190,113) },
                { 14,new Rectangle(980,811,190,115) },
                { 15,new Rectangle(789,811,191,115) },

                { 201,new Rectangle(789,242,190,112) },
                { 202,new Rectangle(981,242,189,112) },
                { 203,new Rectangle(1171,242,189,112) },
                { 204,new Rectangle(1362,241,189,112) },
                { 205,new Rectangle(980,126,190,112) },
                { 206,new Rectangle(1171,127,189,112) },
                { 207,new Rectangle(1362,127,190,112) },
                { 208,new Rectangle(1362,14,190,112) },
                { 209,new Rectangle(981,355,190,112) },
                { 210,new Rectangle(1171,357,189,112) },

                { 301,new Rectangle(408,240,192,115) },
                { 302,new Rectangle(218,240,191,115) },
                { 303,new Rectangle(218,356,192,113) },
                { 304,new Rectangle(28,354,191,115) },
                { 305,new Rectangle(28,470,191,114) },
                { 306,new Rectangle(218,470,191,113) },
                { 307,new Rectangle(27,583,192,115) },
                { 308,new Rectangle(219,584,190,114) },
                { 309,new Rectangle(409,585,190,113) },
                { 310,new Rectangle(28,697,191,115) },
                { 311,new Rectangle(219,698,191,114) },
                { 312,new Rectangle(409,699,191,114) },
                { 313,new Rectangle(600,698,190,114) },
                { 314,new Rectangle(28,812,191,114) },
                { 315,new Rectangle(218,812,191,115) }
            };
        }
    }
}