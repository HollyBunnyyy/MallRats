using UnityEngine;

public class Item
{
    [SerializeField]
    [Min(1)]
    private int _width;
    public int Width
    {
        get { return _width; }
    }

    [SerializeField]
    [Min(1)]
    private int _height;
    public int Height
    {
        get { return _height; }
    }

    public Item(int width, int height)
    {
        _width = width;
        _height = height;
    }
}
