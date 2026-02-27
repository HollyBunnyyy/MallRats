using System.Collections.Generic;
using UnityEngine;

public class GridMap<T> where T : class
{
    private Dictionary<(int x, int y), T> _gridObjects;

    public GridMap()
    {
        _gridObjects = new Dictionary<(int x, int y), T>();
    }

    public T this[int x, int y]
    {
        get => Get(x, y);
    }

    public void Add(int x, int y, T objectToAdd)
    {
        _gridObjects.Add((x, y), objectToAdd);
    }

    public bool Contains(int x, int y)
    {
        return _gridObjects.ContainsKey((x, y));
    }

    public T Get(int x, int y)
    {
        if(Contains(x, y) is false)  
        {
            return null;
        }

        return _gridObjects[(x, y)];
    }

    public IEnumerable<T> GetSurrounding(int x, int y)
    {
        foreach (Direction direction in Direction.Cardinal)
        {
            int xNeighborIndex = x + direction.x;
            int yNeighborIndex = y + direction.y;

            if (Contains(xNeighborIndex, yNeighborIndex))
            {
                yield return this[xNeighborIndex, yNeighborIndex];
            }
        }
    }

    public IEnumerable<T> GetAll() 
    { 
        foreach(T objectToGet in _gridObjects.Values)
        {
            yield return objectToGet;
        }
    }
}
