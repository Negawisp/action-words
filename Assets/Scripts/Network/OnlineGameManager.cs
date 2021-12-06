using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Battle;
using System;

#pragma warning disable 0414
public class OnlineGameManager : MonoBehaviour
{
    [SerializeField] private OnlineBattleManager _onlineBattleManager = null;
    [SerializeField] private Canvas _puzzleCanvas = null;

    [SerializeField] private Text _resultText = null;
    [SerializeField] private Text _actionText = null;

    [SerializeField] private GameObject _deathPanel = null;

    [SerializeField] private string[] _bossNames = null;
    private int _level = 0;
    private bool _isWon = false;

    private bool _gameStarted = false;
    private bool _playerGoesFirst = false;

    private string _word = "$";

    public void OnScrollChange()
    {
        ConnectionManager.SendMsg("-$");
    }
    
    private void OnWordActivation(string word)
    {
        ConnectionManager.SendMsg (word);
        ConnectionManager.GetMsg (_onlineBattleManager.SetEnemyActionReady);
    }
    
    void Start()
    {
        _onlineBattleManager.SetEndTurnCallback(OnWordActivation);
        _onlineBattleManager.SetBattleEndCallback(OnBattleEnd);

        ConnectionManager.SendMsg("1");
        ConnectionManager.GetMsg(SetStartBattleReady);
        //SetStartBattleReady("1");
    }

    private void SetStartBattleReady (string firstOrSecond)
    {
        _gameStarted = true;
        _playerGoesFirst = (firstOrSecond[0] == '1');
    }

    private void StartBattle ()
    {
        _onlineBattleManager.StartBattle (_playerGoesFirst, "DED_GRIB");
    }
    

    private void FixedUpdate()
    {
        if (_gameStarted)
        {
            _gameStarted = false;
            StartBattle();
        }

    }


    private void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void OnDeathButtonClick(){
        ReturnToMainMenu();
    }


    private void OnBattleEnd(bool playerWin)
    {
        // TODO: Implement endbattle subscription with playerWin value in subscription messege

        _deathPanel.SetActive(true);

        gameObject.SetActive(true);
        if (playerWin)
        {
            _resultText.text = "You win!";
            _actionText.text = "Return to main menu";
            _isWon = true;
        }
        else
        {
            _resultText.text = "You lost!";
            _actionText.text = "Return to main menu";
            _isWon = false;
        }
    }
}
