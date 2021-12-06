using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class BaseScrollLetter : MonoBehaviour,
    ISceneLetter,
    IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler,
    IPointerDownHandler, IPointerClickHandler
{
    [SerializeField] private Text  _letterText = null;
    [SerializeField] private Animator _animator = null;

    private enum AnimatorParameters
    {
        Selected = 1
    }
    private int _timesChosen;
    private string AnimatorChosen = "TimesChosen";

    private ScorredLetter _letter;

    public ScorredLetter ScorredLetter => _letter;

    private List<BaseScrollLetter> _neighbourLetters;

    private Action<BaseScrollLetter> _LetterSelected;
    private Action<BaseScrollLetter> _LetterUnselected;
    private Action<BaseScrollLetter> _DragEnded;
    private Action<Vector3> _Drag;

    protected void Awake()
    {
        _timesChosen = 0;

        _letterText  = GetComponentInChildren<Text>();
        _animator = GetComponent<Animator>();
    }


    public virtual void Construct (ScorredLetter letter, int lettersInPentagram)
    {
        _letter = letter;
        _letterText.text = letter.Letter.ToString().ToUpper();
        _neighbourLetters = new List<BaseScrollLetter>(lettersInPentagram - 1);
    }

    


    public void AddLetterSelectedCallback(Action<BaseScrollLetter> action)
    { _LetterSelected += action; }

    public void RemoveLetterSelectedCallback(Action<BaseScrollLetter> action)
    { _LetterSelected -= action; }

    public void SelectLetter()
    {
        _animator.SetInteger(AnimatorChosen, ++_timesChosen);
        _LetterSelected(this);
    }

    public void UnselectLetter()
    {
        _animator.SetInteger(AnimatorChosen, --_timesChosen);
        _LetterUnselected(this);
    }

    public void AddLetterUnselectedCallback(Action<BaseScrollLetter> action)
    { _LetterUnselected += action; }

    public void RemoveLetterUnselectedCallback(Action<BaseScrollLetter> action)
    { _LetterUnselected -= action; }

    public void AddDragEndedCallback(Action<BaseScrollLetter> action)
    { _DragEnded += action; }

    public void RemoveDragEndedCallback(Action<BaseScrollLetter> action)
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
        if (eventData.dragging)
        {
            if (TryToSelect(eventData.pointerDrag.GetComponent<BaseScrollLetter>()))
            {
                eventData.pointerDrag = this.gameObject;
            }
        }
    }

    public bool TryToSelect(BaseScrollLetter previousLetter)
    {
        Debug.Log("Selecting PentaLetter " + _letter + " (" + name + ")");
        if (previousLetter == null)
        {
            SelectLetter();
            return true;
        }

        if (previousLetter.AddToNeighbours(this))
        {
            if (!AddToNeighbours(previousLetter))
            {
                Debug.LogError("Inconsistancy in neighbour tracking!!!");
                return false;
            }
            SelectLetter();
            return true;
        }

        if (previousLetter.GetLastNeighbour() == this)
        {
            if (GetLastNeighbour() != previousLetter)
            {
                Debug.LogError("Inconsistancy in neighbour tracking!!!");
                return false;
            }

            previousLetter.RemoveFromNeighbours(this);
            RemoveFromNeighbours(previousLetter);
            previousLetter.UnselectLetter();
            return true;
        }

        return false;
    }
    

    public BaseScrollLetter GetLastNeighbour()
    {
        return _neighbourLetters[_neighbourLetters.Count - 1];
    }

    public BaseScrollLetter RemoveFromNeighbours(BaseScrollLetter letter)
    {
        if (_neighbourLetters.Contains(letter))
        {
            _neighbourLetters.Remove(letter);
            return letter;
        }
        return null;
    }

    public void Unselect()
    {
        _timesChosen = 0;
        _animator.SetInteger(AnimatorChosen, 0);
        _neighbourLetters.Clear();
    }

    public bool AddToNeighbours(BaseScrollLetter letter)
    {
        if (letter == this) return false;

        if (_neighbourLetters.Contains(letter))
        {
            return false;
        }

        _neighbourLetters.Add(letter);
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
