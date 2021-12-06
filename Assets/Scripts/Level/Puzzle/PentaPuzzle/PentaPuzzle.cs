using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PentaPuzzle : AbstractPuzzle
{
    private Action<string, List<ScorredLetter>> _onWordActivation;
    private Action<object> _nextScrollCallback;

    [SerializeField] private Animator _rewardAnimator = null;
    [SerializeField] private GameObject _changeScrollButton = null;
    [SerializeField] private Text _pentagramsLeftText = null;
    [SerializeField] private Text _selectedWordText = null;
    [SerializeField] private Text _animationText = null;
    [SerializeField] private Scroll[] _scrolls = null;

    [SerializeField] private OpenRandomTip _openRandomTip = null;
    [SerializeField] private OpenRandomWordTip _openRandomWordTip = null;
    [SerializeField] private OpenChosenCellTip _openChosenCellTip = null;

    private PentaPool _pentaPool;
    private PPGameobjectsPool _pool;
    private ScrollManager _scrollManager;
    private Liner _liner;
    private Sequence _sequence;

    public override PuzzleType PuzzleType => PuzzleType.Pentagram;

    //private int _pentagramCounter;
    //private Pentagram[] _pentagrams;

    
    private void Awake()
    {
        _pentaPool = new PentaPool();

        _liner = FindObjectOfType<Liner>();
        _pool = GetComponentInChildren<PPGameobjectsPool>();
        _scrollManager = GetComponentInChildren<ScrollManager>();


        if (_liner == null || _pool == null || _scrollManager == null)
        { Debug.LogError("PuzzleManager couldn't find something."); }

        _selectedWordText.text = "";

        foreach (Scroll scroll in _scrolls)
        {
            scroll.SetSpellActivatedCallBack(OnWordActivation);
            scroll.AddSelectedWordChangedCallback(OnSelectedWordChanged);
        }
    }

    new void Start()
    {
        base.Start();
    }

    
    public void Load()
    {
        Debug.LogError("Method \"Load\" is deprecated!");
        _pentaPool.LoadPentagrams();
        NextScroll();
    }


    public override void Load(ILevel level)
    {
        Debug.LogFormat("Loading level {0}.", level.LevelID);

        _openRandomTip.Load(level.LevelID);
        _openRandomWordTip.Load(level.LevelID);
        _openChosenCellTip.Load(level.LevelID);
        
        LevelToNormalState();

        if (level.PuzzleType != PuzzleType.Pentagram ||
            !(level is IPGLevel)
            )
        {
            Debug.LogErrorFormat(
                "PGPuzzle (PentagramPuzzle) was told to load a level with a different puzzle type: %s.",
                level.PuzzleType.ToString()
                );
        }

        gameObject.SetActive(true);
        IPGLevel pgLevel = (IPGLevel)level;

        _pentaPool.LoadPentagrams(pgLevel);
        NextScroll();
    }


    private void LevelToNormalState()
    {
        transform.GetChild(0).GetComponent<RectTransform>().transform.position = new Vector3(0, 0, 0);
    }

    public override void LoadOut()
    {
        _scrollManager.ReturnLettersToPool();
        gameObject.SetActive(false);
    }

    public override void SetWordActivationCallback(Action<string, List<ScorredLetter>> callback)
    {
        _onWordActivation = null;
        _onWordActivation += callback;
    }

    public override void AddExtraActionCallback(Action<object> callback)
    {
        _nextScrollCallback = null;
        _nextScrollCallback += callback;
    }

    public void OnWordActivation(string word, List<ScorredLetter> chosenLetters)
    {
        _onWordActivation(word, chosenLetters);
        //SetupRewardAnimation();
    }

    private void OnSelectedWordChanged (string word, List<ScorredLetter> scorredLetters)
    {
        ToNormalState();
        if (!"".Equals(word))
        {
            _selectedWordText.text = word.ToUpper();
        }
    }

    public void NextScroll()
    {
        Debug.Log("PuzzleManager: NextScroll");
        Pentagram newPentagram = _pentaPool.GetNextPentagram();
        _scrollManager.ChangeScroll(newPentagram, DirectionEnum.Downwards);
        if (_pentaPool.OutOfPentagrams()) { _changeScrollButton.SetActive(false); }


        _pentagramsLeftText.text = _pentaPool.PentagramsLeft() + " свитков осталось.";

        _nextScrollCallback(newPentagram);
    }

    /// <summary>
    /// Govnocode Bad Code Remake this Refactor required
    /// </summary>
    /// <param name="number"></param>
    public void SetScroll(int number)
    {
        Debug.Log("PuzzleManager: SetScroll");
        Pentagram newPentagram = _pentaPool.GetPentagram(number);
        _scrollManager.ChangeScroll(newPentagram, DirectionEnum.Downwards);
        if (_pentaPool.OutOfPentagrams()) { _changeScrollButton.SetActive(false); }

        _pentagramsLeftText.text = _pentaPool.PentagramsLeft() + " свитков осталось.";

        _nextScrollCallback(newPentagram);
    }


    public IEnumerator EmulateWordActivation(string word)
    {
        Scroll activeScroll = _scrollManager.ActiveScroll;
        yield return StartCoroutine(activeScroll.SelectWord(word));

        //if (word != activeScroll.GetSelectedWord())
        //{
        //    Debug.LogError("Somehow word \"" + word + "\" couldn't have been selected in a pentagram, but the Mage doesn't give a fuck");
        //    yield break;
        //}

        //activeScroll.GetPentagram().TryToUseWord(word);   //TryToUseWord(word) is a method of SmartPentagram

        activeScroll.UnselectLetters();
        _liner.Clear(null);
        _onWordActivation(word, null);
    }
    

    private SpellEffect EffectOfSequence(int wordLength, int[] sequence)
    {
        // Less than 3 letters -> Stun
        if (wordLength <= 2) return SpellEffect.Stun;


        //Cycle -> Shield
        for (int i = 0; i < wordLength; i++)
        {
            for (int j = i + 1; j < wordLength; j++)
            {
                if (sequence[i] == sequence[j])
                {
                    Debug.Log("SHIELD!");
                    return SpellEffect.Shield;
                }
            }
        }

        //None -> None
        return SpellEffect.None;
    }


    public override void SetupAnimation(bool checker, int AnimationType)
    {
        Debug.Log("DOTweenCheck: PentaPuzzle: Setup Aimation");
        switch (AnimationType)
        {
            case 0:
                {
                    if (checker)
                    {
                        _selectedWordText.material.DOColor(Color.clear, 0.8f).timeScale.Equals(0.5f);

                        //Animation Flying Word with Effects

                    }
                    else
                    {
                        _sequence = DOTween.Sequence();
                        _sequence.Insert(0f, _selectedWordText.material.DOColor(new Color(255, 0, 0, 255), 0f))
                            .Insert(0f, _selectedWordText.rectTransform.DOAnchorPos(new Vector2(30, 0), 0.15f))
                            .Append(_selectedWordText.rectTransform.DOAnchorPos(new Vector2(-30, 0), 0.15f))
                            .Append(_selectedWordText.rectTransform.DOAnchorPos(new Vector2(0, 0), 0.15f))
                            .Append(_selectedWordText.material.DOColor(Color.clear, 0.5f));
                    }
                    break;
                }
            case 1:
                {
                    this.transform.GetChild(0).GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, -2000), 0.5f);
                    break;
                }
            case 2:
                {
                    break;
                }
        }

    }

    public override void SetupRewardAnimationTwo()
    {
        Debug.Log("DOTweenCheck: PentaPuzzle SetupAnimationTwo");
        _animationText.color = new Color(1f,1f,1f,1f);
        _animationText.transform.localScale = new Vector3(1f,1f,1f);
        _animationText.DOColor(Color.cyan, 1f);
        _animationText.transform.DOScale(new Vector3(2f, 2f, 2f), 1f);
        _animationText.DOFade(0f, 2f);
        Debug.LogWarning("New Animation Type");
    }


    private void ToNormalState()
    {
        _selectedWordText.material.color = new Color(0, 0, 0, 100f);

    }

    public override void ExtraAction()
    {
        NextScroll();
    }

    public void ReshuffleLetters()
    {
        Debug.Log("Reshuffling letters");
        _scrollManager.ReshuffleLetters();
    }
}
