using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBarScoreIndicator : BaseScoreIndicator
{
    [SerializeField] GameObject _movingHealthBar = null;

    private int _currentHealth;
    private RectTransform _movingHealthBarRectTransform;

    protected new void Awake ()
    {
        base.Awake();
        _movingHealthBarRectTransform = _movingHealthBar.GetComponent<RectTransform>();
    }

    protected new void Start()
    {
        base.Start();
    }

    public override void SetScore(int score)
    {
        Debug.Log("Setting current score");
        _currentScore = score;
        _currentHealth = _requiredScore - _currentScore;
        UpdateScoreBar();
    }

    protected override void UpdateScoreBar()
    {
        Debug.Log("DOTweenCheck: HPBarScrollIndicator: UpdateScoreBar");
        //FindObjectOfType<ImbecilMobileLogger>().Log("15 1 1");
        _scoreText.text = $"{_currentHealth}/{_requiredScore}"; //FindObjectOfType<ImbecilMobileLogger>().Log("15 1 2");
        if (_requiredScore > 0)
        {
            //FindObjectOfType<ImbecilMobileLogger>().Log("15 1 3");
            //Debug.LogFormat("New pos: {0}", _movingHealthBarRectTransform.rect.width * _currentScore / _requiredScore);
            //FindObjectOfType<ImbecilMobileLogger>().Log("Ye? " + (_movingHealthBarRectTransform != null));
            //FindObjectOfType<ImbecilMobileLogger>().Log("15 1 4");
            _movingHealthBarRectTransform.DOLocalMoveX(-_movingHealthBarRectTransform.rect.width * _currentScore / _requiredScore, 0.2f);
            //FindObjectOfType<ImbecilMobileLogger>().Log("15 1 5");
        }
        else
        {
            //FindObjectOfType<ImbecilMobileLogger>().Log("15 1 6");
            Debug.LogFormat("New pos: 0");
            //FindObjectOfType<ImbecilMobileLogger>().Log("15 1 7");
            _movingHealthBarRectTransform.DOLocalMoveX(0, 0);
            //FindObjectOfType<ImbecilMobileLogger>().Log("15 1 8");
        }
        //FindObjectOfType<ImbecilMobileLogger>().Log("15 1 9");
    }
}
