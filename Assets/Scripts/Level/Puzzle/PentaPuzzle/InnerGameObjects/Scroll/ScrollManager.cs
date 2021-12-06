using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollManager : MonoBehaviour
{
    public enum PentagramPosition
    {
        Front,
        Back
    }

    public enum ScrollSlideDirection
    {
        Left,
        Right,
        Down,
        FromBehind
    }

    [SerializeField] private ScrollPlacement[] _scrollPlacements = null;  
    [SerializeField] private float _pentagramRelativeRadius = 0.333f;
    [SerializeField] private float _letterRelativeSize = 0.200f;
    [SerializeField] private float _letterShuffleDuration = 0.15f;

    
    public Scroll ActiveScroll { get; private set; }

    void Awake()
    {
        foreach (ScrollPlacement sp in _scrollPlacements)
        {
            sp.GetScroll().AddThrownAwayCallback(Swap);
        }

        ActiveScroll = _scrollPlacements[1].GetScroll();
    }

    public void ReturnLettersToPool()
    {
        foreach (ScrollPlacement scrollPlacement in _scrollPlacements)
        {
            scrollPlacement.GetScroll().ReturnLettersToPool();
        }
    }

    /// <summary>
    /// Changes active scroll to a one with given pentagram;
    /// launches animation of a scroll change according to given direction.
    /// </summary>
    /// <param name="pentagram">A pentagram(basically a set of letters) of a new active scroll</param>
    /// <param name="direction">A type of scroll change animation.</param>
    public Sequence ChangeScroll(Pentagram pentagram, DirectionEnum direction)
    {
        Sequence sequence = DOTween.Sequence();
        PrepareScroll(PentagramPosition.Back, pentagram);

        ActiveScroll = _scrollPlacements[1].GetScroll();
        sequence
            .Append(_scrollPlacements[(int)PentagramPosition.Front]
                    .GetScroll()
                    .AnimateDisappear(direction))
            .Join(_scrollPlacements[(int)PentagramPosition.Back]
                    .GetScroll()
                    .AnimateAppear(direction));
        return sequence;
    }

    private void Swap(int thisIntIsNotUsed)
    {
        Scroll p0 = _scrollPlacements[0].GetScroll();
        Scroll p1 = _scrollPlacements[1].GetScroll();

        _scrollPlacements[0].SetPentagram(p1);
        _scrollPlacements[1].SetPentagram(p0);
    }

    private void PrepareScroll(PentagramPosition pentagramPosition, Pentagram pentagram)
    {
        Scroll scroll = _scrollPlacements[(int)pentagramPosition].GetScroll();
        scroll.ReturnLettersToPool();


        float scrollWidth = GetComponent<RectTransform>().rect.width;
        //float screenWidth = Screen.width; Debug.Log("ScreenWidth " + screenWidth);
        float radius = scrollWidth * _pentagramRelativeRadius;
        float letterSize = scrollWidth * _letterRelativeSize; Debug.Log("letterSize " + letterSize);

        scroll.Load(pentagram, radius, letterSize, _letterShuffleDuration);
    }

    public void ReshuffleLetters()
    {
        ActiveScroll.ReshuffleLetters();
    }
}
