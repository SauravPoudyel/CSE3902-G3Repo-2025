using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Sprint0
{
    public class MiniMap : IScreen
    {
        private Texture2D mapTexture;
        private Rectangle mapFrameRectangle;
        private Rectangle mapRectangle;
        private GameManager gameManager;
        private Dictionary<int, Rectangle> levelRectangles;

        public bool BlocksInput => false;

        public MiniMap(ContentManager content, GameManager gameManager)
        {
            this.gameManager = gameManager;

            // Load the full game map texture
            mapTexture = content.Load<Texture2D>("UI/Minimap");

            // Define the size and position of the mini-map
            int mapWidth = 300; // Mini-map width
            int mapHeight = 300; // Mini-map height
            int mapX = Globals.SCREENWIDTH - mapWidth - 20; // Position in the bottom-right corner
            int mapY = Globals.SCREENHEIGHT - mapHeight - 20;

            mapFrameRectangle = new Rectangle(mapX, mapY, mapWidth, mapHeight);
            levelRectangles = new Dictionary<int, Rectangle>
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
            };
        }

        public void Update()
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Get the current level number
            int currentLevel = gameManager.LevelNumber;

            if (levelRectangles.TryGetValue(currentLevel, out Rectangle levelRect))
            {
                mapRectangle = new Rectangle(
                    levelRect.X - (mapFrameRectangle.Width - levelRect.Width) / 2,
                    levelRect.Y - (mapFrameRectangle.Height - levelRect.Height) / 2,
                    mapFrameRectangle.Width,
                    mapFrameRectangle.Height
                );
                spriteBatch.Draw(mapTexture, mapFrameRectangle, mapRectangle, Color.White);

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
            Rectangle entityRect = new Rectangle((int)scaledPosition.X, (int)scaledPosition.Y, 5, 5);
            spriteBatch.Draw(mapTexture, entityRect, color);
        }
    }
}