using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCSSubmanager : MonoBehaviour
{
    /// <summary>
    /// 
    /// Activates prefab of the submanager.
    /// From the moment this method is called and untill the call of <b>FinishCutscene()</b> method,
    /// the submanager may behave like the main object on a scene.
    /// 
    /// </summary>
    /// <param name="cutscene">Cutscene to play</param>
    /// <param name="cutsceneFinishedCallback">Method to call when the cutscene is finished</param>
    /// <returns></returns>
    public abstract void LaunchCutscene(AbstractCutscene cutscene, Action cutsceneFinishedCallback);

    /// <summary>
    /// A reaction to a User's input. It may be implemented as a skip or a quickening of a cutscene.
    /// </summary>
    public abstract void Continue();

    /// <summary>
    /// Called to finish cutscene, either prematurely or normally.
    /// </summary>
    public abstract void FinishCutscene();

    public abstract CutsceneType CutsceneType { get; }
}


public abstract class AbstractCSSubmanager<T> : AbstractCSSubmanager where T : AbstractCutscene
{
    protected T _currentCutscene;
    protected Action _cutsceneFinishedCallback;


    /// <summary>
    /// 
    /// Activates prefab of the submanager.
    /// From the moment this method is called and untill the call of <b>FinishCutscene()</b> method,
    /// the submanager may behave like the main object on a scene.
    /// 
    /// </summary>
    /// <param name="cutscene">Cutscene to play</param>
    /// <param name="cutsceneFinishedCallback">Method to call when the cutscene is finished</param>
    /// <returns></returns>
    public abstract void LaunchCutscene(T cutscene, Action cutsceneFinishedCallback);

    public override void LaunchCutscene(AbstractCutscene cutscene, Action cutsceneFinishedCallback)
    {
        if (cutscene is T)
        {
            LaunchCutscene((T)cutscene, cutsceneFinishedCallback);
        }
        else
        {
            Debug.LogError("CutsceneSubmanager couldn't play the cutscene of type " + cutscene.GetType().FullName + ".\n" +
                "Required type is " + typeof(T).FullName + ".");
        }
    }
}