using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to asign to a button activating "Open Chosen Cell" tip.
/// Requires CWCell class to have static "CellClicked" callback.
/// </summary>
public class OpenChosenCellTip : AbstractTip, IDormantCaller
{
    [SerializeField] CWReactor _cWReactor = null;
    [SerializeField] PentaPuzzle _pentaPuzzle = null;

    protected override UserOptions.IntPlayerPref TipUnlockedPlayerPref => UserOptions.Instance.OpenChosenCellTipIsUnlocked;

    //[SerializeField] int _initialTipsNumber = 0;


    public void MakeDormant(IDormantable dormantable)
    {
        dormantable.GoDormant(this);
    }

    public void NotDormantNotify(IDormantable dormantable)
    {
        DisableOpenChosenCellTipMode();
    }

    /// <summary>
    /// Puts necessary clickables to be dormant and sets CellClickedCallback of CWCell class
    /// to be "OpenCell()" method.
    /// </summary>
    public void EnableOpenChosenCellTipMode ()
    {
        if (UserOptions.Instance.OpenChosenLetterTipsNumber.Value <= 0)
        {
            return;
        }

        Debug.Log("Enabling \"Open Chosen Cell\" tip");
        CWCell.SetCellClickedCallback(OpenCell);
        MakeDormant(_pentaPuzzle);
    }

    /// <summary>
    /// Wakes up all objects the tip made dormant and disables CellClickedCallback of CWCell class.
    /// </summary>
    private void DisableOpenChosenCellTipMode ()
    {
        Debug.Log("Disabling \"Open Chosen Cell\" tip");
        CWCell.RemoveCellClickedCallback(OpenCell);
        _pentaPuzzle.DormantWakeUp(false);
    }

    /// <summary>
    /// This method is given as an abstact CellClickedCallback to CWCell class to be called
    /// upon cell being presseed.
    /// 
    /// The cell will be opened, the callback will be removed from class, and
    /// all dormant objects will be woken up.
    /// </summary>
    /// <param name="cell"></param>
    private void OpenCell (CWCell cell)
    {
        if (null == _cWReactor)
        {
            Debug.LogErrorFormat("OpenChosenCellTip attached to gameobject {0} didn't have a CWReactor attached to it!",
                gameObject.name);
            return;
        }
        if (null == _pentaPuzzle)
        {
            Debug.LogErrorFormat("OpenChosenCellTip attached to gameobject {0} didn't have a pentaPuzzle attached to it!",
                gameObject.name);
            return;
        }

        DisableOpenChosenCellTipMode();
        _cWReactor.OpenCell(cell);

        UserOptions.Instance.OpenChosenLetterTipsNumber.Value--;
        UpdateHintsLeft();
    }

    protected override void UpdateHintsLeft()
    {
        _hintsLeftText.text = UserOptions.Instance.OpenChosenLetterTipsNumber.Value.ToString();
    }
}
