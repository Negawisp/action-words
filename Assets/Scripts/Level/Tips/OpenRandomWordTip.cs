using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenRandomWordTip : AbstractTip
{
    [SerializeField] private CWReactor _cwReactor = null;

    protected override UserOptions.IntPlayerPref TipUnlockedPlayerPref => UserOptions.Instance.OpenRandomWordTipIsUnlocked;

    //[SerializeField] int _initialTipsNumber = 0;

    private void Start()
    {
        
    }

    public void OpenRandomWord()
    {
        if (UserOptions.Instance.OpenRandomWordTipsNumber.Value <= 0)
        {
            return;
        }

        var cwWordsCollection = _cwReactor.GetCurCrossword().CWdict.Values;
        ArrayList wordsList = new ArrayList(cwWordsCollection.Count);
        foreach (CWword word in _cwReactor.GetCurCrossword().CWdict.Values)
        {
            if (!word.Opened)
            {
                wordsList.Add(word.Word);
            }
        }

        int wordsN = wordsList.Count;
        if (wordsN > 0)
        {
            int randomWordN = UnityEngine.Random.Range(0, wordsN);
            string randomWord = (string)wordsList[randomWordN];
            _cwReactor.ActivateWord(randomWord, null);
        }
        UserOptions.Instance.OpenRandomWordTipsNumber.Value--;
        UpdateHintsLeft();
    }

    protected override void UpdateHintsLeft()
    {
        _hintsLeftText.text = UserOptions.Instance.OpenRandomWordTipsNumber.Value.ToString();
        ShopButton.gameObject.SetActive(UserOptions.Instance.OpenRandomWordTipsNumber.Value <= 0);
    }
}
