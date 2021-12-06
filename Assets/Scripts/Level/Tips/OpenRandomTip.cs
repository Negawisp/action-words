using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenRandomTip : AbstractTip
{
    [SerializeField] CWReactor _cWReactor = null;

    protected override UserOptions.IntPlayerPref TipUnlockedPlayerPref => UserOptions.Instance.OpenRandomLetterTipIsUnlocked;

    //[SerializeField] int _initialTipsNumber = 0;


    public void OpenRandomCells(int numberOfCells)
    {
        if (null == _cWReactor)
        {
            Debug.LogErrorFormat("OpenRandomTip attached to gameobject {0} didn't have a CWReactor attached to it!",
                gameObject.name);
            return;
        }

        if (UserOptions.Instance.OpenRandomLettersTipsNumber.Value <= 0)
        {
            if (_buyingManager.BuyRandomLetterTip())
                Debug.Log("The tip was added");
            else
                return;
        }

        CWCell[,] cellsArray = _cWReactor.GetGrid().GetCells();
        ArrayList closedCellsList = new ArrayList(cellsArray.Length);
        foreach (CWCell cell in cellsArray)
        {
            if (null != cell && !cell.IsOpened)
            {
                closedCellsList.Add(cell);
            }
        }
        for (int i = 0; i < numberOfCells; i++)
        {
            if (closedCellsList.Count == 0) { break; }
            int randomN = UnityEngine.Random.Range(0, closedCellsList.Count);
            CWCell randomCell = (CWCell)closedCellsList[randomN];
            _cWReactor.OpenCell(randomCell);
            closedCellsList.RemoveAt(randomN);
        }

        UserOptions.Instance.OpenRandomLettersTipsNumber.Value--;
        UpdateHintsLeft();
    }

    protected override void UpdateHintsLeft()
    {
        _hintsLeftText.text = UserOptions.Instance.OpenRandomLettersTipsNumber.Value.ToString();
        ShopButton.gameObject.SetActive(UserOptions.Instance.OpenRandomLettersTipsNumber.Value <= 0);
    }
}
