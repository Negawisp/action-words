using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseScoreReactor : AbstractReactor
{
    [SerializeField] private float _fadingTime = 0;
    [SerializeField] private GameObject[] _animatableReactorGameObjects = null;
    [SerializeField] private BaseScoreIndicator _scoreIndicator = null;
    [SerializeField] private TurnsLeftIndicator _turnsLeftIndicator = null;

    public override ReactorType ReactorType => ReactorType.ScoreTest;

    public override int NewExtraWordsFound => 0;

    private int _turnsLeft;
    private int _currentScore;
    private int _scoreGoal;

    private Action _OnTaskFinished;
    private Action _OnDefeat;
    private Action<bool, int> _PuzzleAnimationCallback;
    private Action _FlyingWordAnimationCallback;

    new protected void Awake()
    {
        base.Awake();
    }

    new protected void Start()
    {
        base.Start();
    }

    public override void Load(ILevel level)
    {
        if (!(level is IBaseScoreLevel))
        {
            Debug.LogErrorFormat(
                "ScoreTextReactor was told to load a level with a different reactor type: {0}.",
                level.ReactorType.ToString()
                );
        }//FindObjectOfType<ImbecilMobileLogger>().Log("10");

        IBaseScoreLevel sTLevel = (IBaseScoreLevel)level;//FindObjectOfType<ImbecilMobileLogger>().Log("11");
        _scoreGoal = sTLevel.ScoreGoal;//FindObjectOfType<ImbecilMobileLogger>().Log("12");
        _currentScore = 0;//FindObjectOfType<ImbecilMobileLogger>().Log("13");
        _turnsLeft = sTLevel.NumberOfTurns;//FindObjectOfType<ImbecilMobileLogger>().Log("14");
        _selectedDictionaryWords.Clear();//FindObjectOfType<ImbecilMobileLogger>().Log("15");
        _scoreIndicator.SetScoreGoal(_scoreGoal);//FindObjectOfType<ImbecilMobileLogger>().Log("16");
        _scoreIndicator.SetScore(0);//FindObjectOfType<ImbecilMobileLogger>().Log("17");
        _turnsLeftIndicator.SetTurnsNumber(_turnsLeft);//FindObjectOfType<ImbecilMobileLogger>().Log("19");
    }

    public override void LoadOut()
    {
        gameObject.SetActive(false);
        _selectedDictionaryWords.Clear();
    }

    private bool WordIsValid (string word)
    {
        return (!_selectedDictionaryWords.Contains(word) && _dictionary.Contatins(word));
    }

    public override void ActivateWord(string word, List<ScorredLetter> scorredLetters)
    {
        int scoreGained = 0;
        if (WordIsValid(word))
        {
            foreach (ScorredLetter scorredLetter in scorredLetters)
            {
                scoreGained += scorredLetter.Score;
            }
            foreach (ScorredLetter scorredLetter in scorredLetters)
            {
                scoreGained *= scorredLetter.Multiplier;
            }

            _currentScore += scoreGained;
            _scoreIndicator.SetScore(_currentScore);
            _selectedDictionaryWords.Add(word);
            _turnsLeftIndicator.SetTurnsNumber(--_turnsLeft);
            _PuzzleAnimationCallback(true, 0);
        } else
        {
            _PuzzleAnimationCallback(false, 0);
        }
        if (_currentScore >= _scoreGoal)
        {
            _OnTaskFinished();
            PlayAnimationEndLevel();
        }
        else if (_turnsLeft <= 0)
        {
            _OnDefeat();
        }
    }

    

    public override void OnPuzzleExtraAction(object extraActionParameters)
    {
        return;
    }

    public override void PlayAnimationEndLevel()
    {
        Debug.Log("DOTweenCheck: BaseScoreReactor: PlayAnimationEndLevel");

        gameObject.SetActive(false);
    }

    public override void SetTaskFinishedCallback(Action onTaskFinished)
    {
        _OnTaskFinished = onTaskFinished;
    }

    public override void SetDefeatCallback(Action onDefeat)
    {
        _OnDefeat = onDefeat;
    }

    public override void SetWordActivatedCallback(Action<string, List<ScorredLetter>> callback)
    {
        throw new NotImplementedException();
    }

    public override void SetWordAnimationCallback(Action<bool, int> wordAnimationCallback)
    {
        Debug.Log("Word animation callback is set.");
        _PuzzleAnimationCallback = wordAnimationCallback;
    }

    public override void SetWordAnimationCallback(Action flyingwordAnimationCallback)
    {
        _FlyingWordAnimationCallback = flyingwordAnimationCallback;
    }
}
