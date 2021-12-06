using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

/// <summary>
/// Placeholds all the cutscene submanagers (VNCutsceneManager, VideoCutsceneManager, etc.).
/// When a cutscene is to be played, selects an appropriate manager to delegate the cutscene to.
/// </summary>
public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private AbstractCutscene[] _detachedCutscenes = null;
    private Dictionary<CutsceneType, AbstractCSSubmanager> _cutsceneSubmanagers;

    public Dictionary<int, AbstractCutscene> DetachedCutscenes { get; private set; }

    private void Start()
    {
        _cutsceneSubmanagers = new Dictionary<CutsceneType, AbstractCSSubmanager>();
        TryToFindSubmanager(typeof(VNCutsceneSubmanager));
        TryToFindSubmanager(typeof(VideoCutsceneSubmanager));
        CheckCutsceneSubmanagers();
        SetSubmanagersActive(false);

        DetachedCutscenes = new Dictionary<int, AbstractCutscene>();
        foreach (AbstractCutscene cutscene in _detachedCutscenes)
        {
            DetachedCutscenes.Add(cutscene.ID, cutscene);
        }
    }

    private void TryToFindSubmanager(Type submanagerType)
    {
        AbstractCSSubmanager submanager = (AbstractCSSubmanager)Resources.FindObjectsOfTypeAll(submanagerType)[0];
        if (null == submanager)
        { Debug.LogWarning("CutsceneManager couldn't find Gabeobject with " + submanagerType); }
        else
        { _cutsceneSubmanagers.Add(submanager.CutsceneType, submanager); }
    }

    private void CheckCutsceneSubmanagers()
    {
        foreach (CutsceneType type in Enum.GetValues(typeof(CutsceneType)))
        {
            if (null == _cutsceneSubmanagers[type])
            {
                Debug.LogWarning("CutseneManager doesn't have access to " + type + " submanager.\n" +
                    "Please, modify the CutseneManager Start() method so that it can find the submanager.");
            }
        }
    }

    public void LaunchCutsene(AbstractCutscene cutscene, Action onCutsceneFinished)
    {
        if (null == onCutsceneFinished) { throw new NullReferenceException("Parameter \"onCutsceneFinished\" can't be null."); }
        if (null == cutscene) { throw new NullReferenceException("Parameter \"cutscene\" can't be null."); }

        Debug.Log("Launching " + cutscene.CutsceneType + " cutscene.");
        AbstractCSSubmanager currentSubmanager = _cutsceneSubmanagers[cutscene.CutsceneType];
        currentSubmanager.gameObject.SetActive(true);
        Debug.LogFormat("Current cutscene submanager: {0}. Is active: {1}.", currentSubmanager.gameObject.name, currentSubmanager.gameObject.activeInHierarchy);
        currentSubmanager.LaunchCutscene(cutscene, onCutsceneFinished + HideSubmanagers);
    }

    public void LaunchCutsene(int cutsceneID, Action onCutsceneFinished)
    {
        //if (cutsceneID < 0) { throw new NullReferenceException("Parameter \"cutsceneNumber\" can't be < 0."); }

        LaunchCutsene(DetachedCutscenes[cutsceneID], onCutsceneFinished);
    }

    public void SetSubmanagersActive(bool active)
    {
        foreach (AbstractCSSubmanager submanager in _cutsceneSubmanagers.Values)
        {
            submanager.gameObject.SetActive(active);
        }
    }

    private void HideSubmanagers()
    {
        SetSubmanagersActive(false);
    }
}
