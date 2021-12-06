using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseScoreIndicator : MonoBehaviour
{
    [SerializeField] protected Text _scoreText;

    protected int _requiredScore;
    protected int _currentScore;

    protected void Awake()
    {
        _requiredScore = 0;
        _currentScore = 0;
    }

    protected void Start()
    {

    }

    public virtual void SetScoreGoal(int scoreGoal)
    {
        _requiredScore = scoreGoal;//FindObjectOfType<ImbecilMobileLogger>().Log("15 1");
        UpdateScoreBar();
        //FindObjectOfType<ImbecilMobileLogger>().Log("15 4");
        Debug.Log("Score goal is set.");
        //FindObjectOfType<ImbecilMobileLogger>().Log("15 5");
    }

    public virtual void SetScore(int score)
    {
        _currentScore = score;
        UpdateScoreBar();
    }

    protected virtual void UpdateScoreBar()
    {
        FindObjectOfType<ImbecilMobileLogger>().Log("15 2");
        _scoreText.text = $"{_currentScore} / {_requiredScore}";
        FindObjectOfType<ImbecilMobileLogger>().Log("15 3");
    }
}
