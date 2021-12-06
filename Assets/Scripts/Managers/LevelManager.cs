using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class LevelManager : MonoBehaviour, ILevelManager
{
    [SerializeField] private AbstractReactor[] _reactors = null;
    [SerializeField] private AbstractPuzzle[] _puzzles = null;
    [SerializeField] private AbstractLevel[] _allLevels = null;

    public Dictionary<int, AbstractLevel> Levels { get; private set; }
    public AbstractLevel CurrentLevel => _currentLevel;

    private Dictionary<PuzzleType, AbstractPuzzle> _puzzlesDictionary;
    private Dictionary<ReactorType, AbstractReactor> _reactorsDictionary;
    private AbstractPuzzle _currentPuzzle;
    private AbstractReactor _currentReactor;
    private AbstractLevel _currentLevel;

    private Action _onPuzzleFinished;
    private Action _onDefeat;

    public AbstractPuzzle CurrentPuzzle => _currentPuzzle;
    public AbstractReactor CurrentReactor => _currentReactor;

    /// <summary>
    /// Adds an external method to be called on an event "LevelFinished"
    /// </summary>
    /// <param name="onCrosswordFinished"></param>
    public void AddPuzzleFinishedCallback(Action callback)
    {
        _onPuzzleFinished += callback;
    }

    public void AddDefeatCallback(Action callback)
    {
        _onDefeat += callback;
    }

    private void Start()
    {
        if (null == _puzzles || 0 == _puzzles.Length) Debug.LogError("Please, serialize Puzzles of LevelManager in a scene editor.");
        if (null == _reactors || 0 == _reactors.Length) Debug.LogError("Please, serialize Reactors of LevelManager in a scene editor.");

        _puzzlesDictionary = new Dictionary<PuzzleType, AbstractPuzzle>();
        _reactorsDictionary = new Dictionary<ReactorType, AbstractReactor>();

        foreach (AbstractPuzzle puzzle in _puzzles) {
            _puzzlesDictionary.Add(puzzle.PuzzleType, puzzle);
        }
        foreach (AbstractReactor reactor in _reactors) {
            _reactorsDictionary.Add(reactor.ReactorType, reactor);
        }

        Levels = new Dictionary<int, AbstractLevel>();
        foreach(AbstractLevel level in _allLevels)
        {
            Levels.Add(level.LevelID, level);
        }
    }

    /// <summary>
    /// Switches on the level indicated with <i>levelNumber</i> parameter.
    /// </summary>
    /// <param name="levelNumber">Number of a level to switch to</param>
    public AbstractLevel LoadLevel(int levelNumber)
    {
        foreach (AbstractReactor reactor in _reactorsDictionary.Values) {
            reactor.gameObject.SetActive(false);
        }
        foreach (AbstractPuzzle puzzle in _puzzlesDictionary.Values) {
            puzzle.gameObject.SetActive(false);
        }

        _currentLevel = Levels[levelNumber];
        Debug.LogFormat("Current level number: {0}. PuzzleType: {1}. ReactorType: {2}",
            levelNumber, _currentLevel.PuzzleType.ToString(), _currentLevel.ReactorType.ToString());
        ///\\\
        _currentPuzzle = _puzzlesDictionary[_currentLevel.PuzzleType];
        _currentReactor = _reactorsDictionary[_currentLevel.ReactorType];


        _currentPuzzle.SetWordActivationCallback(_currentReactor.ActivateWord);
        _currentPuzzle.AddExtraActionCallback(_currentReactor.OnPuzzleExtraAction);
        _currentReactor.SetTaskFinishedCallback(_onPuzzleFinished);
        _currentReactor.SetDefeatCallback(_onDefeat);
        
        _currentReactor.SetWordAnimationCallback(_currentPuzzle.SetupAnimation);
        _currentReactor.SetWordAnimationCallback(_currentPuzzle.SetupRewardAnimationTwo);

        
        SetCurPuzzleActive(true);
        SetCurReactorActive(true);
        FindObjectOfType<ImbecilMobileLogger>().Log("1");
        _currentPuzzle.Load(_currentLevel);
        //FindObjectOfType<ImbecilMobileLogger>().Log("8");
        _currentReactor.Load(_currentLevel);
        //FindObjectOfType<ImbecilMobileLogger>().Log("24");
        return _currentLevel;
    }

    public void LoadLevelOut()
    {
        Debug.LogFormat("Loading level {0} out", _currentLevel.LevelID);
        _currentLevel = null;
        _currentPuzzle.LoadOut();
        _currentReactor.LoadOut();
    }

    /// <summary>
    /// Also tells if the crossword is finished
    /// </summary>
    /// <param name="word"></param>
    private void ActivateWord(string word)
    {
        
    }


    private void LoadPuzzle()
    {

    }

    private void LoadReactor()
    {

    }

    void ILevelManager.LoadLevel(int levelNumber)
    {
        throw new NotImplementedException();
    }

    public void SkipAnimation()
    {
        Debug.Log("DOTweenCheck: LevelManager: SkipAnimation");
        _currentReactor.AnimationsSequence.DOTimeScale(20, 0);
    }

    public void RollLevelDown()
    {
        _currentPuzzle.gameObject.SetActive(false);
        _currentReactor.gameObject.SetActive(false);
    }

    public void RollLevelUp()
    {
        _currentPuzzle.gameObject.SetActive(true);
        _currentReactor.gameObject.SetActive(true);
        gameObject.SetActive(true);
    }

    public void SetCurPuzzleActive(bool param)
    {
        _currentPuzzle.gameObject.SetActive(param);
    }

    public void SetCurReactorActive(bool param)
    {
        _currentReactor.gameObject.SetActive(param);
    }

    public void HideReactorsAndPuzzles()
    {
        foreach (AbstractReactor reactor in _reactors)
        {
            reactor.gameObject.SetActive(false);
        }
        foreach (AbstractPuzzle puzzle in _puzzles)
        {
            puzzle.gameObject.SetActive(false);
        }
    }

}
