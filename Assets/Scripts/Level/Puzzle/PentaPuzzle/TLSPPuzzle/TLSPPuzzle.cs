using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// TLSPPuzzle is an acronym for Turn-Limited Scored Penta Puzzle.
/// The goal of a player is to collect set ammount of score in set ammount of turns.
/// </summary>
public class TLSPPuzzle : AbstractPuzzle
{
    private Action<string, List<ScorredLetter>> _onWordActivation;
    private Action<object> _changeScrollCallback;

    [SerializeField] private AbstractOptionalButtonAnimatorManager _changeScrollForwardButton = null;
    [SerializeField] private AbstractOptionalButtonAnimatorManager _changeScrollBackwardButton = null;
    [SerializeField] private Text _selectedWordText = null;
    [SerializeField] private Text _selectedScoreText = null;
    [SerializeField] private Scroll[] _scrolls = null;

    private PentaPool _pentaPool;
    private PPGameobjectsPool _pool;
    private ScrollManager _scrollManager;
    private Liner _liner;
    private Sequence _sequence;
    private int _currentPentagramNumber;

    public override PuzzleType PuzzleType => PuzzleType.TLSP;

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

    public override void Load(ILevel level)
    {
        Debug.LogFormat("Loading level {0}.", level.LevelID);
        FindObjectOfType<ImbecilMobileLogger>().Log("2");
        LevelToNormalState();
        FindObjectOfType<ImbecilMobileLogger>().Log("3");
        _currentPentagramNumber = 0;

        if (level.PuzzleType != PuzzleType.TLSP ||
            !(level is ITLSPLevel)
            )
        {
            Debug.LogErrorFormat(
                "TSLPPuzzle (Turn-Limited Scorred Penta Puzzle) was told to load a level with a different puzzle type: {0}.",
                level.PuzzleType.ToString()
                );
        }FindObjectOfType<ImbecilMobileLogger>().Log("4");

        ITLSPLevel pgLevel = (ITLSPLevel)level;FindObjectOfType<ImbecilMobileLogger>().Log("5");
        _pentaPool.LoadPentagrams(pgLevel);FindObjectOfType<ImbecilMobileLogger>().Log("6");
        ChangeScroll(DirectionEnum.Default);FindObjectOfType<ImbecilMobileLogger>().Log("7");
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
        Debug.LogWarning("Callback is null: " + (null == callback));
        _onWordActivation = null;
        _onWordActivation += callback;
    }

    public override void AddExtraActionCallback(Action<object> callback)
    {
        _changeScrollCallback = null;
        _changeScrollCallback += callback;
    }

    public void OnWordActivation(string word, List<ScorredLetter> letters)
    {
        _onWordActivation(word, letters);
        //SetupRewardAnimation();
    }

    private void OnSelectedWordChanged(string word, List<ScorredLetter> scorredLetters)
    {
        ToNormalState();
        if (!"".Equals(word))
        {
            _selectedWordText.text = word.ToUpper();

            int scoreGained = 0;
            foreach (ScorredLetter scorredLetter in scorredLetters)
            {
                scoreGained += scorredLetter.Score;
            }
            foreach (ScorredLetter scorredLetter in scorredLetters)
            {
                scoreGained *= scorredLetter.Multiplier;
            }

            _selectedScoreText.text = scoreGained.ToString();
        }
    }

    /// <summary>
    /// Changes pentagram to the next or a previous one.
    /// </summary>
    /// <param name="direction">
    ///         Defines the type of animation of scroll change and
    ///         whether to change the scroll to a next or to a previous one.
    /// </param>
    public void ChangeScroll(DirectionEnum direction)
    {
        Debug.Log("TLSPPuzzle Manager: ChangeScroll");
        switch (direction)
        {
            case DirectionEnum.ToTheRight:
                {
                    _currentPentagramNumber++;
                } break;
            case DirectionEnum.ToTheLeft:
                {
                    _currentPentagramNumber--;
                } break;
            case DirectionEnum.Default:
                {
                    _currentPentagramNumber++;
                } break;
            default:
                {
                    Debug.LogWarningFormat(
                        "Illegal parameter for method ChangeScroll.",
                        DirectionEnum.ToTheRight.ToString(), DirectionEnum.ToTheLeft.ToString());
                    return;
                }
        }

        while (_currentPentagramNumber <  0) {
            _currentPentagramNumber = _currentPentagramNumber += _pentaPool.PentagramsLeft();
        }
        while (_currentPentagramNumber >= _pentaPool.PentagramsLeft()) {
            _currentPentagramNumber = _currentPentagramNumber -= _pentaPool.PentagramsLeft();
        }

        FindObjectOfType<ImbecilMobileLogger>().Log("ChangeScroll 1");
        Pentagram newPentagram = _pentaPool.GetPentagram(_currentPentagramNumber);
        
        Debug.LogWarning("TLSPPuzzle ChangeScroll almost ended...");

        _blockingPannel.SetActive(true);

        _changeScrollBackwardButton.SwitchAppear(_currentPentagramNumber > 0);
        _changeScrollForwardButton.SwitchAppear(_currentPentagramNumber < _pentaPool.PentagramsLeft() - 1);

        _scrollManager.ChangeScroll(newPentagram, direction)
            .AppendInterval(0.2f)
            .AppendCallback(() => {
                _blockingPannel.SetActive(false);
                _changeScrollCallback(newPentagram);
            });
        
        Debug.LogWarning("TLSPPuzzle ChangeScroll ended.");
    }

    /// <summary>
    /// A wrapper of a previous method to use with buttons on scene:
    /// monobehaviours can be button parameters while enums cannot.
    /// </summary>
    /// <param name="direction">A monobehavior wrapping required enum value.</param>
    public void ChangeScroll(DirectionEnumParameter direction)
    {
        ChangeScroll(direction.value);
    }


    public override void SetupAnimation(bool checker, int AnimationType)
    {
        Debug.LogWarning("DOTweenCheck: TLSPPuzzle: SetupAnimation");
        switch (AnimationType)
        {
            case 0:
                {
                    if (checker)
                    {
                        _selectedWordText.material.DOColor(Color.clear, 0.8f).timeScale.Equals(0.5f);
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
        return;
    }


    private void ToNormalState()
    {
        _selectedWordText.material.color = new Color(0, 0, 0, 100f);
    }


    public override void ExtraAction()
    {
        return;
    }

    public void ReshuffleLetters()
    {
        Debug.Log("Reshuffling letters");
        _scrollManager.ReshuffleLetters();
    }
}
