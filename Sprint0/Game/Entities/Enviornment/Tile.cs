using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0
{
    public class Tile
    {
        public enum TileType
        {
            Grass,
            Sand,
            Concrete,
            Dirt
        }

        private TileType tileType;
        private ISprite sprite;
        private Vector2 position;

        public Tile(ContentManager content, TileType type, Vector2 position)
        {
            this.tileType = type;
            this.position = position;
            LoadTileSprite(content);
        }

        private void LoadTileSprite(ContentManager content)
        {
            sprite = new StaticSprite();

            switch (tileType)
            {
                case TileType.Grass:
                    sprite.LoadContent(content, "TDTanksAllSprites", 384, 256, 128, 128, 1);
                    break;
                case TileType.Sand:
                    sprite.LoadContent(content, "TDTanksAllSprites", 128, 128, 128, 128, 1);
                    break;
                case TileType.Concrete:
                    sprite.LoadContent(content, "TDTanksAllSprites", 256, 256, 128, 128, 1);
                    break;
                case TileType.Dirt:
                    sprite.LoadContent(content, "TDTanksAllSprites", 384, 384, 128, 128, 1);
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, position, SpriteEffects.None, 0f);
        }
    }
}
