using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AfterLevelDialogueScreen : BaseDialogueScreen
{
    [SerializeField] private GameObject _receivedGoldIndicator = null;
    [SerializeField] private AfterLevelVariantEnum _afterLevelVariant = AfterLevelVariantEnum.Nothing;
    public AfterLevelVariantEnum AfterLevelVariant { get => _afterLevelVariant; }

    public void Show(int earnedGold)
    {
        _receivedGoldIndicator.GetComponentInChildren<Text>().text = "" + earnedGold;
        base.Show();
    }

    public void ShowInstantly(int earnedGold)
    {
        _receivedGoldIndicator.GetComponentInChildren<Text>().text = "" + earnedGold;
        base.ShowInstantly();
    }

    public override void Hide()
    {
        _receivedGoldIndicator.GetComponentInChildren<Text>().text = "" + -1;
        base.Hide();
    }

    public override void HideInstantly()
    {
        _receivedGoldIndicator.GetComponentInChildren<Text>().text = "" + -1;
        base.HideInstantly();
    }
}
