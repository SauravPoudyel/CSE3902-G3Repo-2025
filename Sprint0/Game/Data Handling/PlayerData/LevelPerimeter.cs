using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Sprint0;
using static Sprint0.Level;
using static Sprint0.EntityKeys;
public class LevelPerimeter
{
    private List<BaseBlock> perimeterBlocks = new List<BaseBlock>();
    private Dictionary<string, Entity> entities;
    private readonly Level level;

    public LevelPerimeter(Level level, Dictionary<string, Entity> entities)
    {
        this.level = level;
        this.entities = entities;
    }

    public void Initialize(ContentManager content)
    {
        perimeterBlocks.Clear();

        for (int x = -1; x < 16; x++)
        {
            if (!level.HasConnectedLevel(Level.Direction.Top) || !level.GetConnectedLevel(Level.Direction.Top).Unlocked)
                AddBlock(content, new Vector2(x, -1));
            if (!level.HasConnectedLevel(Level.Direction.Bottom) || !level.GetConnectedLevel(Level.Direction.Bottom).Unlocked)
                AddBlock(content, new Vector2(x, 9));
        }

        for (int y = -1; y < 9; y++)
        {
            if (!level.HasConnectedLevel(Level.Direction.Left) || !level.GetConnectedLevel(Level.Direction.Left).Unlocked)
                AddBlock(content, new Vector2(-1, y));
            if (!level.HasConnectedLevel(Level.Direction.Right) || !level.GetConnectedLevel(Level.Direction.Right).Unlocked)
                AddBlock(content, new Vector2(16, y));
        }
    }

    public void Unlock(Direction direction)
    {
        Vector2 tileOffset = new Vector2(Globals.TILESIZE / 2f);

        Vector2 min = direction switch
        {
            Direction.Top    => new Vector2(-1, -1),
            Direction.Bottom => new Vector2(-1,  9),
            Direction.Left   => new Vector2(-1, -1),
            Direction.Right  => new Vector2(16, -1),
            _                => Vector2.Zero
        } * Globals.TILESIZE + tileOffset;

        Vector2 max = direction switch
        {
            Direction.Top    => new Vector2(16, -1),
            Direction.Bottom => new Vector2(16,  9),
            Direction.Left   => new Vector2(-1,  9),
            Direction.Right  => new Vector2(16,  9),
            _                => Vector2.Zero
        } * Globals.TILESIZE + tileOffset;

            var toRemove = perimeterBlocks
        .Where(f => f.GetPosition().X >= min.X && f.GetPosition().X <= max.X &&
                    f.GetPosition().Y >= min.Y && f.GetPosition().Y <= max.Y)
        .ToList();

        foreach (var block in toRemove)
        {
            perimeterBlocks.Remove(block);
            if (entities.ContainsKey(block.EntityKey))
                level.Entities.Remove(block.EntityKey);
        }
    }

    private void AddBlock(ContentManager content, Vector2 gridPos)
    {
        Vector2 worldPosition = (gridPos * Globals.TILESIZE) + new Vector2(Globals.TILESIZE / 2f);
        BaseBlock block = BlockFactory.CreateBlock(BlockType.Boarder, content, worldPosition, 0.3f);
        block.EntityKey = $"perimeterBlock_{perimeterBlocks.Count}";
        perimeterBlocks.Add(block);
        entities.Add(block.EntityKey, block);
    }
}
