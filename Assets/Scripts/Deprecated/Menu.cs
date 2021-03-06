using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Battle;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    [SerializeField] private BattleManager _battleManager = null;
    [SerializeField] private Text _resultText = null;
    
    // Start is called before the first frame update
    void Start()
    {
        _battleManager.SetBattleEndCallback(OnBattleEnd);
    }

    public void OnPlayClick()
    {
        gameObject.SetActive(false);
        _battleManager.StartBattle(false, "DED_GRIB");
    }


    private void OnBattleEnd(bool playerWin)
    {
        gameObject.SetActive(true);
        if (playerWin)
        {
            _resultText.text = "Victory!";
        }
        else
        {
            _resultText.text = "Defeat!";
        }
    }
}
