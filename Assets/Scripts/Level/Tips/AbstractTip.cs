using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A script to describe behavior of a tip - a gameplay element to help player to complete a level easier.
/// 
/// Particularly, this script describes
///     - How to introduce the tip to a player,
///     - When to enable the tip on its load and when to not,
///     - How the tip is related to gold-spending, for instance by PurchaseManager.
/// </summary>
public abstract class AbstractTip : MonoBehaviour
{
    [SerializeField] protected int _introductionLevel = -1;
    [SerializeField] protected bool _introduceOnlyOnce = false;
    [SerializeField] protected Text _hintsLeftText = null;
    [SerializeField] protected BaseDialogueScreen _introductionDialogue = null;
    [SerializeField] protected BuyingManager _buyingManager = null;
    [SerializeField] protected Button ShopButton = null;

    abstract protected UserOptions.IntPlayerPref TipUnlockedPlayerPref { get; }
    
    abstract protected void UpdateHintsLeft();


    public virtual void Load(int levelId)
    {
        if (levelId == _introductionLevel)
        {
            Introduce();
        }
        else
        {
            _introductionDialogue.gameObject.SetActive(false);
        }

        if (TipUnlockedPlayerPref.Value < 0) {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
        
        UpdateHintsLeft();
    }

    protected virtual Sequence Introduce()
    {
        if (null == _introductionDialogue)
        {
            Debug.LogErrorFormat("Tip of type {0} didn't have an intro to show!", this.GetType().Name);
            return null;
        }

        Sequence sequence = DOTween.Sequence();

        if (!_introduceOnlyOnce || TipUnlockedPlayerPref.Value < 0)
        {
            TipUnlockedPlayerPref.Value = 1;
            sequence.AppendCallback(() => _introductionDialogue.Show());
        }

        return sequence;
    }
}
