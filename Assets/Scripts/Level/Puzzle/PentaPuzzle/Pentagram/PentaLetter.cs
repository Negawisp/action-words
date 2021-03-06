using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JetBrains.Annotations;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PentaLetter : MonoBehaviour,
    IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler,
    IPointerDownHandler, IPointerClickHandler
{

    private PentaPuzzle _manager;
    private Liner _liner;
    private Text  _text;
    private Animator _animator;
    private enum AnimatorParameters
    {
        Selected = 1
    }
    private int _timesChosen;
    private string AnimatorChosen = "TimesChosen";

    private char _letter;   public char GetLetter() { return _letter; }

    private int             _nNeighbourLetters;
    private PentaLetter[]   _neighbourLetters;

    private Action< PentaLetter >       _LetterSelected;
    private Action< PentaLetter >       _DragEnded;
    private Action< Vector3 >  _Drag;

    void Awake()
    {
        _timesChosen = 0;

        _manager = FindObjectOfType<PentaPuzzle>();
        _liner = FindObjectOfType<Liner>();
        _text  = GetComponentInChildren<Text>();
        _animator = GetComponent<Animator>();
    }


    public void Construct (char letter, int lettersInPentagram)
    {
        _letter = letter;
        _text.text = ("" + letter).ToUpper();

        _neighbourLetters = new PentaLetter[lettersInPentagram - 1];
    }

    


    public void AddLetterSelectedCallback(Action<PentaLetter> action)
    { _LetterSelected += action; }

    public void RemoveLetterSelectedCallback(Action<PentaLetter> action)
    { _LetterSelected -= action; }

    public void OnLetterSelected()
    { _LetterSelected(this); }


    public void AddDragEndedCallback(Action<PentaLetter> action)
    { _DragEnded += action; }

    public void RemoveDragEndedCallback(Action<PentaLetter> action)
    { _DragEnded -= action; }



    public void AddDragCallback(Action<Vector3> action)
    { _Drag += action; }

    public void RemoveDragCallback(Action<Vector3> action)
    { _Drag -= action; }



    private void Update()
    {
        RectTransform rt = GetComponent<RectTransform>();
        //rt.anchoredPosition.Set(0, 0);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        _Drag(eventData.pointerCurrentRaycast.worldPosition);
        RaycastResult name = eventData.pointerCurrentRaycast;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _DragEnded(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.dragging && TryToSelect(eventData.pointerDrag.GetComponent<PentaLetter>()))
        {
            eventData.pointerDrag = this.gameObject;
        }
    }

    public bool TryToSelect(PentaLetter previousLetter)
    {
        Debug.Log("Selecting PentaLetter " + _letter + " (" + name + ")");
        if (previousLetter != null)
        {
            if (!previousLetter.AddToNeighbours(this))
            { return false; }
            AddToNeighbours(previousLetter);
        }
        _animator.SetInteger(AnimatorChosen, ++_timesChosen);
        OnLetterSelected();
        return true;
    }

    public void Unselect()
    {
        _timesChosen = 0;
        _animator.SetInteger(AnimatorChosen, 0);
        _nNeighbourLetters = 0;
    }

    public bool AddToNeighbours(PentaLetter letter)
    {
        if (letter == this) return false;

        for (int i = 0; i < _nNeighbourLetters; i++)
        {
            if (_neighbourLetters[i] == letter) return false;
        }
        
        _neighbourLetters[_nNeighbourLetters++] = letter;
        return true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        TryToSelect(null);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _DragEnded(this);
    }
}
