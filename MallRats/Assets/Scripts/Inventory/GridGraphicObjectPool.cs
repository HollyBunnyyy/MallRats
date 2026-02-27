using System.Collections.Generic;
using UnityEngine;

public class GridGraphicObjectPool : MonoBehaviour
{
    [SerializeField]
    private RectTransform _contentArea;

    [SerializeField]
    private GridGraphic _gridGraphicPrefab;

    [SerializeField]
    private int _amountToPrewarm = 8;

    private Queue<GridGraphic> _gridGraphicPool;

    protected void Awake()
    {
        _gridGraphicPool = new Queue<GridGraphic>();

        _amountToPrewarm = Mathf.Max(1, _amountToPrewarm);

        for (int i = 0; i < _amountToPrewarm; i++)
        {
            GenerateGraphic();
        }
    }

    public GridGraphic GetNext()
    {
        if (_gridGraphicPool.Count <= 1)
        {
            GenerateGraphic();
        }

        GridGraphic gridGraphic = _gridGraphicPool.Dequeue();
        gridGraphic.gameObject.SetActive(true);

        return gridGraphic;
    }

    public void ReturnToPool(GridGraphic gridGraphic)
    {
        gridGraphic.transform.gameObject.SetActive(false);
        gridGraphic.transform.SetAsLastSibling();
        _gridGraphicPool.Enqueue(gridGraphic);
    }

    public void GenerateGraphic()
    {
        GridGraphic graphicToAdd = Instantiate(_gridGraphicPrefab, _contentArea.transform);
        graphicToAdd.transform.gameObject.SetActive(false);
        graphicToAdd.transform.SetAsLastSibling();
        _gridGraphicPool.Enqueue(graphicToAdd);
    }
}

