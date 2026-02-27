// This is primarily used as a reference class in the hashset of GridInventory - so we want these values immutable.
public class GridItem<T> where T : class
{
    public readonly T Item;

    public readonly int xAnchorIndex;
    public readonly int yAnchorIndex;

    public readonly int Width;
    public readonly int Height;

    public GridItem(int xIndex, int yIndex, int width, int height, T item)
    {
        this.Item           = item;
        this.xAnchorIndex   = xIndex;
        this.yAnchorIndex   = yIndex;
        this.Width          = width;
        this.Height         = height;
    }
}
