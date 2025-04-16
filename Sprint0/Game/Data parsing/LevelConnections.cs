using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Sprint0;
using static Sprint0.Level;
using static Sprint0.EntityKeys;
public class LevelConnections
{
    private Dictionary<Level.Direction, Level> connections = new Dictionary<Level.Direction, Level>();

    public void AddConnectedLevel(Level.Direction direction, Level level)
    {
        if (level != null)
            connections[direction] = level;
    }

    public bool HasConnectedLevel(Level.Direction direction)
        => connections.ContainsKey(direction);

    public Level GetConnectedLevel(Level.Direction direction)
        => connections.TryGetValue(direction, out var level) ? level : null;

    public IEnumerable<KeyValuePair<Level.Direction, Level>> GetAll()
        => connections;

    public Level.Direction? GetDirectionOf(Level target)
    {
        foreach (var pair in connections)
        {
            if (pair.Value == target)
                return pair.Key;
        }
        return null;
    }

    public void SetAll(Dictionary<Level.Direction, Level> newConnections)
    {
        connections.Clear();
        foreach (var pair in newConnections)
            AddConnectedLevel(pair.Key, pair.Value);
    }
}
