using System.Collections.Generic;
using UnityEngine;

public class GridGraphicController : MonoBehaviour
{
    [SerializeField]
    private int _cellSize = 32;
    public int CellSize
    {
        get { return _cellSize; }
    }

    [SerializeField]
    private int _cellPadding = 2;
    public int CellPadding
    {
        get { return _cellPadding; }
    }

    [SerializeField]
    private GridGraphicObjectPool _gridGraphicsObjectPool;
    private HashSet<GridGraphic> _gridGraphicsInUse = new HashSet<GridGraphic>();
    private GridInventory<Item> _gridInventory;

    public void SetInventory(GridInventory<Item> gridInventoryToSet)
    {
        if(_gridInventory != null)
        {
            _gridInventory.OnCollectionDirty -= Redraw;
        }

        _gridInventory = gridInventoryToSet;
        _gridInventory.OnCollectionDirty += Redraw;

        Redraw();
    }

    public void MoveGraphic(GridGraphic graphic, Vector2 worldPosition)
    {
        GridItem<Item> gridItem = graphic.GridItem;

        if (_gridGraphicsInUse.Contains(graphic) is false || gridItem is null)
        {
            return;
        }

        // convert the anchored position of the rect transform to grid coordinates.
        Vector2Int positionOnGrid = new Vector2Int()
        {
            x = Mathf.RoundToInt( (worldPosition.x - _cellPadding) / (_cellSize + _cellPadding)),
            y = Mathf.RoundToInt(-(worldPosition.y + _cellPadding) / (_cellSize + _cellPadding))
        };

        // If the new position is outside the inventory's bounds, drop it.
        // I also divide the width and height by 2 here so the bounds have to be fully outside the inventory to be removed.
        if (_gridInventory.IsIndexRangeWithinBounds(positionOnGrid.x, positionOnGrid.y, gridItem.Width / 2, gridItem.Height / 2) is false)
        {
            _gridInventory.RemoveGridItem(gridItem);
        }

        // MoveItem invokes the OnCollectionDirty event which we've binded to redraw.
        // This way it snaps back to its previous position if invalid or moves to the new valid location.
        _gridInventory.MoveItem(ref gridItem, positionOnGrid.x, positionOnGrid.y);
    }

    public void Clear()
    {
        foreach (GridGraphic gridGraphic in _gridGraphicsInUse)
        {
            gridGraphic.SetGridItem(null);
            gridGraphic.OnGraphicMoved -= MoveGraphic;

            _gridGraphicsObjectPool.ReturnToPool(gridGraphic);
        }

        _gridGraphicsInUse.Clear();
    }

    public void Redraw()
    {
        Clear();

        if (_gridInventory == null)
        {
            return;
        }

        foreach (GridItem<Item> gridItem in _gridInventory.GetItems())
        {
            // Get our UI graphic from the object pool.
            GridGraphic gridGraphic = _gridGraphicsObjectPool.GetNext();

            gridGraphic.SetGridItem(gridItem);
            gridGraphic.SetPositionToGrid(gridItem.xAnchorIndex, gridItem.yAnchorIndex, _cellSize, _cellPadding);
            gridGraphic.SetSizeToGrid(gridItem.Width, gridItem.Height, _cellSize, _cellPadding);

            gridGraphic.OnGraphicMoved += MoveGraphic;

            _gridGraphicsInUse.Add(gridGraphic);
        }
    }
}
