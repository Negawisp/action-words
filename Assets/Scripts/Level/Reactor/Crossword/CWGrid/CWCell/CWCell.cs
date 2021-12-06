using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using System;
//using ADBannerView = UnityEngine.iOS.ADBannerView;

public class CWCell : MonoBehaviour,
                        IPointerClickHandler
{
    [SerializeField] private char _hiddenLetter = '\0';
    [SerializeField] private Text _text = null;

    public bool IsOpened { get; private set; }
    public int X { get; private set; }
    public int Y { get; private set; }

    private static Action<CWCell> _onCellClicked;

    public static void SetCellClickedCallback(Action<CWCell> onCellClicked)
    {
        _onCellClicked = onCellClicked;
    }

    public static void AddCellClickedCallback(Action<CWCell> onCellClicked)
    {
        _onCellClicked += onCellClicked;
    }

    public static void RemoveCellClickedCallback(Action<CWCell> onCellClicked)
    {
        _onCellClicked -= onCellClicked;
    }

    public void OpenLetter (float openAnimationDelay)
    {
        Debug.Log("DOTweenCheck: CWCell: OpenLetter");
        if (!IsOpened)
        {
            Sequence sequence = DOTween.Sequence();
            IsOpened = true;
            _text.text = _hiddenLetter.ToString().ToUpper();
            if (_text.color.a == 0f)
            {
                sequence.Insert(openAnimationDelay,
                    _text.DOColor(new Color(1f, 1f, 1f, 1f), 0.15f));
            }
        }
    }
    
    public CWCell SetHiddenLetter (char hiddenLetter)
    {
        _hiddenLetter = hiddenLetter;
        return this;
    }

    public char GetHiddenLetter ()
    {
        return _hiddenLetter;
    }

    public CWCell SetSize (float size)
    {
        this.GetComponent<Transform>();
        var s = GetComponent<RectTransform>().sizeDelta;
        GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
        return this;
    }

    public CWCell TakePlace (Transform newParent, Vector3 relativePosition, int x, int y)
    {
        Debug.Log("Color of the cell: on taking place" + GetComponent<Image>().color.ToString());
        X = x;
        Y = y;
        IsOpened = false;
        RectTransform rt = GetComponent<RectTransform>();
        transform.SetParent(newParent);
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.zero;
//        rt.anchoredPosition3D = relativePosition;

        transform.localPosition = relativePosition;
        return this;
    }

    /// <summary>
    /// Clears cell, preparing it to be stored to pool.
    /// </summary>
    public void Clear()
    {
        _hiddenLetter = ' ';
        _text.text = "";
    }

    public void PlayAnimationAppear()
    {
        Debug.Log("DOTweenCheck: CWCell: PlayAnimationAppear");
        GetComponent<Image>().color = Color.white;
        GetComponent<Image>().DOFade(1f, 1f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("CWCell was clicked!1!!!!");
        _onCellClicked(this);
    }
}
