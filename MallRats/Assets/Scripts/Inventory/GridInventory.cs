using System;
using System.Collections.Generic;

public class GridInventory<T> where T : class
{
    public readonly int Width;
    public readonly int Height;

    // this event is called when the collection has had items added, removed, or moved.
    public event Action OnCollectionDirty;

    // This stores the unique item references and is O(1) for reference lookup.
    private HashSet<GridItem<T>> _items;

    // This tells us whether a slot is occupied (true) or unoccupied (false). The actual item reference is stored in the above hashset.
    // I also use boolean values here as they're 1 byte each which is way better than storing a reference.
    private bool[,] _slots;

    public int Count
    {
        get { return _items.Count; }
    }

    public GridInventory(int width, int height)
    {
        this.Width = width;
        this.Height = height;

        this._items = new HashSet<GridItem<T>>();
        this._slots = new bool[width, height];
    }

    public GridItem<T> this[int x, int y]
    {
        get => GetGridItem(x, y);
    }

    /// <summary>
    /// Returns whether the specified x and y index are within the bounds of the grid.
    /// </summary>
    public bool IsIndexWithinBounds(int xIndex, int yIndex)
    {
        return xIndex >= 0 && yIndex >= 0 && xIndex < Width && yIndex < Height;
    }

    /// <summary>
    /// Returns whether an area of size width and height at the x and y index are within bounds of the grid.
    /// </summary>
    public bool IsIndexRangeWithinBounds(int xIndex, int yIndex, int width, int height)
    {
        return xIndex >= 0 && yIndex >= 0 && (xIndex + width) <= Width && (yIndex + height) <= Height && (width + height) >= 2;
    }

    // Sets the grid flags for a grid item.
    // Generally I try to avoid private methods as much as possible, but if this is changed outside the class it can collapse everything.
    private void SetGridFlags(GridItem<T> gridItem, bool value)
    {
        for (int x = gridItem.xAnchorIndex; x < (gridItem.xAnchorIndex + gridItem.Width); x++)
        {
            for (int y = gridItem.yAnchorIndex; y < (gridItem.yAnchorIndex + gridItem.Height); y++)
            {
                _slots[x, y] = value;
            }
        }
    }

    // Returns an enumerable list of slots within the specified width and height at the x and y index.
    private IEnumerable<bool> GetSlotsWithinRange(int xIndex, int yIndex, int width, int height)
    {
        for (int x = xIndex; x < (xIndex + width); x++)
        {
            for (int y = yIndex; y < (yIndex + height); y++)
            {
                yield return _slots[x, y];
            }
        }
    }

    /// <summary>
    /// Returns an enumerable list of all items in the collection.
    /// </summary>
    public IEnumerable<GridItem<T>> GetItems()
    {
        foreach (GridItem<T> gridItem in _items)
        {
            yield return gridItem;
        }
    }

    /// <summary>
    /// Returns whether the area of width and height at the x and y index can fit within the collection.
    /// </summary>
    /// <remarks>* This also checks if it's overlapping with any other grid items.</remarks>
    public bool CanFit(int xIndex, int yIndex, int width, int height)
    {
        if (IsIndexRangeWithinBounds(xIndex, yIndex, width, height) is false)
        {
            return false;
        }

        foreach (bool gridSlot in GetSlotsWithinRange(xIndex, yIndex, width, height))
        {
            if (gridSlot is true) // If any of the slots are occupied (true), then the item cannot fit.
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Attempts to add an item taking up an area of width and height at the position of x and y to the inventory.
    /// </summary>
    public bool AddItem(int xIndex, int yIndex, int width, int height, T item)
    {
        if (CanFit(xIndex, yIndex, width, height) is false || item == null)
        {
            return false;
        }

        GridItem<T> gridItem = new GridItem<T>(xIndex, yIndex, width, height, item);

        // Add our item to the item list.
        _items.Add(gridItem);

        // Set the grid flags as occupoed (true).
        SetGridFlags(gridItem, true);

        OnCollectionDirty?.Invoke();

        return true;
    }

    /// <summary>
    /// Attempts to add an item taking up an area of width and height to the earliest open spot in the collection.
    /// </summary>
    public bool AddItem(int width, int height, T item)
    {
        for (int y = 0; y <= Height; y++)
        {
            for (int x = 0; x <= Width; x++)
            {
                // I swap the x and y index here to prioritize filling the grid horizontally before vertically.
                if (AddItem(x, y, width, height, item)) 
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Attempts to get an item at the specified x and y index.
    /// </summary>
    public GridItem<T> GetGridItem(int xIndex, int yIndex)
    {
        // Only continue if the specified index is within bounds, and if the slot is occupied (true). Otherwise we know there's no item there.
        if (IsIndexWithinBounds(xIndex, yIndex) is false || _slots[xIndex, yIndex] is false)
        {
            return null;
        }

        foreach (GridItem<T> gridItem in _items)
        {
            // If our x and y index are within the range of the grid item, we know that's the desired item.
            bool xIndexWithinHorizontalRange = (xIndex >= gridItem.xAnchorIndex) && (xIndex < (gridItem.xAnchorIndex + gridItem.Width ));
            bool yIndexWithinVerticalRange   = (yIndex >= gridItem.yAnchorIndex) && (yIndex < (gridItem.yAnchorIndex + gridItem.Height));

            if (xIndexWithinHorizontalRange && yIndexWithinVerticalRange)
            {
                return gridItem;
            }
        }

        return null;
    }

    /// <summary>
    /// Attempts to remove the specified griditem from the collection.
    /// </summary>
    public bool RemoveGridItem(GridItem<T> gridItem)
    {
        if (_items.Contains(gridItem) is false)
        {
            return false;
        }

        // Mark the slots occupied by the item as unoccupied (false) before removing the item reference from the hashset.
        SetGridFlags(gridItem, false);

        _items.Remove(gridItem); // Finally remove the actual item reference from the hashset.

        OnCollectionDirty?.Invoke();

        return true;
    }

    /// <summary>
    /// Attempts to remove the item at the specified x and y index from the collection.
    /// </summary>
    public bool RemoveItem(int xIndex, int yIndex)
    {
        GridItem<T> gridItem = GetGridItem(xIndex, yIndex);

        if (gridItem == null)
        {
            return false;
        }

        return RemoveGridItem(gridItem);
    }

    /// <summary>
    /// Attempts to move the specified griditem to the desired x and y index position.
    /// </summary>
    /// <remarks>* If the movement is valid, the ref pointer for the griditem will point to a new item at the x and y index.</remarks>
    public bool MoveItem(ref GridItem<T> gridItem, int xIndex, int yIndex)
    {
        if (gridItem == null || _items.Contains(gridItem) is false)
        {
            return false;
        }

        // Mark the slots occupied by the item as unoccupied (false) before checking if it can fit in the new position.
        SetGridFlags(gridItem, false);

        if (CanFit(xIndex, yIndex, gridItem.Width, gridItem.Height) is false)
        {
            // Mark the previous slots occupied again if it cannot fit in the new position.
            SetGridFlags(gridItem, true);

            OnCollectionDirty?.Invoke();

            return false;
        }

        // remove the actual item reference from the hashset so it gets deleted by the GC.
        _items.Remove(gridItem);

        // we'll create a new griditem and redirect the ref pointer to point to this new object.
        gridItem = new GridItem<T>(xIndex, yIndex, gridItem.Width, gridItem.Height, gridItem.Item);

        // Add our item to the item list.
        _items.Add(gridItem);

        // Set the underlying gridslots as occupied (true).
        SetGridFlags(gridItem, true);

        OnCollectionDirty?.Invoke();

        return true;
    }
}
