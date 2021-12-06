using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CrosswordGrid : MonoBehaviour
{
    private char _emptySymbol;
    private float _unitLength;
    private char[,] _scheme;
    private CWCell[,] _cells;
    private RectTransform rectTransform;

    [SerializeField] private float _cellSizeOnSpace = 4f;
    [SerializeField] private float _letterOpenningTime = 0.15f;
    [SerializeField] private CellPool _cellPool = null;

    public CWCell[,] GetCells() => _cells;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    /// <summary>
    /// Gets <b>rect</b> of RectTransform to set the scaling parameter of CWCells, so that
    /// the Cells can fit both width and height of the space allowed by CrosswordGrid size.
    /// </summary>
    /// <param name="n">Number of cells vertically</param>
    /// <param name="m">Number of cells horizontally</param>
    /// <returns></returns>
    private void CalculateUnitLength(int n, int m)
    {
        float height = rectTransform.rect.height;
        float width = rectTransform.rect.width;

        float horizUnitSize = width  / (m * (_cellSizeOnSpace + 1) - 1);
        float vertiUnitSize = height / (n * (_cellSizeOnSpace + 1) - 1);

        _unitLength = (horizUnitSize < vertiUnitSize) ? horizUnitSize : vertiUnitSize;
    }

    internal void ReturnCellsToPool()
    {
        if (null == _cells) { return; }

        foreach (CWCell cell in _cells)
        {
            if (null == cell) continue;
            cell.Clear();
            _cellPool.StoreCell(cell);
        }
        _cells = null;
    }

    /// <summary>
    /// Gets necessary ammount of CWCells from pool to accuratly place them on scene,
    /// creating a crossvord field.
    /// </summary>
    /// <param name="scheme">The scheme of a crossword</param>
    public void BuildCrossword (char[,] scheme, char emptySymbol)
    {
        _scheme = scheme;
        _emptySymbol = emptySymbol;
        int n = scheme.GetUpperBound(0) + 1;
        int m = scheme.GetUpperBound(1) + 1;

        CalculateUnitLength(n, m);
        _cells = new CWCell[n, m];

        float vertiOffset  = +_unitLength * (n - 1) * (_cellSizeOnSpace + 1) / 2;
        float horizOffset0 = -_unitLength * (m - 1) * (_cellSizeOnSpace + 1) / 2;
        float horizOffset  = horizOffset0;

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                if (scheme[i,j] != _emptySymbol) {
                    CWCell cell = _cellPool.GetCell();
                    cell.SetHiddenLetter(scheme[i, j])
                        .SetSize(_unitLength * _cellSizeOnSpace)
                        .TakePlace(transform, relativePosition: new Vector3(horizOffset, vertiOffset), i, j)
                        .PlayAnimationAppear();
                    _cells[i, j] = cell;

                    char a = cell.GetHiddenLetter();
                    Debug.Log(i + ", " + j + ": " + a);
                }
                horizOffset += _unitLength * (_cellSizeOnSpace + 1);
            }
            vertiOffset -= _unitLength * (_cellSizeOnSpace + 1);
            horizOffset = horizOffset0;
        }
    }

    internal float AnimationTimeToOpen(string word)
    {
        return word.Length * _letterOpenningTime;
    }

    public Sequence OpenCell (CWCell cell, float openAnimationDelay)
    {
        Debug.Log("DOTweenCheck: CrosswordGrid: OpenCell");
        Sequence sequence = DOTween.Sequence();
        if (!cell.IsOpened)
        {
            cell.OpenLetter(openAnimationDelay);
        }
        return sequence;
    }

    /// <summary>
    /// Opens the word starting from [x,y] cell to the end.
    /// </summary>
    /// <param name="x">X of the first cell of a word in a grid.</param>
    /// <param name="y">Y of the first cell of a word in a grid.</param>
    /// <param name="direction">Tells whether to open vertically or horizontally.</param>
    /// <returns>Animation sequence of openning letters</returns>
    public Sequence OpenWord(int x, int y, CWword.CWDirection direction)
    {
        Debug.Log("DOTweenCheck: CrosswordGrid: OpenWord");
        Debug.Log("Opening word at " + x + ", " + y);
        Sequence sequence = DOTween.Sequence();
        CWCell currCell = _cells[x, y];
        float time = 0;
        while (currCell != null)
        {
            sequence.Join(OpenCell(currCell, time));
            time += _letterOpenningTime;

            if (direction == CWword.CWDirection.vertical)
            {
                if (++x > _cells.GetUpperBound(0)) { break; }
            }
            else
            {
                if (++y > _cells.GetUpperBound(1)) { break; }
            }
            currCell = _cells[x, y];

            Debug.Log("Next cell (" + x + ", " + y + ") is null: " + (currCell == null));
            if (null != currCell)
                Debug.Log("Letter is " + currCell.GetHiddenLetter());
        }
        return sequence;
    }

    public bool CheckWordOpened(int x, int y, CWword.CWDirection direction)
    {
        CWCell currCell = _cells[x, y];
        bool currCellOpened = false;
        Debug.LogFormat("Checking word at [{0}, {1}] (direction: {2})", x, y, direction.ToString());
        while (currCell != null)
        {
            currCellOpened = currCell.IsOpened;
            Debug.LogFormat("Cell [{0}, {1}] is opened: {2}", x, y, currCellOpened);
            if (!currCellOpened) {
                break;
            }


            if (direction == CWword.CWDirection.vertical)
            {
                if (++x > _cells.GetUpperBound(0)) { break; }
            }
            else
            {
                if (++y > _cells.GetUpperBound(1)) { break; }
            }
            currCell = _cells[x, y];
        }

        return currCellOpened;
    }
}
