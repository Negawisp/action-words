using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestGameManager : MonoBehaviour
{
    [SerializeField] private LevelManager _levelManager = null;
    [SerializeField] private BackgroundSetter _OnImageChoose = null;
    [SerializeField] private BackgroundSetter _onImangeFinishedPanelChoose = null;
    [SerializeField] private GameObject _levelEndScreen = null;
    [SerializeField] private Text _resultText = null;
    [SerializeField] private Text _actionText = null;
    [SerializeField] private float _delayTime = 0.1f;

    void Start()
    {
        _levelEndScreen.SetActive(false);
        _OnImageChoose.SetBackground(_levelManager.Levels[0].Background);
        _onImangeFinishedPanelChoose.SetLevelFinishedPanelBackground(_levelManager.Levels[0].Icon);
        _levelManager.AddPuzzleFinishedCallback(OnCrosswordFinished);
        _levelManager.AddDefeatCallback(OnDefeat);
        _levelManager.LoadLevel(0);
    }


    private void TurnOnFinishPanel()
    {
        Debug.Log("DOTweenCheck: TestGameManager: TurnOnFinishPanel");
        Sequence sequence = DOTween.Sequence();

        _levelEndScreen.transform.GetChild(0).GetComponent<Image>().color = Color.clear;
        _levelEndScreen.transform.GetChild(1).gameObject.SetActive(false);
        _levelEndScreen.SetActive(true);

        sequence.AppendInterval(_delayTime)
                .AppendCallback(() => _levelEndScreen.transform.GetChild(1).gameObject.SetActive(true));
    }

    private void ShowLevelFinishedDialogue()
    {
        TurnOnFinishPanel();
        gameObject.SetActive(true);
        _resultText.text = "Просто волшебно!";
        _actionText.text = "Продолжить путь";
    }

    private void OnCrosswordFinished()
    {
        ShowLevelFinishedDialogue();
    }

    private void OnDefeat ()
    {
        ShowLevelFinishedDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
