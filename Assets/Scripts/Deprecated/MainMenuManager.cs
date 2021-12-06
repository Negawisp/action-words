using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Net;
using System.Net.Sockets;

#pragma warning disable 0414
public class MainMenuManager : MonoBehaviour
{
    private bool _connected = false;
    [SerializeField] GameObject _IPInputs = null;
    [SerializeField] GameObject _connectedPannel = null;
    [SerializeField] GameObject _loadingPannel = null;

    private bool _loadMultiplayerReady = false;

    public void OnConnected()
    {
        _connected = true;

        _IPInputs.SetActive(false);
        _connectedPannel.SetActive(true);

        Debug.Log("Trying to load next scene.");
        ConnectionManager.GetMsg(SetLoadMultiplayerReady);
    }

    private void FixedUpdate()
    {
        if (_loadMultiplayerReady) LoadMultiplayer();
    }


    private void OnDisconnected()
    {
        _connected = false;

        _IPInputs.SetActive(true);
        _connectedPannel.SetActive(false);
        _loadingPannel.SetActive(false);
    }

    public void LoadSingleplayer ()
    {
        SceneManager.LoadScene(1);
    }


    public void SetLoadMultiplayerReady(string msg)
    {
        Debug.Log("Load multiplayer ready: " + msg);
        _loadMultiplayerReady = true;
    }

    public void LoadMultiplayer()
    {
        _loadMultiplayerReady = false;
        _loadingPannel.SetActive(true);
        SceneManager.LoadScene(2);
    }


    public void GoVk ()
    {
        Application.OpenURL("https://vk.com/gameformonth");
    }
}
