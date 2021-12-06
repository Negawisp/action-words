using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightScoreReactor : BaseScoreReactor
{
    [SerializeField] private Image _opponentImage = null;
    [SerializeField] private Text _opponentNameText = null;

    public override ReactorType ReactorType => ReactorType.FightScore;

    public override void Load(ILevel level)
    {
        if (!(level is IFightScoreLevel))
        {
            Debug.LogError("Fight Score Reactor was orderd to load a non-fight-score level.");
            return;
        }
        //FindObjectOfType<ImbecilMobileLogger>().Log("9");
        base.Load(level);//FindObjectOfType<ImbecilMobileLogger>().Log("20");

        IFightScoreLevel fSLevel = (IFightScoreLevel)level;//FindObjectOfType<ImbecilMobileLogger>().Log("21");
        _opponentImage.sprite = fSLevel.OpponentSprite;//FindObjectOfType<ImbecilMobileLogger>().Log("22");
        _opponentNameText.text = fSLevel.OpponentName;//FindObjectOfType<ImbecilMobileLogger>().Log("23");
    }
}
