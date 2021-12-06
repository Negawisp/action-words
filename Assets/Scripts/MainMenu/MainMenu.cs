using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu: MonoBehaviour, IMainMenuManager
{
    public void OpenLevel(GameObject level)
    {
        level.SetActive(true);
    }

    public void CloseLevel()
    {
        gameObject.SetActive(false);
    }

    public void OpenScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void RefreshProgress()
    {
        //UserOptions.Instance.CompletedButtonsNumber.Value = 0;
        //PlayerPrefs.SetInt("CompletedLevelsNumber", 0);
    }


    public void ProtectPlayerPrefs()
    {
        PlayerPrefsChecksumManager spp = new PlayerPrefsChecksumManager("MyGame", "GoldCurrency");
        
        PlayerPrefs.SetString("PlayerName", name);
    }
}
