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
            Dirt,
            Snow,
            Ice,
            RightGrassLeftSand,
            LeftGrassRightSand,
            TopGrassBottomSand,
            BottomGrassTopSand,
            GrassCrossroad,
            SandCrossroad,
            VerticalGrassRoad,
            VerticalSandRoad,
            TopGrassBottomSandRoad,
            BottomGrassTopSandRoad,
            LeftGrassRightSandRoad,
            RightGrassLeftSandRoad,
            HorizontalGrassRoad,
            HorizontalSandRoad,
            TopRightGrassCurve,
            TopLeftGrassCurve,
            BottomRightGrassCurve,
            BottomLeftGrassCurve,
            Forest,
            SandShipWreckage,
            Water,
            ItemSpawn,
            PineForest
        }

        private TileType tileType;
        private StaticSprite sprite;
        private Vector2 position;
        private float spriteScaler = 0.9375f; // = 120/128, used to scale between the spritesheet size of 128x128 and grid size of 120

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
                    sprite.LoadContent(content, "TDTanksAllSprites", 256, 0, 128, 128, 1);
                    break;
                case TileType.Concrete:
                    sprite.LoadContent(content, "TDTanksAllSprites", 640, 1256, 128, 128, 1);
                    break;
                case TileType.Dirt:
                    sprite.LoadContent(content, "TDTanksAllSprites", 640, 1128, 128, 128, 1);
                    break;
                case TileType.Snow:
                    sprite.LoadContent(content, "TDTanksAllSprites", 0, 1384, 128, 128, 1);
                    break;
                case TileType.Ice:
                    sprite.LoadContent(content, "TDTanksAllSprites", 512, 1768, 128, 128, 1);
                    break;
                case TileType.LeftGrassRightSand:
                    sprite.LoadContent(content, "TDTanksAllSprites", 640, 128, 128, 128, 1);
                    break;
                case TileType.LeftGrassRightSandRoad:
                    sprite.LoadContent(content, "TDTanksAllSprites", 512, 640, 128, 128, 1);
                    break;
                case TileType.RightGrassLeftSandRoad:
                    sprite.LoadContent(content, "TDTanksAllSprites", 0, 2 * 128, 128, 128, 1);
                    break;
                case TileType.HorizontalGrassRoad:
                    sprite.LoadContent(content, "TDTanksAllSprites", 0, 768, 128, 128, 1);
                    break;
                case TileType.HorizontalSandRoad:
                    sprite.LoadContent(content, "TDTanksAllSprites", 256, 896, 128, 128, 1);
                    break;
                case TileType.RightGrassLeftSand:
                    sprite.LoadContent(content, "TDTanksAllSprites", 4 * 128, 0 * 128, 128, 128, 1);
                    break;
                case TileType.TopGrassBottomSand:
                    sprite.LoadContent(content, "TDTanksAllSprites", 4 * 128, 1 * 128, 128, 128, 1);
                    break;
                case TileType.BottomGrassTopSand:
                    sprite.LoadContent(content, "TDTanksAllSprites", 4 * 128, 2 * 128, 128, 128, 1);
                    break;
                case TileType.GrassCrossroad:
                    sprite.LoadContent(content, "TDTanksAllSprites", 1 * 128, 5 * 128, 128, 128, 1);
                    break;
                case TileType.SandCrossroad:
                    sprite.LoadContent(content, "TDTanksAllSprites", 3 * 128, 1 * 128, 128, 128, 1);
                    break;
                case TileType.VerticalGrassRoad:
                    sprite.LoadContent(content, "TDTanksAllSprites", 1 * 128, 7 * 128, 128, 128, 1);
                    break;
                case TileType.VerticalSandRoad:
                    sprite.LoadContent(content, "TDTanksAllSprites", 2 * 128, 6 * 128, 128, 128, 1);
                    break;
                case TileType.TopGrassBottomSandRoad:
                    sprite.LoadContent(content, "TDTanksAllSprites", 4 * 128, 7 * 128, 128, 128, 1);
                    break;
                case TileType.BottomGrassTopSandRoad:
                    sprite.LoadContent(content, "TDTanksAllSprites", 4 * 128, 3 * 128, 128, 128, 1);
                    break;
                case TileType.TopRightGrassCurve:
                    sprite.LoadContent(content, "TDTanksAllSprites", 0 * 128, 4 * 128, 128, 128, 1);
                    break;
                case TileType.TopLeftGrassCurve:
                    sprite.LoadContent(content, "TDTanksAllSprites", 0 * 128, 5 * 128, 128, 128, 1);
                    break;
                case TileType.BottomRightGrassCurve:
                    sprite.LoadContent(content, "TDTanksAllSprites", 1 * 128, 2 * 128, 128, 128, 1);
                    break;
                case TileType.BottomLeftGrassCurve:
                    sprite.LoadContent(content, "TDTanksAllSprites", 1 * 128, 3 * 128, 128, 128, 1);
                    break;
                case TileType.Forest:
                    sprite.LoadContent(content, "TDTanksAllSprites", 512, 1640, 128, 128, 1);
                    break;
                case TileType.SandShipWreckage:
                    sprite.LoadContent(content, "TDTanksAllSprites", 0, 2872, 128, 128, 1);
                    break;
                case TileType.Water:
                    sprite.LoadContent(content, "TDTanksAllSprites", 1024, 2744, 128, 128, 1);
                    break;
                case TileType.ItemSpawn:
                    sprite.LoadContent(content, "TDTanksAllSprites", 384, 1512, 128, 128, 1);
                    break;
                case TileType.PineForest:
                    sprite.LoadContent(content, "TDTanksAllSprites", 128, 1640, 128, 128, 1);
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, position, SpriteEffects.None, 0f, null, null, spriteScaler);
        }
    }
}
