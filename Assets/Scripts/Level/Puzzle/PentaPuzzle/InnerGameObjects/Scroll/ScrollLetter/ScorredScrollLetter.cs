using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Letter with set <i><b>Score</b></i> and <i><b>Multiplier</b></i>.
/// </summary>
public class ScorredScrollLetter : BaseScrollLetter
{
    [SerializeField] GameObject BaseScoreIndicator = null;
    [SerializeField] GameObject ScoreMultiplierIndicator = null;
    [SerializeField] Text BaseScoreText = null;
    [SerializeField] Text ScoreMultiplierText = null;

    protected new void Awake()
    {
        base.Awake();
    }

    public override void Construct(ScorredLetter letter, int lettersInPentagram)
    {
        base.Construct(letter, lettersInPentagram);

        BaseScoreText.text = letter.Score.ToString();
        ScoreMultiplierText.text = string.Format("x{0}", letter.Multiplier);
        BaseScoreIndicator.SetActive(letter.Score != 1 && letter.Score != 0);
        ScoreMultiplierIndicator.SetActive(letter.Multiplier != 1 && letter.Score != 0);
    }
}
