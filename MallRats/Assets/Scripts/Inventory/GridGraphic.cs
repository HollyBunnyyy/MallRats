using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class GridGraphic : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private RectTransform _rectTransform;
    public RectTransform RectTransform
    {
        get { return _rectTransform; }
    }

    private GridItem<Item> _gridItem;
    public GridItem<Item> GridItem 
    {
        get { return _gridItem; }
    }

    public event Action<GridGraphic, Vector2> OnGraphicMoved;

    public void SetGridItem(GridItem<Item> gridItem)
    {
        _gridItem = gridItem;
    }

    public void SetPositionToGrid(int xIndex, int yIndex, int cellSize, int cellPadding)
    {
        _rectTransform.anchoredPosition = new Vector2Int()
        {
            x =  (xIndex * cellSize) + ((xIndex + 1) * cellPadding),
            y = -(yIndex * cellSize) - ((yIndex + 1) * cellPadding)
        };
    }

    public void SetSizeToGrid(int width, int height, int cellSize, int cellPadding)
    {
        _rectTransform.sizeDelta = new Vector2Int()
        {
            x = (width  * cellSize) + ((width  - 1) * cellPadding),
            y = (height * cellSize) + ((height - 1) * cellPadding)
        };
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / 1.0f;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnGraphicMoved?.Invoke(this, _rectTransform.anchoredPosition);
    }

    public void Reset()
    {
        _rectTransform = GetComponent<RectTransform>();
        _rectTransform.pivot = new Vector2(0.0f, 1.0f);
    }
}
