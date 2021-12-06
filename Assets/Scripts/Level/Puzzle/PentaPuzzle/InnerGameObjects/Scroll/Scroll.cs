using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroll : MonoBehaviour
{
    [SerializeField] private float _letterZ = 0f;
    [SerializeField] private Text _wordsSelectedText = null;

    const float _animationDelayTime = 0.2f;

    private PPGameobjectsPool _pool;
    private Animator _anim;
    public Text GetWordsSelectedCounter() { return _wordsSelectedText; }

    private Pentagram _pentagram;              public Pentagram GetPentagram() {  return _pentagram; }
    private BaseScrollLetter[] _pentaLetters;
    private List<ScorredLetter> _selectedScorredLetters;
    private List<BaseScrollLetter> _selectedLetters;
    private string _currentWord;                    public string GetSelectedWord() { return _currentWord; }
    private float _currentRadius = 0;
    private float _shuffleDuration = 0;


    public Action<string, List<ScorredLetter>> _SelectedWordChangedCallback;
    public Action<string, List<ScorredLetter>> _onWordSelected;
    public Action<int> _ThrownAwayCallback;

    public void AddSelectedWordChangedCallback (Action<string, List<ScorredLetter>> callback)
    {
        _SelectedWordChangedCallback += callback;
    }
    
    public void SetSpellActivatedCallBack(Action<string, List<ScorredLetter>> callback)
    {
        _onWordSelected = callback;
    }


    public void AddThrownAwayCallback(Action<int> callback)
    { _ThrownAwayCallback += callback; }

    public void OnThrownAway()
    {
        ReturnLettersToPool();
        _ThrownAwayCallback(0);
    }

    protected Sequence AnimateAppearFromRight()
    {
        return DOTween.Sequence()
            .AppendCallback(() => _anim.SetTrigger("AppearFromRight"));
    }

    protected Sequence AnimateDisappearRight()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 anchorPos = rectTransform.anchoredPosition;

        return DOTween.Sequence()
                .AppendCallback(() => _anim.SetTrigger("SlideRight"))
                .AppendInterval(_animationDelayTime)
                .AppendCallback(OnThrownAway);
    }

    protected Sequence AnimateAppearFromLeft()
    {
        return DOTween.Sequence()
            .AppendCallback(() => _anim.SetTrigger("AppearFromLeft"));
    }

    protected Sequence AnimateDisappearLeft()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 anchorPos = rectTransform.anchoredPosition;

        return DOTween.Sequence()
                .AppendCallback(() => _anim.SetTrigger("SlideLeft"))
                .AppendInterval(_animationDelayTime)
                .AppendCallback(OnThrownAway);
        //AnimateDisappearThrowAway();
    }

    public Sequence AnimateAppearFromBehind()
    {
        return DOTween.Sequence();
    }

    public Sequence AnimateDisappearThrowAway()
    {
        return DOTween.Sequence()
            .AppendCallback(() => _anim.SetTrigger("ThrownAway"));
    }

    public Sequence AnimateAppear(DirectionEnum direction)
    {
        switch (direction)
        {
            case DirectionEnum.ToTheLeft:
                {
                    return AnimateAppearFromLeft();
                } break;
            case DirectionEnum.ToTheRight:
                {
                    return AnimateAppearFromRight();
                } break;
            default:
                {
                    Debug.LogWarningFormat("{0} appear animation type is not implemented!", direction);
                    return AnimateAppearFromBehind();
                } break;
        }
    }

    public Sequence AnimateDisappear(DirectionEnum direction)
    {
        switch (direction)
        {
            case DirectionEnum.ToTheLeft:
                {
                    return AnimateDisappearRight();
                }
                break;
            case DirectionEnum.ToTheRight:
                {
                    return AnimateDisappearLeft();
                }
                break;
            default:
                {
                    Debug.LogWarningFormat("{0} disappear animation type is not implemented!", direction);
                    return AnimateDisappearThrowAway();
                }
                break;
        }
    }

    public void Awake()
    {
        _anim = GetComponent<Animator>();
        _selectedLetters = new List<BaseScrollLetter>();
        _selectedScorredLetters = new List<ScorredLetter>();
        _pool = FindObjectOfType<PPGameobjectsPool>();
    }


    public void Load(Pentagram pentagram, float radius, float letterSize, float shuffleDuration)
    {
        Debug.Log("Scroll: Load. Pentagram is null: " + (pentagram == null));
        _pentagram = pentagram;
        _currentRadius = radius;
        _shuffleDuration = shuffleDuration;

        int nLetters = pentagram.Letters.Length;
        float turningAngle = 2 * Mathf.PI / nLetters;
        float stepVectorLength = 2 * radius * Mathf.Sin(turningAngle / 2);

        Debug.Log("Radius: " + radius + "\nStep: " + stepVectorLength);

        Vector2 letterPosition = new Vector2(0, radius);
        Vector2 stepVector = new Vector2(0, -stepVectorLength);
        VectorService.RotateVector(ref stepVector, (turningAngle - Mathf.PI) / 2);

        Debug.Log("Going to place " + nLetters + "letters.");
        _pentaLetters = new BaseScrollLetter[nLetters];
        for (int i = 0; i < nLetters; i++)
        {
            _pentaLetters[i] = _pool.GetLetter();
            _pentaLetters[i].Construct(pentagram.Letters[i], nLetters);

            RectTransform rt = _pentaLetters[i].GetComponent<RectTransform>();
            rt.SetParent(this.transform);

            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, letterSize);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, letterSize);

            animateLetterSpawning(_pentaLetters[i], new Vector3(letterPosition.x, letterPosition.y, _letterZ));
            letterPosition = letterPosition + stepVector;

            VectorService.RotateVector(ref stepVector, turningAngle);

            _pentaLetters[i].AddDragEndedCallback(TryActivate);
            _pentaLetters[i].AddLetterSelectedCallback(SelectLetter);
            _pentaLetters[i].AddLetterUnselectedCallback(UnselectLetter);
        }
    }

    public void ReshuffleLetters()
    {
        int n = _pentaLetters.Length;
        int[] newLettersOrder = new int[n];
        do {
            ArrayService.GenerateRandomUnrepeatingSequence(newLettersOrder, 0, n);
            ArrayService.SequenceIsOrdered(newLettersOrder);
        } while (ArrayService.SequenceIsIncreasing(newLettersOrder));
        ReshuffleLetters(newLettersOrder);
    }

    public void ReshuffleLetters(int[] order)
    {
        if (null == order) {
            Debug.LogError("\"Order\" variable was null.");
            return;
        }
        if (null == _pentaLetters) {
            Debug.LogWarning("\"_pentaLetters\" field was null.");
            return;
        }
        if (order.Length != _pentaLetters.Length) {
            Debug.LogErrorFormat("Length of _pentaLetters[] (= {0}) and order[] (= {1}) were not equal.");
            return;
        }

        BaseScrollLetter[] newArray = (BaseScrollLetter[])_pentaLetters.Clone();
        float turningAngle = 2 * Mathf.PI / order.Length;
        float stepVectorLength = 2 * _currentRadius * Mathf.Sin(turningAngle / 2);
        Vector2 letterPosition = new Vector2(0, _currentRadius);
        Vector2 stepVector = new Vector2(0, -stepVectorLength);
        VectorService.RotateVector(ref stepVector, (turningAngle - Mathf.PI) / 2);
        int i = 0;
        foreach (int k in order)
        {
            RectTransform rt = _pentaLetters[k].GetComponent<RectTransform>();
            animateLetterMoving(rt, new Vector3(letterPosition.x, letterPosition.y, _letterZ));
            
            letterPosition = letterPosition + stepVector;
            VectorService.RotateVector(ref stepVector, turningAngle);

            newArray[i++] = _pentaLetters[k];
        }

        _pentaLetters = newArray;
        _pentagram.ChangeLettersOrder(order);
    }

    private void animateLetterMoving(RectTransform letterRectTransform, Vector3 newPosition)
    {
        Debug.Log("DOTweenCheck: Scroll: AnimateLetterMoving");
        letterRectTransform.DOAnchorPos3D(newPosition, _shuffleDuration);
    }

    private void animateLetterSpawning(BaseScrollLetter letter, Vector3 endingPosition)
    {
        letter.GetComponent<RectTransform>().anchoredPosition3D = endingPosition;
    }

    public void ReturnLettersToPool()
    {
        if (_pentaLetters == null || _pentaLetters.Length == 0) return;

        foreach (BaseScrollLetter letter in _pentaLetters)
        {
            letter.RemoveLetterSelectedCallback(SelectLetter);
            letter.RemoveLetterUnselectedCallback(UnselectLetter);
            letter.RemoveDragEndedCallback(TryActivate);
        }
        _pool.Store(_pentaLetters);
        _pentaLetters = null;
    }

    
    



    private void SelectLetter(BaseScrollLetter letter)
    {
        Debug.Log("Scroll: Selected letter " + letter.ScorredLetter.Letter);
        _selectedLetters.Add(letter);
        _selectedScorredLetters.Add(letter.ScorredLetter);
        _currentWord += letter.ScorredLetter.Letter;

        _SelectedWordChangedCallback(_currentWord, _selectedScorredLetters);
    }

    private void UnselectLetter(BaseScrollLetter letter)
    {
        Debug.Log("Scroll: Unselected letter " + letter.ScorredLetter.Letter);
        _selectedLetters.RemoveAt(_selectedLetters.LastIndexOf(letter));
        _selectedScorredLetters.RemoveAt(_selectedScorredLetters.LastIndexOf(letter.ScorredLetter));
        _currentWord = _currentWord.Substring(0, _currentWord.Length-1);

        _SelectedWordChangedCallback(_currentWord, _selectedScorredLetters);
    }

    public void UnselectLetters()
    {
        foreach (BaseScrollLetter letter in _selectedLetters)
        { letter.Unselect(); }
        _selectedLetters.Clear();
        _selectedScorredLetters.Clear();

        _currentWord = "";
        _SelectedWordChangedCallback("", _selectedScorredLetters);
    }

    public void TryActivate(BaseScrollLetter lastSelectedLetter)
    {
        Debug.Log("TryActivate");
        if (_currentWord.Length <= 1)
        {
            UnselectLetters();
        }

        _onWordSelected(_currentWord, _selectedScorredLetters);

        UnselectLetters();
    }

    public IEnumerator SelectWord(string word)
    {
        bool letterFound = false;
        BaseScrollLetter previousLetter = null;
        word = word.ToLower();

        foreach (char letter in word)
        {
            letterFound = false;
            foreach (BaseScrollLetter pentaLetter in _pentaLetters)
            {
                if (pentaLetter.ScorredLetter.Letter == letter)
                {
                    letterFound = true;
                    bool nextLetterIsSelectable = pentaLetter.TryToSelect(previousLetter);
                    yield return new WaitForSecondsRealtime(0.3f);
                    if (!nextLetterIsSelectable) { yield break; }

                    previousLetter = pentaLetter;
                    break;
                }
            }
            if (!letterFound) yield break;
        }
    }
}
