using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPuzzle : MonoBehaviour, IPuzzle
{
    [SerializeField] protected GameObject _blockingPannel = null;

    public abstract PuzzleType PuzzleType { get; }

    public abstract void AddExtraActionCallback(Action<System.Object> callback);
    public abstract void SetWordActivationCallback(Action<string, List<ScorredLetter>> callback);
    public abstract void ExtraAction();
    public abstract void Load(ILevel level);
    public abstract void LoadOut();
    public abstract void SetupAnimation(bool checker, int AnimationType);
    public abstract void SetupRewardAnimationTwo();


    private IDormantCaller _dormantCaller;

    protected void Start()
    {
        _blockingPannel.SetActive(false);
    }

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
    public void DormantWakeUp(bool notifyDormant)
    {
        if (null == _dormantCaller)
        {
            Debug.LogError("Detected a dormant object without a caller.");
            return;
        }

        if (null == _blockingPannel)
        {
            Debug.LogError("Blocking pannel was not assigned to the puzzle.");
        }

        if (notifyDormant) { _dormantCaller.NotDormantNotify(this); }
        _dormantCaller = null;
        _blockingPannel.SetActive(false);
    }
}
