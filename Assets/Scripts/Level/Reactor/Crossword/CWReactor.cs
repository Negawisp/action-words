using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CWReactor : AbstractReactor
{
    [SerializeField] private CrosswordGrid _grid = null;
    [SerializeField] private float _fadingTime = 0;

    private ICWLevel _curLevel;
    private ICrossword _curCrossword;
    private int _mainWordsFound;
    private int _newExtraWordsFound;
    public override int NewExtraWordsFound => _newExtraWordsFound;

    private Action _OnTaskFinished;
    private Action _OnDefeat;
    private Action<bool, int> _PuzzleAnimationCallback;
    private Action _FlyingWordAnimationCallback;

    public override ReactorType ReactorType => ReactorType.Crossword;

    public CrosswordGrid GetGrid() => _grid;
    public ICrossword GetCurCrossword() => _curCrossword;

    new protected void Awake()
    {
        base.Awake();
    }

    new protected void Start()
    {
        base.Start();
        _animationsSequence = DOTween.Sequence();
        _animationsSequence.SetAutoKill(true);
    }

    public override void SetWordAnimationCallback(Action flyingwordAnimationCallback)
    {
        _FlyingWordAnimationCallback = flyingwordAnimationCallback;
    }
    public override void SetWordAnimationCallback(Action<bool, int> wordAnimationCallback)
    {
        _PuzzleAnimationCallback = wordAnimationCallback;
    }

    public override void SetTaskFinishedCallback(Action onTaskFinished)
    {
        _OnTaskFinished = onTaskFinished;
    }
    public void OnTaskFinished()
    {
        _OnTaskFinished();
        UserOptions.Instance.SaveExtraWords(_curLevel, _selectedDictionaryWords);
    }

    public override void SetDefeatCallback(Action onDefeat)
    {
        _OnDefeat = onDefeat;
    }

    public override void Load(ILevel level)
    {
        if (level.ReactorType != ReactorType.Crossword ||
            !(level is ICWLevel)
            )
        {
            Debug.LogErrorFormat(
                "CWReactor (CrosswordReactor) was told to load a level with a different reactor type: %s.",
                level.ReactorType.ToString()
                );
        }

        _curLevel = (ICWLevel)level;

        _mainWordsFound = 0;
        _newExtraWordsFound = 0;
        gameObject.SetActive(true);
        _grid.ReturnCellsToPool();
        UserOptions.Instance.LoadExtraWords(_curLevel, out _selectedDictionaryWords);
        //_curCrossword = _locationCrosswords[levelNumber];
        _curCrossword = _curLevel.Crossword;
        _grid.BuildCrossword(_curCrossword.GetCrosswordTable(), '\0');
    }

    public override void LoadOut()
    {
        gameObject.SetActive(false);
        _grid.ReturnCellsToPool();
        foreach (CWword word in _curCrossword.CWdict.Values)
        {
            word.Opened = false;
        }
    }

    protected bool WordIsInDictionary(string word)
    {
        return (_dictionary.Contatins(word));
    }

    public override void ActivateWord(string word, List<ScorredLetter> scorredLetters)
    {
        Debug.Log("DOTweenCheck: CWReactor: ActivateWord");
        _animationsSequence.DOTimeScale(10, 0);
        _animationsSequence = DOTween.Sequence();
        CWword cwWord = _curCrossword.GetCWword(word);

        Debug.LogFormat("Word {0} is in crossword: {1}", word, (null != cwWord));
        if (null == cwWord)
        {
            Debug.Log("No such word in the crossword");
            if (WordIsInDictionary(word))
            {
                if (!_selectedDictionaryWords.Contains(word)) {
                    Debug.LogFormat("But \"{0}\" is a new extra word!", word);
                    _selectedDictionaryWords.Add(word);
                    _PuzzleAnimationCallback(true, 0);
                    _newExtraWordsFound++;
                    return;
                }
                else
                {
                    Debug.Log("The EXTRA word is already opened.");
                }
            }
            else
            {
                Debug.Log("The word doesn't exist in a dictionary.");
            }
            _PuzzleAnimationCallback(false, 0);
            return;
        }
        else if (cwWord.Opened)
        {
            Debug.Log("The word is already opened.");
            _PuzzleAnimationCallback(false, 0);
            return;
        }

        _PuzzleAnimationCallback(true, 0);

        int x = cwWord.X;
        int y = cwWord.Y;
        CWword.CWDirection dir = cwWord.Direction;
        _grid.OpenWord(x, y, dir);
        _animationsSequence.PrependInterval(_grid.AnimationTimeToOpen(cwWord.Word));
        cwWord.Opened = true;
        CheckAndRegisterOpenedWords();

        ++_mainWordsFound;
        if (_mainWordsFound == _curCrossword.CWdict.Count - 1)
        {
            _FlyingWordAnimationCallback();
        }
        if (_mainWordsFound == _curCrossword.CWdict.Count)
        {
            _animationsSequence.AppendCallback(PlayAnimationEndLevel)
                .AppendCallback(() => _PuzzleAnimationCallback(false, 1)) //1 means FinishAnimationPuzzle 
                .AppendInterval(_fadingTime)
                .AppendCallback(() => OnTaskFinished());
        }
    }

    /// <summary>
    /// Supports any puzzles and any extra actions because doesn't react to the at all.
    /// </summary>
    /// <param name="dull"></param>
    public override void OnPuzzleExtraAction(object dull)
    {
        return;
    }

    public override void SetWordActivatedCallback(Action<string, List<ScorredLetter>> callback)
    {
        throw new NotImplementedException();
    }


    public override void PlayAnimationEndLevel()
    {
        Debug.Log("DOTweenCheck: CWReactor: PlayAnimationEndLevel");
        var childCount = _grid.transform.childCount - 1;
        while (childCount >= 0)
        {
            _grid.transform.GetChild(childCount).GetComponent<Image>().DOFade(0f, _fadingTime);
            _grid.transform.GetChild(childCount).GetChild(0).GetComponent<Text>().DOFade(0f, _fadingTime);
            childCount--;
        }
    }


    /// <summary>
    /// Checks if in crossword there are opened words that are not
    /// yet considered opened by the game, and registers them as opened
    /// so that a player can't open them the second time.
    /// </summary>
    /// <returns>Number of newly opened words</returns>
    public int CheckAndRegisterOpenedWords()
    {
        Debug.Log("Checking words");
        int openedWordsCount = 0;
        foreach (CWword word in _curCrossword.CWdict.Values)
        {
            Debug.LogFormat("Checking word {0}", word.Word);
            if (!word.Opened &&
                _grid.CheckWordOpened(word.X, word.Y, word.Direction))
            {
                Debug.LogFormat("Word {0} was not activated.", word.Word);
                ActivateWord(word.Word, null);
                Debug.LogFormat("Word {0} has been activated.", word.Word);
                openedWordsCount++;
            }
        }
        return openedWordsCount;
    }

    public int OpenCell(CWCell cell)
    {
        _grid.OpenCell(cell, 0);
        return CheckAndRegisterOpenedWords();
    }

}
