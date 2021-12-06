using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractReactor : MonoBehaviour, IReactor
{
    [SerializeField] private GameObject _blockingPannel = null;

    protected Sequence _animationsSequence;

    protected HashWordBook _dictionary;
    protected HashSet<string> _selectedDictionaryWords;

    public Sequence AnimationsSequence => _animationsSequence;

    public abstract ReactorType ReactorType { get; }
    public abstract int NewExtraWordsFound { get; }
    public abstract void ActivateWord(string word, List<ScorredLetter> scorredLetters);
    public abstract void OnPuzzleExtraAction(System.Object extraActionParameters);
    public abstract void Load(ILevel level);
    public abstract void LoadOut();
    public abstract void SetTaskFinishedCallback(Action onTaskFinished);
    public abstract void SetDefeatCallback(Action onDefeat);
    public abstract void SetWordActivatedCallback(Action<string, List<ScorredLetter>> callback);
    public abstract void SetWordAnimationCallback(Action<bool, int> wordAnimationCallback);
    public abstract void SetWordAnimationCallback(Action flyingwordAnimationCallback);

    protected void Awake()
    {
        _selectedDictionaryWords = new HashSet<string>();
    }

    protected void Start()
    {
        _dictionary = UserOptions.Instance.WordBook;
        _blockingPannel.SetActive(false);
    }

    public abstract void PlayAnimationEndLevel();


    private IDormantCaller _dormantCaller;

    /// <summary>
    /// For the puzzles going dormant means activating a once-clickable panel,
    /// which calls "DormantWakeUp" on click.
    /// 
    /// PUZZLE MAY HAVE ONLY ONE DORMANT CALLER!
    /// </summary>
    /// <param name="dormantCaller"></param>
    public void GoDormant(IDormantCaller dormantCaller)
    {
        if (null != _dormantCaller)
        {
            Debug.LogError("A trial detected of making a dormant puzzle dormant!");
            return;
        }

        if (null == dormantCaller)
        {
            Debug.LogError("Detected a call of GoDormant with \"null\" parameter!");
            return;
        }

        if (null == _blockingPannel)
        {
            Debug.LogError("Blocking pannel was not assigned to the puzzle.");
        }

        _dormantCaller = dormantCaller;
        _blockingPannel.SetActive(true);
    }

    /// <summary>
    /// Notifies a DormantCaller of being woken up, disables BlockingPanel, and
    /// removes the saved refference to DormantCaller.
    /// </summary>
    public void DormantWakeUp(bool notifyCaller)
    {
        if (null != _dormantCaller)
        {
            Debug.LogError("A trial detected of making a dormant puzzle dormant!");
            return;
        }

        if (null == _blockingPannel)
        {
            Debug.LogError("Blocking pannel was not assigned to the puzzle.");
        }

        if (notifyCaller) { _dormantCaller.NotDormantNotify(this); }
        _dormantCaller = null;
        _blockingPannel.SetActive(false);
    }
}
