using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Battle;

public class BattleGameManager : MonoBehaviour
{
    [SerializeField] private BattleManager _battleManager = null;
    [SerializeField] private Canvas _puzzleCanvas = null;

    [SerializeField] private Text _resultText = null;
    [SerializeField] private Text _actionText = null;

    [SerializeField] private GameObject _deathPanel = null;

    [SerializeField] private string[] _bossNames = null;
    private int _level = 0;
    private bool _isWon = false;

    void Start()
    {
        _battleManager.SetBattleEndCallback(OnBattleEnd);
        gameObject.SetActive(false);
        _battleManager.StartBattle(false, _bossNames[_level]);
    }

    private void Retry(){
        _deathPanel.SetActive(false);
        _puzzleCanvas.gameObject.SetActive(true);
        _battleManager.StartBattle(true, _bossNames[_level]);
    }

    private void NextLevel(){
        _level++;
        _level = _level % _bossNames.Length;
        _deathPanel.SetActive(false);
        _puzzleCanvas.gameObject.SetActive(true);
         _battleManager.StartBattle(true, _bossNames[_level]);
    }

    public void OnDeathButtonClick(){
        if (_isWon){
            NextLevel();
        }
        else{
            Retry();
        }
    }


    private void OnBattleEnd(bool playerWin)
    {
        //OldToDo: Implement endbattle subscription with playerWin value in subscription messege

        _deathPanel.SetActive(true);

        gameObject.SetActive(true);
        if (playerWin)
        {
            _resultText.text = "You win!";
            _actionText.text = "Next Level";
            _isWon = true;
        }
        else
        {
            _resultText.text = "You lost!";
            _actionText.text = "Retry";
            _isWon = false;
        }


        //SceneManager.LoadScene(0);
    }
}
